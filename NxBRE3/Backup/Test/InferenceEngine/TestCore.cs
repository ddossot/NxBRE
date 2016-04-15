namespace NxBRE.Test.InferenceEngine {
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	using NUnit.Framework;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.Core;
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;

	[TestFixture]
	public class TestCore {
		private	Fact fact1;
		private	Fact fact2;
		private	Fact fact3;
		private	Fact fact3bis;
		private	Fact factX;
		private	Atom imp2;
		private	Atom imp2bis;
		private	Atom impX;
		private	Atom atom2_2;
		private	Atom atom3;
		private Atom atomF;
	
		[TestFixtureSetUp]
		public void Init() {
			atom2_2 = new Atom("luxury", new Variable("product"));
			atom3 = new Atom("spending", new Variable("customer"), new Individual("min(5000,EUR)"), new Individual("previous year"));
			atomF = new Atom("spending", new Variable("customer"), new Function(Function.FunctionResolutionType.Binder, "min(5000,EUR)", null, "min", new string[]{"5000","EUR"}), new Individual("previous year"));			
			
			fact1 = new Fact("premium", new Individual("Peter Miller"));
			fact2 = new Fact("spending", new Individual("Peter Miller"), new Individual("min(5000,EUR)"));
			fact3 = new Fact("spending", new Individual("Peter Miller"), new Individual("min(5000,EUR)"), new Individual("previous year"));
			fact3bis = new Fact("spending", new Individual("Peter Miller"), new Individual("min(5000,EUR)"), new Individual("previous year"));
			factX = new Fact("spending", new Individual("John Q. Doe"), new Individual("min(5000,EUR)"), new Individual("previous year"));
			
			imp2 = new Atom("discount", new Variable("customer"), new Variable("product"), new Individual("7.5 percent"));
			imp2bis = new Atom("discount", new Variable("buyer"), new Variable("product"), new Individual("7.5 percent"));
			impX = new Atom("discount", new Variable("buyer"), new Variable("product"), new Individual("6.5 percent"));
		}
		
		[Test]
		public void Predicates()
		{
			IPredicate v1 = new Variable("one");
			IPredicate v2 = new Variable("two");
			IPredicate i1 = new Individual("one");
			IPredicate i2 = new Individual("two");

			Assert.AreEqual(v1, new Variable("one"), "Similar Variables");
			Assert.IsFalse(v1.Equals(v2), "Different Variables");
			Assert.IsFalse(v1.Equals(i2), "Variable differs from Individual");
			
			Assert.AreEqual(i1, new Individual("one"), "Similar Individuals");
			Assert.IsFalse(i1.Equals(i2), "Different Variables");
			Assert.IsFalse(i2.Equals(v1), "Individual differs from Variable");
		}
		
		[Test]
		public void FactSlots() {
			Fact fact = new Fact("spending", new Slot("client", new Individual("Peter Miller")), new Individual("min(5000,EUR)"));
			
			Assert.AreEqual("Peter Miller", fact.GetPredicateValue("client"), "Slot predicate value");
			
			Fact clone = (Fact)fact.Clone();
			fact = null;
			
			Assert.AreEqual("Peter Miller", clone.GetPredicateValue("client"), "Slot predicate value after cloning");
		}
		
		[Test]
		public void AtomSlots() {
			Atom atom = new Atom("spending", new Slot("client", new Variable("buyer")), new Function(Function.FunctionResolutionType.Binder, "min(5000,EUR)", null, "min", new string[]{"5000","EUR"}));
			
			Assert.AreEqual(typeof(Variable), atom.GetPredicate("client").GetType(), "Slot predicate");
			
			Atom clone = (Atom)atom.Clone();
			atom = null;
			
			Assert.AreEqual(typeof(Variable), clone.GetPredicate("client").GetType(), "Slot predicate after cloning");
		}
		
		[Test][ExpectedException(typeof(ArgumentException))]
		public void SlotInSlot() {
			new Slot("error", new Slot("test", new Individual("44")));
		}

		
		[Test]
		public void TypedRelationMatching() {
			Assert.IsTrue(new Atom("test", new Individual("true"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual(true), new Individual("foo"))),
			              "Boolean");
			
			Assert.IsFalse(new Atom("test", new Individual("false"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual(true), new Individual("foo"))),
			              "Boolean");
			
			Assert.IsTrue(new Atom("test", new Individual("44"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((SByte) 44), new Individual("foo"))),
			              "SByte");
			
			Assert.IsTrue(new Atom("test", new Individual("11"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Int16) 11), new Individual("foo"))),
			              "Int16");
			
			Assert.IsTrue(new Atom("test", new Individual("22"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Int32) 22), new Individual("foo"))),
			              "Int32");
			
			Assert.IsTrue(new Atom("test", new Individual("33"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Int64) 33), new Individual("foo"))),
			              "Int64");
			
			Assert.IsTrue(new Atom("test", new Individual("3.14"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Single) 3.14), new Individual("foo"))),
			              "Single");
			
			Assert.IsTrue(new Atom("test", new Individual("6.28"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Double) 6.28), new Individual("foo"))),
			              "Double");
			
			Assert.IsTrue(new Atom("test", new Individual("9.42"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual((Decimal) 9.42), new Individual("foo"))),
			              "Decimal");
			
			Assert.IsTrue(new Atom("test", new Individual("2003-12-25"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual(new DateTime(2003, 12, 25)), new Individual("foo"))),
			              "Date");
			
			Assert.IsTrue(new Atom("test", new Individual("1969-07-21T02:56:00"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual(new DateTime(1969, 07, 21, 02, 56, 00)), new Individual("foo"))),
			              "DateTime");
			
			Assert.IsTrue(new Atom("test", new Individual("07:15:30"), new Variable("foo"))
			              .Matches(new Fact("test", new Individual(DateTime.Today.Add(new TimeSpan(7, 15, 30))), new Individual("foo"))),
			              "Time");
		}
		
		[Test]
		public void Implications()
		{
			Assert.IsFalse(imp2.Equals(imp2bis), "Different (2 vars, 1 ind)");
			Assert.IsFalse(impX.Equals(imp2), "Different (2 vars, 1 ind)");

			Assert.IsTrue(imp2.Matches(imp2bis), "Matches (2 vars, 1 ind)");
			Assert.IsFalse(impX.Matches(imp2), "Non-matches (2 vars, 1 ind)");
			
			Assert.IsTrue(imp2.IsIntersecting(imp2bis), "Non-similar (2 vars, 1 ind)");
			Assert.IsFalse(impX.IsIntersecting(imp2), "Non-similar bis (2 vars, 1 ind)");
		}
		
		[Test][ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WrongImplicationLowPriority() {
			new Implication(null, -1, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WrongImplicationHiPriority() {
			new Implication(null, 101, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WrongImplicationLowSalience() {
			Implication i = new Implication(null, ImplicationPriority.Medium, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			i.Salience = -1;
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void WrongImplicationHiSalience() {
			Implication i = new Implication(null, ImplicationPriority.Medium, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			i.Salience = 100;
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void ImplicationBaseDuplicatedLabel() {
			ImplicationBase ib = new ImplicationBase();
			ib.Add(new Implication("impMedLow", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3)));
			ib.Add(new Implication("impMedLow", 75, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3)));
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void ImplicationBase() {
			ImplicationBase ib = new ImplicationBase();
			Implication imp = new Implication("impMedLow", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(imp);
			ib.Add(new Implication("impMedHi", 75, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3)));
			Assert.AreEqual(imp, ib.Get("impMedLow"), "Get");
			Assert.IsNull(ib.Get("missing bit"), "Failed Get");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void MutexMissing() {
			ImplicationBase ib = new ImplicationBase();
			Implication imp = new Implication("impMedLow", 25, "missing", String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(imp);
			ib.Add(new Implication("impMedHi", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3)));
			new MutexManager(ib).AnalyzeImplications();
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void PreconditionMissing() {
			ImplicationBase ib = new ImplicationBase();
			Implication impMedLow = new Implication("impMedLow", 25, String.Empty, "missing", imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(impMedLow);
			ib.Add(new Implication("impMedHi", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3)));
			new PreconditionManager(ib).AnalyzeImplications();
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void GetPreconditionChildren() {
			ImplicationBase ib = new ImplicationBase();
			Implication impMedLow = new Implication("impMedLow", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(impMedLow);
			Implication impMedHi = new Implication("impMedHi", 25, String.Empty, "impMedLow", imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(impMedHi);
			new PreconditionManager(ib).AnalyzeImplications();
			
			IList<Implication> preconditionChildren = ib.GetPreconditionChildren(impMedLow);
			Assert.AreEqual(1, preconditionChildren.Count, "preconditionChildren size");
			Assert.IsTrue(preconditionChildren.Contains(impMedHi), "preconditionChildren content");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void PreconditionIsMutex() {
			ImplicationBase ib = new ImplicationBase();
			Implication impMedLow = new Implication("impMedLow", 25, "impMedHi", String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(impMedLow);
			Implication impMedHi = new Implication("impMedHi", 25, String.Empty, "impMedLow", imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			ib.Add(impMedHi);
			new MutexManager(ib).AnalyzeImplications();
			new PreconditionManager(ib).AnalyzeImplications();
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void Matching()
		{
			Assert.IsTrue(atom3.Matches(fact3), "Matching");
			Assert.IsFalse(atom2_2.Matches(fact3), "Non-Matching");		
		}
				
		[Test]
		public void VariableTranslation()	{
			Atom template = new Atom("own", new Variable("person"), new Variable("object"));
			Atom source = new Atom("own", new Variable("person"), new Variable("stuff"));
			Atom target = new Atom("belongs", new Variable("person"), new Variable("stuff"));
			Atom expected = new Atom("belongs", new Variable("person"), new Variable("object"));
			Assert.AreEqual(expected, RulesUtil.TranslateVariables(template, source, target));
		}
		
		private static int CountEnumerator(IEnumerator<Fact> e) {
			int result = 0;
			while(e.MoveNext()) result++;
			return result;
		}
		
		private int RunStrictTypingFactBaseTest(bool strictTyping) {
			FactBase fb = new FactBase();
			fb.strictTyping = strictTyping;
			
			Assert.IsTrue(fb.Assert(new Fact("spending",
								                        new Individual("foo"),
								                        new Individual(7000))), "Assert 'foo Spending'");
			
			Assert.IsTrue(fb.Assert(new Fact("spending",
								                        new Individual("bar"),
								                        new Individual("7000"))), "Assert 'bar Spending'");
			
			Atom filter = new Atom("spending",
			                       new Variable("name"),
			                       new Individual("7000"));
			
			return CountEnumerator(fb.Select(filter, null));
		}
		
		[Test]
		public void StrictTypingFactBase() {
			Assert.AreEqual(1, RunStrictTypingFactBaseTest(true), "Strict typing selection");
		}
		
		[Test]
		public void NonStrictTypingFactBase() {
			Assert.AreEqual(2, RunStrictTypingFactBaseTest(false), "Non-Strict typing selection");
		}
		
		[Test]
		public void ExcludeFactsFromSelection() {
			FactBase fb = new FactBase();
			
			Assert.IsTrue(fb.Assert(new Fact("spending",
								                        new Individual("Peter Miller"),
								                        new Individual(5000),
								                        new Individual("previous year"))), "Assert 'P.Miller Spending'");
			
			Fact jqdSpending = new Fact("JQD Spending",
								                   "spending",
						                        new Individual("John Q.Clone Doe"),
						                        new Individual(7000),
						                        new Individual("previous year"));
			
			Assert.IsTrue(fb.Assert(jqdSpending), "Assert 'JQD Spending'");
			
			Atom filter = new Atom("spending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThanEqualTo", "5000"),
			                       new Individual("previous year"));
			
			Assert.AreEqual(2, CountEnumerator(fb.Select(filter, null)), "Query with NxBRE function and no fact exclusion");
			
			IList<Fact> excludedFacts = new List<Fact>();
			excludedFacts.Add(jqdSpending);
			
			Assert.AreEqual(1, CountEnumerator(fb.Select(filter, excludedFacts)), "Query with NxBRE function with fact exclusion");
		}
		
		private void PopulateFactBase(FactBase fb) {
			Assert.IsTrue(fb.Assert(new Fact("spending",
								                        new Individual("foo"),
								                        new Individual(7000))), "Assert 'foo Spending'");
			
			Assert.IsTrue(fb.Assert(new Fact("spending",
								                        new Individual("Peter Miller"),
								                        new Individual(5000),
								                        new Individual("previous year"))), "Assert 'P.Miller Spending'");
			
			Assert.IsTrue(fb.Assert(new Fact("JQD Spending",
										                   "spending",
								                        new Individual("John Q.Clone Doe"),
								                        new Individual(7000),
								                        new Individual("previous year"))), "Assert 'JQD Spending'");
			
			Assert.IsTrue(fb.Assert(new Fact("lending",
								                        new Individual("John Q.Clone Doe"),
								                        new Individual(7000),
								                        new Individual("previous year"))), "Assert 'JQD Lending'");
			
		}
		
		[Test]
		public void PredicateNxBREFunction() {
			FactBase fb = new FactBase();
			PopulateFactBase(fb);
			
			Atom filter = new Atom("spending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "5000"),
			                       new Individual("previous year"));
			
			Assert.AreEqual(1, CountEnumerator(fb.Select(filter, null)), "Query with NxBRE function");
		}
		
		[Test]
		public void ProcessAnd() {
			FactBase fb = new FactBase();
			PopulateFactBase(fb);
			
			Atom atom1 = new Atom("spending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "5000"),
			                       new Individual("previous year"));
			
			Atom atom2 = new Atom("lending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "5000"),
			                       new Individual("previous year"));
			
			IList<IList<FactBase.PositiveMatchResult>> result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.And, atom1, atom2));
			Assert.AreEqual(1, result.Count, "2 positive atoms");
			                                                                        
			atom2 = new Atom("lending",
                       new Variable("name"),
                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "15000"),
                       new Individual("previous year"));
			
			result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.And, atom1, atom2));
			Assert.AreEqual(0, result.Count, "1 positive and 1 negative atoms");

			atom2 = new Atom("spending", new Variable("otherName"), new Variable("whateverAmount"), new Individual("previous year"));
			
			result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.And, atom1, atom2));
			Assert.AreEqual(2, result.Count, "2 positive combinatorial atoms");
			
			atom2 = new Atom("spending", new Variable("otherName"), new Variable("whateverAmount"), new Individual("unknown year"));
			result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.And, atom1, atom2));
			Assert.AreEqual(0, result.Count, "1 positive and 1 negative combinatorial atoms");
		}
		
		[Test]
		public void ProcessOr() {
			FactBase fb = new FactBase();
			PopulateFactBase(fb);
			
			Atom atom1 = new Atom("spending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "1000"),
			                       new Individual("previous year"));
			
			Atom atom2 = new Atom("lending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "5000"),
			                       new Individual("previous year"));
			
			IList<IList<FactBase.PositiveMatchResult>> result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.Or, atom1, atom2));
			Assert.AreEqual(3, result.Count, "2 positive atoms");
			
			atom1 = new Atom("spending",
                       new Variable("name"),
                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "9999"),
                       new Individual("previous year"));
			
			result = fb.ProcessAtomGroup(new AtomGroup(AtomGroup.LogicalOperator.Or, atom1, atom2));
			Assert.AreEqual(1, result.Count, "1 positive atom");
			
		}
		
		[Test]
		public void FactBaseCloning() {
			FactBase fb = new FactBase();
			PopulateFactBase(fb);
			
			FactBase clone = (FactBase)fb.Clone();
			Assert.AreEqual(fb.Count, clone.Count, "Same number of facts");
			
			Fact jqdSpending = clone.GetFact("JQD Spending");
			Assert.AreEqual("JQD Spending", jqdSpending.Label, "Correct fact label");
			
			int countBefore = clone.Count;
			Assert.IsTrue(clone.Retract(jqdSpending), "Retracted 'JQD Spending'");
			Assert.AreEqual(countBefore-1, clone.Count, "Count after retraction");
			
			Assert.IsFalse(clone.Exists(jqdSpending), "'JQD Spending' really retracted");
			Assert.IsTrue(fb.Exists(jqdSpending), "'JQD Spending' still in original");
			
			Atom filter = new Atom("spending",
			                       new Variable("name"),
			                       new Function(Function.FunctionResolutionType.NxBRE, "foo", null, "GreaterThan", "5000"),
			                       new Individual("previous year"));

			Assert.AreEqual(1, CountEnumerator(fb.Select(filter, null)), "Query on the original");
			Assert.AreEqual(0, CountEnumerator(clone.Select(filter, null)), "Query on the clone");
		}
		
		[Test]
		public void AtomFunctionBinder() {
			Assert.IsTrue(new AtomFunction(AtomFunction.RelationResolutionType.NxBRE, false, null, "nxbre:Equals", new Individual(123), new Individual(123)).PositiveRelation, "AtomFunction: true PositiveRelation");
			Assert.IsFalse(new AtomFunction(AtomFunction.RelationResolutionType.NxBRE, false, null, "nxbre:Equals", new Individual(123), new Individual(321)).PositiveRelation, "AtomFunction: false PositiveRelation");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void AtomFunctionWithFunctionPredicate() {
			new AtomFunction(AtomFunction.RelationResolutionType.NxBRE, false, null, "nxbre:Equals", new Individual(123), new Function(Function.FunctionResolutionType.Binder, "123", null, "Test", new string[0]));
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void Facts()
		{
			Assert.AreEqual(fact3, fact3bis, "Equal (3 ind)");
			Assert.IsTrue(fact3.IsFact, "fact3 IsFact");
			Assert.IsFalse(factX.Equals(fact3), "Different (3 ind)");
			Assert.IsFalse(atom3.IsFact, "atom3 Is Not Fact");
			
			Assert.IsFalse(new Fact("spouse", new Individual("Man"), new Individual("Woman"))
			               .Equals(new Fact("spouse", new Individual("Woman"), new Individual("Man"))),
			               "Same type non-equal facts");

		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void FactNonFact() {
			new Fact("luxury", new Variable("product"));
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void RenameFact()
		{
			Fact spendingPM1 = new Fact("Spending of Peter Miller",
			                            "spending",
						                       new Individual("Peter Miller"),
						                       new Individual("min(5000,EUR)"),
						                       new Individual("previous year"));
			
			String newLabel = "Expenses of Peter Miller";
			Fact spendingPM2 = spendingPM1.ChangeLabel(newLabel);
			
			Assert.AreEqual(spendingPM2.Label, newLabel, "Fact has been renamed");
			Assert.AreEqual(spendingPM1, spendingPM2, "Renamed facts are equals");
		}
		
		[Test]
		public void FactBaseAssertRetract()
		{
			FactBase fb = new FactBase();
			
			Assert.IsTrue(fb.Assert(new Fact("spending",
	                        new Individual("Peter Miller"),
	                        new Individual("min(5000,EUR)"),
	                        new Individual("previous year"))), "Assert 'P.Miller Spending'");
			
			Assert.IsTrue(fb.Assert(new Fact("JQD Spending",
			                   "spending",
	                        new Individual("John Q.Clone Doe"),
	                        new Individual("min(7000,EUR)"),
	                        new Individual("previous year"))), "Assert 'JQD Spending'");
		
			Assert.IsNotNull(fb.GetFact("JQD Spending"), "Exist 'JQD Spending'");
			
			Assert.IsFalse(fb.Assert(fact3bis), "Assert fact3bis");
			Assert.IsTrue(fb.Assert(factX), "Assert factX");
			
			Fact jqdSpending = fb.GetFact("JQD Spending");
			Assert.IsTrue(fb.Exists(jqdSpending));
			Assert.IsTrue(fb.Retract(jqdSpending));
			Assert.IsFalse(fb.Exists(jqdSpending));
			
			Assert.IsNull(fb.GetFact("JQD Spending"), "Get Retracted 'JQD Spending'");
		}
		
		[Test]
		public void FactBaseAssertModify()
		{
			FactBase fb = new FactBase();
			
			Fact spendingPM1 = new Fact("Spending of Peter Miller",
			                            "spending",
						                       new Individual("Peter Miller"),
						                       new Individual("min(5000,EUR)"),
						                       new Individual("previous year"));
			
			Assert.IsTrue(fb.Assert(spendingPM1), "Assert 'P.Miller Spending' v1");
			Assert.IsFalse(fb.Assert(spendingPM1), "Can not assert 'P.Miller Spending' v1 twice!");
			Assert.IsTrue(fb.Exists(spendingPM1), "Exists 'P.Miller Spending' v1");
			
			Fact spendingJQD = new Fact("Spending of John Q. Doe",
			                            "spending",
					                        new Individual("John Q.Clone Doe"),
					                        new Individual("min(7000,EUR)"),
					                        new Individual("previous year"));
			
			Assert.IsTrue(fb.Assert(spendingJQD), "Assert 'JQD Spending'");
		
			Fact spendingPM2 = new Fact("spending",
						                       new Individual("Peter Miller"),
						                       new Individual("min(9999,EUR)"),
						                       new Individual("previous year"));
			
			Assert.IsTrue(fb.Modify(spendingPM1, spendingPM2), "Modify 'P.Miller Spending' v1 to v2");
			Assert.IsNotNull(fb.GetFact(spendingPM1.Label), "(1) Label has been maintained");
			Assert.AreEqual(2, fb.Count, "(1) FactBase size");
			Assert.IsFalse(fb.Exists(spendingPM1), "(1) Not Exists 'P.Miller Spending' v1");
			Assert.IsTrue(fb.Exists(spendingPM2), "(1) Exists 'P.Miller Spending' v2");
			Assert.IsTrue(fb.Exists(spendingJQD), "(1) Exists 'JQD Spending'");

			Assert.IsFalse(fb.Modify(spendingPM1, spendingPM2), "Can not modify an inexistant fact");
			Assert.AreEqual(2, fb.Count, "(2) FactBase size");
			Assert.IsFalse(fb.Exists(spendingPM1), "(2) Not Exists 'P.Miller Spending' v1");
			Assert.IsTrue(fb.Exists(spendingPM2), "(2) Exists 'P.Miller Spending' v2");
			Assert.IsTrue(fb.Exists(spendingJQD), "(2) Exists 'JQD Spending'");

			Assert.IsTrue(fb.Modify(spendingPM2, spendingPM2), "Can modify a fact to itself");
			Assert.AreEqual(2, fb.Count, "(3) FactBase size");
			Assert.IsFalse(fb.Exists(spendingPM1), "(3) Not Exists 'P.Miller Spending' v1");
			Assert.IsTrue(fb.Exists(spendingPM2), "(3) Exists 'P.Miller Spending' v2");
			Assert.IsTrue(fb.Exists(spendingJQD), "(3) Exists 'JQD Spending'");

			Assert.IsTrue(fb.Modify(spendingJQD, spendingPM2), "Modify 'JQD Spending' to 'P.Miller Spending' v2");
			Assert.IsNotNull(fb.GetFact(spendingJQD.Label), "(4) Label has been maintained");
			Assert.AreEqual(1, fb.Count, "(4) FactBase size");
			Assert.IsFalse(fb.Exists(spendingPM1), "(4) Not Exists 'P.Miller Spending' v1");
			Assert.IsTrue(fb.Exists(spendingPM2), "(4) Exists 'P.Miller Spending' v2");
			Assert.IsFalse(fb.Exists(spendingJQD), "(4) Not Exists 'JQD Spending'");
		}
				
		[Test]
		public void Agenda() {
			Agenda a = new Agenda();
			
			Implication impLow = new Implication("impLow", ImplicationPriority.Minimum, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Implication impLowMed = new Implication("impLowMed", 25, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Implication impMed = new Implication("impMed", ImplicationPriority.Medium, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Implication impMedSaLo = new Implication("impMedSaLo", ImplicationPriority.Medium, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			impMedSaLo.Salience = 1;
			Assert.AreEqual(5101, impMedSaLo.Weight, "Weight impMedSaLo");
			Implication impMedSaHi = new Implication("impMedSaHi", ImplicationPriority.Medium, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			impMedSaHi.Salience = 99;
			Assert.AreEqual(5199, impMedSaHi.Weight, "Weight impMedSaLo");
			Implication impMedHi = new Implication("impMedHi", 75, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			Implication impHi = new Implication("impHi", ImplicationPriority.Maximum, String.Empty, String.Empty, imp2, new AtomGroup(AtomGroup.LogicalOperator.And, atom2_2, atom3));
			
			// schedule in any order
			a.Schedule(impMedSaHi);
			a.Schedule(impLow);
			a.Schedule(impMed);
			a.Schedule(impMedHi);
			a.Schedule(impMedSaLo);
			a.Schedule(impLowMed);
			a.Schedule(impHi);
			
			// prepare for execution, which should sort this mess out
			a.PrepareExecution();
			Implication[] expected = new Implication[] {impHi, impMedHi, impMedSaHi, impMedSaLo, impMed, impLowMed, impLow};
			int i = 0;
			Implication implicationParser;
			while (a.HasMoreToExecute) {
				implicationParser = a.NextToExecute;
				Assert.IsTrue(implicationParser.Equals(expected[i]),
				              "Agenda not ordered: " +
				              implicationParser.Label +
				              "::" +
				              implicationParser.Weight +
				              " != " +
				              expected[i].Label +
				              "::" +
				              expected[i].Weight);
				i++;
			}
		}
		
		[Test]
		public void QueryIsNotImplication() {
			Query query = new Query("get spending",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
								                      new Variable("customer"),
								                      new Individual("min(5000,EUR)"),
								                      new Individual("previous year"))));

			Implication implication = new Implication("get spending",
			                                          ImplicationPriority.Medium,
			                                          String.Empty,
			                                          String.Empty,
			                                          atom2_2,
			                                          query.AtomGroup);
			
			Assert.IsFalse(query.Equals(implication), "QueryIsNotImplication");
		}
		
		[Test][ExpectedException(typeof(BREException))]
		public void QueryBaseDuplicatedLabel() {
			QueryBase qb = new QueryBase();
			qb.Add(new Query("get spending",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
								                      new Variable("customer"),
								                      new Individual("min(5000,EUR)"),
								                      new Individual("previous year")))));
			qb.Add(new Query("get spending",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
								                      new Variable("customer"),
								                      new Individual("min(7000,EUR)"),
								                      new Individual("previous year")))));
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(IndexOutOfRangeException))]
		public void QueryBaseIndexSubZero() {
			QueryBase qb = new QueryBase();
			qb.Add(new Query("get spending",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
								                      new Variable("customer"),
								                      new Individual("min(5000,EUR)"),
								                      new Individual("previous year")))));
			qb.Get(-1);
			Assert.Fail("Should never reach me!");
		}
		
		[Test][ExpectedException(typeof(IndexOutOfRangeException))]
		public void QueryBaseIndexTooHigh() {
			QueryBase qb = new QueryBase();
			qb.Add(new Query("get spending",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
								                      new Variable("customer"),
								                      new Individual("min(5000,EUR)"),
								                      new Individual("previous year")))));
			qb.Get(2);
			Assert.Fail("Should never reach me!");
		}
		
		[Test]
		public void QueryBase() {
			QueryBase qb = new QueryBase();
			Query query1 = new Query("get spending",
					                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("spending",
										                      new Variable("customer"),
										                      new Individual("min(5000,EUR)"),
										                      new Individual("previous year"))));
			qb.Add(query1);
			Assert.AreEqual(1, qb.Count, "(1) QueriesCount");
			
			Query getQ1 = qb.Get("get spending");
			Assert.AreEqual(query1, getQ1, "Get Query Is Equal");
			Assert.IsTrue(((Atom)query1.AtomGroup.Members[0]).IsIntersecting((Atom)getQ1.AtomGroup.Members[0]), "Get Query Is Similar");

			Assert.IsNull(qb.Get("find me if you can"), "Missing query");
			
			Query query2 = new Query("get earning",
					                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("earning",
										                      new Variable("customer"),
										                      new Individual("min(99999,EUR)"),
										                      new Individual("previous year"))));
			qb.Add(query2);
			Assert.AreEqual(2, qb.Count, "(2) QueriesCount");

			qb.Remove(qb.Get(0));
			Assert.AreEqual(1, qb.Count, "(3) QueriesCount");
			Query getQ2 = qb.Get(0);
			Assert.AreEqual(query2, getQ2, "(3) Get Query Is Equal");
			Assert.IsTrue(((Atom)query2.AtomGroup.Members[0]).IsIntersecting((Atom)getQ2.AtomGroup.Members[0]), "(3) Get Query Is Similar");

			qb.Add(new Query("to be killed",
			                      new AtomGroup(AtomGroup.LogicalOperator.And, new Atom("victim",
								                      new Variable("test"),
								                      new Individual("whatsoever")))));
			Assert.AreEqual(2, qb.Count, "(4) QueriesCount");
			
			qb.Remove(qb.Get("to be killed"));
			Assert.AreEqual(1, qb.Count, "(5) QueriesCount");
			
			qb.Remove(query2);
			Assert.AreEqual(0, qb.Count, "(6) QueriesCount");
			
		}
		
		[Test]
		public void AtomResolveFunctions() {
			Assert.AreEqual(atom3, RulesUtil.ResolveFunctions(atom3), "Atom without functions");
			Assert.IsTrue(atomF.HasFunction, "atom Hasfunction");
			Atom atomNoF = RulesUtil.ResolveFunctions(atomF);                  
			Assert.IsFalse(atomF.Equals(atomNoF), "Atom with functions");
			Assert.IsFalse(atomNoF.HasFunction, "atomNoF Hasfunction");
			Assert.IsTrue(fact3.Matches(atomNoF), "Fact and Atom match");
		}
		
		[Test]
		public void FactResolve() {
			Fact fact2bis = new Fact("spending", new Individual("Peter Miller"), new Individual(35000));
			Fact fact2ter = RulesUtil.Resolve(fact2bis, atomF);
			Assert.IsFalse(fact2.Equals(fact2bis));
			Assert.IsTrue(fact2.Equals(fact2ter));
		}
		
		[Test]
		public void AtomGroupMemberOrdering() {
			AtomGroup child = new AtomGroup(AtomGroup.LogicalOperator.Or, atom2_2, atomF);
			AtomGroup parent = new AtomGroup(AtomGroup.LogicalOperator.And, child, atom3);
			Assert.AreEqual(child, parent.Members[0], "First member match");
			Assert.AreEqual(atom3, parent.Members[1], "Second member match");
		}
		
		[Test]
		public void AtomGroupOrderedMemberOrdering() {
			AtomGroup child = new AtomGroup(AtomGroup.LogicalOperator.Or, atom2_2, atomF);
			AtomGroup parent = new AtomGroup(AtomGroup.LogicalOperator.And, child, atom3);
			Assert.AreEqual(child, parent.OrderedMembers[1], "First member match");
			Assert.AreEqual(atom3, parent.OrderedMembers[0], "Second member match");
		}

	}
}
