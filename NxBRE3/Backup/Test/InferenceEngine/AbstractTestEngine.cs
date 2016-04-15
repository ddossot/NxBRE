namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	using System.Diagnostics;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	public abstract class AbstractTestEngine {
		protected IInferenceEngine ie;
		protected int deducted;
		protected int deleted;
		protected int modified;
		protected int deductionChecker;
		protected string[] deductionsToCheck;
		protected bool wrongDeduction;
		protected IList<IList<Fact>> qrs;
		
		protected readonly string ruleFilesFolder;
		protected readonly string outFilesFolder;

		protected AbstractTestEngine() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
			outFilesFolder = Parameter.GetString("unittest.outputfolder") + "/";
			
			// activate verbose trace to console
//			Logger.InferenceEngineSource.Switch.Level = System.Diagnostics.SourceLevels.Verbose;
//			Logger.RefreshBooleanSwitches();
//			Logger.InferenceEngineSource.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
		}

		protected void HandleNewFactEvent(NewFactEventArgs nfea) 
	  {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "+ Deducted: " + nfea.Fact);
	  	deducted++;	
	  }
	  
		protected void HandleDeletedFactEvent(NewFactEventArgs nfea) 
	  {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "- Deleted: " + nfea.Fact);
	  	deleted++;	
	  }
	  
		protected void HandleModifiedFactEvent(NewFactEventArgs nfea) 
	  {
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "* Modified: " + nfea.Fact + " -> " + nfea.OtherFact);
	  	modified++;	
	  }
	  
		protected void HandleExpectedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null) && (Array.IndexOf(deductionsToCheck, nfea.Fact.ToString(false))<0)) {
  			wrongDeduction = true;
				Console.Error.WriteLine("* Wrongly Deducted: {0} @ {1} !!! !!!", nfea.Fact, deductionChecker);
	  	}
			
	  	deductionChecker++;
	  }
	  
		protected void HandleOrderedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null) && (deductionsToCheck[deductionChecker] != nfea.Fact.ToString(false))) {
  			wrongDeduction = true;
				Console.Error.WriteLine("* Wrongly Deducted: {0} @ {1}, Expected: {2}", nfea.Fact, deductionChecker, deductionsToCheck[deductionChecker]);
  		}
			
	  	deductionChecker++;
	  }
	  
		protected virtual void NewIEImpl(IBinder bob) {
			if (bob != null) ie = new IEImpl(bob);
			else ie = new IEImpl();
		}
		
	  protected void InitIE(IBinder bob) {
			NewIEImpl(bob);
			
			((IEImpl)ie).strictImplication = false;
	  	ie.NewFactHandler += new NewFactEvent(HandleNewFactEvent);
	  	ie.DeleteFactHandler += new NewFactEvent(HandleDeletedFactEvent);
	  	ie.ModifyFactHandler += new NewFactEvent(HandleModifiedFactEvent);
	  	
	  	if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "InitIE()");  	
	  }
	  
	  [SetUp]
	  public void InitIE() {
	  	InitIE(null);
	  }
	  
	  [TearDown]
	  public void DestroyIE() {
			ie = null;
	  	if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "DestroyIE()");  	
	  }
	  
	  private void PreProcess() {
	  	deducted = 0;
	  	deleted = 0;
	  	modified = 0;
	  	deductionChecker = 0;
	  	wrongDeduction = false;	  	
	  }
	  
	  protected void Process() {
	  	PreProcess();
			ie.Process();
	  }
	  
	  protected void Process(Hashtable businessObjects) {
	  	PreProcess();
			ie.Process(businessObjects);
	  }
	  
		protected void ParseResult() {
			wrongDeduction = false;
			deductionChecker = 0;
			
			int i = 0;
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "-(Query Results) -");
			
			foreach(IList<Fact> facts in qrs) {
				i++;
				if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, " (Result #{" + i + "})");
				foreach(Fact fact in facts) {
					HandleExpectedNewFact(new NewFactEventArgs(fact));
					if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "  " + fact);
				}
			}
			if (Logger.IsInferenceEngineVerbose) Logger.InferenceEngineSource.TraceEvent(TraceEventType.Verbose, 0, "-(End Results)-\n");
			
			deductionsToCheck = null;
		}

	}
}
