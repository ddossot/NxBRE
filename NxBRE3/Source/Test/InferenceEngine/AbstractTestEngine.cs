namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	
	using NUnit.Framework;
	
	using net.ideaity.util.events;
	
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
		protected int logThreshold;
		
		protected readonly string ruleFilesFolder;
		protected readonly string outFilesFolder;

		protected AbstractTestEngine() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
			outFilesFolder = Parameter.GetString("unittest.outputfolder") + "/";
		}

		protected void HandleLogEvent(object obj, ILogEvent aLog)
		{
			if (aLog.Priority >= logThreshold)
				Console.WriteLine("NxBRE LOG " + aLog.Priority + " MSG  : " + aLog.Message);
		}

		protected void HandleNewFactEvent(NewFactEventArgs nfea) 
	  {
			if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("+ Deducted: {0}", nfea.Fact);
	  	deducted++;	
	  }
	  
		protected void HandleDeletedFactEvent(NewFactEventArgs nfea) 
	  {
			if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("- Deleted : {0}", nfea.Fact);
	  	deleted++;	
	  }
	  
		protected void HandleModifiedFactEvent(NewFactEventArgs nfea) 
	  {
			if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("* Modified : {0}->{1}", nfea.Fact, nfea.OtherFact);
	  	modified++;	
	  }
	  
		protected void HandleExpectedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null)
	  		&& (Array.IndexOf(deductionsToCheck, nfea.Fact.ToString())<0)) {
	  			wrongDeduction = true;
					Console.WriteLine("* Wrongly Deducted: {0} @ {1} !!! !!!", nfea.Fact, deductionChecker);
	  		}
	  	deductionChecker++;
	  }
	  
		protected void HandleOrderedNewFact(NewFactEventArgs nfea) 
	  {
	  	if ((deductionsToCheck != null)
	  		&& (deductionsToCheck[deductionChecker] != nfea.Fact.ToString())) {
	  			wrongDeduction = true;
					Console.WriteLine("* Wrongly Deducted: {0} @ {1}, Expected: {2}", nfea.Fact, deductionChecker, deductionsToCheck[deductionChecker]);
	  		}
	  	deductionChecker++;
	  }
	  
		protected virtual void NewIEImpl(IBinder bob) {
			if (bob != null) ie = new IEImpl(bob);
			else ie = new IEImpl();
		}
		
	  protected void InitIE(IBinder bob) {
			IEImpl.StrictImplication = false;
			logThreshold = LogEventImpl.INFO;
			
			NewIEImpl(bob);
			
			ie.LogHandlers += new DispatchLog(HandleLogEvent);
	  	ie.NewFactHandler += new NewFactEvent(HandleNewFactEvent);
	  	ie.DeleteFactHandler += new NewFactEvent(HandleDeletedFactEvent);
	  	ie.ModifyFactHandler += new NewFactEvent(HandleModifiedFactEvent);
	  	
			Console.WriteLine("InitIE()");  	
	  }
	  
	  [SetUp]
	  public void InitIE() {
	  	InitIE(null);
	  }
	  
	  [TearDown]
	  public void DestroyIE() {
			ie = null;
			Console.WriteLine("DestroyIE()");
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
			if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("-(Query Results) -");
			foreach(IList<Fact> facts in qrs) {
				i++;
				if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine(" (Result #{0})", i);
				foreach(Fact fact in facts) {
					HandleExpectedNewFact(new NewFactEventArgs(fact));
					if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("  {0}", fact);
				}
			}
			if (logThreshold == LogEventImpl.DEBUG) Console.WriteLine("-(End Results)-\n");
			
			deductionsToCheck = null;
		}

	}
}
