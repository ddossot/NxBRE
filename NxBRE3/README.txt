NxBRE README v3.2.0
===================

URL          : http://nxbre.org
EMail        : contact@nxbre.org
License      : LGPL


## RELEASE NOTES ##

v3.2.0

 General

  * Fixed argument grouping method to support null arguments (bug 1926553).

 Flow Engine

  * Added backward chaining behavior (feature request 1915027).

 Inference Engine

  * Added support for labeled fact atoms from the rule base (feature request 1798783).

  * Added a configuration parameter to allow the addition of extra DLLs in the on-the-fly compilation context (feature request 1873414).



v3.1.1

 Inference Engine

  * Added a CompositeRuleBaseAdapter that allows merging several rulebases in one (contributed by Amitay Dolbo).

  * Made "AllAtoms" property public in "AtomGroup" class (feature request 1788291).

  * Implied Atom with multiple formulas with arguments now work (bug 1815223).

  * Strengthened FileRegistry so it ignores unprocessable file system events (feature request 1817201).

  * Strengthened HRF adapter to better detect syntax errors (bug 1850255).

  * Ensured RuleML adapters close the underlying input stream in case of error to release the resource (bug 1850290).



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

