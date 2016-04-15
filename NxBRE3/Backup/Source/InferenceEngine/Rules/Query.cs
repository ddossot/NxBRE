namespace NxBRE.InferenceEngine.Rules {
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine.Rules;
	
	using NxBRE.Util;
	
	/// <summary>
	/// A Query is a collection of non-fact atoms that can be labelled, which can be executed
	/// against a Fact base in order to produce an ArrayList of facts (a selection of the facts
	/// matching the data pattern defined by the AtomGroups hierarchy. It is immutable.
	/// </summary>
	/// <author>David Dossot</author>
	public class Query {
		private readonly string label;
		private readonly AtomGroup atomGroup;

		/// <summary>
		/// The optional Label of the Query.
		/// </summary>
		public string Label {
			get {
				return label;
			}
		}	

		/// <summary>
		/// The top AtomGroup of the Query.
		/// </summary>
		public AtomGroup AtomGroup {
			get {
				return atomGroup;
			}
		}
		
		/// <summary>
		/// Instantiates a new anonymous (non-labelled) Query based on an AtomGroup.
		/// </summary>
		/// <param name="atomGroup">The AtomGroup used in the new Query.</param>
		public Query(AtomGroup atomGroup):this(null, atomGroup) {}
		
		/// <summary>
		/// Instantiates a new labelled Query based on an AtomGroup.
		/// </summary>
		/// <param name="label">The Label of the new Query.</param>
		/// <param name="atomGroup">The AtomGroup used in the new Query.</param>
		public Query(string label, AtomGroup atomGroup) {
			if ((null != label) && (String.Empty == label)) this.label = null;
			else this.label = label;
			
			this.atomGroup = atomGroup;
		}
	
		/// <summary>
		/// Returns the String representation of the Query for display purpose only.
		/// </summary>
		/// <returns>The String representation of the Query.</returns>
		public override string ToString() {
			return "Query[Label:" + Label + "\n" + AtomGroup + "]";
		}
		
		/// <summary>
		/// Checks if the current Query is equal to another one, based on their hashcode.
		/// </summary>
		/// <param name="o">The other Query to test the equality.</param>
		/// <returns>True if the two queries are equal.</returns>
		public override bool Equals(object o) {
			if (o.GetType() != this.GetType()) return false;
			return (o.GetHashCode() == this.GetHashCode());
		}
	
		/// <summary>
		/// Calculates the hashcode of the current Query: if the label is present, it becomes
		/// the main identifier, else the AtomGroup hashcode is returned.
		/// </summary>
		/// <returns>The hashcode of the current Query.</returns>
		public override int GetHashCode() {
			if ((Label != null) && (Label != String.Empty)) return Label.GetHashCode();
			else return AtomGroup.GetHashCode();
		}
		
	}

}

