grammar inferenceRules;

/*
options {
language=CSharp;
}
*/

@header {
import java.util.HashMap;
}

@lexer::header {

}

@members {
HashMap<Integer, String> logicBlocks = new HashMap<Integer, String>();
}


rulebase 
	:	'rulebase:' SPACE QUOTE words QUOTE (rule | ignored)* {System.out.println("rulebase label: "+$words.text);};

rule 	:	'rule:' SPACE QUOTE words QUOTE NEWLINE meta condition action 'end.' {System.out.println("rule label: "+$words.text);};

meta	:	priority? precondition? mutex*;

priority
	:	TAB 'priority' SPACE NUMERIC NEWLINE {System.out.println("priority: "+$NUMERIC.text);};

precondition
	:	TAB 'precondition' SPACE QUOTE words QUOTE NEWLINE {System.out.println("precondition: "+$words.text);};

mutex	:	TAB 'mutex' SPACE QUOTE words QUOTE NEWLINE {System.out.println("mutex: "+$words.text);};

condition
	:	'if' NEWLINE statement (logic statement)*;

action	:	('deduct' | 'forget' | 'count' | 'modify')+ NEWLINE statement;

statement
	:	indent words NEWLINE {System.out.println("depth of: '"+$words.text+"' is: "+$indent.text.length());};

logic	:	indent BOOLEAN NEWLINE {
			int depth = $indent.text.length();
			String operator = $BOOLEAN.text;
			
			System.out.println("depth of op.: '"+operator+"' is: "+depth);
			
			String existingOperator = logicBlocks.get(depth);
			if (existingOperator != null && !operator.equals(existingOperator)) throw new RuntimeException("Operator mismatch!");
			logicBlocks.put(depth, operator);
		};

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
