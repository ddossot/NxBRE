namespace NxBRE.InferenceEngine.Registry {
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Diagnostics;
	using System.IO;
	using System.Threading;
	using System.Xml;
	using System.Xml.Serialization;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Registry;
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
	/// <remarks>
	/// The registry does not reload itself if its configuration file is changed.
	/// </remarks>
	public class FileRegistry:IRegistry {
		/// <summary>
		/// Dictionary that stores all the engines, made immutable to prevent any modifications outside of the constructor.
		/// </summary>
		private readonly IDictionary<string, CachedEngine> registry;
		
		/// <summary>
		/// Dictionary that stores all the rule and binder files names that are involved in the registry and the ID
		/// of the engine they are related to. This is a kind of "reverse phone book" that allows finding the engine
		/// that depends on a certain file.
		/// </summary>
		private readonly IDictionary<string, IList<CachedEngine>> fileIndex;
		
		/// <summary>
		/// The configuration used for the current file registry instance.
		/// </summary>
		private readonly FileRegistryConfiguration configuration;
		
		/// <summary>
		/// Get a named instance of an inference engine out of the registry.
		/// </summary>
		/// <param name="engineID">The ID of the engine to get</param>
		/// <returns>The desired engine instance or null if it is not available in the registry</returns>
		public IInferenceEngine GetEngine(string engineID) {
			CachedEngine cachedEngine = registry[engineID];
			
			if (cachedEngine != null) return cachedEngine.Engine;
			else return null;
		}
		
		public int Count {
			get {
				return registry.Count;
			}
		}
		
		public ICollection<string> EngineIDs {
			get {
				return registry.Keys;
			}
		}
		
	
		/// <summary>
		/// Initializes a new registry of NxBRE Inference Engines.
		/// </summary>
		/// <param name="registryConfigurationFile">The full path and file name of the registry configuration file.</param>
		public FileRegistry(string registryConfigurationFile) {
			// we use non-synchronized ListDictionary, switch to Hashtable if much more than 10 engines are in the registry.
			registry = new Dictionary<string, CachedEngine>();
			fileIndex = new Dictionary<string, IList<CachedEngine>>();
			
			// load the configuration
			configuration = (FileRegistryConfiguration) new XmlSerializer(typeof(FileRegistryConfiguration)).Deserialize(new FileStream(registryConfigurationFile, FileMode.Open, FileAccess.Read, FileShare.Read));
			
			// store the configuration folder because the rule files and binders are stored into it
			string configurationFolder = ((configuration.Folder != null) && (configuration.Folder != String.Empty))?configuration.Folder:new FileInfo(registryConfigurationFile).DirectoryName;
			
			if (Logger.IsInferenceEngineInformation)
				Logger.InferenceEngineSource.TraceEvent(TraceEventType.Information,
                                                0,
                                                "Loaded configuration, Folder: "
                                                + configurationFolder
                                                + ", FileLockedPonderatingTime: "
                                                + configuration.FileLockedPonderatingTime
                                                + ", Engines: "
                                                + configuration.Engines.Length);

			
			// parse the configuration to load up the different engines
			foreach(EngineConfiguration engineConfiguration in configuration.Engines) {
				CachedEngine cachedEngine = new CachedEngine(configurationFolder, engineConfiguration);
				
				// initialize the engine
				cachedEngine.LoadRules();
				
				// store the engine in the registry
				registry.Add(cachedEngine.ID, cachedEngine);
				
				// register the ruleFile -> CachedEngine & binderFile -> CachedEngine pairs
				AddEngineSpecificFileToCache(cachedEngine.RuleFile, cachedEngine);
				AddEngineSpecificFileToCache(cachedEngine.BinderFile, cachedEngine);
			}
			
			// activate the file system listener
			FileSystemWatcher watcher = new FileSystemWatcher(configurationFolder);
			watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
			watcher.Changed += new FileSystemEventHandler(this.OnFileChanged);
			watcher.Renamed += new RenamedEventHandler(this.OnFileRenamed);
			watcher.EnableRaisingEvents = true;
		}
		
		private void AddEngineSpecificFileToCache(string file, CachedEngine cachedEngine) {
			if (file != null) {
				if (!fileIndex.ContainsKey(file)) fileIndex.Add(file, new List<CachedEngine>());
				IList<CachedEngine> cachedEngineList = fileIndex[file];
				cachedEngineList.Add(cachedEngine);
			}
		}

		/// <summary>
		/// The class used to hold an engine details and performs its (re)loading.
		/// </summary>
		private sealed class CachedEngine {
			private readonly EngineConfiguration engineConfiguration;
			private readonly string configurationFolder;
			private readonly IInferenceEngine engine;
			
			private string binderFile;
			
			public string ID {
				get {
					return engineConfiguration.Id;
				}
			}
			
			public string RuleFile {
				get {
					return engineConfiguration.Rules.File;
				}
			}
			
			public string BinderFile {
				get {
					return binderFile;
				}
			}
			
			public CachedEngine(string configurationFolder, EngineConfiguration engineConfiguration)
			{
				this.configurationFolder = configurationFolder;
				this.engineConfiguration = engineConfiguration;
				
				// instantiate the IInferenceEngine
				engine = new IEImpl(ThreadingModelTypes.MultiHotSwap);
			}
			
			public void LoadRules() {
				// prepare the rule base adapter for reading the rule file
				string ruleFileFullPath = configurationFolder + Path.DirectorySeparatorChar + RuleFile;

				if (Logger.IsInferenceEngineVerbose)
					Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
                                                   0,
                                                   "Loading rule file: "
                                                   + ruleFileFullPath
                                                   + " of format: "
                                                   + engineConfiguration.Rules.Format);

				IRuleBaseAdapter ruleBaseAdapter = null;
				
				switch(engineConfiguration.Rules.Format) {
					case RulesFormat.HRF086:
						ruleBaseAdapter = new HRF086Adapter(ruleFileFullPath, FileAccess.Read);
						break;
						
					case RulesFormat.RuleML08Datalog:
						ruleBaseAdapter = new RuleML08DatalogAdapter(ruleFileFullPath, FileAccess.Read);
						break;
						
					case RulesFormat.RuleML086Datalog:
						ruleBaseAdapter = new RuleML086DatalogAdapter(ruleFileFullPath, FileAccess.Read);
						break;
						
					case RulesFormat.RuleML086NafDatalog:
						ruleBaseAdapter = new RuleML086NafDatalogAdapter(ruleFileFullPath, FileAccess.Read);
						break;
						
					case RulesFormat.RuleML09NafDatalog:
						ruleBaseAdapter = new RuleML09NafDatalogAdapter(ruleFileFullPath, FileAccess.Read);
						break;
						
					case RulesFormat.Visio2003:
						ruleBaseAdapter = new Visio2003Adapter(ruleFileFullPath, FileAccess.Read);
						break;
				}
				       
				// estimate if a binder is present
				if (engineConfiguration.Binder != null) {
					if (engineConfiguration.Binder is CSharpBinderConfiguration) {
						CSharpBinderConfiguration cSharpBinderConfiguration = (CSharpBinderConfiguration)engineConfiguration.Binder;
						binderFile = cSharpBinderConfiguration.File;
						
						if (Logger.IsInferenceEngineVerbose)
							Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
		                                                   0,
		                                                   "Using CSharp binder file: "
		                                                   + binderFile);
	
						// we load the binder code in a string and then compile it: this method is more reliable than
						// providing a file path directly
						using (StreamReader sr = File.OpenText(configurationFolder + Path.DirectorySeparatorChar + binderFile)) {
							engine.LoadRuleBase(ruleBaseAdapter,
							                    CSharpBinderFactory.LoadFromString(cSharpBinderConfiguration.Class, sr.ReadToEnd()));
						}
					}
					else if (engineConfiguration.Binder is VisualBasicBinderConfiguration) {
						VisualBasicBinderConfiguration visualBasicBinderConfiguration = (VisualBasicBinderConfiguration)engineConfiguration.Binder;
						binderFile = visualBasicBinderConfiguration.File;
						
						if (Logger.IsInferenceEngineVerbose)
							Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
		                                                   0,
		                                                   "Using Visual Basic binder file: "
		                                                   + binderFile);
	
						// we load the binder code in a string and then compile it: this method is more reliable than
						// providing a file path directly
						using (StreamReader sr = File.OpenText(configurationFolder + Path.DirectorySeparatorChar + binderFile)) {
							engine.LoadRuleBase(ruleBaseAdapter,
							                    VisualBasicBinderFactory.LoadFromString(visualBasicBinderConfiguration.Class, sr.ReadToEnd()));
						}
					}
					else if (engineConfiguration.Binder is NxBRE.InferenceEngine.Registry.FlowEngineBinderConfiguration) {
						NxBRE.InferenceEngine.Registry.FlowEngineBinderConfiguration flowEngineBinderConfiguration = (NxBRE.InferenceEngine.Registry.FlowEngineBinderConfiguration)engineConfiguration.Binder;
						binderFile = flowEngineBinderConfiguration.File;
						
						if (Logger.IsInferenceEngineVerbose)
							Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
		                                                   0,
		                                                   "Using FlowEngine binder file: "
		                                                   + binderFile);
						
						engine.LoadRuleBase(ruleBaseAdapter,
						                    new NxBRE.InferenceEngine.IO.FlowEngineBinder(configurationFolder + Path.DirectorySeparatorChar + binderFile, flowEngineBinderConfiguration.Type));
					}
					else {
						throw new BREException("Unexpected type of binder object in registry configuration: " + engineConfiguration.Binder.GetType().FullName);
					}
				}
				else {
					// no binder specified
					binderFile = null;
					engine.LoadRuleBase(ruleBaseAdapter);
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
		/// <param name="fullFileName">The complete name of the file that has been somehow altered.</param>
		/// <param name="fileName">The name of the registry file that has been somehow altered.</param>
		private void OnRegistryFileEvent(String fullFileName, String fileName) {
			// FR-1817201 ensure file is under registry control before waiting for it
			IList<CachedEngine> cachedEngines;
			
			if (fileIndex.TryGetValue(fileName, out cachedEngines)) {
				foreach(CachedEngine cachedEngine in cachedEngines) {
					// we check if the file is available for reading, if not we ponder until it is.
					WaitUntilFileCanBeRead(fullFileName);
					
					// we use a lock to prevent two threads from reloading the same engine at the same time
					lock(cachedEngine) {
						cachedEngine.LoadRules();
					}
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
					if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose,
					                                                                             0,
					                                                                             "Ponderating "
					                                                                             + configuration.FileLockedPonderatingTime
					                                                                             + "ms because the following file can not read: "
					                                                                             + fullFileName);
					Thread.Sleep(configuration.FileLockedPonderatingTime);
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
