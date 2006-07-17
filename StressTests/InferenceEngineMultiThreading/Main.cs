namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Threading;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	public class MainClass {
		private readonly static string[] RULES = new string[]{"multiply", "add"};
		private const int LIMIT = 1000;
		private const string XMLFOLDER = "../files/";
		private const int DURATION = 12;
		private const int NBTHREADS = 10;
		private static bool HOT_SWAP = true;
		
		private static IInferenceEngine MAIN_IE;
		private static IInferenceEngine SECOND_IE;
		
		private static volatile bool switched = false;
		private static volatile bool binder = false;
		private static volatile bool csharpBinder = true;
		
		private long hits;
		private long errors;
		private long iniTime;
		private Random rnd = new Random();
		private bool running = false;
		private Hashtable threadMap = Hashtable.Synchronized(new Hashtable());
		
		private Timer switchRuleBaseTimer;
		private Timer switchBinder;
		private Timer stopTests;
		
		
		private static string ActiveRule {
			get {
				return switched?RULES[1]:RULES[0];
			}
		}
		
		private static string RuleBaseFile {
			get {
				return XMLFOLDER + ActiveRule + (binder?"-bound":"") + ".ruleml";
			}
		}
		
		private static void InitMainEngine() {
			IBinder binderObject = null;
			
			if (binder) {
				if (csharpBinder) {
					String code;
					using (StreamReader sr = File.OpenText(RuleBaseFile + ".ccb")) code = sr.ReadToEnd();
					//FIXME: correct binders (namespace and using)
					binderObject = CSharpBinderFactory.LoadFromString("org.nxbre.examples." + ActiveRule.ToUpper() + "_Binder" , code);
				}
				else {
					binderObject = new FlowEngineBinder(RuleBaseFile + ".xbre", BindingTypes.BeforeAfter);
				}
			}
			
			MAIN_IE.LoadRuleBase(new RuleML086DatalogAdapter(RuleBaseFile, System.IO.FileAccess.Read), binderObject);
		}

		public static void Main(string[] args)
		{
			HOT_SWAP = Boolean.Parse(args[0]);
			binder = Boolean.Parse(args[1]);
			if (args.Length > 2) csharpBinder = Boolean.Parse(args[2]);
			
			MAIN_IE = new IEImpl((HOT_SWAP)?ThreadingModelTypes.MultiHotSwap:ThreadingModelTypes.Multi);
			//MAIN_IE = new IEImpl(ThreadingModelTypes.MultiHotSwap);
			SECOND_IE = new IEImpl((HOT_SWAP)?ThreadingModelTypes.MultiHotSwap:ThreadingModelTypes.Single);
			
			MainClass mc = new MainClass();
			
			InitMainEngine();
			
			SECOND_IE.LoadRuleBase(new RuleML086DatalogAdapter(XMLFOLDER + "subtract.ruleml", System.IO.FileAccess.Read));
			
			mc.RunTests();
		}
		
		private void RunTests() {
			hits = 0;
			errors = 0;
			running = true;
			iniTime = DateTime.Now.Ticks;
			
			for(int i=0;i<NBTHREADS;i++) {
				Stresser s = new Stresser(i, this, (i!=0)?MAIN_IE:SECOND_IE);
				Thread t = new Thread(new ThreadStart(s.Run));
				t.Name = "Stresser_" + i;
				t.Start();
			}
			
			if (HOT_SWAP) {
				switchRuleBaseTimer = new Timer(new TimerCallback(SwitchRuleBase), null, DURATION*500, Timeout.Infinite);
				if (binder) switchBinder = new Timer(new TimerCallback(SwitchBinder), null, DURATION*250, DURATION*500);
			}
			
			stopTests = new Timer(new TimerCallback(StopTests), null, DURATION*1000, Timeout.Infinite);
		}
		
		private void StopTests(object o) {
			running = false;
			
			Console.WriteLine();
			Console.WriteLine("********************************");

			Console.WriteLine("{0} hits, {1} errors in {2} msec",
	                  hits,
	                  errors,
	                  (long)(DateTime.Now.Ticks - iniTime)/10000);
	
			Console.WriteLine("********************************");
			Console.WriteLine();
		}
		
		private void SwitchBinder(object o) {
			csharpBinder = !csharpBinder;
			InitMainEngine();
		}
		
		private void SwitchRuleBase(object o) {
			switched = true;
			InitMainEngine();
		}
		
		private class Stresser {
			private MainClass mc;
			private int a;
			private int b;
			private int result;
			private int stresserID;
			private IInferenceEngine ie;
			private int bumps;
			
			public Stresser(int stresserID, MainClass mc, IInferenceEngine ie) {
				this.stresserID = stresserID;
				this.mc = mc;
				this.ie = ie;
				this.bumps = 0;
			}
			
			public void Run() {
				mc.threadMap.Add(Thread.CurrentThread, stresserID);
				
				//Console.WriteLine("Stresser {0} Starting...", stresserID);
				
				try {
					while(mc.running) {
						mc.hits++;
						bumps++;
						a = mc.rnd.Next(1, 1+LIMIT);
						b = mc.rnd.Next(1, 1+LIMIT);
						
						ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
						
						if ((ie.Label != "subtract") && (binder)) {
							Hashtable bo = new Hashtable();
							bo.Add(ie.Label + "-a", a);
							bo.Add(ie.Label + "-b", b);
							ie.Process(bo);
						}
						else {
							ie.Assert(new Fact("values", new Individual(a), new Individual(b)));
							ie.Process();
						}
						
						if (ie.FactsCount != 2) throw new Exception("Got wrong fact count: " + ie.FactsCount);
						
						result = (int)ie.GetFact("result").GetPredicateValue(0);
						
						if (((ie.Label == "multiply") && (result != (7+a*b))) ||
						    ((ie.Label == "add") && (result != (3+a+b))) ||
						    ((ie.Label == "subtract") && (result != (11+a-b)))) {
							mc.errors++;
							//Console.Write("[{0}?{1}={2}:{3}]  ", a, b, result, ie.Label);
						}
						else
							if ((ie.Label != "multiply") && (ie.Label != "add") && (ie.Label != "subtract"))
								throw new Exception("Unknown label: " + ie.Label);
								
						if (HOT_SWAP) ie.DisposeIsolatedMemory();
						
					}
					
					if ((switched) && (ie.Label == RULES[0])) throw new Exception("Thread should have switched, but is still: " + ie.Label);
					
					//Console.WriteLine("Stresser {0} Stopped (bumps: {3} - label:{2})", stresserID, switched, ie.Label, bumps);
				} catch (Exception e) {
					mc.errors++;
					Console.WriteLine("Stresser {0} Dead (bumps: {2} - ActiveRule:{3} - label:{4})", stresserID, switched, bumps, ActiveRule, ie.Label);
					Console.WriteLine(e);
				}
			}
		}
	}
}
