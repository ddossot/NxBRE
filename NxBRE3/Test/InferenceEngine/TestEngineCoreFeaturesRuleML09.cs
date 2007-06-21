namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.IO;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	[TestFixture]
	public class TestEngineCoreFeaturesRuleML09:AbstractTestEngine {

		private class Account {
			private int id;
			private int balance;
			
			public int Id {
				get {
					return id;
				}
			}
			public int Balance {
				get {
					return balance;
				}
			}
			
			public Account(int id):this(id, 0) {}
			
			public Account(int id, int balance) {
				this.id = id;
				this.balance = balance;
			}
			public override string ToString() {
				return "ACCNT#" + id.ToString() + " $" + balance;
			}
		}
		
		protected virtual IRuleBaseAdapter NewTestAdapter() {
			return new RuleML09NafDatalogAdapter(ruleFilesFolder + "test-0_9.ruleml", FileAccess.Read);
		}	
		
		protected virtual IRuleBaseAdapter NewGedcomAdapter() {
			return new RuleML09NafDatalogAdapter(ruleFilesFolder + "gedcom-relations-0_9.ruleml", FileAccess.Read);
		}
		
		[Test]
		public void RuleBaseLabel() {
			IInferenceEngine ie = new IEImpl();
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Label.StartsWith("NxBRE"));
		}
		
		[Test]
		public void IsolatedMemoryCloning() {
			//Regression test for bug #1340799
			IInferenceEngine ie1 = new IEImpl();
			ie1.LoadRuleBase(NewTestAdapter());
			int ie1facts = ie1.FactsCount;
			
			IInferenceEngine ie2 = new IEImpl();
			ie2.LoadRuleBase(NewGedcomAdapter());
			ie2.Assert(new Fact("sex", new Individual("dad"), new Individual("m")));
			ie2.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			
			Assert.AreEqual(ie1facts, ie1.FactsCount, "FactsCount has wrongly changed");
		}
		
		[Test]
		public void IsolatedThreadSafeMemoryCloning() {
			//Regression test for bug #1340799
			IInferenceEngine ie1 = new IEImpl(ThreadingModelTypes.Multi);
			ie1.LoadRuleBase(NewTestAdapter());
			int ie1facts = ie1.FactsCount;
			
			IInferenceEngine ie2 = new IEImpl(ThreadingModelTypes.Multi);
			ie2.LoadRuleBase(NewGedcomAdapter());
			ie2.Assert(new Fact("sex", new Individual("dad"), new Individual("m")));
			ie2.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			
			Assert.AreEqual(ie1facts, ie1.FactsCount, "FactsCount has wrongly changed");
		}
			
		[Test]
		public void QueryAndSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("flag", new Individual("foo"), new Individual("andTestA")));
			ie.Assert(new Fact("flag", new Individual("bar"), new Individual("andTestA")));
			ie.Assert(new Fact("flag", new Individual("foo"), new Individual("andTestB")));
			ie.Assert(new Fact("flag", new Individual("rab"), new Individual("andTestB")));
			ie.Assert(new Fact("flag", new Individual("turnip"), new Individual("andTestC")));
			ie.Assert(new Fact("flag", new Individual("kilroy"), new Individual("andTestC")));
			
			deductionsToCheck = new string[] {"flag{foo,andTestA}", "flag{foo,andTestB}"};
			qrs = ie.RunQuery("Query A and B");
			
			ParseResult();
			Assert.AreEqual(1, qrs.Count, "Query A and B: Count");
			Assert.IsFalse(wrongDeduction, "Query A and B Deductions OK");
			
			deductionsToCheck = new string[] {"flag{foo,andTestA}", "flag{foo,andTestB}", "flag{turnip,andTestC}",
																				"flag{foo,andTestA}", "flag{foo,andTestB}", "flag{kilroy,andTestC}"};
			qrs = ie.RunQuery("Query A and B and C");
			Assert.AreEqual(2, qrs.Count, "Query A and B and C: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "Query A and B and C Deductions OK");
		}
		
		[Test]
		public virtual void QueryVarLessSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("varless.value", new Individual("A")));
			ie.Assert(new Fact("varless.value", new Individual("E")));
			ie.Assert(new Fact("varless.value", new Individual("F")));
			
			deductionsToCheck = new string[] {"varless.value{E}", "varless.value{F}"};
			qrs = ie.RunQuery("One Atom Var-less");
			Assert.AreEqual(2, qrs.Count, "One Atom Var-less: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "One Atom Var-less Deductions OK");
			
			deductionsToCheck = new string[] {"varless.value{A}", "varless.value{F}",
																				"varless.value{A}", "varless.value{E}",
																				"varless.value{E}", "varless.value{F}",
																				"varless.value{E}", "varless.value{E}",
																				"varless.value{F}", "varless.value{F}",
																				"varless.value{F}", "varless.value{E}"};
			qrs = ie.RunQuery("Two Atoms Var-less");
			Assert.AreEqual(6, qrs.Count, "Two Atoms Var-less: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "Two Atoms Var-less Deductions OK");

			qrs = ie.RunQuery("Two Atoms");
			Assert.AreEqual(6, qrs.Count, "Two Atoms: Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "Two Atoms Deductions OK");
		}
		
		[Test]
		public virtual void ImplicationVarLessSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("varless.value", new Individual("A")));
			ie.Assert(new Fact("varless.value", new Individual("E")));
			ie.Assert(new Fact("varless.value", new Individual("F")));

			deductionsToCheck = new string[]{"first.deduction{correct}", "third.deduction{correct}", "forth.deduction{correct}"};
	  	NewFactEvent honf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += honf;

	  	Process();

	  	Assert.AreEqual(3, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");

			ie.NewFactHandler -= honf;
		}
			
	
		[Test]
		public void ImplicationLogicalSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("andTestA")));
			ie.Assert(new Fact("flag", new Individual("other"), new Individual("andTestA")));
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");
			
			deductionsToCheck = new string[] {"testAandB{probe}", "testBorC{probe}", "testBorC{other}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("andTestB")));
			ie.Assert(new Fact("flag", new Individual("other"), new Individual("andTestC")));
			Process();
			Assert.AreEqual(3, deducted, "(2) Deducted");
			Assert.IsFalse(wrongDeduction, "(2) Wrong deduction");

	  	ie.NewFactHandler -= henf;
	  	deductionsToCheck = null;
		}
		
		[Test]
		public void PrioritySupport() {
			deductionsToCheck = new string[]{"higherImpl{probe}", "mediumImpl{probe}", "lowerImpl{probe}"};
	  	NewFactEvent honf = new NewFactEvent(HandleOrderedNewFact);
	  	ie.NewFactHandler += honf;
			
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("testPriority")));
			Process();
			Assert.AreEqual(3, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");

	  	ie.NewFactHandler -= honf;
	  	deductionsToCheck = null;
		}

		[Test]
		public void MutexSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			Fact locker = new Fact("locker", "flag", new Individual("lock"), new Individual("mutexLock"));
			ie.Assert(locker);
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredA")));
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredB")));
			Process();
			Assert.AreEqual(1, deducted, "(1) Deducted");
			
			// no garantee if A or B is deducted because they implications have same priority level
			ie.Retract(locker);
			Process();
			Assert.AreEqual(1, deducted, "(2) Deducted");
			
			// C should be deducted
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredC")));
			deductionsToCheck = new string[] {"mutexC{probe}"};
	  	NewFactEvent honf = new NewFactEvent(HandleOrderedNewFact);
	  	ie.NewFactHandler += honf;
			Process();
			Assert.AreEqual(1, deducted, "(3) Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");
	  	ie.NewFactHandler -= honf;
	  	deductionsToCheck = null;
			
			// nothing must be rededucted as  
			Process();
			Assert.AreEqual(0, deducted, "(3) Deducted");
		}

		[Test]
		public void PreconditionSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredX")));
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredY")));
			
			// Isolate memory for a first round of test - step by step
			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");

			// The fact allows the precondition rule to fire (first deduction), then Y can fire (second deduction)
			// Z will not fire because if its triggering flag is not asserted, therefore X will not fire.
			ie.Assert(new Fact("flag", new Individual("trigger"), new Individual("preconditionTrigger")));
			Process();
			Assert.AreEqual(2, deducted, "(2) Deducted");

			// This time Z and X should fire
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredZ")));
			Process();
			Assert.AreEqual(2, deducted, "(3) Deducted");

			// Come back to the global one for a second test - all in one
			ie.NewWorkingMemory(WorkingMemoryTypes.Global);
			ie.Assert(new Fact("flag", new Individual("trigger"), new Individual("preconditionTrigger")));
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggeredZ")));
			deductionsToCheck = new string[]{"preconditionTriggered{trigger}",
																			 "preconditionZ{probe}",
																			 "preconditionX{probe}",
																			 "preconditionY{probe}"};
	  	NewFactEvent honf = new NewFactEvent(HandleOrderedNewFact);
	  	ie.NewFactHandler += honf;
			Process();
			Assert.AreEqual(4, deducted, "(4) Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");
	  	ie.NewFactHandler -= honf;
	  	deductionsToCheck = null;
			
			// Another process should lead to nothng
			Process();
			Assert.AreEqual(0, deducted, "(5) Deducted");
		}

		[Test]
		public void ObjectPredicates() {
			ie.LoadRuleBase(NewTestAdapter());

			// Account ID 1234 should not be valid
			Account a1234 = new Account(1234, 25);
			ie.Assert(new Fact("balance",
			                   new Individual(a1234),
			                   new Individual("actualValue"),
			                   new Individual(a1234.Balance)));
			
			ie.Assert(new Fact("balance",
			                   new Individual(a1234),
			                   new Individual("expectedValue"),
			                   new Individual(50)));
			
			// Account ID 9876 should be valid
			Account a9876 = new Account(9876, 50);
			ie.Assert(new Fact("balance",
			                   new Individual(a9876),
			                   new Individual("actualValue"),
			                   new Individual(a9876.Balance)));
			
			ie.Assert(new Fact("balance",
			                   new Individual(a9876),
			                   new Individual("expectedValue"),
			                   new Individual(50)));
			
			deductionsToCheck = new string[] {"isValid{ACCNT#9876 $50}", "isTwentyFive{ACCNT#1234 $25}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;

			Process();
	  	
	  	ie.NewFactHandler -= henf;

			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			Assert.AreEqual(2, deducted, "Deducted");
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("isValid", new Variable("account")))));
			Assert.AreEqual(1, qrs.Count, "Query Size");
			if (qrs.Count > 0) Assert.AreEqual(a9876, qrs[0][0].Members[0].Value, "Correct Result");
		}
		
		[Test]
		public virtual void NxBREOperators() {
			NxBREOperators(true);
		}
		
		protected void NxBREOperators(bool supportRegex) {
			ie.LoadRuleBase(NewTestAdapter());
			
			Account a50 = new Account(123, 50);
			ie.Assert(new Fact("probe",
			                   new Individual(a50),
			                   new Individual(a50.Balance)));

			Account a100 = new Account(456, 100);
			ie.Assert(new Fact("probe",
			                   new Individual(a100),
			                   new Individual(a100.Balance)));

			Account a200 = new Account(789, 200);
			ie.Assert(new Fact("probe",
			                   new Individual(a200),
			                   new Individual(a200.Balance)));
			
			qrs = ie.RunQuery("Operator e");
			Assert.AreEqual(1, qrs.Count, "Operator e: Query Size");
			if (qrs.Count > 0)
				Assert.AreEqual(a100, qrs[0][0].Members[0].Value, "Operator e: Correct Result");

			qrs = ie.RunQuery("Operator gt");
			Assert.AreEqual(1, qrs.Count, "Operator gt: Query Size");
			if (qrs.Count > 0)
				Assert.AreEqual(a200, qrs[0][0].Members[0].Value, "Operator gt: Correct Result");
			
			qrs = ie.RunQuery("Operator gte");
			Assert.AreEqual(2, qrs.Count, "Operator gte: Query Size");

			qrs = ie.RunQuery("Operator lt");
			Assert.AreEqual(1, qrs.Count, "Operator lt: Query Size");
			if (qrs.Count > 0)
				Assert.AreEqual(a50, qrs[0][0].Members[0].Value, "Operator lt: Correct Result");
			
			qrs = ie.RunQuery("Operator lte");
			Assert.AreEqual(2, qrs.Count, "Operator lte: Query Size");

			qrs = ie.RunQuery("Operator ne");
			Assert.AreEqual(2, qrs.Count, "Operator ne: Query Size");
			
			if (supportRegex) {
				ie.Assert(new Fact("matchesProbe",
				                   new Individual("ok"),
				                   new Individual("19.99")));
				
				ie.Assert(new Fact("matchesProbe",
				                   new Individual("ko"),
				                   new Individual("100 USD")));
	
				qrs = ie.RunQuery("Operator matches");
				Assert.AreEqual(1, qrs.Count, "Operator matches: Query Size");
			}
		}
		
		[Test]
		public virtual void AdvancedImplicationLogicalSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Account a50 = new Account(123, 50);
			ie.Assert(new Fact("id",
			                   new Individual(a50),
			                   new Individual(a50.Id)));
			ie.Assert(new Fact("balance",
			                   new Individual(a50),
			                   new Individual(a50.Balance)));

			Account a100 = new Account(456, 100);
			ie.Assert(new Fact("id",
			                   new Individual(a100),
			                   new Individual(a100.Id)));
			ie.Assert(new Fact("balance",
			                   new Individual(a100),
			                   new Individual(a100.Balance)));

			Account a200 = new Account(789, 200);
			ie.Assert(new Fact("id",
			                   new Individual(a200),
			                   new Individual(a200.Id)));
			ie.Assert(new Fact("balance",
			                   new Individual(a200),
			                   new Individual(a200.Balance)));
			
			deductionsToCheck = new string[] {"testAandForGorAandG{ACCNT#789 $200}",
																				"testCorBandA{ACCNT#456 $100}", "testCorBandA{ACCNT#123 $50}",
																				"testAandBorC{ACCNT#123 $50}", "testAandBorC{ACCNT#456 $100}",
																				"testAorDandEandBorC{ACCNT#123 $50}",
																				"testAandForGorAandG{ACCNT#123 $50}"};
	  	
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			Process();
			Assert.AreEqual(7, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Deductions OK");
			
	  	ie.NewFactHandler -= henf;
	  	deductionsToCheck = null;

		}

		[Test][ExpectedException(typeof(BREException))]
		public void CommitNotIsolatedMemory() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.CommitIsolatedMemory();
		}
	
		[Test]
		public void CommitIsolatedMemory() {
			ie.LoadRuleBase(NewTestAdapter());
			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			
			// only one of three atoms satisfied
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryC")));
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");

			// only two of three atoms satisfied
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryA")));
			Process();
			Assert.AreEqual(0, deducted, "(2) Deducted");

			// three of three atoms satisfied -> should deduct
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryB")));
			Process();
			Assert.AreEqual(1, deducted, "(3) Deducted");
			
			int isolatedFactCounts = ie.FactsCount;
			
			ie.CommitIsolatedMemory();
			Assert.AreEqual(isolatedFactCounts, ie.FactsCount, "Facts count after commit");
		}

	
		[Test]
		public void CommitIsolatedEmptyMemory() {
			ie.LoadRuleBase(NewTestAdapter());
			int initialFactCount = ie.FactsCount;
			
			// only one of three atoms satisfied
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryC")));
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");

			Assert.AreEqual(initialFactCount + 1, ie.FactsCount, "(1) Facts count before WorkingMemoryTypes.IsolatedEmpty");
			ie.NewWorkingMemory(WorkingMemoryTypes.IsolatedEmpty);
			Assert.AreEqual(0, ie.FactsCount, "(1) Facts count after WorkingMemoryTypes.IsolatedEmpty");

			// only two of three atoms satisfied
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryA")));
			Process();
			Assert.AreEqual(0, deducted, "(2) Deducted");

			// three of three atoms satisfied -> should deduct
			ie.Assert(new Fact("flag", new Individual("probe"), new Individual("triggerQueryB")));
			Process();
			Assert.AreEqual(0, deducted, "(3) Deducted");
			
			Assert.AreEqual(2, ie.FactsCount, "(3) Facts count before WorkingMemoryTypes.IsolatedEmpty");
			ie.CommitIsolatedMemory();
			Assert.AreEqual(initialFactCount + 3, ie.FactsCount, "(3) Facts count after WorkingMemoryTypes.IsolatedEmpty");
			
			Process();
			Assert.AreEqual(1, deducted, "(4) Deducted");
		}
			
		[Test]
		public void FactExistence() {
			ie.LoadRuleBase(NewTestAdapter());
			Assert.IsTrue(ie.FactExists("Porsche Luxury"), "Existing fact label");
			Assert.IsFalse(ie.FactExists("Porsche Regular"), "Non-existing fact label");
			
			Fact factHondaRegular = new Fact("regular", new Individual("Honda"));
			Fact factHondaLuxury = new Fact("luxury", new Individual("Honda"));
			Assert.IsTrue(ie.FactExists(factHondaRegular), "Existing fact");
			Assert.IsFalse(ie.FactExists(factHondaLuxury), "Non-existing fact");
		}
		
		[Test]
		public void RetractingImplication() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Fact factToRetract = new Fact("toRetract", new Individual("easy target"));
			Assert.IsFalse(ie.FactExists(factToRetract), "1st Pre-check before target assertion");
			ie.Assert(factToRetract);
			Assert.IsTrue(ie.FactExists(factToRetract), "2nd Pre-check before target assertion");
			
			// this process should leave the target unchanged
			Process();
			Assert.IsTrue(ie.FactExists(factToRetract), "1st process: fact unchanged");
			Assert.AreEqual(0, deleted, "1st process: deleted");
			Assert.AreEqual(0, deducted, "1st process: deleted");
			
			// after asserting the trigger, process should retract the target
			Fact factRetractionTrigger = new Fact("retractionTrigger", new Individual("easy target"));
			Fact factRetractionConfirmation = new Fact("retractionConfirmation", new Individual("easy target"));
			ie.Assert(factRetractionTrigger);
			Process();
			Assert.IsFalse(ie.FactExists(factToRetract), "2nd process: fact retracted");
			Assert.IsTrue(ie.FactExists(factRetractionConfirmation), "2nd process: retracting implication positive pre-conditionned");
			Assert.AreEqual(1, deleted, "2nd process: deleted");
			Assert.AreEqual(1, deducted, "2nd process: deducted");
		}
			
		[Test]
		public void CheckNonStrictImplicationBehavior() {
			ie.LoadRuleBase(NewTestAdapter());
			Assert.IsFalse(((IEImpl)ie).strictImplication, "Ensure IE is not strict");
			
			Assert.IsTrue(ie.Assert(new Fact("flagStrictA", new Individual("something"))), "Asserted fact A");
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");
			
			Assert.IsTrue(ie.Assert(new Fact("flagStrictB", new Individual("anything"), new Individual("whatever"))), "Asserted fact B");
			deductionsToCheck = new string[] {"testStrict{anything,whatever}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			Process();
			Assert.AreEqual(1, deducted, "(2) Deducted");
			Assert.IsFalse(wrongDeduction, "(2) Wrong deduction");
	  	ie.NewFactHandler -= henf;
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void CheckStrictImplicationBehavior() {
			ie.LoadRuleBase(NewTestAdapter());
			((IEImpl)ie).strictImplication = true;
			Assert.IsTrue(((IEImpl)ie).strictImplication, "Ensure IE is strict");
			
			Assert.IsTrue(ie.Assert(new Fact("flagStrictA", new Individual("something"))), "Asserted fact A");
			Process();
		}
		
		[Test]
		public void OrNafCombo() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("CheckProductInfo", new Individual("true"))), "Asserted trigger fact");
			
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");
			
			deductionsToCheck = new string[] {"NotAllCategoriesCovered{true}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
	  	Assert.IsTrue(ie.Retract("Honda Regular"), "(2) Retracted Regular");
			Process();
			Assert.IsFalse(wrongDeduction, "(2) Wrong deduction");
			Assert.AreEqual(1, deducted, "(2) Deducted");
			
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
	  	Assert.IsTrue(ie.Retract("Porsche Luxury"), "(3) Retracted Luxury");
			Process();
			Assert.IsFalse(wrongDeduction, "(3) Wrong deduction");
			Assert.AreEqual(1, deducted, "(3) Deducted");
			
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
	  	Assert.IsTrue(ie.Retract("Honda Regular"), "(4) Retracted Regular");
	  	Assert.IsTrue(ie.Retract("Porsche Luxury"), "(4) Retracted Luxury");
			Process();
			Assert.IsFalse(wrongDeduction, "(4) Wrong deduction");
			Assert.AreEqual(1, deducted, "(4) Deducted");
			
	  	ie.NewFactHandler -= henf;
		}
		
		[Test]
		public void Gedcom() {
			deductionsToCheck = new string[]{
				"sibling{Girl,Boy}", "sibling{Boy,Girl}", "parent{Girl,Daddy}",
				"parent{Girl,Mummy}", "parent{Boy,Daddy}", "parent{Boy,Mummy}", "ancestor{Girl,Daddy}",
				"ancestor{Girl,Mummy}", "ancestor{Boy,Daddy}", "ancestor{Boy,Mummy}", "descendent{Daddy,Girl}",
				"descendent{Mummy,Girl}", "descendent{Daddy,Boy}", "descendent{Mummy,Boy}", "child{Daddy,Girl}",
				"child{Mummy,Girl}", "child{Daddy,Boy}", "child{Mummy,Boy}", "spouse{Daddy,Mummy}", "spouse{Mummy,Daddy}",
				"wife{Daddy,Mummy}", "daughter{Daddy,Girl}", "daughter{Mummy,Girl}", "mother{Girl,Mummy}",
				"mother{Boy,Mummy}", "sister{Boy,Girl}", "son{Daddy,Boy}", "son{Mummy,Boy}", "father{Girl,Daddy}",
				"father{Boy,Daddy}", "brother{Girl,Boy}", "husband{Mummy,Daddy}", "ancestor{Daddy,Old'Pa}",
				"descendent{Old'Pa,Daddy}", "ancestor{Girl,Old'Pa}", "ancestor{Boy,Old'Pa}", "descendent{Old'Pa,Girl}",
				"descendent{Old'Pa,Boy}", "grandparent{Girl,Old'Pa}", "grandparent{Boy,Old'Pa}", "granddaughter{Girl,Old'Pa}",
				"grandchild{Old'Pa,Girl}", "grandchild{Old'Pa,Boy}", "child{Old'Pa,Daddy}", "grandson{Old'Pa,Boy}",
				"son{Old'Pa,Daddy}", "grandfather{Girl,Old'Pa}", "grandfather{Boy,Old'Pa}", "father{Daddy,Old'Pa}",
				"ancestor{Uncle,Old'Pa}", "sibling{Daddy,Uncle}", "father{Uncle,Old'Pa}", "child{Old'Pa,Uncle}",
				"brother{Uncle,Daddy}", "brother{Daddy,Uncle}", "descendent{Old'Pa,Uncle}", "niece{Uncle,Girl}",
				"nephew{Uncle,Boy}", "son{Old'Pa,Uncle}", "uncle{Girl,Uncle}", "uncle{Boy,Uncle}",
				"firstCousin{Girl,Cousin}", "firstCousin{Boy,Cousin}", "firstCousin{Cousin,Girl}", "firstCousin{Cousin,Boy}",
				"ancestor{Cousin,Uncle}", "ancestor{Cousin,Old'Pa}", "father{Cousin,Uncle}", "child{Uncle,Cousin}",
				"grandparent{Cousin,Old'Pa}", "uncle{Cousin,Daddy}", "descendent{Uncle,Cousin}", "descendent{Old'Pa,Cousin}",
				"aunt{Cousin,Mummy}", "grandchild{Old'Pa,Cousin}", "cousin{Cousin,Girl}", "cousin{Cousin,Boy}",
				"cousin{Girl,Cousin}", "cousin{Boy,Cousin}", "grandfather{Cousin,Old'Pa}", "daughter{Uncle,Cousin}",
				"granddaughter{Cousin,Old'Pa}", "niece{Daddy,Cousin}"
			};

	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
			
	  	ie.LoadRuleBase(NewGedcomAdapter());
			Individual male = new Individual("M");
			Individual female = new Individual("F");
			Individual happyFamily = new Individual("Happy");
			Individual daddy = new Individual("Daddy");
			Individual mummy = new Individual("Mummy");
			Individual girl = new Individual("Girl");
			Individual boy = new Individual("Boy");
			
			ie.Assert(new Fact("sex", daddy, male));
			ie.Assert(new Fact("sex", mummy, female));
			ie.Assert(new Fact("sex", girl, female));
			ie.Assert(new Fact("sex", boy, male));
			
			ie.Assert(new Fact("spouseIn", daddy, happyFamily));
			ie.Assert(new Fact("spouseIn", mummy, happyFamily));
			ie.Assert(new Fact("childIn", girl, happyFamily));
			ie.Assert(new Fact("childIn", boy, happyFamily));
			Process();
			
			Assert.AreEqual(32, deducted, "(1) Deducted");
			Assert.IsFalse(wrongDeduction, "(1) Deductions OK");

			Individual oldpa = new Individual("Old'Pa");
			ie.Assert(new Fact("sex", oldpa, male));
			ie.Assert(new Fact("parent", daddy, oldpa));
			Process();

			Assert.AreEqual(17, deducted, "(2) Deducted");
			Assert.IsFalse(wrongDeduction, "(2) Deductions OK");

			Individual uncle = new Individual("Uncle");
			ie.Assert(new Fact("sex", uncle, male));
			ie.Assert(new Fact("parent", uncle, oldpa));
			ie.Assert(new Fact("sibling", uncle, daddy));
			Process();
			
			Assert.AreEqual(12, deducted, "(3) Deducted");
			Assert.IsFalse(wrongDeduction, "(3) Deductions OK");

			Individual cousin = new Individual("Cousin");
			ie.Assert(new Fact("sex", cousin, female));
			ie.Assert(new Fact("parent", cousin, uncle));
			Process();
			
			Assert.AreEqual(22, deducted, "(4) Deducted");
			Assert.IsFalse(wrongDeduction, "(4) Deductions OK");

			ie.NewFactHandler -= henf;
	  	deductionsToCheck = null;
		}

		[Test]
		public virtual void QueryWithExpressionInAtomRelation() {
			ie.LoadRuleBase(NewTestAdapter());

			DateTime doy = DateTime.Now.AddDays(-1d);
			Assert.IsTrue(ie.Assert(new Fact("Contract Details",
			                                 new Individual("CoY"),
			                                 new Individual(doy))),
			              "Asserted 'CoY' Contract Details");

			Assert.IsTrue(ie.Assert(new Fact("Contract Details",
			                                 new Individual("CoT"),
			                                 new Individual(DateTime.Now.AddDays(1d)))),
			              "Asserted 'CoT' Contract Details");
			
			deductionsToCheck = new string[] {"Contract Details{CoY," + doy.ToString() + "}"};
			qrs = ie.RunQuery("Expression in Atom Relation");
			Assert.AreEqual(1, qrs.Count, "Query Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "Query Result");
		}

		[Test]
		public virtual void QueryWithExpressionInIndividualEvaluation() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("talking",
			                                 new Individual("The Duke"),
			                                 new Individual("hello world"))), "Asserted The Duke Details");
			
			Assert.IsTrue(ie.Assert(new Fact("talking",
			                                 new Individual("Bobby Bob"),
			                                 new Individual("what the?"))), "Asserted Bobby Bob Details");
			
			deductionsToCheck = new string[] {"talking{The Duke,hello world}"};
			qrs = ie.RunQuery("Expression in Individual Evaluation");
			Assert.AreEqual(1, qrs.Count, "Query Count");
			ParseResult();
			Assert.IsFalse(wrongDeduction, "Query Result");
		}
		
		[Test]
		public void ImplicationModifySupport() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("modifyTrigger",
			                                 new Individual("foo"),
			                                 new Individual("bar"))), "Asserted trigger fact");
			
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			Process();
			Assert.AreEqual(0, deducted, "(1) Deducted");
			
			deductionsToCheck = new string[] {"modifyTarget{bar,after}"};
	  	
	  	// test with non labeled fact
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
	  	
			Fact toModify = new Fact("modifyTarget",
                               new Individual("bar"),
                               new Individual("before"));
			
			Assert.IsFalse(ie.FactExists(toModify), "(2) Target fact not present");
			Assert.IsTrue(ie.Assert(toModify), "(2) Asserted target fact");
			Assert.IsTrue(ie.FactExists(toModify), "(2) Target fact present");
			int initialFactsCount = ie.FactsCount;
			Process();
			Assert.AreEqual(initialFactsCount, ie.FactsCount, "(2) Stable FactsCount");
			Assert.AreEqual(0, deducted, "(2) Deducted");
			Assert.AreEqual(0, deleted, "(2) Deleted");
			Assert.AreEqual(1, modified, "(2) Modified");
			Assert.IsFalse(ie.FactExists(toModify), "(2) Target fact retracted");
			
	  	// test with labeled fact
	  	ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
	  	
	  	string label = "label of modifyTarget";
			toModify = new Fact(label,
                          "modifyTarget",
                          new Individual("bar"),
                          new Individual("before"));
			
			Assert.IsFalse(ie.FactExists(toModify), "(3) Target fact not present");
			Assert.IsFalse(ie.FactExists(label), "(3) Target fact label not present");
			Assert.IsTrue(ie.Assert(toModify), "(3) Asserted target fact");
			Assert.IsTrue(ie.FactExists(toModify), "(3) Target fact present");
			Assert.IsTrue(ie.FactExists(label), "(3) Target fact label present");
			initialFactsCount = ie.FactsCount;
			Process();
			Assert.AreEqual(initialFactsCount, ie.FactsCount, "(3) Stable FactsCount");
			Assert.AreEqual(0, deducted, "(3) Deducted");
			Assert.AreEqual(0, deleted, "(3) Deleted");
			Assert.AreEqual(1, modified, "(3) Modified");
			Assert.IsFalse(ie.FactExists(toModify), "(3) Target fact retracted");
			Assert.IsTrue(ie.FactExists(label), "(3) Target fact label present");
		}

		[Test]
		public virtual void ExpressionAssertion(){
			ie.LoadRuleBase(NewTestAdapter());
			
			int a = 3;
			int b = 2;
			int result = a+5*b;
			
			Assert.IsTrue(ie.Assert(new Fact("expressionAssertionTrigger",
			                                 new Individual(a),
			                                 new Individual(b))), "Asserted trigger fact");
			
			
			deductionsToCheck = new string[] {"expressionAssertionResults{" + result + ",done}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;

			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("expressionAssertionResults", new Variable("result"), new Individual("done")))));
			Assert.AreEqual(1, qrs.Count, "Query Size");
			if (qrs.Count > 0) Assert.AreEqual(result, qrs[0][0].Members[0].Value, "Correct Typed Result");
		}

		[Test]
		public virtual void ExpressionModification(){
			ie.LoadRuleBase(NewTestAdapter());
		
			int expectedModifiedCount = 0;
			
			String referenceA = "A1234";
			DateTime subscriptionDateA = DateTime.Parse("1998/03/12");
			int periodInMonthsA = 18;
			DateTime nextRenewalDateA = subscriptionDateA;
			while(nextRenewalDateA < DateTime.Now) {
				nextRenewalDateA = nextRenewalDateA.AddMonths(periodInMonthsA);
				expectedModifiedCount++;
			}
			
			Assert.IsTrue(ie.Assert(new Fact("Contract Details",
			                                 new Individual(referenceA),
			                                 new Individual(subscriptionDateA),
			                                 new Individual(periodInMonthsA))), "Asserted 'A' Contract Details");
	  	
			String referenceB = "B9876";
			DateTime subscriptionDateB = DateTime.Parse("1948/03/12");
			int periodInMonthsB = 36;
			DateTime nextRenewalDateB = subscriptionDateB;
			while(nextRenewalDateB < DateTime.Now) {
				nextRenewalDateB = nextRenewalDateB.AddMonths(periodInMonthsB);
				expectedModifiedCount++;
			}
			
			Assert.IsTrue(ie.Assert(new Fact("Contract Details",
			                                 new Individual(referenceB),
			                                 new Individual(subscriptionDateB),
			                                 new Individual(periodInMonthsB))), "Asserted 'B' Contract Details");
	  	
			Process();
			Assert.AreEqual(0, deducted, "Deducted");
			Assert.AreEqual(0, deleted, "Deleted");
			Assert.AreEqual(expectedModifiedCount, modified, "Modified");
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("Contract Details", new Individual(referenceA), new Variable("Next Renewal Date"), new Variable("Period Months")))));
			Assert.AreEqual(1, qrs.Count, "Query 'A' Size");
			if (qrs.Count > 0)
				Assert.AreEqual(nextRenewalDateA, qrs[0][0].Members[1].Value, "Correct Typed 'A' Result");
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("Contract Details", new Individual(referenceA), new Variable("Next Renewal Date"), new Variable("Period Months")))));
			Assert.AreEqual(1, qrs.Count, "Query 'B' Size");
			if (qrs.Count > 0)
				Assert.AreEqual(nextRenewalDateA, qrs[0][0].Members[1].Value, "Correct Typed 'B' Result");
			
		}

		[Test]
		public virtual void WrongMultipleFactInDataTables_Bug_1252700(){
			ie.LoadRuleBase(NewTestAdapter());
		
			Assert.IsTrue(ie.Assert(new Fact("ClientNum",
			                                 new Individual("D16.0"),
			                                 new Individual("1"),
			                                 new Individual(366))), "Asserted ClientNum '1'");
		
			Assert.IsTrue(ie.Assert(new Fact("ClientNum",
			                                 new Individual("D16.0"),
			                                 new Individual("2"),
			                                 new Individual(10000))), "Asserted ClientNum '2'");
	  	
			Assert.IsTrue(ie.Assert(new Fact("SumD16.0",
			                                 new Individual(0))), "Asserted SumD16.0");
	  	
			Process();
			
			Assert.AreEqual(1, deducted, "Deducted");
			Assert.AreEqual(0, deleted, "Deleted");
			Assert.AreEqual(2, modified, "Modified");
		}

		[Test]
		public virtual void ExpressionIndividualEvaluation(){
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("talking",
			                                 new Individual("The Duke"),
			                                 new Individual("hello world"))), "Asserted The Duke Details");
			
			Assert.IsTrue(ie.Assert(new Fact("talking",
			                                 new Individual("Bobby Bob"),
			                                 new Individual("what the?"))), "Asserted Bobby Bob Details");
			
			deductionsToCheck = new string[] {"education{The Duke,polite}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
		}
		
		[Test]
		public virtual void SimilarNafMatching() {
			// regression test for bug 1332214
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("naf-probe",
			                                 new Individual(123),
			                                 new Individual("bar"))), "Asserted naf-probe");
			
			Assert.IsTrue(ie.Assert(new Fact("naf-switch",
			                                 new Individual(123),
			                                 new Individual("pivot"),
			                                 new Individual("baz"))), "Asserted first naf-switch");
			
			Fact switchToKill = new Fact("naf-switch",
	                                 new Individual(123),
	                                 new Individual("pivot"),
	                                 new Individual("foo"));
			
			Assert.IsTrue(ie.Assert(switchToKill), "Asserted second naf-switch");
			
			Process();

			Assert.AreEqual(0, deducted, "Deducted");
			
			Assert.IsTrue(ie.Retract(switchToKill));
			
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
		}
				
		[Test]
		public virtual void AndBlockContainingOnlyOrBlocks() {
			// regression test for bug 1358781
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("dog",
			                                 new Individual("foo"),
			                                 new Individual("Black"))), "Asserted dog");
			
			Assert.IsTrue(ie.Assert(new Fact("cat",
			                                 new Individual("foo"),
			                                 new Individual("Brown"))), "Asserted cat");
			
			deductionsToCheck = new string[] {"result{foo}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
		}
				
		[Test]
		public virtual void IndividualBasedAtomCombination() {
			// regression test for bug 1346078
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("trigger-atom-combination",
			                                 new Individual(123),
			                                 new Individual("bar"))), "Asserted first trigger-atom-combination");
			
			Assert.IsTrue(ie.Assert(new Fact("trigger-atom-combination",
			                                 new Individual(456),
			                                 new Individual("bar"))), "Asserted second trigger-atom-combination");
			
			Process();

			Assert.AreEqual(2, deducted, "Deducted");
		}
				
		[Test]
		public virtual void HyperLinkIndividual() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("want to review",
			                                 new Individual("fred"),
			                                 new Individual("rule principles"))), "Asserted trigger fact");
			
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("may look at", new Variable("who"), new Variable("where")))));
			Assert.AreEqual(1, qrs.Count, "Query 'B' Size");
			
			Fact result = qrs[0][0];
			
			Assert.AreEqual("fred", result.GetPredicateValue(0));
			
			Assert.AreEqual(typeof(HyperLink), result.GetPredicateValue(1).GetType());
			
			HyperLink hl = (HyperLink) result.GetPredicateValue(1);
			Assert.AreEqual("Rule-Based Systems", hl.Text);
			Assert.AreEqual("http://www.cs.brandeis.edu/...", hl.Uri);
		}
				
		[Test]
		public virtual void EquivalentDirect() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("belongs",
			                                 new Individual("book"),
			                                 new Individual("mary"))), "Asserted trigger fact");
			
			ie.NewWorkingMemory(WorkingMemoryTypes.Isolated);
			
			deductionsToCheck = new string[] {"gift{mary,book}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
			
			ie.DisposeIsolatedMemory();
			
			Assert.IsTrue(ie.Assert(new Fact("bought",
			                                 new Individual("mary"),
			                                 new Individual("book"))), "Asserted naf blocker fact");

			Process();

			Assert.AreEqual(0, deducted, "Deducted");
		}		
				
		[Test]
		public virtual void EquivalentTwoLevels() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("possess",
			                                 new Individual("mary"),
			                                 new Individual("book"))), "Asserted trigger fact");
			
			deductionsToCheck = new string[] {"gift{mary,book}"};
	  	NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  	ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
		}		
						
		[Test]
		public virtual void EquivalentNaf() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("possess",
			                                 new Individual("mary"),
			                                 new Individual("book"))), "Asserted trigger fact");
			
			Assert.IsTrue(ie.Assert(new Fact("sold",
			                                 new Individual("book"),
			                                 new Individual("mary"))), "Asserted naf blocker fact");
			
			Process();

			Assert.AreEqual(0, deducted, "Deducted");
		}		
						
		[Test]
		public virtual void ProtectIntegrity() {
			ie.LoadRuleBase(NewTestAdapter());
			Process();
			
			Assert.IsTrue(ie.Assert(new Fact("gold", new Individual("ring"))), "Asserted gold fact");
			Process();
			
			Assert.IsTrue(ie.Assert(new Fact("rusty", new Individual("ring"))), "Asserted rusty fact");
			
			try {
				Process();
				Assert.Fail("Integrity was not protected");
			}
			catch(IntegrityException iex) {
				Assert.IsNotNull(iex, "Received IntegrityException");
			}
		}

		[Test]
		public virtual void SlotSupport() {
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("success", new Individual("Bobby"), new Individual(1000))), "Asserted gold fact");
			
			deductionsToCheck = new string[] {"bonus{Bobby,amount=3000}", "miles{Bobby,5000}"};
	  		NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  		ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(2, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
			
			qrs = ie.RunQuery(new Query(new AtomGroup(AtomGroup.LogicalOperator.And,
			                                          new Atom("bonus", new Individual("Bobby"), new Variable("bonus amount")))));
			
			Assert.AreEqual(1, qrs.Count, "Query Size");
			
			if (qrs.Count > 0) Assert.AreEqual(3000, qrs[0][0].GetPredicateValue("amount"), "Correct Sloted Result");
		}

		[Test]
		public virtual void DataInBodySupport() {
			// regression test for bug 1469851
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("Threshold-Check", new Individual("PASS"), new Individual(50))), "Asserted passing fact");
			Assert.IsTrue(ie.Assert(new Fact("Threshold-Check", new Individual("FAIL"), new Individual(5))), "Asserted failing fact");
			
			deductionsToCheck = new string[] {"Threshold-Result{PASS,50}"};
	  		NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  		ie.NewFactHandler += henf;
	  	
			Process();

			Assert.AreEqual(1, deducted, "Deducted");
			Assert.IsFalse(wrongDeduction, "Wrong deduction");
			ie.NewFactHandler -= henf;
		}		

		[Test]
		public virtual void SlotContributeNamedValues() {
			// regression test for RFE 1483072
			ie.LoadRuleBase(NewTestAdapter());
			
			Assert.IsTrue(ie.Assert(new Fact("Slot-Threshold-Check", new Individual("PASS"), new Individual(50))), "Asserted passing fact");
			Assert.IsTrue(ie.Assert(new Fact("Slot-Threshold-Check", new Individual("FAIL"), new Individual(5))), "Asserted failing fact");
			
			deductionsToCheck = new string[] {"Slot-Threshold-Result{PASS,50}"};
	  		NewFactEvent henf = new NewFactEvent(HandleExpectedNewFact);
	  		ie.NewFactHandler += henf;
	  	
	  	//logThreshold = LogEventImpl.DEBUG;
			Process();

			Assert.AreEqual(1, deducted, "(1) Deducted");
			Assert.IsFalse(wrongDeduction, "(1) Wrong deduction");
			
			Assert.IsTrue(ie.Assert(new Fact("Slot-Contribution", new Individual("SUCCESS"), new Individual("PASS"), new Individual(50))), "Asserted second passing fact");

			deductionsToCheck = new string[] {"Slot-Contribution-Result{PASS,50}", "Slot-Contribution-Result-Bis{PASS,50}"};

			Process();

			Assert.AreEqual(2, deducted, "(2) Deducted");
			Assert.IsFalse(wrongDeduction, "(2) Wrong deduction");
			
			ie.NewFactHandler -= henf;
		}		
	}
}
