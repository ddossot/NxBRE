namespace NxBRE.InferenceEngine.IO {
	using System;

	using NxBRE.Util;

	/// <summary>
	/// This class is a helper for instantiating IBinder objects from CSharp files,
	/// giving the performance of a binder embedded in the main application and the
	/// flexibility of a file-based external binder.
	/// </summary>
	public abstract class CSharpBinderFactory {
		
		private CSharpBinderFactory() {}
		
		/// <summary>
		/// Instantiates a new IBinder class.
		/// </summary>
		/// <param name="binderClassName">The fully qualified class name of the binder class.</param>
		/// <param name="binderClassFileName">The full path of the file containing the C# source code of the class.</param>
		/// <returns>A new instance of IBinder based on the class definition.</returns>
		public static IBinder LoadFromFile(string binderClassName, string binderClassFileName) {
			return (IBinder)Compilation.LoadCSClass(binderClassName, binderClassFileName, false);
		}
		
		/// <summary>
		/// Instantiates a new IBinder class.
		/// </summary>
		/// <param name="binderClassName">The fully qualified class name of the binder class.</param>
		/// <param name="binderClassSource">The C# source code of the class.</param>
		/// <returns>A new instance of IBinder based on the class definition.</returns>
		public static IBinder LoadFromString(string binderClassName, string binderClassSource) {
			return (IBinder)Compilation.LoadCSClass(binderClassName, binderClassSource, true);
		}
		
	}
}
