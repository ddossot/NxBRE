namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.IO;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	[TestFixture]
	public class TestEngineMisc:AbstractTestEngine {

		[Test]
		public void Discount() {
			ie.LoadRuleBase(new RuleML08DatalogAdapter(ruleFilesFolder + "discount.ruleml", FileAccess.Read));

			Process();
			Assert.AreEqual(3, deducted, "(1) Deducted");
			Assert.AreEqual(6, ie.FactsCount, "(1) Total Facts Count");
	
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
																				"discount{Peter Miller,Porsche,7.5 percent}"};
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("discount", new Variable("customer"),
												                              				 new Variable("product"),
												                              				 new Variable("amount")))));
			Assert.AreEqual(2, qrs.Count, "(1) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(1) Query Results");

			// Fact 4 - this is a bummer
			Fact fact4 = new Fact("bummer",
                            "spending",
                            new Individual("John Q. Doe"),
               						  new Individual("min 5000 euro"),
               						  new Individual("previous year"),
               						  new Individual("current year"));
			ie.Assert(fact4);
			
			// Fact 5
			ie.Assert(new Fact("spending",
			                    new Individual("Jean Dupont"),
	             						new Individual("min 5000 euro"),
	             						new Individual("previous year")));
	
			Process();
			
			Assert.AreEqual(3, deducted, "(2) Deducted");
			Assert.AreEqual(11, ie.FactsCount, "(2) Total Facts Count");
			
			// Run anonymous (not named) query
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
																				"discount{Peter Miller,Porsche,7.5 percent}",
																				"discount{Jean Dupont,Honda,5.0 percent}",
																				"discount{Jean Dupont,Porsche,7.5 percent}"};
			qrs = ie.RunQuery(0);
			Assert.AreEqual(4, qrs.Count, "(2) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(2) Query Results");
		}
		
		[Test]
		public void DiscountVisio2003() {
			CommonDiscountVisio2003(new Visio2003Adapter(ruleFilesFolder + "discount.vdx", FileAccess.Read));
		}
		
		[Test]
		public void DiscountStrictVisio2003() {
			CommonDiscountVisio2003(new Visio2003Adapter(ruleFilesFolder + "discount_spoiled.vdx", FileAccess.Read, true));
		}

		private void CommonDiscountVisio2003(IRuleBaseAdapter rba) {
			ie.LoadRuleBase(rba);
			Process();
			Assert.AreEqual(3, deducted, "(1) Deducted");
			Assert.AreEqual(6, ie.FactsCount, "(2) Total Facts Count");
	
			deductionsToCheck = new string[] {"Discount{Peter Miller,Honda,5.0}",
																				"Discount{Peter Miller,Porsche,7.5}"};
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("Discount", new Variable("customer"),
												                              				 new Variable("product"),
												                              				 new Variable("amount")))));
			
			Assert.AreEqual(2, qrs.Count, "(1) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(1) Query Results");

			// Spending JQDoe
			Assert.IsTrue(ie.Assert(new Fact("Spending",
		                          new Individual("John Q. Doe"),
		             						  new Individual(2004),
		             						  new Individual(123.45f))),
		             		"jqdoeSpending asserted");
			
			Fact jqdoePremiumRating = new Fact("Customer Rating",
			                                   new Individual("John Q. Doe"),
			                                   new Individual("Premium"));
			
			Assert.IsTrue(ie.Assert(jqdoePremiumRating), "jqdoePremiumRating asserted");
			
			// SpendingJDupont
			Assert.IsTrue(ie.Assert(new Fact("Spending",
					                    new Individual("Jean Dupont"),
			             						new Individual(2004),
			             						new Individual(3245.25f))),
		             		"jdupontSpending asserted");
	
			Process();
			
			Assert.IsFalse(ie.FactExists(jqdoePremiumRating), "jqdoePremiumRating was retracted");
			
			Assert.AreEqual(4, deducted, "(2) Deducted");
			Assert.AreEqual(12, ie.FactsCount, "(2) Total Facts Count");
			
			// Run named queries
			deductionsToCheck = new string[] {"Discount{Peter Miller,Honda,5.0}",
																				"Discount{Peter Miller,Porsche,7.5}",
																				"Discount{Jean Dupont,Honda,5.0}",
																				"Discount{Jean Dupont,Porsche,7.5}"};
			qrs = ie.RunQuery("Calculated Discounts");
			Assert.AreEqual(4, qrs.Count, "(2) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(2) Query Results");
			
			deductionsToCheck = new string[] {"Customer Rating{Peter Miller,Premium}",
																				"Customer Rating{John Q. Doe,Regular}",
																				"Customer Rating{Jean Dupont,Premium}"};
			qrs = ie.RunQuery("Customer Ratings");
			Assert.AreEqual(3, qrs.Count, "(3) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(3) Query Results");
		}
		
		[Test]
		public void DiscountVisio2003Pages() {
			deductionsToCheck = new string[]{"Customer Rating{Peter Miller,Premium}"};
	  	NewFactEvent honf = new NewFactEvent(HandleOrderedNewFact);
	  	ie.NewFactHandler += honf;

			ie.LoadRuleBase(new Visio2003Adapter(ruleFilesFolder + "discount.vdx",
			                                     FileAccess.Read,
			                                     "Customer Rules",
			                                     "Customer Data"));
			Process();
			Assert.AreEqual(1, deducted, "(1) Deducted");
			Assert.AreEqual(2, ie.FactsCount, "(2) Total Facts Count");
			Assert.IsFalse(wrongDeduction, "(3) Query Results");

	  	ie.NewFactHandler -= honf;
	  	deductionsToCheck = null;
		}
		
		[Test]
		public void DiscountLabelized() {
			ie.LoadRuleBase(new RuleML08DatalogAdapter(ruleFilesFolder + "discount_lab.ruleml", FileAccess.Read));
			Assert.AreEqual("forward", ie.Direction, "Direction");
			Assert.AreEqual("Discount Knowledge Base", ie.Label, "Label");
			Assert.AreEqual(3, ie.FactsCount, "Facts Count");
			Assert.AreEqual(1, ie.QueriesCount, "Queries Count");
			IList<string> queryLabels = ie.QueryLabels;
			Assert.AreEqual(1, queryLabels.Count, "Query Labels");
			Assert.AreEqual(3, ie.ImplicationsCount, "Implications Count");

			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			Process();
			Assert.AreEqual(3, deducted, "(1) Deducted, Isolated");
			qrs = ie.RunQuery("Calculated Discounts");
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
																				"discount{Peter Miller,Porsche,7.5 percent}"};
			Assert.AreEqual(2, qrs.Count, "(1) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(1) Query Results");
			ie.Assert(new Fact("spending",
			                    new Individual("Jean Dupont"),
	             						new Individual("min 5000 euro"),
	             						new Individual("previous year")));
			Process();
			Assert.AreEqual(3, deducted, "(1bis) Deducted, Isolated");

			// Same facts should be rededucted as memory was previously isolated
			ie.NewWorkingMemory(WorkingMemoryTypes.Global);
			Process();
			Assert.AreEqual(3, deducted, "(2) Deducted, Shared");
			qrs = ie.RunQuery("Calculated Discounts");
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
																				"discount{Peter Miller,Porsche,7.5 percent}"};
			Assert.AreEqual(2, qrs.Count, "(2) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(2) Query Results");

			// No new fact should be found (there memory type does not matter)
			Process();
			Assert.AreEqual(0, deducted, "(3) Deducted");

			qrs = ie.RunQuery("Calculated Discounts");
			deductionsToCheck = new string[] {"discount{Peter Miller,Honda,5.0 percent}",
																				"discount{Peter Miller,Porsche,7.5 percent}"};
			Assert.AreEqual(2, qrs.Count, "(3) Query Result Size");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(3) Query Results");
		}


		[Test][ExpectedException(typeof(BREException))]
		public void EndlessLoopException() {
			ie = new IEImpl();
			ie.LoadRuleBase(new RuleML086DatalogAdapter(ruleFilesFolder + "endlessloop.ruleml", FileAccess.Read));
			// Processing should reach iteration limit and throw a BREexception
			ie.Process();
			Assert.Fail("Should never reach me!");
		}
		

		[Test]
		public void NafSupportRuleML() {
			PerformNafSupport(new RuleML086NafDatalogAdapter(ruleFilesFolder + "fire-alarm.ruleml",
			                                               	 FileAccess.Read));
		}
	
		[Test]
		public void NafSupportVisio2003() {
			PerformNafSupport(new Visio2003Adapter(ruleFilesFolder + "fire-alarm.vdx",
			                                       FileAccess.Read));
		}
	
		private void PerformNafSupport(IRuleBaseAdapter irba) {
			ie.LoadRuleBase(irba);
			
			deductionsToCheck = new string[] {"Detector In Room{A102}", "Detector In Room{A100}",
																				"Firemen In Room{A102}"};
			qrs = ie.RunQuery("Safe Room List");
			Assert.AreEqual(3, qrs.Count, "(1) Safe Room List: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(1) Safe Room List Deductions OK");
			
			deductionsToCheck = new string[] {"Alarm Fault In Room{Smoke Detected,A100}",
																				"Safe Room{A102}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			Process();
			Assert.AreEqual(2, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");
	  	ie.NewFactHandler -= henf;

			deductionsToCheck = new string[] {"Detector In Room{A102}", "Firemen In Room{A102}"};
			qrs = ie.RunQuery("Safe Room List");
			Assert.AreEqual(2, qrs.Count, "(2) Safe Room List: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "(2) Safe Room List Deductions OK");

			deductionsToCheck = null;
		}
	
		[Test]
		public void CountingImpFunctionRelSupportFEB() {
      InitIE(new FlowEngineBinder(ruleFilesFolder + "exams.ruleml.xbre",
			                       			BindingTypes.BeforeAfter));
      
      PerformCountingImpFunctionRelSupport(new RuleML086NafDatalogAdapter(ruleFilesFolder + "exams.ruleml",
			                                               											FileAccess.Read),
			                                     true);
		}
	
		[Test]
		public void CountingImpFunctionRelSupportFEBVisio2003() {
      InitIE(new FlowEngineBinder(ruleFilesFolder + "exams.ruleml.xbre",
			                       			BindingTypes.BeforeAfter));
      
      PerformCountingImpFunctionRelSupport(new Visio2003Adapter(ruleFilesFolder + "exams.vdx",
			                                               						FileAccess.Read),
			                                     true);
		}

		[Test]
		public void CountingImpFunctionRelSupportCCB() {
			// use this binder load method to overcome loading problems because 
			// the ccb file is not in the same folder as NxBRE.dll
			using (StreamReader sr = File.OpenText(ruleFilesFolder + "exams.ruleml.ccb"))
	      InitIE(CSharpBinderFactory.LoadFromString("NxBRE.Examples.ExamsBinder", sr.ReadToEnd()));
			
      PerformCountingImpFunctionRelSupport(new RuleML086NafDatalogAdapter(ruleFilesFolder + "exams.ruleml",
			                                               											FileAccess.Read),
			                                     true);
		}

		[Test]
		public void CountingImpFunctionExpression() {
	   	InitIE();
			
      PerformCountingImpFunctionRelSupport(new RuleML086NafDatalogAdapter(ruleFilesFolder + "exams-binderless.ruleml",
			                                               											FileAccess.Read),
			                                     false);
		}

    private void PerformCountingImpFunctionRelSupport(IRuleBaseAdapter irba, bool withBinder) {
			ie.LoadRuleBase(irba);
			
			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			deductionsToCheck = new string[] {"Result{Physics,Passed}", "Result{Poetry,Passed}",
																				"Result Count{2,Passed}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	Process(withBinder?new Hashtable():null);
			Assert.AreEqual(3, deducted, "(1) Deducted");
			Assert.IsFalse(wrongDeduction, "(1) Deductions OK");
	  	ie.NewFactHandler -= henf;
			
			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			// let's turn maths failure into success
			ie.Retract("Maths Score");
			ie.Assert(new Fact("Score", new Individual("Maths"), new Individual(88)));
			deductionsToCheck = new string[] {"Result{Physics,Passed}", "Result{Poetry,Passed}",
																				"Result{Maths,Passed}", "Result Count{3,Passed}",
																				"Graduation Success{}"};
	  	henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			Process(withBinder?new Hashtable():null);
			Assert.AreEqual(5, deducted, "(2) Deducted");
			Assert.IsFalse(wrongDeduction, "(2) Deductions OK");
	  	ie.NewFactHandler -= henf;

			deductionsToCheck = null;
		}
		
		[Test]
		public void ChocolateBox() {
			PerformChocolateBoxTwiddling(new RuleML09NafDatalogAdapter(ruleFilesFolder + "chocolatebox-binderless.ruleml",
			                                                           FileAccess.Read),
			                             false);
		}

		[Test]
		public void ChocolateBoxCCB() {
			// use this binder load method to overcome loading problems because 
			// the ccb file is not in the same folder as NxBRE.dll
			using (StreamReader sr = File.OpenText(ruleFilesFolder + "chocolatebox.ruleml.ccb"))
	      InitIE(CSharpBinderFactory.LoadFromString("NxBRE.Test.InferenceEngine.ChocolateBoxBinder", sr.ReadToEnd()));

			PerformChocolateBoxTwiddling(new RuleML09NafDatalogAdapter(ruleFilesFolder + "chocolatebox.ruleml",
			                                                           FileAccess.Read),
			                             true);
		}

		[Test]
		public void ChocolateBoxFEB() {
      InitIE(new FlowEngineBinder(ruleFilesFolder + "chocolatebox.ruleml.xbre",
			                       			BindingTypes.BeforeAfter));
			
			PerformChocolateBoxTwiddling(new RuleML09NafDatalogAdapter(ruleFilesFolder + "chocolatebox.ruleml",
			                                                           FileAccess.Read),
			                             true);
		}
		
		private void PerformChocolateBoxTwiddling(IRuleBaseAdapter irba, bool withBinder) {
			ie.LoadRuleBase(irba);
			
			Process(withBinder?new Hashtable():null);
			Assert.AreEqual(0, deducted, "Deducted");
			Assert.AreEqual(0, deleted, "Deleted");
			Assert.AreEqual(2, modified, "Modified");
			
			Assert.AreEqual("Chocolate_Box_Weight{MyBox,2.5}", ie.GetFact("Total Weight").ToString(), "Total Weight OK");
		}

	}
}
