using System;
using System.Collections;
using System.Collections.Generic;

using NUnit.Framework;

using NxBRE.InferenceEngine;
using NxBRE.InferenceEngine.Core;
using NxBRE.InferenceEngine.Rules;

using NxBRE.Util;
using NxBRE.InferenceEngine.IO;
using System.IO;
using System.Diagnostics;

namespace  NxBRE.Test.InferenceEngine 
{
    [TestFixture]
    public class TestCompositeAdapter
    {
        private IExtendedRuleBaseAdapter m_CompositeAdapter;
        private IExtendedRuleBaseAdapter m_GedcomAdapter;
        private IExtendedRuleBaseAdapter m_TestUnitAdapter;
        protected readonly string ruleFilesFolder;


        public TestCompositeAdapter()
        {
            ruleFilesFolder = Parameter.GetString("unittest.ruleml.inputfolder") + "/";
        }

        [TestFixtureSetUp]
        public void Init()
        {
            m_GedcomAdapter = new RuleML09NafDatalogAdapter(ruleFilesFolder + "gedcom-relations-0_9.ruleml", FileAccess.Read);
            m_TestUnitAdapter = new RuleML09NafDatalogAdapter(ruleFilesFolder + "test-0_9.ruleml", FileAccess.Read);
            m_CompositeAdapter = new CompositeRuleBaseAdapter("TestCompositeAdapter", "forward",
                                                            m_GedcomAdapter,
                                                            m_TestUnitAdapter);


        }

        [Test]
        public void FactCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Facts.Count + m_TestUnitAdapter.Facts.Count, m_CompositeAdapter.Facts.Count);
            Trace.WriteLine("Facts: " + m_CompositeAdapter.Facts.Count);
        }
        [Test]
        public void ImplicationCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Implications.Count + m_TestUnitAdapter.Implications.Count, m_CompositeAdapter.Implications.Count);
            Trace.WriteLine("Implications: " + m_CompositeAdapter.Implications.Count);
        }
        [Test]
        public void QueryCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Queries.Count + m_TestUnitAdapter.Queries.Count, m_CompositeAdapter.Queries.Count);
            Trace.WriteLine("Queries: " + m_CompositeAdapter.Queries.Count);
        }


        [Test]
        public void AssertionsCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Assertions.Count + m_TestUnitAdapter.Assertions.Count, m_CompositeAdapter.Assertions.Count);
            Trace.WriteLine("Assertions: " + m_CompositeAdapter.Assertions.Count);
        }

        [Test]
        public void EquivalentsCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Equivalents.Count + m_TestUnitAdapter.Equivalents.Count, m_CompositeAdapter.Equivalents.Count);
            Trace.WriteLine("Equivalents: " + m_CompositeAdapter.Equivalents.Count);
        }

        [Test]
        public void IntegrityQueriesCount()
        {
            Assert.AreEqual(m_GedcomAdapter.IntegrityQueries.Count + m_TestUnitAdapter.IntegrityQueries.Count, m_CompositeAdapter.IntegrityQueries.Count);
            Trace.WriteLine("IntegrityQueries: " + m_CompositeAdapter.IntegrityQueries.Count);
        }

        [Test]
        public void RetractionsCount()
        {
            Assert.AreEqual(m_GedcomAdapter.Retractions.Count + m_TestUnitAdapter.Retractions.Count, m_CompositeAdapter.Retractions.Count);
            Trace.WriteLine("Retractions: " + m_CompositeAdapter.Retractions.Count);
        }

        [Test]
        public void RunEngine()
        {
            IInferenceEngine ie = new IEImpl();
            ie.LoadRuleBase(m_CompositeAdapter);
            Trace.WriteLine("Facts Before RunEngine:" + ie.FactsCount);
            ie.Process();
            Trace.WriteLine("Facts After RunEngine:" + ie.FactsCount);
        }


        
    }


}
