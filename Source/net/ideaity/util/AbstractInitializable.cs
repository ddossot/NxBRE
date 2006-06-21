namespace net.ideaity.util
{
	using System;
	using System.Collections;
	
	/// <summary> Define a generic interface that allows developers to ensure
	/// that an object, upon newInstance() or creation, has a callable
	/// method to initialize the object.
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public abstract class AbstractInitializable : IInitializable
	{
		public abstract bool Init(object aObj);
		
		/// <summary> Returns a list of all initialization parameters
		/// *
		/// </summary>
		/// <returns> Object array containing the initialization parameters
		/// 
		/// </returns>
		virtual public System.Object[] GetInitParams()
		{
			return initParams;
		}
		
		/// <summary> Returns a list of the required initialization parameters
		/// *
		/// </summary>
		/// <returns> Object array containing the required initialization parameters
		/// 
		/// </returns>
		virtual public System.Object[] GetReqInitParams()
		{
				return requiredInitParams;
		}
		
		/// <summary> Returns the number of init parameters
		/// *
		/// </summary>
		/// <returns> Total number of  init parameters
		/// 
		/// </returns>
		virtual public int InitParamsCount
		{
			get
			{
				return initParams.Length;
			}
			
		}
		/// <summary> Returns the number of required parameters
		/// *
		/// </summary>
		/// <returns> Total number of required parameters
		/// 
		/// </returns>
		virtual public int ReqInitParamsCount
		{
			get
			{
				return requiredInitParams.Length;
			}
			
		}
		/// <summary> Sets the required initialization parameters
		/// *
		/// </summary>
		/// <param name="value">Array of required initialization parameters
		/// 
		/// </param>
		protected internal static void SetReqInitParams(System.Object[] value)
		{
				requiredInitParams = value;
		}
		/// <summary> Sets the initialization parameters
		/// *
		/// </summary>
		/// <param name="value">Array of  initialization parameters
		/// 
		/// </param>
		protected internal static void SetInitParams(System.Object[] value)
		{
				initParams = value;
		}
		
		private static System.Object[] requiredInitParams = null;
		
		private static System.Object[] initParams = null;
		
		/// <summary> Protected constructor
		/// *
		/// </summary>
		/// <param name="aReqInitParams">The required initialization parameters
		/// </param>
		/// <param name="aInitParams">All initialization parameters
		/// 
		/// </param>
		protected internal AbstractInitializable(System.Object[] aReqInitParams, System.Object[] aInitParams)
		{
			requiredInitParams = aReqInitParams;
			initParams = aInitParams;
		}
		
		/// <summary> Checks required parameters against the passed Map and throws MissingResourceException
		/// if required parameter is not found
		/// *
		/// </summary>
		/// <param name="aMap">The map to check
		/// @throws MissingResourceException if parameter is not found
		/// 
		/// </param>
		protected internal virtual bool checkInitParamsReq(Hashtable aMap)
		{
			for (int i = 0; i < requiredInitParams.Length; i++)
			{
				object key = requiredInitParams[i];
				if (!aMap.ContainsKey(key))
					throw new SystemException("Required initialization parameter not found: " + key.GetType().FullName + "=" + key.ToString());
			}
			return true;
		}
		
		/// <summary> Checks to see if the passed Map contains the required initialization
		/// parameters
		/// *
		/// </summary>
		/// <param name="aMap">The map to check
		/// 
		/// </param>
		public virtual bool isParamsReqMet(Hashtable aMap)
		{
			for (int i = 0; i < requiredInitParams.Length; i++)
			{
				if (!aMap.ContainsKey(requiredInitParams[i]))
				{
					return false;
				}
			}
			return true;
		}
		
		
		
		
		
	}
}
