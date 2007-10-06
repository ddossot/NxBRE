using System;
using System.Collections.Generic;
using System.Text;
using NxBRE.InferenceEngine.Rules;

namespace NxBRE.InferenceEngine.IO
{
    /// <summary>
    /// An inference engine Rulebase Adapter made of a composition of other RuleBase Adapters.
    /// The Adapter exposes the merged collections of the Facts, Implications and queries from the contained  adapters.
    /// </summary>
    /// <author>Amitay Dobo</author>
    public class CompositeRuleBaseAdapter : IExtendedRuleBaseAdapter
    {
        private IList<IRuleBaseAdapter> m_Adapters;
        private IList<Query> m_Queries;
        private IList<Implication> m_Implications;
        private IList<Fact> m_Facts;
        private string m_Direction;
        private string m_Label;
        private IBinder m_Binder;
        private bool m_Disposed;

        //IExtendedRuleBaseAdapter
        IList<Fact> m_Assertions;
        IList<Fact> m_Retractions;
        IList<Equivalent> m_Equivalents;
        IList<Query> m_IntegrityQueries;



        /// <summary>
        /// Instanate a new Composite Rulebase adapter. The RuleBaseAdapter are composed from the supplied adapters at this stage.
        /// </summary>
        /// <example>
        ///             CompositeRuleBaseAdapter = new CompositeRuleBaseAdapter("Test", "forward",
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\RuleMLMerchantExample.xml", System.IO.FileAccess.Read),
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\SimpleRuleFile.xml", System.IO.FileAccess.Read)
        ///            );
        /// </example>
        /// <param name="label">Label for the rulebase</param>
        /// <param name="direction">rulebase direction ("forward" or "backward")</param>
        /// <param name="adapters">Instances of adapters to compose the adapter from</param>
        public CompositeRuleBaseAdapter(string label, string direction, params IRuleBaseAdapter[] adapters)
        {
            Label = label;
            Direction = direction;
            m_Adapters = new List<IRuleBaseAdapter>(adapters);
            MergeAdapters(m_Adapters);
        }



        /// <summary>
        /// Perform the merge of for all the adapters
        /// </summary>
        /// <param name="adapters"></param>
        protected void MergeAdapters(IList<IRuleBaseAdapter> adapters)
        {
            int numberOfFacts = 0;
            int numberOfQueries = 0;
            int numberOfImplications = 0;
            int numberOfAssertions = 0;
            int numberOfRetrations = 0;
            int numberOfIntegrityQueries = 0;
            int numberOfEquivalents = 0;

            //Get the combined count of the different lists to allocate lists
            foreach (IRuleBaseAdapter adapter in adapters)
            {
                numberOfFacts += adapter.Facts.Count;
                numberOfImplications += adapter.Facts.Count;
                numberOfQueries += adapter.Queries.Count;
                //get the count of extended adapter lists
                if (adapter is IExtendedRuleBaseAdapter)
                {
                    IExtendedRuleBaseAdapter extendedAdapter = (IExtendedRuleBaseAdapter)adapter;
                    numberOfAssertions += extendedAdapter.Assertions.Count;
                    numberOfRetrations += extendedAdapter.Retractions.Count;
                    numberOfIntegrityQueries += extendedAdapter.IntegrityQueries.Count;
                    numberOfEquivalents += extendedAdapter.Equivalents.Count;
                }
            }

            //create the merged lists
            List<Fact> mergedFacts = new List<Fact>(numberOfFacts);
            List<Implication> mergedImplications = new List<Implication>(numberOfImplications);
            List<Query> mergedQueries = new List<Query>(numberOfQueries);

            List<Fact> mergedAssertions = new List<Fact>(numberOfAssertions);
            List<Fact> mergedRetractions = new List<Fact>(numberOfRetrations);
            List<Equivalent> mergedEquivalents = new List<Equivalent>(numberOfEquivalents);
            List<Query> mergedIntegrityQueries = new List<Query>(numberOfIntegrityQueries);


            //for each adapter merge the lists
            foreach (IRuleBaseAdapter adapter in adapters)
            {
                mergedFacts.AddRange(adapter.Facts);
                mergedImplications.AddRange(adapter.Implications);
                mergedQueries.AddRange(adapter.Queries);
                //merge extended adapter lists if it is one
                if (adapter is IExtendedRuleBaseAdapter)
                {
                    IExtendedRuleBaseAdapter extendedAdapter = (IExtendedRuleBaseAdapter)adapter;
                    mergedAssertions.AddRange(extendedAdapter.Assertions);
                    mergedRetractions.AddRange(extendedAdapter.Retractions);
                    mergedEquivalents.AddRange(extendedAdapter.Equivalents);
                    mergedIntegrityQueries.AddRange(extendedAdapter.IntegrityQueries);
                }
            }

            //Assign the merged lists
            Facts = mergedFacts;
            Queries = mergedQueries;
            Implications = mergedImplications;
            Assertions = mergedAssertions;
            IntegrityQueries = mergedIntegrityQueries;
            Equivalents = mergedEquivalents;
            Retractions = mergedRetractions;

        }



        #region IRuleBaseAdapter interface members

        /// <summary>
        /// Collection containing all the queries in the rulebase.
        /// </summary>
        public IList<Query> Queries
        {
            get { return m_Queries; }
            set { m_Queries = value; }
        }

        /// <summary>
        /// Collection containing all the implications in the rulebase.
        /// </summary>
        public IList<Implication> Implications
        {
            get { return m_Implications; }
            set { m_Implications = value; }
        }

        #endregion

        #region IFaceBaseAdapter interface members

        /// <summary>
        /// Optional direction of the rulebase: forward, backward or bidirectional.
        /// </summary>
        public string Direction
        {
            get { return m_Direction; }
            set { m_Direction = value; }
        }

        /// <summary>
        /// Optional label of the rulebase.
        /// </summary>
        public string Label
        {
            get { return m_Label; }
            set { m_Label = value; }
        }

        /// <summary>
        /// Collection containing all the facts in the factbase.
        /// </summary>
        public IList<Fact> Facts
        {
            get { return m_Facts; }
            set { m_Facts = value; }
        }

        /// <summary>
        /// Returns an instance of the associated Binder or null.
        /// </summary>
        public IBinder Binder
        {
            set { m_Binder = value; }
        }

        #endregion

        #region IExtendedRuleBaseAdapter Members

        /// <summary>
        /// Collection containing all the facts to assert.
        /// </summary>
        /// <remarks>
        /// This will make IFactBaseAdapter.Facts obsolete.
        /// </remarks>
        public IList<Fact> Assertions
        {
            get { return m_Assertions; }
            set { m_Assertions = value; }
        }

        /// <summary>
        /// Collection containing all the facts to retract.
        /// </summary>
        public IList<Fact> Retractions
        {
            get { return m_Retractions; }
            set { m_Retractions = value; }
        }

        /// <summary>
        /// Collection containing all the equivalent atom pairs in the rulebase.
        /// </summary>
        public IList<Equivalent> Equivalents
        {
            get { return m_Equivalents; }
            set { m_Equivalents = value; }
        }

        /// <summary>
        /// Collection containing all the integrity queries.
        /// </summary>
        public IList<Query> IntegrityQueries
        {
            get { return m_IntegrityQueries; }
            set { m_IntegrityQueries = value; }
        }

        #endregion


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// preform actual dispoing of the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_Disposed)
            {
                if (disposing)
                {
                    //Dispose contained adapters
                    if (m_Adapters != null)
                    {
                        for (int i = 0; i < m_Adapters.Count; i++)
                        {
                            m_Adapters[i].Dispose();
                        }
                    }
                }
            }
            m_Disposed = true;


        }

        #endregion
    }
}

