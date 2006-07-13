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
		}

		protected void HandleNewFactEvent(NewFactEventArgs nfea) 
	  {
			Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "+ Deducted: " + nfea.Fact);
	  	deducted++;	
	  }
	  
		protected void HandleDeletedFactEvent(NewFactEventArgs nfea) 
	  {
			Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "- Deleted: " + nfea.Fact);
	  	deleted++;	
	  }
	  
		protected void HandleModifiedFactEvent(NewFactEventArgs nfea) 
	  {
			Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "* Modified: " + nfea.Fact + " -> " + nfea.OtherFact);
	  	modified++;	
	  }
	  
		protected void HandleExpectedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null)
	  		&& (Array.IndexOf(deductionsToCheck, nfea.Fact.ToString())<0)) {
	  			wrongDeduction = true;
					Console.Error.WriteLine("* Wrongly Deducted: {0} @ {1} !!! !!!", nfea.Fact, deductionChecker);
	  		}
	  	deductionChecker++;
	  }
	  
		protected void HandleOrderedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null)
	  		&& (deductionsToCheck[deductionChecker] != nfea.Fact.ToString())) {
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
			IEImpl.StrictImplication = false;
			
			NewIEImpl(bob);
			
	  	ie.NewFactHandler += new NewFactEvent(HandleNewFactEvent);
	  	ie.DeleteFactHandler += new NewFactEvent(HandleDeletedFactEvent);
	  	ie.ModifyFactHandler += new NewFactEvent(HandleModifiedFactEvent);
	  	
	  	Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "InitIE()");  	
	  }
	  
	  [SetUp]
	  public void InitIE() {
	  	InitIE(null);
	  }
	  
	  [TearDown]
	  public void DestroyIE() {
			ie = null;
	  	Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "DestroyIE()");  	
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
			Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "-(Query Results) -");
			
			foreach(IList<Fact> facts in qrs) {
				i++;
				Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, " (Result #{" + i + "})");
				foreach(Fact fact in facts) {
					HandleExpectedNewFact(new NewFactEventArgs(fact));
					Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "  " + fact);
				}
			}
			Trace.WriteLineIf(Misc.IE_TS.TraceVerbose, "-(End Results)-\n");
			
			deductionsToCheck = null;
		}

	}
}
