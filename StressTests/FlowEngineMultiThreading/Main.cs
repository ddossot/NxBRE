namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Threading;
	
	using NxBRE.FlowEngine;
	using NxBRE.FlowEngine.Factories;
	using NxBRE.FlowEngine.IO;
	using NxBRE.FlowEngine.Rules;
	
	public class MainClass
	{
		//FIXME: work in memory
		private const string XMLFILE = "c:/temp/stresstest.xml";
		private const int LIMIT = 39;	
		private int DURATION = 60;
		private int NBTHREADS = 20;
		
		private long hits;
		private long errors;
		private long iniTime;
		private Random rnd = new Random();
		private bool running = false;
		private Timer stopTimer;
		private Hashtable threadMap = Hashtable.Synchronized(new Hashtable());
		
		public static void Main(string[] args)
		{
			MainClass mc = new MainClass();
			
			if (args.Length > 0)
				mc.DURATION = Int32.Parse(args[0]);

			if (args.Length > 1)
				mc.NBTHREADS = Int32.Parse(args[1]);
				
			mc.WriteRules();
			mc.RunTests();
			
			Console.WriteLine();
			Console.WriteLine("Enter to shutdown, or when all processes are finished");
			Console.ReadLine();
			if (mc.running) mc.StopTests(null);
			GC.Collect();
			Console.WriteLine("GC done. Thread state analysis:");
			
			// this is done just for test purpose
			IDictionaryEnumerator threadEnumerator = mc.threadMap.GetEnumerator();
			ArrayList deadThreads = new ArrayList();
			while(threadEnumerator.MoveNext()) {
				Thread t = ((Thread)threadEnumerator.Key);
				Console.WriteLine("#{0}.IsAlive={1}", threadEnumerator.Value, t.IsAlive);
				if (!t.IsAlive) deadThreads.Add(t);
			}
			foreach(Thread t in deadThreads) {
				mc.threadMap.Remove(t);
				Console.WriteLine("{0} has been dereferenced", t);
			}
			
      Console.WriteLine();
			Console.WriteLine("Enter to quit");
			Console.ReadLine();
		}
		
		private void WriteRules() {
			using(StreamWriter sw = new StreamWriter(XMLFILE)) {
				
				sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				sw.WriteLine("<xBusinessRules xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"http://nxbre.org/xBusinessRules.xsd\">");
				sw.WriteLine("<Integer id=\"RESULT\" value=\"0\"/>");
				for (int i=1;i<=LIMIT;i++)
					sw.WriteLine("<Integer id=\""+i+"i\" value=\""+i+"\"/>");
	
				for (int i=1;i<=LIMIT;i++) {
					sw.WriteLine("<Set id=\"A"+i+"\">");
					sw.WriteLine("<Logic>");
	
					for (int j=1;j<=LIMIT;j++) {
						if (j == 1) {
							sw.WriteLine("<If><And><Equals leftId=\"B\" rightId=\""+j+"i\"/>");
							sw.WriteLine("</And><Do>");
							sw.WriteLine("<Modify id=\"RESULT\" value=\""+i*j+"\" type=\"Integer\"/>");
							sw.WriteLine("</Do></If>");
						}
						else {
							sw.WriteLine("<ElseIf><And><Equals leftId=\"B\" rightId=\""+j+"i\"/>");
							sw.WriteLine("</And><Do>");
							sw.WriteLine("<Modify id=\"RESULT\" value=\""+i*j+"\" type=\"Integer\"/>");
							sw.WriteLine("</Do></ElseIf>");
						}
					}
					sw.WriteLine("<Else>");
					sw.WriteLine("<Modify id=\"RESULT\" value=\"-1\" type=\"Integer\"/>");
					sw.WriteLine("</Else>");
					sw.WriteLine("</Logic>");
					sw.WriteLine("</Set>");
				}
				
				sw.WriteLine("</xBusinessRules>");
			}
		}
		
		private void RunTests() {
			BRECloneFactory brecf = new BRECloneFactory(new XBusinessRulesFileDriver(XMLFILE));
	
			hits = 0;
			errors = 0;
			running = true;
			iniTime = DateTime.Now.Ticks;
			
			for(int i=0;i<NBTHREADS;i++) {
				Stresser s = new Stresser(i, brecf.NewBRE(), this);
				Thread t = new Thread(new ThreadStart(s.Run));
				t.Start();
			}
			
			stopTimer = new Timer(new TimerCallback(StopTests), null, DURATION*1000, Timeout.Infinite);
			
			Console.WriteLine();
			Console.WriteLine("Running...");
		}
		
		private void StopTests(object o) {
			running = false;
			Console.WriteLine("{0} hits, {1} errors in {2} msec",
	                  hits,
	                  errors,
	                  (long)(DateTime.Now.Ticks - iniTime)/10000);
	
		}
		
		private class Stresser {
			private MainClass mc;
			private int a;
			private int b;
			private int result;
			private int stresserID;
			private IFlowEngine bre;
			
			public Stresser(int stresserID, IFlowEngine bre, MainClass mc) {
				this.stresserID = stresserID;
				this.mc = mc;
				this.bre = bre;
			}
			
			public void Run() {
				mc.threadMap.Add(Thread.CurrentThread, stresserID);
				
				Console.WriteLine("Stresser {0} Starting...", stresserID);
				try {
					while(mc.running) {
						mc.hits++;
						a = mc.rnd.Next(1, 1+LIMIT);
						b = mc.rnd.Next(1, 1+LIMIT);
						bre.RuleContext.SetObject("B", b);
						bre.Process("A"+a);
						result = (Int32)bre.RuleContext.GetObject("RESULT");
						if (result != a*b) mc.errors++; //Console.WriteLine("{0}*{1}={2}", a, b, result);
						bre.Reset();
					}
					Console.WriteLine("Stresser {0} Stopped", stresserID);
				} catch (System.Exception) {
					mc.errors++;
					Console.WriteLine("Stresser {0} Dead", stresserID);
				}
			}
		}
	}
}
