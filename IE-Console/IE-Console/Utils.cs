namespace NxBRE.InferenceEngine.Console {
	using System;
	using System.Collections;
	using System.IO;
	using System.Reflection;
	using Microsoft.Win32;

	/// <summary>
	/// Basic Console for NxBRE Inference Engine
	/// </summary>
	/// <remarks>
	/// Shamlessly uncommented code.
	/// </remarks>
	public abstract class Utils {
		private Utils() {}
		
		private class AssemblyNameSorter:IComparer  {
	    int IComparer.Compare(Object x, Object y)  {
				return((new CaseInsensitiveComparer()).Compare(((AssemblyName)x).Name, ((AssemblyName)y).Name));
	    }
	  }

		private static void LoadDLLInFolder(string folder, string pattern) {
			if (folder == String.Empty) return;
			foreach(FileInfo actFileInfo in new DirectoryInfo(folder).GetFiles(pattern))
				try {
					Assembly.LoadFrom(actFileInfo.FullName, AppDomain.CurrentDomain.Evidence);
				} catch(Exception) {}

		}
		
		public static ArrayList LoadAssemblies() {
			// load assemblies in the same folder as the current executing assembly
			// and in the folder that contains mscorlib (.NET Framework)
			foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				if (assembly.GetName().Name == "mscorlib")
					LoadDLLInFolder(Path.GetDirectoryName(assembly.Location), "System.*.dll");
				else if (assembly.GetName().Name == Assembly.GetExecutingAssembly().GetName().Name)
					LoadDLLInFolder(Path.GetDirectoryName(assembly.Location), "*.dll");
			
			ArrayList loadedAssemblyNames = new ArrayList();
			
			foreach(Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				loadedAssemblyNames.Add(assembly.GetName());
			
			loadedAssemblyNames.Sort(new AssemblyNameSorter());
			
			return loadedAssemblyNames;
		}
		
		public static void SaveUserPreferences(UserPreferences up) {
			try {
				RegistryKey rkNxBRE = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("NxBRE IE Console", true);
				if (rkNxBRE == null) {
					RegistryKey rkSoftware = Registry.CurrentUser.OpenSubKey("Software", true);
					rkNxBRE = rkSoftware.CreateSubKey("NxBRE IE Console");
				}
				rkNxBRE.SetValue("LastCCBClassName", up.lastBinderClassName);
				rkNxBRE.SetValue("LastHRFFact", up.lastHRFFact);
			} catch(Exception) { // it is not a required feature, so if it does, let it pass away!
			}
		}
		
		public static UserPreferences LoadUserPreferences() {
			UserPreferences up = new UserPreferences();
			up.lastBinderClassName = String.Empty;
			up.lastHRFFact = String.Empty;

			try {
				RegistryKey rkNxBRE = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("NxBRE IE Console");
				if (rkNxBRE != null) {
					up.lastBinderClassName = (string)rkNxBRE.GetValue("LastCCBClassName", String.Empty);
					up.lastHRFFact = (string)rkNxBRE.GetValue("LastHRFFact", String.Empty);
				}
			} catch(Exception) { // it is not a required feature, so if it does, let it pass away!
			}

			return up;
		}
	}
}
