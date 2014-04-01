NxBRE README v2.5.3
===================

.NET Version : David Dossot (david@dossot.net)
             : Ron Evans
             : Andre Weber
             : Stephane Joyeux
JxBRE Author : Sloan Seaman (sloan@sgi.net)
Date         : 17-JUL-2006
URL          : http://nxbre.org
License      : MIT



## AUTHORS REQUEST ##

If you use NxBRE please tell us. It is nice to know when people are using your code. Praise, gripes, suggestions, all are welcome.

Thanks!



## REFACTORING ##

Any refactoring suggestions are welcome and encouraged. There are some classes we are not satisfied with, we won't tell you which ones, but if you have similar feelings while contemplating the design of some parts of NxBRE and come up with a cool refactoring idea, please tell it to us!

This refactoring will be the first step of NxBRE v3.0.




## INTRODUCTION ##

.NET Business Rule Engine

NxBRE is the first open-source rule engine for the .NET platform and a lightweight Business Rule Engine (aka Rule-Based Engine) that offers two different approaches:

    * the Flow Engine, which uses XML as a way to control process flow for an application in an external entity. It is basically a wrapper on C#, as it offers all its flow control commands (if/then/else, while, foreach), plus a context of business objects and results
. It is a port of JxBRE v1.7.1 (SourceForge Project from Sloan Seaman) to .NET's Visual C#.

    * the Inference Engine, which is a forward-chaining (data driven) deduction engine and that supports concepts like Facts, Queries and Implications (as defined in RuleML 0.86 Datalog) and like Rule Priority, Mutual Exclusion and Precondition (as found in many commercial engines). It is designed in a way that encourages the separation of roles between the expert who designs the business rules and the programmer who binds them to the business object.


NxBRE's interest lies first into its simplicity, second in the possibility of easily extending its features by delegating to custom code in the Flow Engine or by writing custom RuleBase adapters or Business Objects binders in the Inference Engine.

NxBRE can be really useful for projects that have to deal with:

    * complex business rules that can not be expressed into one uniform structured manner but require the possibility to have free logical expressions,

    * changing business rules that force recompilation if the new rules must meet unexpected requirements.




## BUILDING W/NANT ##

One of the easiest ways of building NxBRE is simply to use NAnt. In the source folder, just start nant.exe and here you go with a release-grade NxBRE.dll!




## BUILDING W/VISUAL STUDIO ##

Being developed with SharpDevelop, there is clearly a difficulty when building NxBRE with VS.NET. It is a pity that IDEs, which should be neutral when it comes to building libraries (forms are another story), can turn to be such a bar to a simple process like compiling & building.

Anyway, thanks to user's feedback please note that:

 - the provided solution/project files are far from perfection (for example the type of assembly is not set to library),

 - the resources embedded in NxBRE.DLL are not referenced identically between IDEs. This is why an alternate config file to use when building with VS.NET has been provided. Simply rename NxBRE.dll.config.vsnet
 into NxBRE.dll.config.





## TESTING ##

NxBRE testing is a blend of unit testing (method level) and black-box testing (the system as a whole).

All the tests are run from NUnit, which is maybe a twist of the tool original goal (class level unit testing) but is very convenient, as only one push of a button launches all the tests.

The distributed NxBRE.dll is built with references to NUnit v2.2, if you use a different version of NUnit and want to run the unit tests, you will have to rebuild NxBRE.dll.

To be able to run the tests be sure to have the configuration file (NxBRE.dll.config) correctly pointing to the different folders. Be also sure to have the required XML schema files (ruleml-0_86-datalog.xsd and ruleml-0_86-nafdatalog.xsd) in the folder referenced by: nxbre.unittest.outputfolder.




## TUTORIAL ##

The best is to first read the PDF documentation, then work on the unit test file and the examples. Each and every feature is demonstrated: all this is pretty straightforward.

Command lines for running the examples:

  Login -l %1 -e %2 "C:/NxBRE/Rulefiles/login.xbre"
     where %1 is the log trace level (1-9) and %2 the error trace level (1-9)

  Discount -l %1 -e %2 "C:/NxBRE/Rulefiles/discount.xbre"
     where %1 is the log trace level (1-9) and %2 the error trace level (1-9)

  FraudControl %1 "C:\NxBRE\Rulefiles\fraudcontrol.ruleml"
     where %1 is the number of tens of dummy customers data to create (advice: use 10 or more)

  Telco %1 "C:\My Documents\Development\NxBRE\Rulefiles\telco-rating.ruleml"
     where %1 is the number of dozens of dummy calls data to create (advice: use 100 or more)





## RELEASE NOTES ##

v2.5.3

 Inference Engine

  * Individual expressions were preventing correct atom matching (bug 1516953).



v2.5.2

 Inference Engine

  *  The binder method Compute can now receive extra String parameters in the arguments (FR 1504353).

 Flow Engine

  * The ToString() overload in AbstractBRERuleResult could easily result in NullReferenceExceptions (bug 1502909).

  * Testing of HTTP rule file loading has been disabled because it basically proves that .NET can read from a URL which is not so interesting.

 Util

  *  Code compiler was failing under heavy load (bug 1498862).



v2.5.1

 Inference Engine

  * Slots now contribute named values like variables (FR 1483072): see PDF documentation and KB for more information.

  * Debugged compiled evaluator that was failin on dynamic modules (bug 1482753).

  * The Inference Engine was reacting strangely with typed data (bug 1470721).

  * The Inference engine was rejectong valid RuleML 0.9 rulebases (bug 1469851).

  * Visio adapter had a broken master inheritance (bug 1432027).

 Util

  * Reflection.CastValue() was failing for enumerated types (bug 1474032).



v2.5

 Inference Engine

  * Partial (but significant!) support of RuleML 0.9 NafDatalog sub-language has been introduced (turn to the PDF documentation to discover what is new).

  * A static parameter (ReferenceLinkMode) has been introduced in org.nxbre.util.Compilation to control its strategy for referencing other assemblies (the default behavior of v2.4 has been reported as problematic with ASP.NET 2.0).

  * Debugged overflow error when pausing execution (bug 1421333).

  * Debugged problem with AND group that contains only OR groups (bug 1358781).

  * RuleML adapters accepting XSD resources embedded by VS.NET (bug 1365353).

  * IInferenceEngine has been introduced as an interface for IEImpl.

  * IFactBaseAdapter has been introduced as a superinterface of IRuleBaseAdapter, allowing a minimal implementation for adapters that only need to load/save facts.

 Flow Engine

  * IBRE has been deprecated in favor of the clearer IFlowEngine (users of the Flow Engine must switch now as IBRE will be removed from the next release).
 


v.2.4.1

 Inference Engine

  * Made the adapters "configuration friendly" (they now read the schema location first in the configuration file).



v.2.4.1

 Inference Engine

  * Debugged the fact that the inference engine was missing combinations of facts (bug 1346078).

  * Debugged implication with C# expression failure when Naf atoms are in the body of the implication (bug 1342607).

  * Debugged engine fact assert error (bug 1340799).

  * Debugged the engine that was crashing in case of fact deletion without deletion handler (bug 1344165).

  * Debugged Naf atom and match atom problem (bug 1332214).



v.2.4

 Inference Engine

  * Thread safe modes have been introduced (see chapter 3.7 of the PDF documentation for more information).

  * The Inference Engine Facade exposes the rule base label to allow the creation of adaptative binders.

  * Debugged the HRF adapter that generates rulebases outside of RuleML Namespace if no direction pragma is included in the HRF rule file.

 General

  * Moved to NUnit 2.2



v.2.3.1

 Inference Engine

  * Debugged fact assertion causing trouble to similar implications (bug 1252700).

  * Introduced the fully functional HRF adapter (patch 1234633).



v.2.3

 Inference Engine

  * Refactored the core of the engine to make most of the objects immutable.

  * Introduced the "modify" implication action.

  * Introduced a C# based expression language.

  * Introduced the prefix "binder:" to facilitate usage of binder with HRF.

  * Debugged binder support of OnDeleteFact event (bug 1179849).

 Flow Engine

  * Debugged the assertion mechanism to support empty Strings (bug 1190485).



v.2.2.1

 Inference Engine

  * Refactored the core of the engine (FactBase & Processor) to use the versatile DataTable object to obtain a rock-solid pattern matching implementation. 

  * Added DispatchLog to the Inference Engine Fa�ade available in the binder to allow custom messages logging.

  * Debugged Naf error with retracted facts.

  * Debugged mathematical overflows (bug 1086498)


v.2.2

  * The distribution has been simplified and consists now of two archives: everything except the documentation, and the documentation.

 Inference Engine

  * Support of RuleML NafDatalog 0.86 Schema (except slots) has been introduced: basically this adds the <naf> element to the Datalog schema, i.e. negation-as-failure that allows testing for absence of facts.

  * Queries now returned unique rows (similar to SQL's select distinct).

  * Implications do not throw exception anymore if a result row from the body part does not provide enough values to form a complete fact from the header. It is possible to restore the previous stricter behavior by changing a boolean switch on the engine.

  * Support of function based atom relations has been added.

  * Support of <var>less queries and implications has been added.

  * Support for counting & retracting implications has been added to the engine ; the DeleteFact event has been added to the engine and IBinder has been modified to support it (with a minor impact because the default abstract implementation has been modified accordingly).

  * The Visio 2003 Stencil has been updated to support negative atoms, couting and retracting implications.

  * The "After" behaviour of the Before/After Binder has been modified to restart an inference if a fact has been successfully retracted (previously, it was restarting only on new fact assertion).

  * Minor read-only parameters have been added to IEimpl to allow the development of the basic IE Console.

  * The Visio2003 adapter now fully supports all the custom properties of the stencil's objects.

  * The RuleML 0.86 adapters are now a little more 'nervous' and shouts when fed with RuleML 0.8 format.

 Flow Engine
  
  * Debugged Value helper to support empty constructors when using the Assert command (bug 1044404). 


v.2.1

  * Partial support of RuleML Datalog 0.86 Schema has been added, offering to the Inference Engine the "OR" operator and logical blocks nesting ("slots" are not yet supported).

  * NxBRE RI Helper Operators (org.nxbre.ri.helpers.operators.*) are now accessible from the Inference Engine by following a pattern in the Individual predicate content, as this example shows: NxBRE:GreaterThanEqualTo(100). Obviously the operator "InstanceOf" makes no sense, as the rules predicates are always casted to the strongly typed ones, i.e. the fact predicates.

  * A Visio 2003 adapter able to parse the DatadiagramML files (VDX) has been added to the Inference Engine. A shapes definition file is provided for creating the rule base in Visio. Turn to the PDF documentation for ,ore information.

  * The IsolatedEmpty memory mode has been added, allowing to temporarily work on an empty fact base, with the option to commit the generated facts in the global fact base.

  * A SaveFacts method has been added to IEimpl, enabling the persistence of facts-only rule bases.

  * An *experimental* adapter able to parse the "Human Readable Format" has been added to the Inference Engine. It currently only supports RuleML 0.8 equivalent syntax (no OR, no logical block nesting) and US-ASCII rule bases.

  * The boolean Running has been added to IBRE, in order to support the compiled rules flow engine.

  * Brendan Ingram has contributed code for improving org.nxbre.ie.adapters.CSharpBinderFactory
.


  * The SharpDevelop Combine and Project files and *generated* Visual Studio Solution and Project files have been added to the archives in order to simplify importing NxBRE in these IDEs.


v.2.0

  * xBusinessRules.xsd has been introduced in replacement of the DTD. Please note that DateTime elements has been completed by Date and Time elements. Data typing is now strongly enforced by the schema, which reduces further possible casting problem in the engine.

  * Introduction of the Inference Engine with RuleML 0.8 Datalog support (see documentation).

  * Debugged ObjectLookup to support multi-signature Properties, like Item (bug 918864). Null support has been improved.

  * Added utility classes to facilitate basic operations like maths, object item access (data row, array), etc...

  * ObjectLookup can now directly call simple methods with params arguments.

  * All resource files have been moved to a \resource folder in order to facilitate their importation in the project. The matching between these resource files and the actual fully qualified name to reach them is now made in the config file, and not in constants in the code.
 
  * Test classes moved under a root folder named test for better separation and easier removal.


v.1.8.3

  * A general documentation and a CHM/HTML documentation are available.

  * ForEach loop has been added: it is able to iterate on IEnumerable objects.

  * Retract statement is now available: it can be used to remove an object from the context result.

  * IsAsserted operator has been added: it allows checking if an object refered by its id exists in the context result.

  * All rule file drivers are now unit tested.

  * Html pseudocode rendering is now possible by using an embedded utility.


v.1.8.2

  * Repackaged net.ideaity.nxbre to org.nxbre (and bought domain name nxbre.org).

  * Moved BREFactories from org.nxbre.ri to org.nxbre.ri.factories.

  * Capitalized all methods to have a better .NET look'n'feel.

  * Improved IBRE to present all necessary methods/events so there is no need to cast to BREImpl anymore.

  * Improved performances by using XPathDocument instead of XmlDocument and performing a nodes pre-selection at XML root level.

  * Debugged <While> executing too much times: the <Do> part was always executed (bug 870783).

  * Exception priority is now correctly carried from the rules to the handler.

  * Implemented IBRE.Stop() that allows stoping immediatly the rules processing.

  * Implemented IBRE.Reset() that can be used to reset the context call stack and results.

  * Removed IBRE.WriteRules() as it is not of BRE's responsability to write to the disk: it now exposes the XmlDocumentRules and it is up to save it, if necessary.

  * Implemented BREImpl.Clone() that allows shallow copying of a preloaded ready-to fire rule-engine, which is very usefull in a multi-threaded environment where each thread uses a different clone of a pre-instantiated BRE singleton.

  * Created BRECloneFactory to easily generate clones.

  * Debugged decimal assertion that was regional setting sensitive for the separator.

  * InvokeSet can now get the set id from an asserted object ToString().

  * Throw[Fatal]Exception and Log can now get their message values from an asserted object ToString().

  * Incrementors can be setted/reseted to a initial value.

  * Used foreach to parse NodeLists when possible

  * IRulesDriver now defines GetXmlReader instead of GetXmlDocument: this leaves the opportunity for the implementation of IBRE to use whatever XML parsing technique.

  * AbstractRulesFileDriver does not take care anymore of watching the file system for possible rule file change: this was the perfect prototype of the false good idead as it could have come up to consistency problems in a multi-threaded environment, and it is better to leave to the implementor the decision on how and when he/she will monitor the rule source and re-initialize the engine.

  * Unit test is configured in NxBRE.dll.config (no need to recompile test class anymore).


v.1.8.1

  * Test class runs now on NUnit v2.1 and is integrated in main assembly.

  * Capitalized more public methods to have a better .NET look'n'feel.

  * Added AbstractBRERuleContext.SetObject to clarify the syntax when passing a business object to the rule engine:
       bre.RuleContext.SetResult("TestObject", new BRERuleObject(myObject));
    is now:
       bre.RuleContext.SetObject("TestObject", myObject);
       
  * Similarly added AbstractBRERuleContext.GetObject.

  * xBusinessRules.dtd has been modified to improve grammar consistency and to add operators Between, IsTrue, IsFalse and In.
  
  * transformXRules.xsl has been modified to support the changes in xBusinessRules.dtd.


v.1.8 (compared to JxBRE v1.7.1)

    * Introduction of an alternate high level XML grammar for expressing rules in a more neutral and less technical manner,

    * Full reflection support for accessing fields, properties and methods on business objects (and classes for statics) ; and for instantiating objects,

    * Not operator added,

    * InvokeSet statement added.

