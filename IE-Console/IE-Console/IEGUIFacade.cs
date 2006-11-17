namespace NxBRE.InferenceEngine.Console {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Reflection;
	using System.Text;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	public struct UserPreferences {
		public string lastBinderClassName;
		public string lastHRFFact;
	}
	
	/// <summary>
	/// Basic Console for NxBRE Inference Engine
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public class IEGUIFacade {
		private IInferenceEngine ie = null;
		private UserPreferences up;
		
		public IEGUIFacade() {
			up = Utils.LoadUserPreferences();
		}
		
		public UserPreferences UserPrefs {
			get {
				return up;
			}
		}
		
		public bool Valid {
			get {
				return ((ie != null) && (ie.Initialized));
			}
		}

		public bool HasImplications {
			get {
				return (Valid) && (ie.ImplicationsCount > 0);
			}
		}

		public bool HasQueries {
			get {
				return (Valid) && (ie.QueriesCount > 0);
			}
		}

		public bool HasFacts {
			get {
				return (Valid) && (ie.FactsCount > 0);
			}
		}

		public int FactsCount {
			get {
				if (Valid) return ie.FactsCount;
				else return 0;
			}
		}

		public WorkingMemoryTypes WMType {
			get {
				return ie.WorkingMemoryType;
			}
			set {
				ie.NewWorkingMemory(value);
			}
		}
		
		public void WMCommit() {
			ie.CommitIsolatedMemory();
		}

		public string Status {
			get {
				if (Valid)
					return "RuleBase '" + ie.Label + "' details:|" +
								 ((ie.BinderType != null)?(" Binding type: " + ie.BinderType + "|"):"") +
								 " Working memory: " + ie.WorkingMemoryType + "|" +
								 " " + ie.FactsCount + " facts|" +
								 " " + ie.ImplicationsCount + " implications|" +
								 " " + ie.QueriesCount + " queries|";
				else
					return "No rulebase loaded.";
			}
		}
		
		public void DumpFacts(ConsoleWriter factDumperTarget) {
			factDumperTarget("Facts in " + WMType + " working memory:");
			FactDumperAdapter fda = new FactDumperAdapter(factDumperTarget);
			ie.SaveFacts(fda);
		}
		
		public IList<string> QueryLabels {
			get {
				return ie.QueryLabels;
			}
		}
		
		public string PromptFactOperation(MainForm mf, string operation) {
			up.lastHRFFact = mf.PromptForString("Manual Fact " + operation,
	                                        "Enter a fact in Human Readable Format (ASCII):",
	                                     	  up.lastHRFFact);
			
			if (up.lastHRFFact != String.Empty) {
				if (!up.lastHRFFact.EndsWith(";")) up.lastHRFFact += ";";
				int iniNbFacts = ie.FactsCount;
				using(HRF086Adapter hrfa = new HRF086Adapter(new MemoryStream(Encoding.ASCII.GetBytes("#DIRECTION_FORWARD\n\r+" + up.lastHRFFact)),
				                                           FileAccess.Read)) {
				
					foreach(Fact f in hrfa.Facts)
						if (operation == "Assertion") ie.Assert(f);
						else ie.Retract(f);
				}
				
				if (iniNbFacts != ie.FactsCount) return up.lastHRFFact;
			}
			
			return String.Empty;
		}
		
		public void LoadRuleBase(MainForm mf, string uri, bool onlyFacts) {
			IBinder binder = null;
			// give priority to custom compiled binders
			if (File.Exists(uri + ".ccb")) {
				up.lastBinderClassName = mf.PromptForString("C# Custom Binder - " + uri + ".ccb",
				                                      	 "Enter the fully qualified name of the binder class:",
				                                     		 up.lastBinderClassName);
				binder = CSharpBinderFactory.LoadFromFile(up.lastBinderClassName, uri + ".ccb");
			}
			else if (File.Exists(uri + ".vcb")) {
				up.lastBinderClassName = mf.PromptForString("VB.NET Custom Binder - " + uri + ".vcb",
				                                      	 "Enter the fully qualified name of the binder class:",
				                                     		 up.lastBinderClassName);
				binder = VisualBasicBinderFactory.LoadFromFile(up.lastBinderClassName, uri + ".vcb");
			}
			else if (File.Exists(uri + ".xbre")) {
				bool isBeforeAfter = mf.AskYesNoQuestion("Flow Engine Binder - " + uri + ".xbre",
				                                         uri + ".xbre\n\nIs this binder running in Before/After mode ?\n\n(No would mean that it runs in Control Process mode)");
				
				binder = new FlowEngineBinder(uri + ".xbre", isBeforeAfter?BindingTypes.BeforeAfter:BindingTypes.Control);
			}
			
			if (!onlyFacts) ie = new IEImpl(binder);

			switch(Path.GetExtension(uri).ToLower()) {
				case ".ruleml":
					try {
						if (onlyFacts) ie.LoadFacts(new RuleML09NafDatalogAdapter(uri, FileAccess.Read));
						else ie.LoadRuleBase(new RuleML09NafDatalogAdapter(uri, FileAccess.Read));
					}
					catch(Exception firstAttemptException) {
						try {
							if (onlyFacts) ie.LoadFacts(new RuleML086NafDatalogAdapter(uri, FileAccess.Read));
							else ie.LoadRuleBase(new RuleML086NafDatalogAdapter(uri, FileAccess.Read));
						}
						catch(Exception) {
							try {
								if (onlyFacts) ie.LoadFacts(new RuleML086DatalogAdapter(uri, FileAccess.Read));
								else ie.LoadRuleBase(new RuleML086DatalogAdapter(uri, FileAccess.Read));
							}
							catch(Exception) {
								try {
									if (onlyFacts) ie.LoadFacts(new RuleML08DatalogAdapter(uri, FileAccess.Read));
									else ie.LoadRuleBase(new RuleML08DatalogAdapter(uri, FileAccess.Read));
								} catch(Exception) {
									// the fall-back policy failed, hence throw the original exception
									throw firstAttemptException;
								}
							}
						}
					}
					break;
				
				case ".hrf":
					if (onlyFacts) ie.LoadFacts(new HRF086Adapter(uri, FileAccess.Read));
					else ie.LoadRuleBase(new HRF086Adapter(uri, FileAccess.Read));
					break;
				
				case ".vdx":
					string[] selectedPages = mf.PromptForVisioPageNameSelection(Visio2003Adapter.GetPageNames(uri));
					if (selectedPages != null) {
						if (onlyFacts) ie.LoadFacts(new Visio2003Adapter(uri, FileAccess.Read, selectedPages));
						else ie.LoadRuleBase(new Visio2003Adapter(uri, FileAccess.Read, selectedPages));
					}
					break;
				
				default:
					throw new Exception(Path.GetExtension(uri) + " is an unknown extension.");
			}
		}
		
		public void SaveRuleBase(MainForm mf, string uri, bool onlyFacts) {
			switch(Path.GetExtension(uri).ToLower()) {
				case ".ruleml":
					if (onlyFacts) ie.SaveFacts(new RuleML09NafDatalogAdapter(uri, FileAccess.Write));
					else ie.SaveRuleBase(new RuleML09NafDatalogAdapter(uri, FileAccess.Write));
					break;
				
				case ".hrf":
					if (onlyFacts) ie.SaveFacts(new HRF086Adapter(uri, FileAccess.Write));
					else ie.SaveRuleBase(new HRF086Adapter(uri, FileAccess.Write));
					break;
				
				default:
					throw new Exception(Path.GetExtension(uri) + " is an unsupported saving extension.");
			}
		}
		
		public void Process(NewFactEvent nfh, NewFactEvent dfh, NewFactEvent mfh) {
			ie.NewFactHandler += nfh;
			ie.DeleteFactHandler += dfh;
			ie.ModifyFactHandler += mfh;
			
			if (ie.BinderType != null) ie.Process(new Hashtable());
			else ie.Process();
			
			ie.DeleteFactHandler -= dfh;
			ie.NewFactHandler -= nfh;
		}
		
		public string RunQuery(int queryIndex) {
			IList<IList<Fact>> qrs = ie.RunQuery(queryIndex);
			string result = "Query results:|";
			int i = 0;
			foreach(IList<Fact> facts in qrs) {
				i++;
				result += (" #" + i + ":|");
				foreach(Fact fact in facts) result += ("  " + fact.ToString() + "|");
			}
			return result;
		}
	}
	
}
