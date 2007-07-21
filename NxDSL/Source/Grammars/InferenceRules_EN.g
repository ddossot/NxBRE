grammar InferenceRules_EN;

options {
language=CSharp;
}

tokens {
	RULEBASE='rulebase';
	FACT='fact';
	QUERY='query';
	RULE='rule';
	PRIORITY='priority';
	PRECONDITION='precondition';
	MUTEX='mutex';
	IF='if';
	THEN='then';
	DEDUCT='deduct';
	FORGET='forget';
	COUNT='count';
	MODIFY='modify';
	AND='and';
	OR='or';
}

@header {
using System.Collections.Generic;
}

@lexer::header {

}

@members {
IDictionary<int, string> logicBlocks = new Dictionary<int, string>();

public override void ReportError(RecognitionException re) {
  throw new NxDSL.DslException(re);
}
}

rulebase 
	:	RULEBASE SPACE QUOTE words QUOTE (rule | query | fact | ignored)* EOF {Console.WriteLine("rulebase label: "+$words.text);};
	
fact	:	FACT (SPACE QUOTE words QUOTE)? NEWLINE statement;

query	:	QUERY SPACE QUOTE words QUOTE NEWLINE condition;

rule 	:	RULE SPACE QUOTE words QUOTE NEWLINE meta IF NEWLINE condition action {Console.WriteLine("rule label: "+$words.text);};

meta	:	priority? precondition? mutex*;

priority
	:	TAB PRIORITY SPACE NUMERIC NEWLINE {Console.WriteLine("priority: "+$NUMERIC.text);};

precondition
	:	TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE {Console.WriteLine("precondition: "+$words.text);};

mutex	:	TAB MUTEX SPACE QUOTE words QUOTE NEWLINE {Console.WriteLine("mutex: "+$words.text);};

condition
	:	statement (logic statement)*;

action	:	THEN SPACE (DEDUCT | FORGET | COUNT | MODIFY)+ NEWLINE statement;

statement
	:	indent words NEWLINE {Console.WriteLine("depth of: '"+$words.text+"' is: "+$indent.text.Length);};

logic	:	indent booleanToken NEWLINE {
			int depth = $indent.text.Length;
			string newOperator = $booleanToken.text;
			
			Console.WriteLine("depth of op.: '{0}' is: {1}", newOperator, depth);
			
			string existingOperator;
			
			if (logicBlocks.TryGetValue(depth, out existingOperator)) {
				if (!newOperator.Equals(existingOperator)) throw new Exception("Operator mismatch at depth: " + depth);
			}
			else {
				logicBlocks.Add(depth, newOperator);
			}			
		};

words	:	word (SPACE word)*;

word	:	(anyToken | CHAR | NUMERIC)+;

ignored	:	(TAB | SPACE)* NEWLINE;

indent	:	TAB+;

anyToken
	:	(RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken);

booleanToken
	:	(AND | OR);

NUMERIC	:	('0'..'9')+;
CHAR	:	('!' | '\u0023'..'\u002F' | '\u003A'..'\u00FF')+;
NEWLINE	:	('\r'? '\n')+;
SPACE	:	' '+;
TAB	:	'\t';
QUOTE	:	'"';

