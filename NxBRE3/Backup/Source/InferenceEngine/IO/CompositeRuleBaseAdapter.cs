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
    /// <remarks>The merging itself is done "just in time", on the different IList getters, to 
    /// preserve the same behaviour of the different adapters, and allow working with a Binder, which is set from the inference engine
    /// implementation before loading the facts, implications, etc.</remarks>
    /// <author>Amitay Dobo</author>
    public class CompositeRuleBaseAdapter : IExtendedRuleBaseAdapter
    {
        private IList<IRuleBaseAdapter> m_Adapters;
        private string m_Direction;
        private string m_Label;
        private IBinder m_Binder;
        private bool m_Disposed;

        private enum AdapterListType
        {
            Fact,
            Query,
            Implication,
            Assertion,
            Retraction,
            Equivalent,
            IntegrityQuery
        }

        /// <summary>
        /// Instantiate a new Composite Rulebase adapter. The RuleBaseAdapter are composed from the supplied adapters at this stage.
        /// </summary>
        /// <example>
        ///             CompositeRuleBaseAdapter = new CompositeRuleBaseAdapter("Test", "forward", null,
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\RuleMLMerchantExample.xml", System.IO.FileAccess.Read),
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\SimpleRuleFile.xml", System.IO.FileAccess.Read)
        ///            );
        /// </example>
        /// <param name="label">Label for the rulebase</param>
        /// <param name="direction">Rulebase direction ("forward" or "backward")</param>
        /// <param name="binder">The binder to use for loading the rulebases, or null.</param>
        /// <param name="adapters">Instances of adapters to compose the adapter from</param>
        public CompositeRuleBaseAdapter(string label, string direction, IBinder binder ,params IRuleBaseAdapter[] adapters)
        {
            this.Binder = binder;
            Label = label;
            Direction = direction;
            m_Adapters = new List<IRuleBaseAdapter>(adapters);
        }

        /// <summary>
        /// Instantiate a new Composite Rulebase adapter. The RuleBaseAdapter are composed from the supplied adapters at this stage.
        /// </summary>
        /// <example>
        ///             CompositeRuleBaseAdapter = new CompositeRuleBaseAdapter("Test", "forward",
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\RuleMLMerchantExample.xml", System.IO.FileAccess.Read),
        ///            new RuleML09NafDatalogAdapter(@"D:\Dev\nxbre-3_1_0\Examples\QMRuleTest\SimpleRuleFile.xml", System.IO.FileAccess.Read)
        ///            );
        /// </example>
        /// <param name="label">Label for the rulebase</param>
        /// <param name="direction">Rulebase direction ("forward" or "backward")</param>
        /// <param name="adapters">Instances of adapters to compose the adapter from</param>
        public CompositeRuleBaseAdapter(string label, string direction, params IRuleBaseAdapter[] adapters):this(label, direction, null, adapters)
        {
        	// NOOP
        }

        /// <summary>
        /// A generic function for merging a list from the different adapters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listType"></param>
        /// <returns></returns>
        private IList<T> GetMergedList<T>(AdapterListType listType)
        {

                int numberOfItems = 0;

                //Get the combined count of the different lists to allocate list capacity
                foreach (IRuleBaseAdapter adapter in m_Adapters)
                {
                    adapter.Binder = m_Binder;
                    numberOfItems += GetAdaptersList<T>(adapter, listType).Count;
                }

                //create the merged lists
                List<T> mergedFacts = new List<T>(numberOfItems);

                //for each adapter merge the lists
                foreach (IRuleBaseAdapter adapter in m_Adapters)
                {
                    mergedFacts.AddRange(GetAdaptersList<T>(adapter, listType));
                }

                //Assign the merged lists
                return mergedFacts.AsReadOnly();

        }

        /// <summary>
        /// Get a specific list type from an adapter.
        /// </summary>
        /// <typeparam name="T">Type of the list to return</typeparam>
        /// <param name="adapter">adapter to return the list from</param>
        /// <param name="listType"></param>
        /// <returns></returns>
        private IList<T> GetAdaptersList<T>(IRuleBaseAdapter adapter, AdapterListType listType)
        {
            switch (listType)
            {
                case AdapterListType.Fact:
                    return (IList<T>)adapter.Facts;
                case AdapterListType.Implication:
                    return (IList<T>)adapter.Implications;
                case AdapterListType.Query:
                    return (IList<T>)adapter.Queries;

            }

            IExtendedRuleBaseAdapter extendedAdapter = adapter as IExtendedRuleBaseAdapter;
            if (adapter != null)
            {
                switch (listType)
                {
                    case AdapterListType.Retraction:
                        return (IList<T>)extendedAdapter.Retractions;
                    case AdapterListType.IntegrityQuery:
                        return (IList<T>)extendedAdapter.IntegrityQueries;
                    case AdapterListType.Equivalent:
                        return (IList<T>)extendedAdapter.Equivalents;
                    case AdapterListType.Assertion:
                        return (IList<T>)extendedAdapter.Assertions;
                }
            }

            return new List<T>(0);
        }






        #region IRuleBaseAdapter interface members

        /// <summary>
        /// Collection containing all the queries in the rulebase.
        /// </summary>
        public IList<Query> Queries
        {
            get { return GetMergedList<Query>(AdapterListType.Query); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
        }

        /// <summary>
        /// Collection containing all the implications in the rulebase.
        /// </summary>
        public IList<Implication> Implications
        {
            get { return GetMergedList<Implication>(AdapterListType.Implication); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
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
            get { return GetMergedList<Fact>(AdapterListType.Fact); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
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
            get { return GetMergedList<Fact>(AdapterListType.Assertion); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
        }

        /// <summary>
        /// Collection containing all the facts to retract.
        /// </summary>
        public IList<Fact> Retractions
        {
            get { return GetMergedList<Fact>(AdapterListType.Retraction); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
        }

        /// <summary>
        /// Collection containing all the equivalent atom pairs in the rulebase.
        /// </summary>
        public IList<Equivalent> Equivalents
        {
            get { return GetMergedList<Equivalent>(AdapterListType.Equivalent); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
        }

        /// <summary>
        /// Collection containing all the integrity queries.
        /// </summary>
        public IList<Query> IntegrityQueries
        {
            get { return GetMergedList<Query>(AdapterListType.IntegrityQuery); }
            set { throw new NotImplementedException("Composite Adapter is read only."); }
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

