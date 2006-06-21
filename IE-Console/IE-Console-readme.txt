NxBRE Inference Engine Console README v1.2.1
============================================

Author       : David Dossot (david@dossot.net)
Date         : 20-MAY-2006
URL          : http://nxbre.org
License      : LGPL

## INTRODUCTION ##

The Inference Engine Console is a very basic but useful tool for easily testing rule bases while developing them.

It supports binding, though obviously an empty Hashtable is passed to Process(). The test binder should have a setup method that prefills the Hashtable with the business objects expected from the running system.

At startup it loads all the assemblies found in the same folder as NxBRE-IE-Console.exe and all the assemblies of the executing .NET framework: this allows your binder to use your assemblies (stored in the same folder as the console) and all .NET classes (Data, XML, etc...).

Any contribution to this sub-project is welcome!



## BUILDING W/VISUAL STUDIO ##

Being develop with SharpDevelop, there is clearly a difficulty when building the Console with VS.NET. The few classes are all in the same folder, for easy importation in a VS.NET project.

The challenge remains the forms resources that are embedded (hence referred) differently on the different IDEs. Users feedback can help others doing the exercise...



## RELEASE NOTES ##

v.1.2.1

  * Debugged fact outputting (bug #1439847).


v.1.2

  * Rebuilt for NxBRE v2.5 with support of RuleML 0.9 NafDatalog.


v.1.1.2

  * Debugged non-functional "Show Modifications" tick box.

  * Debugged fact assertion/retraction function (bug #1244012).


v.1.1.1

  * Rebuilt for NxBRE v2.3.


v.1.1

  * Minor GUI changes.


v.1.0

  * The Console requires NxBRE v2.2 or better.

