grammar inferenceRules;

@header {
package test;
import java.util.HashMap;
}

@lexer::header {package test;}

@members {
/** Map variable name to Integer object holding value */
HashMap memory = new HashMap();
}

rulebase 
	:	'rulebase' SPACE QUOTE words QUOTE (rule | ignored)* {System.out.println("rulebase label: "+$words.text);};

rule 	:	'rule' NEWLINE meta? condition action 'end';

meta	:	TAB 'priority' SPACE NUMERIC NEWLINE {System.out.println("priority: "+$NUMERIC.text);};

condition
	:	'if' NEWLINE statement (logic statement)*;

action	:	('deduct' | 'forget' | 'count' | 'modify')+ NEWLINE statement;

statement
	:	indent words NEWLINE {System.out.println("depth of: '"+$words.text+"' is: "+$indent.text.length());};

logic	:	indent BOOLEAN NEWLINE {System.out.println("depth of: '"+$BOOLEAN.text+"' is: "+$indent.text.length());};

words	:	word (SPACE word)*;

word	:	(ALPHA | NUMERIC)+;

ignored	:	(TAB | SPACE)* NEWLINE;

indent	:	TAB+;

BOOLEAN	: 	('and' | 'or' | 'not');
ALPHA	:	('a'..'z' | 'A'..'Z')+;
NUMERIC	:	'0'..'9'+;
NEWLINE	:	('\r'? '\n')+;
SPACE	:	' '+;
TAB	:	'\t';
QUOTE	:	'"';
