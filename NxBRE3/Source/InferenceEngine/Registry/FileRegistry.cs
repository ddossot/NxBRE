namespace NxBRE.InferenceEngine.Registry {
	using System;
	using System.Collections;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.IO;
	using System.Threading;
	using System.Xml;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.Util;

	/// <summary>
	/// A registry of preloaded NxBRE Inference Engines instances. The registry is configured by a specific definition file and
	/// RuleML and binder files, all stored in the same folder.
	/// 
	/// Each engine is identified in the registry by a String ID.
	/// 
	/// If any of the file that was used to load the engine (rule file or binder) is modified, the registry automatically reloads
	/// it, without disrupting the users of the engines.
	/// </summary>
	public class FileRegistry:IRegistry {
		//FIXME: figure a way to unit test this class!
		/// <summary>
		/// Dictionary that stores all the engines, made immutable to prevent any modifications outside of the constructor.
		/// </summary>
		private readonly IDictionary registry;
		
		/// <summary>
		/// Dictionary that stores all the rule and binder files names that are involved in the registry and the ID
		/// of the engine they are related to. This is a kind of "reverse phone book" that allows finding the engine
		/// that depends on a certain file.
		/// </summary>
		private readonly IDictionary fileIndex;
		
		/// <summary>
		/// The time in milliseconds to wait until re-trying to access in read mode a file referenced in the registry.
		/// </summary>
		/// <remarks>
		/// The default is 500ms.
		/// </remarks>
		private int fileLockedPonderatingTime = Parameter.Get<int>("registryFileLockedPonderatingTime", 500);
		
		/// <summary>
		/// Get a named instance of an inference engine out of the registry.
		/// </summary>
		/// <param name="engineID">The ID of the engine to get</param>
		/// <returns>The desired engine instance or null if it is not available in the registry</returns>
		public IInferenceEngine GetEngine(string engineID) {
			CachedEngine cachedEngine = (CachedEngine)registry[engineID];
			if (cachedEngine != null) return cachedEngine.Engine;
			else return null;
		}
	
		/// <summary>
		/// Initializes a new registry of NxBRE Inference Engines.
		/// </summary>
		/// <param name="registryConfigurationFile">The full path and file name of the registry configuration file.</param>
		public FileRegistry(string registryConfigurationFile) {
			// we use non-synchronized ListDictionary, switch to Hashtable if much more than 10 engines are in the registry.
			registry = new ListDictionary();
			fileIndex = new ListDictionary();
			
			// store the configuration folder because the rule files and binders are stored into it
			string configurationFolder = new FileInfo(registryConfigurationFile).DirectoryName;
			
			// read the configuration
			XmlTextReader reader = new XmlTextReader(registryConfigurationFile);
			
			while (reader.Read()) {
				if ((reader.NodeType == XmlNodeType.Element) && (reader.Name == "engine")) {
					string ruleFile = reader.GetAttribute("ruleFile");
					string binderFile = reader.GetAttribute("binderFile");
					
					CachedEngine cachedEngine = new CachedEngine(reader.GetAttribute("id"),
					                                             configurationFolder,
					                                             ruleFile,
					                                             binderFile,
					                                             reader.GetAttribute("binderClass"));
					
					// initialize the engine
					cachedEngine.LoadRules();
					
					// store the engine in the registry
					registry.Add(cachedEngine.ID, cachedEngine);
					
					// register the ruleFile -> CachedEngine & binderFile -> CachedEngine pairs
					fileIndex.Add(ruleFile, cachedEngine);
					if ((binderFile != null) && (binderFile != String.Empty))	fileIndex.Add(binderFile, cachedEngine);
				}
			}
			
			// activate the file system listener
			FileSystemWatcher watcher = new FileSystemWatcher(configurationFolder);
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
			watcher.Changed += new FileSystemEventHandler(this.OnFileChanged);
			watcher.Renamed += new RenamedEventHandler(this.OnFileRenamed);
			watcher.EnableRaisingEvents = true;
		}

		/// <summary>
		/// The class used to hold an engine details and performs its (re)loading.
		/// </summary>
		private sealed class CachedEngine {
			private readonly string engineID;
			private readonly string configurationFolder;
			private readonly string ruleFile;
			private readonly string binderFile;
			private readonly string binderClass;
			private readonly IInferenceEngine engine;
			
			public string ID {
				get {
					return engineID;
				}
			}
			
			public CachedEngine(string engineID, string configurationFolder, string ruleFile, string binderFile, string binderClass)
			{
				this.engineID = engineID;
				this.configurationFolder = configurationFolder;
				this.binderFile = binderFile;
				this.ruleFile = ruleFile;
				this.binderClass = binderClass;
				//TODO: make the implementation to use a parameter (for now NxBRE.InferenceEngine.IEImpl is the only choice)
				this.engine = new IEImpl(ThreadingModelTypes.MultiHotSwap);
			}
			
			public void LoadRules() {
				if ((binderFile == null) || (binderFile == String.Empty)) {
					// no binder specified
					engine.LoadRuleBase(new RuleML09NafDatalogAdapter(configurationFolder + "/" + ruleFile, FileAccess.Read));
				}
				else {
					// we load the binder code in a string and then compile it: this method is more reliable than
					// providing a file path directly
					using (StreamReader sr = File.OpenText(configurationFolder + "/" + binderFile)) {
						engine.LoadRuleBase(new RuleML09NafDatalogAdapter(configurationFolder + "/" + ruleFile, FileAccess.Read),
						                    CSharpBinderFactory.LoadFromString(binderClass, sr.ReadToEnd()));
					}
				}
			}
			
			public IInferenceEngine Engine {
				get {
					return engine;
				}
			}
		}
		
		/// <summary>
		/// Handles all file events that are intercepted on the file system.
		/// </summary>
		/// <param name="fileName">The name of the registry file that has been somehow altered</param>
		private void OnRegistryFileEvent(String fullFileName, String fileName) {
			CachedEngine cachedEngine = (CachedEngine) fileIndex[fileName];
			
			// if the modified file is involved in the registry, then we get the impacted CachedEngine
			if (cachedEngine != null) {
				// we check if the file is available for reading, if not we ponder until it is.
				WaitUntilFileCanBeRead(fullFileName);
				
				// we use a lock to prevent two threads from reloading the same engine at the same time
				lock(cachedEngine) {
					cachedEngine.LoadRules();
				}
			}
		}
		
		/// <summary>
		/// Ponders for ever until the file referenced in the registry is available for reading.
		/// </summary>
		/// <param name="fullFileName">The full path to the file.</param>
		private void WaitUntilFileCanBeRead(String fullFileName) {
			while (true) {
				try {
					// test shared load
					using(FileStream fs = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.Read)) {
						fs.Close();
						return;
					}
				} catch	(IOException) {
					// can not read, wait half a second
					if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "Ponderating " + fileLockedPonderatingTime + "ms because the following file can not read: " + fullFileName);
					Thread.Sleep(fileLockedPonderatingTime);
				}
			}
		}
		
		/// <summary>
		/// Handles file system events fired when a file is changed.
		/// </summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">The FileSystemEventArgs that contains the event data.</param>
		private void OnFileChanged(object source, FileSystemEventArgs e)
    {
			// delegate to the common handler
			OnRegistryFileEvent(e.FullPath, e.Name);
    }
		
		/// <summary>
		/// Handles file system events fired when a file is renamed.
		/// </summary>
		/// <param name="source">The source of the event. </param>
		/// <param name="e">The RenamedEventArgs that contains the event data.</param>
    private void OnFileRenamed(object source, RenamedEventArgs e)
    {
			// delegate to the common handler
			OnRegistryFileEvent(e.OldFullPath, e.OldName);
			OnRegistryFileEvent(e.FullPath, e.Name);
    }

		}
}
