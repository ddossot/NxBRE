namespace NxBRE.Util
{
	using System;
	using System.Collections;
	using System.Globalization;
	using System.Reflection;
	
	/// <summary>NxBRE utilities for manipulations on objects: reflection, type casting...</summary>
	/// <author>David Dossot</author>
	public abstract class Reflection {
		/// <summary>
		/// When casting values, NxBRE uses this format: en-US.
		/// </summary>
		public static CultureInfo CULTURE_INFO = new CultureInfo(Parameter.Get<string>("cultureInfo", "en-US"), false);

		/// <summary>
		/// Version of the current NxBRE engine.
		/// </summary>
		public static readonly string NXBRE_VERSION = Assembly.GetAssembly(typeof(NxBRE.Util.Reflection)).GetName().Version.ToString();
		
		private Reflection() {}
		
		/// <summary>
		/// Tests if an object is null.
		/// </summary>
		/// <param name="o">The object to test.</param>
		/// <returns>True if null was passed as argument.</returns>
		public static bool IsNull(object o) {
			return (null==o);
		}
		
		/// <summary>
		/// Casts a certain valueObject into a certain type, by using ChangeType when possible
		/// (i.e. except for exceptions).
		/// </summary>
		/// <param name="valueObject">The object to cast</param>
		/// <param name="typeName">The full name of the type to cast to</param>
		/// <returns>A properly casted object, unless an exception occurs.</returns>
		public static object CastValue(object valueObject, string typeName) {
			return CastValue(valueObject, Type.GetType(typeName));
		}

		/// <summary>
		/// Casts a certain valueObject into a certain type, by using ChangeType when possible
		/// (i.e. except for exceptions).
		/// </summary>
		/// <param name="valueObject">The object to cast</param>
		/// <param name="type">The type to cast to</param>
		/// <returns>A properly casted object, unless an exception occurs.</returns>
		public static object CastValue(object valueObject, Type type) {
			if (type == typeof(System.Exception))
				return new System.Exception((string)valueObject);
			// fix to bug 1474032 provided by Brian Matthews
			else if (type.IsEnum)
				return Enum.Parse(type, valueObject.ToString(), false);
			else
				return Convert.ChangeType(valueObject, type, CULTURE_INFO);
		}

		/// <summary>
		/// Converts either object1 to the type of object2, or the contrary, based on the object having
		/// the weakest type, i.e. System.String. If none of the objects are System.String
		/// object2 will be casted to object1.
		/// </summary>
		/// <param name="pair">A reference to the pair of objects to cast.</param>
		public static void CastToStrongType(ObjectPair pair) {
			if ((pair.First.GetType().GetInterface("System.IConvertible", false) != null) &&
			    (pair.Second.GetType().GetInterface("System.IConvertible", false) != null)) {
				// if they are both convertible, convert to the strongest type (non string)
				// or arbitrary cast the second to the first
			  if (pair.First is System.String) pair.First = CastValue(pair.First, pair.Second.GetType());
			  else pair.Second = CastValue(pair.Second, pair.First.GetType());
			}
		}
		
		/// <summary>
		/// Calls a member on an object, either to get or set an attribute or property,
		/// or to invoke a method.
		/// </summary>
		/// <param name="target">The object on which the member will be called.</param>
		/// <param name="name">The name of the member to call.</param>
		/// <param name="argValues">An array of arguments. Use null if no argument is needed.</param>
		/// <returns>The value returned when the member is a method, else null.</returns>
		public static object ObjectCall(object target, string name, object[] argValues) {
			return Call(target.GetType(), target, name, argValues);
		}
		
		/// <summary>
		/// Calls a static member on a class, either to get or set an attribute or property,
		/// or to invoke a method.
		/// </summary>
		/// <param name="type">The fully qualified name of the class to call.</param>
		/// <param name="name">The name of the member to call.</param>
		/// <param name="argValues">An array of arguments. Use null if no argument is needed.</param>
		/// <returns>The value returned when the member is a method, else null.</returns>
		public static object ClassCall(string type, string name, object[] argValues) {
			return Call(GetRuntimeType(type), null, name, argValues);
		}
		
		/// <summary>
		/// Instantiates a new object.
		/// </summary>
		/// <param name="type">The fully qualified name of the class to instantiate.</param>
		/// <param name="argValues">An array of contructor arguments. Use null if no argument is needed.</param>
		/// <returns></returns>
		public static object ClassNew(string type, object[] argValues) {
			if (argValues == null) argValues = new object[0];
			Type toInstantiate = GetRuntimeType(type);
			ConstructorInfo ci = toInstantiate.GetConstructor(GetArgumentTypes(argValues));
			if (ci == null)
				throw new TargetException("No matching constructor found on "+type);
			return ci.Invoke(argValues);
		}
		
		// Private methods ------------------------------------------------------------------
		
		private static object Call(Type type, object target, string name, object[] argValues) {
			int nbOfProvidedArgs = 0;
			if (argValues != null) nbOfProvidedArgs = argValues.Length;
	
	 		// Check if it is a field name
			FieldInfo fi = type.GetField(name);
			if (fi != null) {
				if (nbOfProvidedArgs > 0) {
					if (nbOfProvidedArgs == 1) {
						fi.SetValue(target, argValues[0]);
						return null;
					}
					else throw new TargetException(nbOfProvidedArgs+" argument(s) provided for field "+target+"."+name+" when 1 expected");
				}
				else return fi.GetValue(target);
			}
	
			// Check if it is a property name
			MethodInfo mi = null;
			PropertyInfo pi;
			if (nbOfProvidedArgs > 0) {
				// If we have arguments passed, try to locate a property mathing those
				pi = type.GetProperty(name, GetArgumentTypes(argValues));
				if (pi != null) {
					// If the property is found, give priority to getter
					// This is of course a limitation, for very specific things 
					// NxBRE users will enjoy implementing code delegates!
					mi = pi.GetGetMethod();
					if (mi == null) mi = pi.GetSetMethod();
				}
				else {
					// If the property is found, try to find a basic non-argument property
					// and target the set method as we have arguments passed
					pi = type.GetProperty(name);
					if (pi != null) mi = pi.GetSetMethod();
				}
			}
			else {
				// Having no arguments passed, we clearly want a getter.
				pi = type.GetProperty(name);
				if (pi != null) mi = pi.GetGetMethod();
			}
	
			// check if it is a method name
			Type[] types = GetArgumentTypes(argValues);
			if (mi == null) mi = type.GetMethod(name, types);
			
			// let's try by group the identical arguments of same types to try them as a param array
			if (mi == null) {
				object[] reArgValues = Parameter.GroupFinal(argValues);
				if (!Array.ReferenceEquals(argValues, reArgValues)) {
					types = GetArgumentTypes(reArgValues);
					mi = type.GetMethod(name, types);
					if (mi != null) argValues = reArgValues;
				}
			}

			// the last option is to try to find if there is a single method with the desired name
			if (mi == null)	{
				try {
					mi = type.GetMethod(name);
				} catch(AmbiguousMatchException) {}
			}
			
			// if something has been found
			if (mi != null)	{
				// if the number of arguments match, perform the invocation
				if (argValues.Length == mi.GetParameters().Length)
					return mi.Invoke(target, argValues);
				else {
					// try a last trick by grouping the last arguments in an array
					// in order to reach the right number of arguments
					object[] reArgValues = Parameter.GroupFinal(argValues,
					                                            mi.GetParameters().Length,
					                                            mi.GetParameters()[mi.GetParameters().Length-1].ParameterType);
					return mi.Invoke(target, reArgValues);
				}
			}
			
			// nothing found
			throw new TargetException("Can not find member "+type.FullName+"."+name);
		}
		
		private static Type GetRuntimeType(string type) {
			Type runtimeType = Type.GetType(type, true);
			if (runtimeType == null) throw new TargetException("Can not find class type "+type);
			return runtimeType;			
		}
		
		private static Type[] GetArgumentTypes(object[] arguments) {
			ArrayList argumentTypes = new ArrayList();
			for(int i=0;i<arguments.Length;i++)
				if (arguments[i] != null)
					argumentTypes.Add(arguments[i].GetType());
				else
					argumentTypes.Add(typeof(Object));
			return (Type[])argumentTypes.ToArray(typeof(Type));
		}	
	}
}
