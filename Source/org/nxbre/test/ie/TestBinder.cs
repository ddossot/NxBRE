namespace org.nxbre.test.ie {
	using System;
	using System.Collections;
	using System.IO;
	
	using NUnit.Framework;

	using net.ideaity.util.events;

	using org.nxbre.ie;
	using org.nxbre.ie.adapters;
	using org.nxbre.ie.core;
	using org.nxbre.ie.predicates;
	using org.nxbre.ie.rule;
	
	using org.nxbre.util;
	
	[TestFixture]
	public class TestBinder {
	
		private string ruleFilesFolder;
		private int deducted;

		protected void ShowAllNewFacts(NewFactEventArgs nfea) 
	  {
			deducted++;
			Console.WriteLine("NxBRE Deducted: {0}", nfea.Fact);
	  }

		protected void ShowAllLogs(object obj, ILogEvent aLog)
		{
			Console.WriteLine("NxBRE Log " + aLog.Priority + " : " + aLog.Message);
		}

		public class Character {
			private string name;
			private string sentence;
			private string education;
			
			public string Name {
				get {
					return name;
				}
				set {
					name = value;
				}
			}
			
			public string Sentence {
				get {
					return sentence;
				}
				set {
					sentence = value;
				}
			}
			
			public string Education {
				get {
					return education;
				}
				set {
					education = value;
				}
			}
			
			public Character(string name, string sentence)
			{
				Name = name;
				Sentence = sentence;
				Education = String.Empty;
			}
			
			public override string ToString() {
				return Name;
			}
		}
	
		public TestBinder() {
			ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
		}

		[Test][ExpectedException(typeof(BREException))]
		public void TestErrorProcessBO() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "test-0_9.ruleml", FileAccess.Read));
			ie.Process(new Hashtable());
		}
		
		/// <summary>
		/// This Example is very important as it demonstrate a "correctly wrong" behaviour
		/// of the binder: because the different characters to evaluate politness are asserted
		/// by a BeforeAfter binder, they are all grouped in the same process batch, therefore
		/// the mutex (and precondition locks) are maintained, which prevents the correct deduction
		/// of mundane education.
		/// </summary>
		[Test]
		public void TestBeforeAfterFlowEngineBinder() {
			IInferenceEngine ie = new IEImpl(new FlowEngineBinder(ruleFilesFolder + "testbinder1.ruleml.xbre",
			                       											BindingTypes.BeforeAfter));
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "testbinder.ruleml",
			                                           FileAccess.Read));
			
			ie.NewFactHandler += new NewFactEvent(ShowAllNewFacts);

			Hashtable bo = new Hashtable();
			bo.Add("THEDUKE", new Character("The Duke", "hello world"));
			bo.Add("BOBBYBOB", new Character("Bobby Bob", "what the?"));
			bo.Add("JOHNQDOE", new Character("John Q. Doe", "hello, who am i?"));
			bo.Add("DANNYDAN", new Character("Danny Dan", "get out of my world"));
			
			ie.Process(bo);
			
			QueryResultSet qrs = ie.RunQuery("all polite");
			Assert.AreEqual(2, qrs.Count, "polite Count");
			
			// here, we should have got one result (Danny Dan), but the politness of The Duke has
			// mutexed the "mundane" implication.
			qrs = ie.RunQuery("all mundane");
			Assert.AreEqual(0, qrs.Count, "mundane Count");
		}
		
		[Test]
		public void TestAfterBinderRetraction() {
			Character theDuke = new Character("The Duke", "hello world");
			Hashtable bo = new Hashtable();
			bo.Add("THEDUKE", theDuke);	
			
			// first test to confirm that a pre-existing fact is not rededucted
			IInferenceEngine ie = new IEImpl(new FlowEngineBinder(ruleFilesFolder + "testbinder3.ruleml.xbre",
			                                                      BindingTypes.BeforeAfter));
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "testbinder.ruleml",
			                                           		FileAccess.Read));
			
			ie.Assert(new Fact("retract_target", "education", new Individual(theDuke), new Individual("polite")));
			ie.NewFactHandler += new NewFactEvent(ShowAllNewFacts);
			deducted = 0;
			ie.Process(bo);
			Assert.AreEqual(0, deducted, "The Duke was already polite!");
			
			// second test to confirm that retracting the fact in the after binder restarts inference
			ie = new IEImpl(new FlowEngineBinder(ruleFilesFolder + "testbinder4.ruleml.xbre",
			                       							 BindingTypes.BeforeAfter));
			
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "testbinder.ruleml", FileAccess.Read));
			
			ie.Assert(new Fact("retract_target", "education", new Individual(theDuke), new Individual("polite")));
			ie.NewFactHandler += new NewFactEvent(ShowAllNewFacts);
			deducted = 0;
			ie.Process(bo);
			Assert.AreEqual(1, deducted, "The Duke was re-deducted polite!");
		}

		[Test]
		public void TestControlFlowEngineBinder() {
			IInferenceEngine ie = new IEImpl(new FlowEngineBinder(ruleFilesFolder + "testbinder2.ruleml.xbre",
			                       											BindingTypes.Control));

			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "testbinder.ruleml",
			                                           FileAccess.Read));
			
			ie.NewFactHandler += new NewFactEvent(ShowAllNewFacts);

			Character theDuke = new Character("The Duke", "hello world");
			Character bobbyBob = new Character("Bobby Bob", "what the?");
			Character johnQDoe = new Character("John Q. Doe", "hello, who am i?");
			Character dannyDan = new Character("Danny Dan", "get out of my world");

			Hashtable bo = new Hashtable();
			ArrayList al = new ArrayList();
			al.Add(theDuke);
			al.Add(bobbyBob);
			al.Add(johnQDoe);
			al.Add(dannyDan);
			bo.Add("CHARACTERS", al);
			ie.Process(bo);

			// the binder uses isolated memory for each character,
			// so we will not run the queries but check the objects directly
			Assert.AreEqual("polite", theDuke.Education, "theDuke");
			Assert.AreEqual(String.Empty, bobbyBob.Education, "bobbyBob");
			Assert.AreEqual("polite", johnQDoe.Education, "johnQDoe");
			Assert.AreEqual("mundane", dannyDan.Education, "dannyDan");
		}
		
		private void RunTestBinderEvents(IInferenceEngine ie) {
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleFilesFolder + "events-test.ruleml",
		                                           	 FileAccess.Read));

			//ie.LogHandlers += new DispatchLog(ShowAllLogs);
      Hashtable bo = new Hashtable();
      bo.Add("ASSERTED", new ArrayList());
      bo.Add("RETRACTED", new ArrayList());
      bo.Add("MODIFIED", new ArrayList());
			
      ie.Process(bo);
			
			Assert.AreEqual(1, ((ArrayList)bo["ASSERTED"]).Count, "Count Asserted");
			Assert.AreEqual("toAssert{whatever}", ((ArrayList)bo["ASSERTED"])[0].ToString(), "Asserted Right");
			
			Assert.AreEqual(1, ((ArrayList)bo["RETRACTED"]).Count, "Count Retracted");
			Assert.AreEqual("toRetract{whatever}", ((ArrayList)bo["RETRACTED"])[0].ToString(), "Retracted Right");
			
			Assert.AreEqual(2, ((ArrayList)bo["MODIFIED"]).Count, "Count Modified");
			Assert.AreEqual("toModify{whatever}", ((ArrayList)bo["MODIFIED"])[0].ToString(), "Modified From Right");
			Assert.AreEqual("toModify{done}", ((ArrayList)bo["MODIFIED"])[1].ToString(), "Modified To Right");
		}
		
		[Test]
		public void TestBinderEventsCCB() {
			// use this binder load method to overcome loading problems because 
			// the ccb file is not in the same folder as NxBRE.dll
			using (StreamReader sr = File.OpenText(ruleFilesFolder + "events-test.ruleml.ccb")) {
	      IInferenceEngine ie = new IEImpl(CSharpBinderFactory.LoadFromString("org.nxbre.test.ie.EventTestBinder", sr.ReadToEnd()));
	      RunTestBinderEvents(ie);
			}
		}
		
		[Test]
		public void TestBinderEventsFCE() {
      IInferenceEngine ie = new IEImpl(new FlowEngineBinder(ruleFilesFolder + "events-test.ruleml.xbre",
		                       											BindingTypes.BeforeAfter));
      RunTestBinderEvents(ie);
		}
	}
	
}
