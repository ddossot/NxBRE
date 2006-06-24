namespace NxBRE.Util
{
	using System;
	using System.Runtime.Remoting.Metadata.W3cXsd2001;
	using System.Xml;
	
	using NxBRE.FlowEngine;

	/// <summary>Schema utilities.</summary>
	/// <author>David Dossot</author>
	public abstract class Schema {
		public const string NS_URI = "http://www.w3.org/2001/XMLSchema-instance";
		
		private Schema() {}
		
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
		
		public static string FromClr(object value) {
			return FromClr(value, GetSchemaTypeFromClr(value));
		}

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
					return XmlConvert.ToDateTime(value);
				
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
