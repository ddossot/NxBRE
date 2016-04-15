namespace NxBRE.FlowEngine.Rules
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;
		
	using NxBRE.FlowEngine;

	/// <summary> Compares one object to a regexp to see if they match.
	/// </summary>
	/// <author>  David Dossot
	/// </author>
	public sealed class Matches : IBREOperator
	{
		private readonly Dictionary<string, Regex> regexCache = new Dictionary<string, Regex>();
		
		/// <summary>Defines whether an operator is able to deal with empty operands</summary>
		/// <returns>True for the current operator</returns>
		public bool AcceptsNulls {
			get {
				return true;
			}
		}

		/// <summary> Check if object matches a regexp pattern
		/// </summary>
		/// <param name="aBRC">The BRERuleContext object containing all the state
		/// information for use by this method.
		/// </param>
		/// <param name="aMap">Not used yet. An IDictionary object containing the parameters from the XML
		/// </param>
		/// <param name="compareTo">The object to compare it to.
		/// </param>
		/// <param name="regexPattern">The regexp pattern to match against.
		/// </param>
		/// <returns> True if they meet the condition, False otherwise.
		/// </returns>
		public bool ExecuteComparison(IBRERuleContext aBRC, IDictionary aMap, object compareTo, object regexPatternObject) {
			if ((regexPatternObject == null) || (compareTo == null)) {
				return false;
			}
			else {
				lock(regexCache) {
					Regex regex;
					string regexPattern = regexPatternObject.ToString();
					
					if (!regexCache.TryGetValue(regexPattern, out regex)) {
						regex = new Regex(regexPattern);
						regexCache.Add(regexPattern, regex);
					}
					
					return regex.IsMatch(compareTo.ToString());
				}
			}
		}
	}
}
