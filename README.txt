NxBRE README v3.1.0
===================

URL          : http://nxbre.org
EMail        : contact@nxbre.org
License      : LGPL



## INTRODUCTION ##

.NET Business Rule Engine

NxBRE is the first open-source rule engine for the .NET platform and a lightweight Business Rule Engine (aka Rule-Based Engine) that offers two different approaches:

    * the Flow Engine, which uses XML as a way to control process flow for an application in an external entity. It is basically a wrapper on C#, as it offers all its flow control commands (if/then/else, while, foreach), plus a context of business objects and results. It is a port of JxBRE v1.7.1 (SourceForge Project from Sloan Seaman) to .NET's Visual C#.

    * the Inference Engine, which is a forward-chaining (data driven) deduction engine and that supports concepts like Facts, Queries and Implications (as defined in RuleML 0.86 Datalog) and like Rule Priority, Mutual Exclusion and Precondition (as found in many commercial engines). It is designed in a way that encourages the separation of roles between the expert who designs the business rules and the programmer who binds them to the business object.


NxBRE's interest lies first into its simplicity, second in the possibility of easily extending its features by delegating to custom code in the Flow Engine or by writing custom RuleBase adapters or Business Objects binders in the Inference Engine.

NxBRE can be really useful for projects that have to deal with:

    * complex business rules that can not be expressed into one uniform structured manner but require the possibility to have free logical expressions,

    * changing business rules that force recompilation if the new rules must meet unexpected requirements.



## v2 -> v3 MIGRATION NOTES ##

Since classes have not been renamed hence migration is pretty straightforward.

Traces are emitted from 4 sources :

	FlowEngine         : core flow engine events
	FlowEngineRuleBase : events emitted from the flow engine rule base with Log, Exception and FatalException statements
	InferenceEngine    : core inference engine events
	Util               : utilities events

Users of the FlowEngine log and error events and InferenceEngine log event will have to connect their listeners to these different sources, while setting the source swith to the desired verbosity level.

Deduction related events for the FlowEngine and the InferenceEngine are unchanged.


Inference Engine now have to use these namespaces:

	NxBRE.InferenceEngine       : core engine features
	NxBRE.InferenceEngine.IO    : only if adapter/binder are used in the current class
	NxBRE.InferenceEngine.Rules : only if facts/queries/predicates are used in the current class

and replace usage of deleted QueryResultSet by IList<IList<Fact>>. Do not forget to correct C# binder files (ccb)!


Flow Engine users now have to use these namespaces:

	NxBRE.FlowEngine           : core engine features
	NxBRE.FlowEngine.Factories : only if engine factories are used in the current class
	NxBRE.FlowEngine.IO        : only if drivers are used in the current class
	NxBRE.FlowEngine.Rules     : only if operators or statements are used in the current class

All static calls on NxBRE helpers and utilities, either from FlowEngine rules or InferenceEngine binders, must be corrected.


## RELEASE NOTES ##

v3.1.1

 Inference Engine

  * Added a CompositeRuleBaseAdapter that allows merging several rulebases in one (contributed by Amitay Dolbo).

  * Made "AllAtoms" property public in "AtomGroup" class (feature request 1788291).

  * Implied Atom with multiple formulas with arguments now work (bug 1815223)



v3.1.0

 General

  * Extracted unit tests in a friend assembly (feature request 1739462)

  * Allowed CompilerOptions to be pulled from config file (feature request 1715165)

  * Corrected method to get NxBRE assembly (bug 1709279)

 Flow Engine

  * Added regex matcher operator (feature request 1545354).

  * The engine can now load operators and rule factories from a fully qualified class name (bug 1741059).

  * Allow BRE "Compare" element in xBRE schema (feature request 1740988).

 Inference Engine

  * Added features for controlling engine behavior at rule base load time and process time regarding performative and connective rules.

  * Improved adapter load method to allow sharing (feature request 1710153).

  * Added event context to allow discovering the facts and implication at the origin of the event (feature request 1657420).

  * Added properties to get an enumeration of the queries and implications in the engine (feature request 1786544).



v3.0.0

  * Added comments to improve the API documentation.



v3.0.0RC2

 Inference Engine

  * Added support for Visual Basic Compiled Binders (feature request 1535952)

  * Included the Inference Engine Registry in main distribution (feature request 1511119)

  * Refactored the core fact base to leverage generics and to use a much simpler and efficient fact storage

  * The Visio2003 adapter now supports the following RuleML concepts:
      - typed predicates: prefix like this (xs:int)
      - named predicates (aka RuleML slots): prefix like this (?Size)
      - integrity queries (new shape in the stencil)
      - equivalent atoms (new shape in the stencil)



v3.0.0RC1 (compared to NxBRE v2.5.3)

 General

  * Target .NET framework version is now 2.0

  * Development switched to SharpDevelop2 (working with SLN files directly compatible with VS.NET 2005)

  * Improved API (used generics where it makes sense, use interfaces instead of implementations)

  * Fully reorganized source code (Flow Engine and Inference Engine clearly separated)

  * Improved namespaces (less namespaces, more compliant to .NET practices, no need to import Core namespaces)

  * Switched from legacy custom made to standard .NET 2.0 tracing mechanism (TraceSource)

  * All NxBRE features are now configured from the standard config file (instead of disseminated static values)

  * Included improvements from Chuck Cross in on-the-fly compilation module and more informative exception/toString in some places

 Inference Engine

  * IEFacade now supports simpler fact assertion syntax: IEF.AssertNewFactOrFail("Hour", cd, cd.Time.Hour) instead of IEF.AssertNewFactOrFail("Hour", new object[]{cd, cd.Time.Hour})



