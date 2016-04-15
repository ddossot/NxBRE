namespace NxBRE.Util
{
	using System;
	using System.IO;
	using System.Collections;
	using System.Configuration;
	using System.Diagnostics;
	using System.Reflection;

	/// <summary>An helper class for easily accessing NxBRE's application settings and manipulating parameters.
	/// </summary>
	/// <author>David Dossot</author>
	public abstract class Parameter {
		
		/// <summary>
		/// Chars that NxBRE recognizes as parenthesis.
		/// </summary>
		public readonly static char[] PARENTHESIS = new char[] {'(', ')'};
		
		/// <summary>
		/// Char that NxBRE recognizes as a column.
		/// </summary>
		public const char COLUMN = ':';
		
		private Parameter() {}
		
		private static AppSettingsReader appSettingsReader = new AppSettingsReader();
		
		private static string embeddedResourcePrefix = Parameter.Get<string>("embeddedResourcePrefix", "NxBRE.Resources.");
		
		/// <summary>
		/// Gets a Stream for a resource embedded in the assembly.
		/// </summary>
		/// <param name="resourceName">The name of the resource to get.</param>
		/// <returns>The Stream that matches the passed resource name, or an exception if it can not be read.</returns>
		public static Stream GetEmbeddedResourceStream(string resourceName) {
			string fullResourceName = embeddedResourcePrefix + resourceName;
			
			if (Logger.IsUtilVerbose) Logger.UtilSource.TraceEvent(TraceEventType.Verbose, 0, "Fetching resource '" + fullResourceName + "' from assembly manifest");
			
			return Reflection.NxBREAssembly.GetManifestResourceStream(fullResourceName);
		}
		
		///<summary>Gets the string value of the config file entry.</summary>
		/// <param name="settingKey">The NxBRE setting key, which is automatically prefixed by "nxbre."</param>
		/// <param name="defaultValue">The default value to use in case the configuration entry is not found.</param>
		/// <returns>The config entry string value, unless it is not found then it returns the default value.</returns>
		public static T Get<T>(string settingKey, T defaultValue) {
			T settingValue;
			
			try {
				settingValue = (T)appSettingsReader.GetValue("nxbre." + settingKey, typeof(T));
			} catch(Exception e) {
				//something went wrong --> use the default value
				settingValue = defaultValue;
				
				if (Logger.IsUtilInformation) Logger.UtilSource.TraceEvent(TraceEventType.Information,
				                                                           0,
				                                                           "Can not find setting key: '"
				                                                           	+ settingKey
				                                                           	+ "', using default value: '"
				                                                           	+ defaultValue
				                                                           	+ "' (Exception message: "
				                                                           	+ e.Message
				                                                           	+ ")");
			}
			
			return settingValue;
		}
		
		/// <summary>
		/// Gets the enum value of the config file entry.
		/// </summary>
		/// <param name="settingKey">The NxBRE setting key, which is automatically prefixed by "nxbre."</param>
		/// <param name="enumType">The type of Enum.</param>
		/// <param name="defaultValue">The default value to use in case the configuration entry is not found.</param>
		/// <returns>The config entry enum value, unless it is not found then it returns the default value.</returns>
		public static object GetEnum(string settingKey, Type enumType, object defaultValue) {
			try
			{
			    // I wish I could have used a constrained generic for this method but Enums can not be used as contraints...
				var settingValue = Get<string>(settingKey, null);
			    return settingValue == null ? defaultValue : Enum.Parse(enumType, settingValue, false);
			}
			catch (Exception e) {
				// exceptions at this stage might be confusing, let's throw a more detailed one
				throw new BREException("Configuration exception when parsing enum value of type: '"
				                       + enumType.ToString()
				                       + "' for setting key: '"
				                       + settingKey
				                       + "'",
				                      e);
			}
		}
		
		///<summary>Gets the string value of the config file entry.</summary>
		/// <param name="settingKey">The NXBRE setting key, which is automatically prefixed by "nxbre."</param>
		/// <returns>The config entry string value, unless it is not found then it returns null.</returns>
		public static string GetString(string settingKey) {
			return Get<string>(settingKey, null);
		}
		
		///<summary>
		/// Returns a value from a tagged string.
		/// The tag is separated from the value by a column (:).
		/// Each tag:value pair is separated from the other by a semi-column (;).
		/// The final semi-column is optional.
		/// Example: if source is GetTaggedInfo("a:1;b:2", "a") returns "1".
		/// </summary>
		/// <param name="source">The string containing tagged info.</param>
		/// <param name="tag">The sought tag, without column (:).</param>
		/// <returns>The value from the selected tag.</returns>
		public static string GetTaggedInfo(string source, string tag) {
			var result = string.Empty;
		    if ((source == null) || (tag == null)) return result;
		    var posTag = source.IndexOf(tag + COLUMN, StringComparison.Ordinal);
		    if (posTag >= 0)
		        result = source.Substring(posTag + tag.Length + 1).Split(';')[0];
		    return result;
		}

		/// <summary>
		/// Analyzes an array of arguments and group in an array the final ones that would be of same type.
		/// </summary>
		/// <remarks>
		/// This is very useful when calling a method with a final argument being a param array.
		/// </remarks>
		/// <param name="arguments">An array of arguments.</param>
		/// <returns>A new array of arguments, potentially grouped.</returns>
		public static object[] GroupFinal(object[] arguments) {
			if (arguments.Length < 2)	return arguments;
			
			var posLastSame = arguments.Length;
			for(var i=arguments.Length-2;i>=0;i--) {
				if ((arguments[i] != null) &&
				    (arguments[i+1] != null) &&
				    (arguments[i].GetType() == arguments[i+1].GetType())) {
					posLastSame = i;
				}
				else {
					break;
				}
			}
			if (posLastSame != arguments.Length) {
				// create a new Array and copy the non grouped arguments
				var result = new object[posLastSame + 1];
				Array.Copy(arguments, result, result.Length - 1);
				var parameters = Array.CreateInstance(arguments[posLastSame].GetType(), arguments.Length - posLastSame);
				var j=0;
				for(var i=posLastSame;i<arguments.Length;i++) {
					parameters.SetValue(arguments[i], j);
					j++;
				}
				result[result.Length - 1] = parameters;
				return result;
			}
		    return arguments;
		}
	
		/// <summary>
		/// Groups the final objects of an array of arguments in an array, in order to reach
		/// a certain number of arguments.
		/// </summary>
		/// <param name="arguments">An array of arguments.</param>
		/// <param name="lengthToReach">The desired length for the new array of arguments.</param>
		/// <param name="targetType">The type of array that will contain the grouped arguments.</param>
		/// <returns>A new array of arguments, with the final objects grouped in an array.</returns>
		public static object[] GroupFinal(object[] arguments, int lengthToReach, Type targetType) {
			var numberToGroup = arguments.Length - lengthToReach + 1;
			if ((numberToGroup) < 2) return arguments;
			// create a new Array and copy the non grouped arguments
			var result = new object[lengthToReach];
			Array.Copy(arguments, result, lengthToReach-1);
			
			// group the last arguments
			if (targetType.IsArray) targetType = targetType.GetElementType();
			var grouped = Array.CreateInstance(targetType, numberToGroup);
			for (var i=0; i<numberToGroup; i++) grouped.SetValue(arguments[lengthToReach + i - 1], i);
			result[lengthToReach-1] = grouped;
			
			return result;
		}
		
		/// <summary>
		/// Builds a function signature that takes in account the name and the number of arguments.
		/// </summary>
		/// <param name="functionName">The name of the function.</param>
		/// <param name="arguments">An array of arguments.</param>
		/// <returns>A string that represents the function signature.</returns>
		internal static string BuildFunctionSignature(string functionName, object[] arguments) {
			var functionSignature = TrimPrefix(functionName.Split(PARENTHESIS)[0]);
			
			for(var i=0; i<arguments.Length; i++) functionSignature += "_Arg" + i;
			
			return functionSignature;
		}
		
		/// <summary>
		/// Extracts the operator name and its unique argument that must be between parenthesis.
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		internal static ObjectPair ParseOperatorCall(string source) {
			var indexOfFirstOpeningParenthesis = source.IndexOf('(');
			var operatorName = source.Substring(0, indexOfFirstOpeningParenthesis);
			
			var indexOfLastClosingParenthesis = source.LastIndexOf(')');
			var operatorArgument = source.Substring(indexOfFirstOpeningParenthesis + 1, indexOfLastClosingParenthesis - indexOfFirstOpeningParenthesis -1);
				
			return new ObjectPair(operatorName, operatorArgument);
		}
		
		/// <summary>
		/// Builds a formula signature that takes in account the name and the string arguments passed in the parenthesis.
		/// </summary>
		/// <param name="formula">The complete formula.</param>
		/// <returns>A string that represents the function signature.</returns>
		internal static FormulaSignature BuildFormulaSignature(string formula) {
			var formulaTokens = formula.Split(PARENTHESIS);
			var formulaName = TrimPrefix(formulaTokens[0]);

			return formulaTokens.Length > 1 ? new FormulaSignature(formulaName, formulaTokens[1]) : new FormulaSignature(formulaName);
		}
		
		/// <summary>
		/// Trims the prefix that could prepend a formula (like "expr:" in "expr:XYZ")
		/// </summary>
		/// <param name="formulaName">The formula name, which could be prefixed</param>
		/// <returns>The unprefixed formula name</returns>
		private static string TrimPrefix(string formulaName)
		{
		    var posFirstColumn = formulaName.IndexOf(COLUMN);
		    return posFirstColumn >= 0 ? formulaName.Substring(posFirstColumn + 1) : formulaName;
		}
	}
	
	/// <summary>
	/// A internal immutable class used as a return value for method BuildFormulaSignature.
	/// </summary>
	internal class FormulaSignature {
		private readonly string name;
		private readonly IList arguments;
		
		public FormulaSignature(string name):this(name, null){}
		
		public FormulaSignature(string name, string arguments)
		{
			this.name = name;
			
			IList tempArgumentList = new ArrayList();
			
			if (arguments != null)
				foreach(var argument in arguments.Split(','))
					tempArgumentList.Add(argument.Trim());
			
			this.arguments = ArrayList.ReadOnly(tempArgumentList);
		}
		
		public IList Arguments {
			get {
				return arguments;
			}
		}
		
		public string Name {
			get {
				return name;
			}
		}
	}
	
	/// <summary>
	/// A class designed for olding pairs of objects
	/// </summary>
	public class ObjectPair {
		private object first;
		private object second;
		
		/// <summary>
		/// Instantiates a new pair object with no reference (null) to any object.
		/// </summary>
		public ObjectPair():this(null, null){}
		
		/// <summary>
		/// Instantiates a new pair object with references to the provided objects.
		/// </summary>
		/// <param name="first">Reference of the first object in the pair.</param>
		/// <param name="second">Reference of the second object in the pair.</param>
		public ObjectPair(object first, object second) {
			this.first = first;
			this.second = second;
		}
		
		/// <summary>
		/// Gets or sets the reference of the first object in the pair.
		/// </summary>
		public object First {
			get {
				return first;
			}
			set {
				first = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the reference of the second object in the pair.
		/// </summary>
		public object Second {
			get {
				return second;
			}
			set {
				second = value;
			}
		}
	}
	
	/// <summary>
	/// An immutable class designed for storing an hyperlink
	/// </summary>
	public class HyperLink {
		private readonly string uri;
		private readonly string text;
		
		public HyperLink(string text, string uri)
		{
			this.text = text;
			this.uri = uri;
		}
		
		public string Text {
			get {
				return text;
			}
		}
		
		public string Uri {
			get {
				return uri;
			}
		}
		
	}
	
}
