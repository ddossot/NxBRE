namespace org.nxbre.examples {
	using System;
	using System.Collections;
	using System.Threading;
	
	using net.ideaity.util.events;
	
	using org.nxbre.ie;
	using org.nxbre.ie.adapters;
	using org.nxbre.ie.core;
	using org.nxbre.ie.rule;
	using org.nxbre.ie.predicates;

	public class RatingEngine {
		private int nbDodecaCalls;
		private IInferenceEngine ie;
		
		private void HandleNewFactEvent(NewFactEventArgs nfea) 
	  {
			Console.WriteLine("* Deducted: {0}", nfea.Fact);
	  }
	  
		private void HandleLogEvent(object obj, ILogEvent aLog)
		{
			if (aLog.Priority >= LogEventImpl.DEBUG)
				Console.WriteLine("[" + aLog.Priority + "] " + aLog.Message);
		}	

		public RatingEngine(int verbosityLevel, int nbDodecaCalls, string ruleBaseFile) {
			this.nbDodecaCalls = nbDodecaCalls;

			ie = new IEImpl(CSharpBinderFactory.LoadFromFile("org.nxbre.examples.TelcoRatingBinder",
			                       																	ruleBaseFile + ".ccb"));

			if (verbosityLevel >= 2) ie.LogHandlers += new DispatchLog(HandleLogEvent);
			if (verbosityLevel >= 1) ie.NewFactHandler += new NewFactEvent(HandleNewFactEvent);
			
			ie.LoadRuleBase(new RuleML086DatalogAdapter(ruleBaseFile, System.IO.FileAccess.Read));

		}

		public void Run() {
			
			// prepare some dummy data
			Hashtable businessObjects = new Hashtable();
			ArrayList callData = DummyData.GetInstance().GetBusinessObjects(nbDodecaCalls);
			businessObjects.Add("CALLSDATA", callData);
			
			// process this data
			Console.WriteLine("Using RuleBase: {0}", ie.Label);
			long iniTime = DateTime.Now.Ticks;
			ie.Process(businessObjects);
			Console.WriteLine("{0} Calls processed in: {1} milliseconds",
			                  12 * nbDodecaCalls,
			                  (int)(DateTime.Now.Ticks - iniTime)/10000);
			
			// ensure the engine is rating correctly
			foreach(CallData cd in callData) cd.CheckValidity();
		}
		
		/// <summary>
		/// Starts the Telco Rating Engine.
		/// </summary>
		/// <param name="args">
		/// args[0] the number of dozens of calls data to create
		/// args[1] the full path of telco-rating.ruleml
		/// </param>
		public static void Main(string[] args) {
			RatingEngine re = new RatingEngine(0, Int32.Parse(args[0]), args[1]);
			
			int maxRun = (args.Length==3)?Int32.Parse(args[2]):Int32.MaxValue;
			
			for(int i=0; i<maxRun; i++) {
				re.Run();
				GC.Collect();
				Console.WriteLine("Total Memory: {0}", GC.GetTotalMemory(true));
			}
		}
		
	}
}
