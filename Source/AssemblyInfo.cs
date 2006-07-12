using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly:AssemblyTitle("NxBRE")]
[assembly:AssemblyDescription(".NET Business Rules Engine")]
[assembly:AssemblyConfiguration("")]
[assembly:AssemblyCompany("AgilePartner S.A.")]
[assembly:AssemblyProduct("NxBRE")]
[assembly:AssemblyCopyright("Copyright (C) 2003-2006 David Dossot et al.")]
[assembly:AssemblyTrademark("NxBRE is distributed under the GNU LESSER GENERAL PUBLIC LICENSE.")]
[assembly:AssemblyCulture("")]
[assembly:AssemblyVersion("3.0.0.*")]
[assembly:AssemblyDelaySign(false)]
[assembly:AssemblyKeyFile("")]
[assembly:CLSCompliant(true)]

//TODO: ensure no internal class or method in non-core package - split classes like Fact in Fact & Core/FactUtils
//TODO: Replace current custom logging mechanism with a standard one
//TODO: Improve configuration
//TODO: try to add config in the project and have the file outputed in the bin folder
