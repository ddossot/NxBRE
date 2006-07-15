namespace NxBRE.Util
{
	using System;
	using System.Collections;
	using System.Diagnostics;
	using System.Runtime.Remoting.Metadata.W3cXsd2001;
	using System.Xml;
	using System.Xml.Xsl;
	using System.Xml.Schema;
	
	/// <summary>Schema utilities.</summary>
	/// <author>David Dossot</author>
	public abstract class Xml {
		public const string NS_URI = "http://www.w3.org/2001/XMLSchema-instance";
		
		private Xml() {}
		
				/// <summary>
		/// Ready made identity XSLT, usefull for XML persistence
		/// </summary>
		/// <returns>An XslTransform ready to performe an identity transform</returns>
		public static XslCompiledTransform IdentityXSLT {
			get {
				return GetCachedCompiledTransform("identity.xsl");
			}
		}
		
		/// <summary>
		/// Cache of compiled XSL templates.
		/// </summary>
		/// <remarks>Inspired by patch 1516398 submitted by Koen Muilwijk</remarks>
		private static IDictionary compiledTransformCache = new Hashtable();
		
		/// <summary>
		/// Access the internal cache of XslCompiledTransform object built from embedded resources.
		/// </summary>
		/// <param name="xslResourceName">Embedded Xsl resource name</param>
		/// <returns>The XslCompiledTransform built from this resource</returns>
		internal static XslCompiledTransform GetCachedCompiledTransform(string xslResourceName) {
			lock(compiledTransformCache) {
				XslCompiledTransform result = (XslCompiledTransform) compiledTransformCache[xslResourceName];
				
				if (result == null) {
					if (Logger.IsUtilVerbose) Logger.UtilSource.TraceEvent(TraceEventType.Verbose, 0, "XslCompiledTransform cache miss for: " + xslResourceName);
					result = new XslCompiledTransform();
					result.Load(new XmlTextReader(Parameter.GetEmbeddedResourceStream(xslResourceName)), null, null);
					compiledTransformCache.Add(xslResourceName, result);
				}
				else {
					if (Logger.IsUtilVerbose) Logger.UtilSource.TraceEvent(TraceEventType.Verbose, 0, "XslCompiledTransform cache hit for: " + xslResourceName);
				}

				return result;
			}
		}
		
		/// <summary>
		/// Instantiates a new validating XML reader, with validation type being None.
		/// </summary>
		/// <param name="xmlReader"></param>
		/// <returns></returns>
		public static XmlReader NewValidatingReader(XmlReader xmlReader) {
			return NewValidatingReader(xmlReader, ValidationType.None);
		}
		
		/// <summary>
		/// Instantiates a new validating XML reader.
		/// </summary>
		/// <param name="xmlReader"></param>
		/// <param name="validationType"></param>
		/// <returns></returns>
		public static XmlReader NewValidatingReader(XmlReader xmlReader, ValidationType validationType) {
			return NewValidatingReader(xmlReader, validationType, null);
		}

		/// <summary>
		/// Instantiates a new validating XML reader.
		/// </summary>
		/// <param name="xmlReader"></param>
		/// <param name="validationType"></param>
		/// <param name="xsdResourceName"></param>
		/// <returns></returns>
		public static XmlReader NewValidatingReader(XmlReader xmlReader, ValidationType validationType, params string[] xsdResourceName) {
			if (Logger.IsUtilVerbose) Logger.UtilSource.TraceEvent(TraceEventType.Verbose, 0, "Instantiating new validating reader with validation: " + validationType + " and XSDs " + Misc.IListToString(xsdResourceName));
			
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			
			if (validationType == ValidationType.DTD) {
				xmlReaderSettings.ProhibitDtd = false;
			}
			else if ((validationType == ValidationType.Schema) && (xsdResourceName != null)) {
				XmlSchemaSet sc = new XmlSchemaSet();
				sc.Add(XmlSchema.Read(Parameter.GetEmbeddedResourceStream(xsdResourceName[0]),	null));
				xmlReaderSettings.Schemas.Add(sc);
			}
			else if (validationType != ValidationType.None) {
				throw new BREException("Validation type should be DTD, Schema or None. If Schema, a schema resource name must be passed.");
			}

			xmlReaderSettings.ValidationType = validationType;
					
			return XmlReader.Create(xmlReader, xmlReaderSettings);
		}
		
		/// <summary>
		/// Gets the best matching Xml schema type for an object.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string GetSchemaTypeFromClr(object value) {
			switch(value.GetType().FullName) {
				case "System.Uri":
					return "anyuri";
					
				case "System.Byte":
					return "unsignedbyte";
					
				case "System.Byte[]":
					return "base64binary";
					
				case "System.Boolean":
					return "boolean";
					
				case "System.SByte":
					return "byte";
					
				case "System.DateTime":
					return "datetime";
					
				case "System.Decimal":
					return "decimal";
					
				case "System.Double":
					return "double";
					
				case "System.TimeSpan":
					return "duration";
					
				case "System.String":
					return "string";
					
				case "System.Single":
					return "float";
					
				case "System.Int16":
					return "short";
					
				case "System.Int32":
					return "int";
					
				case "System.Int64":
					return "long";
					
				case "System.Xml.XmlQualifiedName":
					return "qname";
					
				case "System.UInt16":
					return "unsignedshort";
					
				case "System.UInt32":
					return "unsignedint";
					
				case "System.UInt64":
					return "unsignedlong";
					
				default:
					throw new ArgumentException("No default behavior for object of type" + value.GetType().FullName);
			}		                                       	
		}

		/// <summary>
		/// Gets an XML string out of an object, using the most appropriate XML schema type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string FromClr(object value) {
			return FromClr(value, GetSchemaTypeFromClr(value));
		}

		/// <summary>
		/// Gets an XML string out of an object following a specific XML schema type.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="schemaType"></param>
		/// <returns></returns>
		public static string FromClr(object value, string schemaType) 
		{
			switch(schemaType.ToLower()) {
				case "anyuri":
					return ((Uri)value).AbsoluteUri;
					
				case "base64binary":
					return Convert.ToBase64String((byte[])value);
					
				case "boolean":
					return XmlConvert.ToString((bool)value);
					
				case "byte":
					return XmlConvert.ToString((sbyte)value);
					
				case "date":
					return XmlConvert.ToString((DateTime)value, "yyyy-MM-dd");
					
				case "datetime":
					return XmlConvert.ToString(((DateTime)value).ToUniversalTime(), "yyyy-MM-ddTHH:mm:ssZ");
					
				case "gday":
					return XmlConvert.ToString((DateTime)value, "---dd");
					
				case "gmonth":
					return XmlConvert.ToString((DateTime)value, "--MM--");
					
				case "gmonthday":
					return XmlConvert.ToString((DateTime)value, "--MM-dd");
					
				case "gyear":
					return ((DateTime)value).Year.ToString();
					
				case "gyearmonth":
					return XmlConvert.ToString((DateTime)value, "yyyy-MM");
					
				case "time":
					return XmlConvert.ToString((DateTime)value, "HH:mm:ss");
					
				case "decimal":
				case "integer":
				case "negativeinteger":
				case "nonnegativeinteger":
				case "nonpositiveinteger":
				case "positiveinteger":
					return XmlConvert.ToString((Decimal)value);
					
				case "double":
					return XmlConvert.ToString((Double)value);
					
				case "float":
					return XmlConvert.ToString((Single)value);
					
				case "duration":
					return XmlConvert.ToString((TimeSpan)value);
					
				case "hexbinary":
					return new SoapHexBinary((byte[])value).ToString();
					
				case "entity":
				case "id":
				case "idref":
				case "language":
				case "name":
				case "ncname":
				case "nmtoken":
				case "normalizedstring":
				case "notation":
				case "string":
				case "token":
					return value.ToString();
					
				case "entities":
				case "idrefs":
				case "nmtokens":
					return String.Join(" ",(string[])value);
					
				case "short":
					return XmlConvert.ToString((short)value);
					
				case "int":
					return XmlConvert.ToString((int)value);
					
				case "long":
					return XmlConvert.ToString((long)value);
					
				case "qname":
					XmlQualifiedName xq = (XmlQualifiedName)value;
					return String.Concat(xq.Name, ":", xq.Namespace); 	
					
				case "unsignedbyte":
					return XmlConvert.ToString((byte)value);
					
				case "unsignedshort":
					return XmlConvert.ToString((UInt16)value);
					
				case "unsignedint":
					return XmlConvert.ToString((UInt32)value);
					
				case "unsignedlong":
					return XmlConvert.ToString((UInt64)value);
					
				default:
					throw new ArgumentException(schemaType + " is not a valid schema type");
			}
		}
		
		/// <summary>
		/// Gets an object instance out of an XML string and an XML schema type.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="schemaType"></param>
		/// <returns></returns>
		public static object ToClr(string value, string schemaType) {
			switch(schemaType.ToLower()) {
       	case "anyuri":
       	return new Uri(value);
       	
       	case "base64binary":
       	return Convert.FromBase64String(value);
       	
				case "boolean":
					return XmlConvert.ToBoolean(value);
				
				case "byte":
					return XmlConvert.ToSByte(value);
				
				case "date":
				case "datetime":
				case "gday":
				case "gmonth":
				case "gmonthday":
				case "gyear":
				case "gyearmonth":
				case "time":
					return XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc);
				
				case "decimal":
				case "integer":
				case "negativeinteger":
				case "nonnegativeinteger":
				case "nonpositiveinteger":
				case "positiveinteger":
					return XmlConvert.ToDecimal(value);
				
				case "double":
					return XmlConvert.ToDouble(value);
				
				case "duration":
					return XmlConvert.ToTimeSpan(value);
				
				case "entities":
				case "idrefs":
				case "nmtokens":
					return value.Split(' ');
       	
       	case "entity":
				case "id":
				case "idref":
				case "language":
				case "name":
				case "ncname":
				case "nmtoken":
				case "normalizedstring":
				case "notation":
				case "string":
				case "token":
       		return value;
       	
				case "float":
					return XmlConvert.ToSingle(value);
				
				case "hexbinary":
					return SoapHexBinary.Parse(value).Value;
       	
				case "int":
					return XmlConvert.ToInt32(value);
					
				case "long":
					return XmlConvert.ToInt64(value);
					
				case "qname":
					string[] qNameTuple = value.Split(':');
					return new XmlQualifiedName(qNameTuple[0], qNameTuple[1]);
					
				case "short":
					return XmlConvert.ToInt16(value);

				case "unsignedbyte":
					return XmlConvert.ToByte(value);

				case "unsignedint":
					return XmlConvert.ToUInt32(value);
					
				case "unsignedlong":
					return XmlConvert.ToUInt64(value);
					
				case "unsignedshort":
					return XmlConvert.ToUInt16(value);
					
       	default:
					throw new ArgumentException(schemaType + " is not a valid schema type");
			}
		}
		
	}
}
