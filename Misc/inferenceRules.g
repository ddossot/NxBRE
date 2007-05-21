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
	:	 'rulebase' SPACE QUOTE words QUOTE NEWLINE rule*;

rule 	:	'rule' NEWLINE meta? condition action 'end' NEWLINE;

meta	:	'priority' SPACE NUMERIC NEWLINE;

condition
	:	'if' NEWLINE statement statement2*;

action	:	('deduct'|'forget')+ NEWLINE statement;

statement
	:	TAB+ words NEWLINE;

statement2
	:	TAB+ logic SPACE words NEWLINE;

logic	:	('and' | 'or' | 'not');

words	:	WORD (SPACE WORD)*;

WORD	:	(ALPHA | NUMERIC)+;
ALPHA	:	('a'..'z'|'A'..'Z')+;
NUMERIC	:	'0'..'9'+;
NEWLINE	:	('\r'? '\n')+;
SPACE	:	' '+;
TAB	:	'\t';
QUOTE	:	'"';
