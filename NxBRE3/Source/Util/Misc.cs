using System.Linq;

namespace NxBRE.Util
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	/// <summary>Misc NxBRE utilities.</summary>
	/// <author>David Dossot</author>
	public abstract class Misc {
		private Misc() {}
		
		/// <summary>
		/// An empty read-only IDictionary.
		/// </summary>
		public static readonly IDictionary EMPTY_DICTIONARY = new ReadOnlyHashtable();
		private class ReadOnlyHashtable : Hashtable {
			public override void Add(object key, object value) { throw new NotImplementedException(); }
			public override bool IsReadOnly {get {return true;} }
		}
		
		/// <summary>
		/// An empty read-only IList.
		/// </summary>
		public static readonly IList EMPTY_LIST = new ReadOnlyArrayList();
		private class ReadOnlyArrayList : ArrayList {
			public override int Add(object key) { throw new NotImplementedException(); }
			public override bool IsReadOnly {get {return true;} }
		}

		///<summary>
		/// Determines if two ArrayList are intersecting, i.e. have an object in common.
		///</summary>
		/// <param name="collectionA">One of the two collections to evaluate.</param>
		/// <param name="collectionB">The other of the two collections to evaluate.</param>
		/// <returns>True if at least one object is found in both collections, otherwise false.</returns>
		/// <remarks>
		/// For performance reasons, it iterates on the smallest collection and use contains on the other.
		/// </remarks>
		public static bool AreIntersecting<T>(IList<T> collectionA, IList<T> collectionB)
		{
		    return collectionA.Count > collectionB.Count ? collectionB.Any(collectionA.Contains) : collectionA.Any(collectionB.Contains);
		}

	    ///<summary>
		/// Determines if two ArrayList are intersecting, i.e. have an object in common.
		///</summary>
		/// <param name="collectionA">One of the two collections to evaluate.</param>
		/// <param name="collectionB">The other of the two collections to evaluate.</param>
		/// <returns>True if at least one object is found in both collections, otherwise false.</returns>
		/// <remarks>
		/// For performance reasons, it iterates on the smallest collection and use contains on the other.
		/// </remarks>
		public static bool AreIntersecting(IList collectionA, IList collectionB) {
			return collectionA.Count > collectionB.Count ? collectionB.Cast<object>().Any(collectionA.Contains) : collectionA.Cast<object>().Any(collectionB.Contains);
		}
		
		/// <summary>
		/// Outputs the content of an ArrayList in a string.
		/// </summary>
		/// <param name="objects">The ArrayList to output.</param>
		/// <returns>The content of the ArrayList in a string.</returns>
		[Obsolete("This method has been deprecated. Please use Misc.IListToString instead.")]
		public static string ArrayListToString(ArrayList objects) {
			return ArrayListToString(objects, String.Empty);	
		}

		/// <summary>
		/// Outputs the content of an ArrayList in a string.
		/// </summary>
		/// <param name="objects">The ArrayList to output.</param>
		/// <param name="margin">A left margin string to place before each value.</param>
		/// <returns>The content of the ArrayList in a string.</returns>
		[Obsolete("This method has been deprecated. Please use Misc.IListToString instead.")]
		public static string ArrayListToString(ArrayList objects, string margin) {
			return IListToString(objects, margin);
		}

		/// <summary>
		/// Outputs the content of an IList in a string.
		/// </summary>
		/// <param name="objects">The IList to output.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString(IList objects) {
			return IListToString(objects, string.Empty);	
		}

		/// <summary>
		/// Outputs the content of an Array in a string.
		/// </summary>
		/// <param name="array">The Array to output.</param>
		/// <returns>The content of the Array in a string.</returns>
		public static string ArrayToString(Array array) {
			return IListToString(array, string.Empty);	
		}
		
		/// <summary>
		/// Outputs the content of a generic IList in a string.
		/// </summary>
		/// <param name="objects">The IList to output.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString<T>(IList<T> objects) {
			return IListToString((IList)objects, string.Empty);
		}
		
		/// <summary>
		/// Outputs the content of a generic IList in a string.
		/// </summary>
		/// <param name="objects">The generic IList to output.</param>
		/// <param name="margin">A left margin string to place before each value.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString<T>(IList<T> objects, string margin) {
			return IListToString((IList) objects, margin);
		}

		/// <summary>
		/// Outputs the content of an IList in a string.
		/// </summary>
		/// <param name="objects">The IList to output.</param>
		/// <param name="margin">A left margin string to place before each value.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString(IList objects, string margin) {
			var result = "(";

		    if (objects == null) return result + ")";
		    var first = true;
		    foreach (var o in objects) {
		        string stringContent;

		        var map = o as IDictionary;
		        if (map != null) stringContent = IDictionaryToString(map);
		        else if (o is IList) stringContent = IListToString((IList)o);
		        else stringContent = o.ToString();
					
		        if (margin != String.Empty) result = result + margin + stringContent + "\n";
		        else result = result + (first?String.Empty:",") + stringContent;
					
		        first = false;
		    }

		    return result + ")";	
		}
				
		/// <summary>
		/// Outputs the content of a generic IDictionary in a string.
		/// </summary>
		/// <param name="map">The IDictionary to output.</param>
		/// <returns>The content of the IDictionary in a string.</returns>
		public static string IDictionaryToString<TKey, TValue>(IDictionary<TKey, TValue> map) {
			return IDictionaryToString((IDictionary) map);
		}
		
		/// <summary>
		/// Outputs the content of an IDictionary in a string.
		/// </summary>
		/// <param name="map">The IDictionary to output.</param>
		/// <returns>The content of the IDictionary in a string.</returns>
		public static string IDictionaryToString(IDictionary map) {
			var result = "[";

		    if (map == null) return result + "]";
		    var first = true;
		    foreach(var key in map.Keys) {
		        var content = map[key];
		        string stringContent;

		        var dictionary = content as IDictionary;
		        if (dictionary != null) stringContent = IDictionaryToString(dictionary);
		        else if (content is IList) stringContent = IListToString((IList)content);
		        else stringContent = content.ToString();
					
		        result = result + (first?string.Empty:";") + key + "=" + stringContent;
					
		        first = false;
		    }

		    return result + "]";
		}

	}
}
