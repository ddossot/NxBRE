grammar InferenceRules_EN;

options {
language=CSharp;
}

@header {
using System.Collections.Generic;
}

@lexer::header {

}

@members {
IDictionary<int, string> logicBlocks = new Dictionary<int, string>();
}


rulebase 
	:	'rulebase:' SPACE QUOTE words QUOTE (rule | ignored)* {Console.WriteLine("rulebase label: "+$words.text);};

rule 	:	'rule:' SPACE QUOTE words QUOTE NEWLINE meta condition action 'end.' {Console.WriteLine("rule label: "+$words.text);};

meta	:	priority? precondition? mutex*;

priority
	:	TAB 'priority' SPACE NUMERIC NEWLINE {Console.WriteLine("priority: "+$NUMERIC.text);};

precondition
	:	TAB 'precondition' SPACE QUOTE words QUOTE NEWLINE {Console.WriteLine("precondition: "+$words.text);};

mutex	:	TAB 'mutex' SPACE QUOTE words QUOTE NEWLINE {Console.WriteLine("mutex: "+$words.text);};

condition
	:	'if' NEWLINE statement (logic statement)*;

action	:	('deduct' | 'forget' | 'count' | 'modify')+ NEWLINE statement;

statement
	:	indent words NEWLINE {Console.WriteLine("depth of: '"+$words.text+"' is: "+$indent.text.Length);};

logic	:	indent BOOLEAN NEWLINE {
			int depth = $indent.text.Length;
			string newOperator = $BOOLEAN.text;
			
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

word	:	(ALPHA | NUMERIC | SYMBOL)+;

ignored	:	(TAB | SPACE)* NEWLINE;

indent	:	TAB+;

BOOLEAN	: 	('and' | 'or' | 'not');
ALPHA	:	('a'..'z' | 'A'..'Z')+;
NUMERIC	:	'0'..'9'+;
NEWLINE	:	('\r'? '\n')+;
SYMBOL	:	('-' | '_')+;
SPACE	:	' '+;
TAB	:	'\t';
QUOTE	:	'"';
