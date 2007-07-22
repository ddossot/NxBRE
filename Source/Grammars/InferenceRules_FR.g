grammar InferenceRules_FR;

options {
language=CSharp;
}

tokens {
	RULEBASE='base_de_règles';
	FACT='fait';
	QUERY='requête';
	RULE='règle';
	PRIORITY='priorité';
	PRECONDITION='précondition';
	MUTEX='mutex';
	IF='si';
	THEN='alors';
	DEDUCT='déduis';
	FORGET='oublis';
	COUNT='compte';
	MODIFY='modifie';
	AND='et';
	OR='ou';
}

@header {
using NxDSL;
}

@lexer::header {

}

@members {

internal RuleBaseBuilder rbb;

public override void ReportError(RecognitionException re) {
  throw new DslException(re);
}
}

rulebase 
	:	RULEBASE SPACE QUOTE words QUOTE (rule | query | fact | ignored)* EOF { rbb.Label = $words.text;};
	
fact	:	FACT (SPACE QUOTE words QUOTE)? NEWLINE statement {
			// fix this as soon as I know how to test for the nullity of words
			try {
				rbb.AddFact($words.text);
			}
			catch(NullReferenceException) {
				rbb.AddFact(null);
			}		
		};

query	:	QUERY SPACE QUOTE words QUOTE NEWLINE condition { rbb.AddQuery($words.text); };

rule 	:	RULE SPACE QUOTE words QUOTE NEWLINE meta IF NEWLINE condition action { rbb.AddImplies($words.text); };

meta	:	priority? precondition? mutex*;

priority
	:	TAB PRIORITY SPACE NUMERIC NEWLINE { rbb.CurrentImplicationData.priority=$NUMERIC.text; };

precondition
	:	TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE { rbb.CurrentImplicationData.precondition=$words.text; };

mutex	:	TAB MUTEX SPACE QUOTE words QUOTE NEWLINE { rbb.CurrentImplicationData.mutex=$words.text; };

condition
	:	statement (logic statement)*;

action	:	THEN SPACE verb NEWLINE statement { rbb.CurrentImplicationData.action=$verb.value; ;};

statement
	:	indent words NEWLINE { rbb.AddStatement($indent.text.Length, $words.text); };

logic	:	indent booleanToken NEWLINE {
			rbb.AddLogicBlock($indent.text.Length, $booleanToken.value);
		};
		
verb returns[string value]
	:	(DEDUCT { $value = "assert"; }
		| FORGET { $value = "retract"; }
		| COUNT { $value = "count"; }
		| MODIFY { $value = "modify"; });

words	:	word (SPACE word)*;

word	:	(anyToken | CHAR | NUMERIC)+;

ignored	:	(TAB | SPACE)* NEWLINE;

indent	:	TAB+;

anyToken
	:	(RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken);

booleanToken returns[string value]
	:	( AND { $value = "And"; }
		| OR  { $value = "Or"; });

NUMERIC	:	('0'..'9')+;
CHAR	:	('!' | '\u0023'..'\u002F' | '\u003A'..'\u00FF')+;
NEWLINE	:	('\r'? '\n')+;
SPACE	:	' '+;
TAB	:	'\t';
QUOTE	:	'"';

