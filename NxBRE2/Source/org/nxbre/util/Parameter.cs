namespace org.nxbre.util
{
	using System;
	using System.Collections;
	using System.Configuration;

	/// <summary>An helper class for easily accessing NxBRE's application settings and manipulating parameters.
	/// </summary>
	/// <author>David Dossot</author>
	/// <version>2.0</version>
	public abstract class Parameter {
		public readonly static char[] PARENTHESIS = new char[] {'(', ')'};
		public const char COLUMN = ':';
		
		private Parameter() {}
		
		private static AppSettingsReader appSettingsReader = null;
		
		///<summary>Gets the string value of the config file entry.</summary>
		/// <param name="settingKey">The NXBRE setting key, which is automatically prefixed by "nxbre."</param>
		/// <param name="defaultValue">The default value to use in case the configuration entry is not found.</param>
		/// <returns>The config entry string value, unless it is not found then it returns the default value.</returns>
		public static string GetString(string settingKey, string defaultValue) {
			if (null == appSettingsReader)
				appSettingsReader = new AppSettingsReader();

			string settingValue;
			
			try {
				settingValue = (string)appSettingsReader.GetValue("nxbre." + settingKey, typeof(string));
			} catch(InvalidOperationException) {
				//something went wrong --> use the default value
				settingValue = defaultValue;
			}
			
			return settingValue;
		}
		
		///<summary>Gets the string value of the config file entry.</summary>
		/// <param name="settingKey">The NXBRE setting key, which is automatically prefixed by "nxbre."</param>
		/// <returns>The config entry string value, unless it is not found then it returns null.</returns>
		public static string GetString(string settingKey) {
			return GetString(settingKey, null);
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
			string result = String.Empty;
			if ((source != null) && (tag != null)) {
				int posTag = source.IndexOf(tag + COLUMN);
				if (posTag >= 0)
					result = source.Substring(posTag + tag.Length + 1).Split(';')[0];
			}
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
			
			int posLastSame = arguments.Length;
			for(int i=arguments.Length-2;i>=0;i--) {
				Type t = arguments[i].GetType();
				if (arguments[i].GetType() == arguments[i+1].GetType()) posLastSame = i;
				else break;
			}
			if (posLastSame != arguments.Length) {
				// create a new Array and copy the non grouped arguments
				object[] result = new object[posLastSame + 1];
				Array.Copy(arguments, result, result.Length - 1);
				Array parameters = Array.CreateInstance(arguments[posLastSame].GetType(), arguments.Length - posLastSame);
				int j=0;
				for(int i=posLastSame;i<arguments.Length;i++) {
					parameters.SetValue(arguments[i], j);
					j++;
				}
				result[result.Length - 1] = parameters;
				return result;
			}
			else
				// no grouping possible
				return arguments;
		}
	
		/// <summary>
		/// Groups the final objects of an array of arguments in an array, in order to reach
		/// a certain number of arguments.
		/// </summary>
		/// <param name="arguments">An array of arguments.</param>
		/// <param name="lengthToReach">The desired length for the new array of arguments.</param>
		/// <returns>A new array of arguments, with the final objects grouped in an array.</returns>
		public static object[] GroupFinal(object[] arguments, int lengthToReach, Type targetType) {
			int numberToGroup = arguments.Length - lengthToReach + 1;
			if ((numberToGroup) < 2) return arguments;
			// create a new Array and copy the non grouped arguments
			object[] result = new object[lengthToReach];
			Array.Copy(arguments, result, lengthToReach-1);
			
			// group the last arguments
			if (targetType.IsArray) targetType = targetType.GetElementType();
			Array grouped = Array.CreateInstance(targetType, numberToGroup);
			for (int i=0; i<numberToGroup; i++) grouped.SetValue(arguments[lengthToReach + i - 1], i);
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
			string functionSignature = TrimPrefix(functionName.Split(PARENTHESIS)[0]);
			
			for(int i=0; i<arguments.Length; i++) functionSignature += "_Arg" + i;
			
			return functionSignature;
		}
		
		/// <summary>
		/// Builds a formula signature that takes in account the name and the string arguments passed in the parenthesis.
		/// </summary>
		/// <param name="formula">The complete formula.</param>
		/// <returns>A string that represents the function signature.</returns>
		internal static FormulaSignature BuildFormulaSignature(string formula) {
			string[] formulaTokens = formula.Split(PARENTHESIS);
			string formulaName = TrimPrefix(formulaTokens[0]);

			if (formulaTokens.Length > 1) return new FormulaSignature(formulaName, formulaTokens[1]);
			else return new FormulaSignature(formulaName);
		}
		
		/// <summary>
		/// Trims the prefix that could prepend a formula (like "expr:" in "expr:XYZ")
		/// </summary>
		/// <param name="formulaName">The formula name, which could be prefixed</param>
		/// <returns>The unprefixed formula name</returns>
		private static string TrimPrefix(string formulaName) {
			int posFirstColumn = formulaName.IndexOf(COLUMN);
			if (posFirstColumn >= 0) return formulaName.Substring(posFirstColumn + 1);
			else return formulaName;
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
				foreach(string argument in arguments.Split(','))
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
		
		public ObjectPair():this(null, null){}
		
		public ObjectPair(object first, object second) {
			this.first = first;
			this.second = second;
		}
		
		public object First {
			get {
				return first;
			}
			set {
				first = value;
			}
		}
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
