namespace org.nxbre.util
{
	using System;
	using System.Collections;
	using System.IO;
	using System.Xml;
	using System.Xml.Xsl;
	
	using org.nxbre.rule;

	/// <summary>Misc NxBRE utilities.</summary>
	/// <author>David Dossot</author>
	/// <version>2.0</version>
	public abstract class Misc {
		private Misc() {}
		
		/// <summary>
		/// The identity XSL.
		/// </summary>
		public const string IDENTITY_XSL = "<xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\"><xsl:template match=\"@*|node()\"><xsl:copy><xsl:apply-templates select=\"@*|node()\"/></xsl:copy></xsl:template></xsl:stylesheet>";
		
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

		/// <summary>
		/// Ready made identity XSLT, usefull for XML persistence
		/// </summary>
		/// <returns>An XslTransform ready to performe an identity transform</returns>
		public static XslTransform IdentityXSLT {
			get {
			XslTransform xslt = new XslTransform();
			xslt.Load(new XmlTextReader(new StringReader(IDENTITY_XSL)), null, null);
			return xslt;
			}
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
		public static bool AreIntersecting(ArrayList collectionA, ArrayList collectionB) {
			if (collectionA.Count > collectionB.Count) {
				foreach(object o in collectionB)
					if (collectionA.Contains(o))
						return true;
			}
			else {
				foreach(object o in collectionA)
					if (collectionB.Contains(o))
						return true;
			}
			return false;
		}

		/// <summary>
		/// Outputs the content of an ArrayList in a string.
		/// </summary>
		/// <param name="objects">The ArrayList to output.</param>
		/// <returns>The content of the ArrayList in a string.</returns>
		public static string ArrayListToString(ArrayList objects) {
			return ArrayListToString(objects, String.Empty);	
		}

		/// <summary>
		/// Outputs the content of an ArrayList in a string.
		/// </summary>
		/// <param name="objects">The ArrayList to output.</param>
		/// <param name="margin">A left margin string to place before each value.</param>
		/// <returns>The content of the ArrayList in a string.</returns>
		public static string ArrayListToString(ArrayList objects, string margin) {
			return IListToString(objects, margin);
		}

		/// <summary>
		/// Outputs the content of an IList in a string.
		/// </summary>
		/// <param name="objects">The IList to output.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString(IList objects) {
			return IListToString(objects, String.Empty);	
		}

		/// <summary>
		/// Outputs the content of an IList in a string.
		/// </summary>
		/// <param name="objects">The IList to output.</param>
		/// <param name="margin">A left margin string to place before each value.</param>
		/// <returns>The content of the IList in a string.</returns>
		public static string IListToString(IList objects, string margin) {
			string result = String.Empty;
			
			if (objects != null) {
				foreach (object o in objects) {
					result = result + margin + ((o is IList)?String.Concat("{", IListToString((IList)o, margin), "}"):o.ToString()) + "\n";
				}
			}
				
			return result;	
		}
		
		/// <summary>
		/// Outputs the content of an IDictionary in a string.
		/// </summary>
		/// <param name="map">The IDictionary to output.</param>
		/// <returns>The content of the IDictionary in a string.</returns>
		public static string IDictionaryToString(IDictionary map) {
			string result = "[";

			if (map != null) {
				foreach(object key in map.Keys) {
					object content = map[key];
					
					if (content is IDictionary) content = IDictionaryToString((IDictionary)content);
					else if (content is IList) content = IListToString((IList)content);
					
					result = result + key + "=" + content + ";";
				}
			}
				
			return result + "]";
		}
		
		/// <summary>
		/// Returns either the string value if o is a string, else a string representation of its hashcode.
		/// </summary>
		/// <param name="o"></param>
		/// <returns></returns>
		public static string GetStringHashcode(object o) {
			return (o is string)?(string)o:o.GetHashCode().ToString();
		}
		
	}
}
