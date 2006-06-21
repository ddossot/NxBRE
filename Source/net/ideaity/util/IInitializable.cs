namespace net.ideaity.util
{
	using System;
	
	/// <summary> Define a generic interface that allows developers to ensure
	/// that an object, upon newInstance() or creation, has a callable
	/// method to initialize the object.
	/// *
	/// </summary>
	/// <author>  Sloan Seaman
	/// </author>
	/// <version>  .90
	/// </version>
	public interface IInitializable
		{
			/// <summary> Initialized the object
			/// *
			/// </summary>
			/// <param name="aObj">An object that is passed upon initialization
			/// </param>
			/// <returns> True is successful, False otherwise
			/// @throws Exception if there is an initialization error
			/// 
			/// </returns>
			bool Init(object aObj);
		}
}
