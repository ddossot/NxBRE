// $ANTLR 3.0 C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g 2007-07-21 21:02:45

using NxDSL;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



internal class InferenceRules_FRParser : Parser 
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"RULEBASE", 
		"FACT", 
		"QUERY", 
		"RULE", 
		"PRIORITY", 
		"PRECONDITION", 
		"MUTEX", 
		"IF", 
		"THEN", 
		"DEDUCT", 
		"FORGET", 
		"COUNT", 
		"MODIFY", 
		"AND", 
		"OR", 
		"SPACE", 
		"QUOTE", 
		"NEWLINE", 
		"TAB", 
		"NUMERIC", 
		"CHAR"
    };

    public const int DEDUCT = 13;
    public const int MODIFY = 16;
    public const int RULE = 7;
    public const int RULEBASE = 4;
    public const int CHAR = 24;
    public const int TAB = 22;
    public const int COUNT = 15;
    public const int NUMERIC = 23;
    public const int AND = 17;
    public const int EOF = -1;
    public const int SPACE = 19;
    public const int MUTEX = 10;
    public const int IF = 11;
    public const int QUOTE = 20;
    public const int THEN = 12;
    public const int NEWLINE = 21;
    public const int PRIORITY = 8;
    public const int PRECONDITION = 9;
    public const int OR = 18;
    public const int QUERY = 6;
    public const int FORGET = 14;
    public const int FACT = 5;
    
    
        public InferenceRules_FRParser(ITokenStream input) 
    		: base(input)
    	{
    		InitializeCyclicDFAs();
        }
        

    override public string[] TokenNames
	{
		get { return tokenNames; }
	}

    override public string GrammarFileName
	{
		get { return "C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g"; }
	}

    
    
    internal RuleBaseBuilder rbb;
    
    public override void ReportError(RecognitionException re) {
      throw new DslException(re);
    }


    
    // $ANTLR start rulebase
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:42:1: rulebase : RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF ;
    public void rulebase() // throws RecognitionException [1]
    {   
        words_return words1 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:4: ( RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:4: RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF
            {
            	Match(input,RULEBASE,FOLLOW_RULEBASE_in_rulebase138); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rulebase140); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase142); 
            	PushFollow(FOLLOW_words_in_rulebase144);
            	words1 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase146); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:37: ( rule | query | fact | ignored )*
            	do 
            	{
            	    int alt1 = 5;
            	    switch ( input.LA(1) ) 
            	    {
            	    case RULE:
            	    	{
            	        alt1 = 1;
            	        }
            	        break;
            	    case QUERY:
            	    	{
            	        alt1 = 2;
            	        }
            	        break;
            	    case FACT:
            	    	{
            	        alt1 = 3;
            	        }
            	        break;
            	    case SPACE:
            	    case NEWLINE:
            	    case TAB:
            	    	{
            	        alt1 = 4;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:38: rule
            			    {
            			    	PushFollow(FOLLOW_rule_in_rulebase149);
            			    	rule();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:45: query
            			    {
            			    	PushFollow(FOLLOW_query_in_rulebase153);
            			    	query();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:53: fact
            			    {
            			    	PushFollow(FOLLOW_fact_in_rulebase157);
            			    	fact();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 4 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:43:60: ignored
            			    {
            			    	PushFollow(FOLLOW_ignored_in_rulebase161);
            			    	ignored();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop1;
            	    }
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            	Match(input,EOF,FOLLOW_EOF_in_rulebase165); 
            	 rbb.Label = input.ToString(words1.start,words1.stop);
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end rulebase

    
    // $ANTLR start fact
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:45:1: fact : FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement ;
    public void fact() // throws RecognitionException [1]
    {   
        words_return words2 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:45:8: ( FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:45:8: FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement
            {
            	Match(input,FACT,FOLLOW_FACT_in_fact176); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:45:13: ( SPACE QUOTE words QUOTE )?
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);
            	
            	if ( (LA2_0 == SPACE) )
            	{
            	    alt2 = 1;
            	}
            	switch (alt2) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:45:14: SPACE QUOTE words QUOTE
            	        {
            	        	Match(input,SPACE,FOLLOW_SPACE_in_fact179); 
            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact181); 
            	        	PushFollow(FOLLOW_words_in_fact183);
            	        	words2 = words();
            	        	followingStackPointer_--;

            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact185); 
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_fact189); 
            	PushFollow(FOLLOW_statement_in_fact191);
            	statement();
            	followingStackPointer_--;

            	
            				// fix this as soon as I know how to test for the nullity of words
            				try {
            					rbb.AddFact(input.ToString(words2.start,words2.stop));
            				}
            				catch(NullReferenceException) {
            					rbb.AddFact(null);
            				}		
            			
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end fact

    
    // $ANTLR start query
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:55:1: query : QUERY SPACE QUOTE words QUOTE NEWLINE condition ;
    public void query() // throws RecognitionException [1]
    {   
        words_return words3 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:55:9: ( QUERY SPACE QUOTE words QUOTE NEWLINE condition )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:55:9: QUERY SPACE QUOTE words QUOTE NEWLINE condition
            {
            	Match(input,QUERY,FOLLOW_QUERY_in_query201); 
            	Match(input,SPACE,FOLLOW_SPACE_in_query203); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_query205); 
            	PushFollow(FOLLOW_words_in_query207);
            	words3 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_query209); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_query211); 
            	PushFollow(FOLLOW_condition_in_query213);
            	condition();
            	followingStackPointer_--;

            	 rbb.AddQuery(input.ToString(words3.start,words3.stop)); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end query

    
    // $ANTLR start rule
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:57:1: rule : RULE SPACE QUOTE words QUOTE NEWLINE meta IF NEWLINE condition action ;
    public void rule() // throws RecognitionException [1]
    {   
        words_return words4 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:57:9: ( RULE SPACE QUOTE words QUOTE NEWLINE meta IF NEWLINE condition action )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:57:9: RULE SPACE QUOTE words QUOTE NEWLINE meta IF NEWLINE condition action
            {
            	Match(input,RULE,FOLLOW_RULE_in_rule224); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rule226); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule228); 
            	PushFollow(FOLLOW_words_in_rule230);
            	words4 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule232); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule234); 
            	PushFollow(FOLLOW_meta_in_rule236);
            	meta();
            	followingStackPointer_--;

            	Match(input,IF,FOLLOW_IF_in_rule238); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule240); 
            	PushFollow(FOLLOW_condition_in_rule242);
            	condition();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_action_in_rule244);
            	action();
            	followingStackPointer_--;

            	 rbb.AddImplies(input.ToString(words4.start,words4.stop)); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end rule

    
    // $ANTLR start meta
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:1: meta : ( priority )? ( precondition )? ( mutex )* ;
    public void meta() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:8: ( ( priority )? ( precondition )? ( mutex )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:8: ( priority )? ( precondition )? ( mutex )*
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:8: ( priority )?
            	int alt3 = 2;
            	int LA3_0 = input.LA(1);
            	
            	if ( (LA3_0 == TAB) )
            	{
            	    int LA3_1 = input.LA(2);
            	    
            	    if ( (LA3_1 == PRIORITY) )
            	    {
            	        alt3 = 1;
            	    }
            	}
            	switch (alt3) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:8: priority
            	        {
            	        	PushFollow(FOLLOW_priority_in_meta254);
            	        	priority();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:18: ( precondition )?
            	int alt4 = 2;
            	int LA4_0 = input.LA(1);
            	
            	if ( (LA4_0 == TAB) )
            	{
            	    int LA4_1 = input.LA(2);
            	    
            	    if ( (LA4_1 == PRECONDITION) )
            	    {
            	        alt4 = 1;
            	    }
            	}
            	switch (alt4) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:18: precondition
            	        {
            	        	PushFollow(FOLLOW_precondition_in_meta257);
            	        	precondition();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:32: ( mutex )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);
            	    
            	    if ( (LA5_0 == TAB) )
            	    {
            	        alt5 = 1;
            	    }
            	    
            	
            	    switch (alt5) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:59:32: mutex
            			    {
            			    	PushFollow(FOLLOW_mutex_in_meta260);
            			    	mutex();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop5;
            	    }
            	} while (true);
            	
            	loop5:
            		;	// Stops C# compiler whinging that label 'loop5' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end meta

    
    // $ANTLR start priority
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:61:1: priority : TAB PRIORITY SPACE NUMERIC NEWLINE ;
    public void priority() // throws RecognitionException [1]
    {   
        IToken NUMERIC5 = null;
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:62:4: ( TAB PRIORITY SPACE NUMERIC NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:62:4: TAB PRIORITY SPACE NUMERIC NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_priority270); 
            	Match(input,PRIORITY,FOLLOW_PRIORITY_in_priority272); 
            	Match(input,SPACE,FOLLOW_SPACE_in_priority274); 
            	NUMERIC5 = (IToken)input.LT(1);
            	Match(input,NUMERIC,FOLLOW_NUMERIC_in_priority276); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_priority278); 
            	 rbb.CurrentImplicationData.priority=NUMERIC5.Text; 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end priority

    
    // $ANTLR start precondition
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:64:1: precondition : TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE ;
    public void precondition() // throws RecognitionException [1]
    {   
        words_return words6 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:65:4: ( TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:65:4: TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_precondition289); 
            	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_precondition291); 
            	Match(input,SPACE,FOLLOW_SPACE_in_precondition293); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition295); 
            	PushFollow(FOLLOW_words_in_precondition297);
            	words6 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition299); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_precondition301); 
            	 rbb.CurrentImplicationData.precondition=input.ToString(words6.start,words6.stop); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end precondition

    
    // $ANTLR start mutex
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:67:1: mutex : TAB MUTEX SPACE QUOTE words QUOTE NEWLINE ;
    public void mutex() // throws RecognitionException [1]
    {   
        words_return words7 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:67:9: ( TAB MUTEX SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:67:9: TAB MUTEX SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_mutex311); 
            	Match(input,MUTEX,FOLLOW_MUTEX_in_mutex313); 
            	Match(input,SPACE,FOLLOW_SPACE_in_mutex315); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex317); 
            	PushFollow(FOLLOW_words_in_mutex319);
            	words7 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex321); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_mutex323); 
            	 rbb.CurrentImplicationData.mutex=input.ToString(words7.start,words7.stop); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end mutex

    
    // $ANTLR start condition
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:69:1: condition : statement ( logic statement )* ;
    public void condition() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:70:4: ( statement ( logic statement )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:70:4: statement ( logic statement )*
            {
            	PushFollow(FOLLOW_statement_in_condition334);
            	statement();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:70:14: ( logic statement )*
            	do 
            	{
            	    int alt6 = 2;
            	    alt6 = dfa6.Predict(input);
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:70:15: logic statement
            			    {
            			    	PushFollow(FOLLOW_logic_in_condition337);
            			    	logic();
            			    	followingStackPointer_--;

            			    	PushFollow(FOLLOW_statement_in_condition339);
            			    	statement();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop6;
            	    }
            	} while (true);
            	
            	loop6:
            		;	// Stops C# compiler whinging that label 'loop6' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end condition

    
    // $ANTLR start action
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:72:1: action : THEN SPACE verb NEWLINE statement ;
    public void action() // throws RecognitionException [1]
    {   
        string verb8 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:72:10: ( THEN SPACE verb NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:72:10: THEN SPACE verb NEWLINE statement
            {
            	Match(input,THEN,FOLLOW_THEN_in_action349); 
            	Match(input,SPACE,FOLLOW_SPACE_in_action351); 
            	PushFollow(FOLLOW_verb_in_action353);
            	verb8 = verb();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_action355); 
            	PushFollow(FOLLOW_statement_in_action357);
            	statement();
            	followingStackPointer_--;

            	 rbb.CurrentImplicationData.action=verb8; ;
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end action

    
    // $ANTLR start statement
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:74:1: statement : indent words NEWLINE ;
    public void statement() // throws RecognitionException [1]
    {   
        indent_return indent9 = null;

        words_return words10 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:75:4: ( indent words NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:75:4: indent words NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_statement368);
            	indent9 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_words_in_statement370);
            	words10 = words();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_statement372); 
            	 rbb.AddStatement(input.ToString(indent9.start,indent9.stop).Length, input.ToString(words10.start,words10.stop)); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end statement

    
    // $ANTLR start logic
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:77:1: logic : indent booleanToken NEWLINE ;
    public void logic() // throws RecognitionException [1]
    {   
        indent_return indent11 = null;

        string booleanToken12 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:77:9: ( indent booleanToken NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:77:9: indent booleanToken NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_logic382);
            	indent11 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_booleanToken_in_logic384);
            	booleanToken12 = booleanToken();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_logic386); 
            	
            				rbb.AddLogicBlock(input.ToString(indent11.start,indent11.stop).Length, booleanToken12);
            			
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end logic

    
    // $ANTLR start verb
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:81:1: verb returns [string value] : ( DEDUCT | FORGET | COUNT | MODIFY ) ;
    public string verb() // throws RecognitionException [1]
    {   

        string value = null;
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:82:4: ( ( DEDUCT | FORGET | COUNT | MODIFY ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:82:4: ( DEDUCT | FORGET | COUNT | MODIFY )
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:82:4: ( DEDUCT | FORGET | COUNT | MODIFY )
            	int alt7 = 4;
            	switch ( input.LA(1) ) 
            	{
            	case DEDUCT:
            		{
            	    alt7 = 1;
            	    }
            	    break;
            	case FORGET:
            		{
            	    alt7 = 2;
            	    }
            	    break;
            	case COUNT:
            		{
            	    alt7 = 3;
            	    }
            	    break;
            	case MODIFY:
            		{
            	    alt7 = 4;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d7s0 =
            		        new NoViableAltException("82:4: ( DEDUCT | FORGET | COUNT | MODIFY )", 7, 0, input);
            	
            		    throw nvae_d7s0;
            	}
            	
            	switch (alt7) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:82:5: DEDUCT
            	        {
            	        	Match(input,DEDUCT,FOLLOW_DEDUCT_in_verb403); 
            	        	 value =  "assert"; 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:83:5: FORGET
            	        {
            	        	Match(input,FORGET,FOLLOW_FORGET_in_verb411); 
            	        	 value =  "retract"; 
            	        
            	        }
            	        break;
            	    case 3 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:84:5: COUNT
            	        {
            	        	Match(input,COUNT,FOLLOW_COUNT_in_verb419); 
            	        	 value =  "count"; 
            	        
            	        }
            	        break;
            	    case 4 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:85:5: MODIFY
            	        {
            	        	Match(input,MODIFY,FOLLOW_MODIFY_in_verb427); 
            	        	 value =  "modify"; 
            	        
            	        }
            	        break;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return value;
    }
    // $ANTLR end verb

    public class words_return : ParserRuleReturnScope 
    {
    };
    
    // $ANTLR start words
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:87:1: words : word ( SPACE word )* ;
    public words_return words() // throws RecognitionException [1]
    {   
        words_return retval = new words_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:87:9: ( word ( SPACE word )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:87:9: word ( SPACE word )*
            {
            	PushFollow(FOLLOW_word_in_words438);
            	word();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:87:14: ( SPACE word )*
            	do 
            	{
            	    int alt8 = 2;
            	    int LA8_0 = input.LA(1);
            	    
            	    if ( (LA8_0 == SPACE) )
            	    {
            	        alt8 = 1;
            	    }
            	    
            	
            	    switch (alt8) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:87:15: SPACE word
            			    {
            			    	Match(input,SPACE,FOLLOW_SPACE_in_words441); 
            			    	PushFollow(FOLLOW_word_in_words443);
            			    	word();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop8;
            	    }
            	} while (true);
            	
            	loop8:
            		;	// Stops C# compiler whinging that label 'loop8' has no statements

            
            }
    
            retval.stop = input.LT(-1);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end words

    
    // $ANTLR start word
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:1: word : ( anyToken | CHAR | NUMERIC )+ ;
    public void word() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:8: ( ( anyToken | CHAR | NUMERIC )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:8: ( anyToken | CHAR | NUMERIC )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:8: ( anyToken | CHAR | NUMERIC )+
            	int cnt9 = 0;
            	do 
            	{
            	    int alt9 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case RULEBASE:
            	    case FACT:
            	    case QUERY:
            	    case RULE:
            	    case PRIORITY:
            	    case PRECONDITION:
            	    case MUTEX:
            	    case IF:
            	    case THEN:
            	    case DEDUCT:
            	    case FORGET:
            	    case COUNT:
            	    case MODIFY:
            	    case AND:
            	    case OR:
            	    	{
            	        alt9 = 1;
            	        }
            	        break;
            	    case CHAR:
            	    	{
            	        alt9 = 2;
            	        }
            	        break;
            	    case NUMERIC:
            	    	{
            	        alt9 = 3;
            	        }
            	        break;
            	    
            	    }
            	
            	    switch (alt9) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:9: anyToken
            			    {
            			    	PushFollow(FOLLOW_anyToken_in_word454);
            			    	anyToken();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:20: CHAR
            			    {
            			    	Match(input,CHAR,FOLLOW_CHAR_in_word458); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:89:27: NUMERIC
            			    {
            			    	Match(input,NUMERIC,FOLLOW_NUMERIC_in_word462); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt9 >= 1 ) goto loop9;
            		            EarlyExitException eee =
            		                new EarlyExitException(9, input);
            		            throw eee;
            	    }
            	    cnt9++;
            	} while (true);
            	
            	loop9:
            		;	// Stops C# compiler whinging that label 'loop9' has no statements

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end word

    
    // $ANTLR start ignored
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:91:1: ignored : ( TAB | SPACE )* NEWLINE ;
    public void ignored() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:91:11: ( ( TAB | SPACE )* NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:91:11: ( TAB | SPACE )* NEWLINE
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:91:11: ( TAB | SPACE )*
            	do 
            	{
            	    int alt10 = 2;
            	    int LA10_0 = input.LA(1);
            	    
            	    if ( (LA10_0 == SPACE || LA10_0 == TAB) )
            	    {
            	        alt10 = 1;
            	    }
            	    
            	
            	    switch (alt10) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:
            			    {
            			    	if ( input.LA(1) == SPACE || input.LA(1) == TAB ) 
            			    	{
            			    	    input.Consume();
            			    	    errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_ignored472);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop10;
            	    }
            	} while (true);
            	
            	loop10:
            		;	// Stops C# compiler whinging that label 'loop10' has no statements

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_ignored481); 
            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end ignored

    public class indent_return : ParserRuleReturnScope 
    {
    };
    
    // $ANTLR start indent
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:93:1: indent : ( TAB )+ ;
    public indent_return indent() // throws RecognitionException [1]
    {   
        indent_return retval = new indent_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:93:10: ( ( TAB )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:93:10: ( TAB )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:93:10: ( TAB )+
            	int cnt11 = 0;
            	do 
            	{
            	    int alt11 = 2;
            	    int LA11_0 = input.LA(1);
            	    
            	    if ( (LA11_0 == TAB) )
            	    {
            	        alt11 = 1;
            	    }
            	    
            	
            	    switch (alt11) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:93:10: TAB
            			    {
            			    	Match(input,TAB,FOLLOW_TAB_in_indent489); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt11 >= 1 ) goto loop11;
            		            EarlyExitException eee =
            		                new EarlyExitException(11, input);
            		            throw eee;
            	    }
            	    cnt11++;
            	} while (true);
            	
            	loop11:
            		;	// Stops C# compiler whinging that label 'loop11' has no statements

            
            }
    
            retval.stop = input.LT(-1);
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end indent

    
    // $ANTLR start anyToken
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:95:1: anyToken : ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) ;
    public void anyToken() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:4: ( ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            	int alt12 = 14;
            	switch ( input.LA(1) ) 
            	{
            	case RULEBASE:
            		{
            	    alt12 = 1;
            	    }
            	    break;
            	case FACT:
            		{
            	    alt12 = 2;
            	    }
            	    break;
            	case QUERY:
            		{
            	    alt12 = 3;
            	    }
            	    break;
            	case RULE:
            		{
            	    alt12 = 4;
            	    }
            	    break;
            	case PRIORITY:
            		{
            	    alt12 = 5;
            	    }
            	    break;
            	case PRECONDITION:
            		{
            	    alt12 = 6;
            	    }
            	    break;
            	case MUTEX:
            		{
            	    alt12 = 7;
            	    }
            	    break;
            	case IF:
            		{
            	    alt12 = 8;
            	    }
            	    break;
            	case THEN:
            		{
            	    alt12 = 9;
            	    }
            	    break;
            	case DEDUCT:
            		{
            	    alt12 = 10;
            	    }
            	    break;
            	case FORGET:
            		{
            	    alt12 = 11;
            	    }
            	    break;
            	case COUNT:
            		{
            	    alt12 = 12;
            	    }
            	    break;
            	case MODIFY:
            		{
            	    alt12 = 13;
            	    }
            	    break;
            	case AND:
            	case OR:
            		{
            	    alt12 = 14;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d12s0 =
            		        new NoViableAltException("96:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )", 12, 0, input);
            	
            		    throw nvae_d12s0;
            	}
            	
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:5: RULEBASE
            	        {
            	        	Match(input,RULEBASE,FOLLOW_RULEBASE_in_anyToken500); 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:16: FACT
            	        {
            	        	Match(input,FACT,FOLLOW_FACT_in_anyToken504); 
            	        
            	        }
            	        break;
            	    case 3 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:23: QUERY
            	        {
            	        	Match(input,QUERY,FOLLOW_QUERY_in_anyToken508); 
            	        
            	        }
            	        break;
            	    case 4 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:31: RULE
            	        {
            	        	Match(input,RULE,FOLLOW_RULE_in_anyToken512); 
            	        
            	        }
            	        break;
            	    case 5 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:38: PRIORITY
            	        {
            	        	Match(input,PRIORITY,FOLLOW_PRIORITY_in_anyToken516); 
            	        
            	        }
            	        break;
            	    case 6 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:49: PRECONDITION
            	        {
            	        	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_anyToken520); 
            	        
            	        }
            	        break;
            	    case 7 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:64: MUTEX
            	        {
            	        	Match(input,MUTEX,FOLLOW_MUTEX_in_anyToken524); 
            	        
            	        }
            	        break;
            	    case 8 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:72: IF
            	        {
            	        	Match(input,IF,FOLLOW_IF_in_anyToken528); 
            	        
            	        }
            	        break;
            	    case 9 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:77: THEN
            	        {
            	        	Match(input,THEN,FOLLOW_THEN_in_anyToken532); 
            	        
            	        }
            	        break;
            	    case 10 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:84: DEDUCT
            	        {
            	        	Match(input,DEDUCT,FOLLOW_DEDUCT_in_anyToken536); 
            	        
            	        }
            	        break;
            	    case 11 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:93: FORGET
            	        {
            	        	Match(input,FORGET,FOLLOW_FORGET_in_anyToken540); 
            	        
            	        }
            	        break;
            	    case 12 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:102: COUNT
            	        {
            	        	Match(input,COUNT,FOLLOW_COUNT_in_anyToken544); 
            	        
            	        }
            	        break;
            	    case 13 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:110: MODIFY
            	        {
            	        	Match(input,MODIFY,FOLLOW_MODIFY_in_anyToken548); 
            	        
            	        }
            	        break;
            	    case 14 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:96:119: booleanToken
            	        {
            	        	PushFollow(FOLLOW_booleanToken_in_anyToken552);
            	        	booleanToken();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return ;
    }
    // $ANTLR end anyToken

    
    // $ANTLR start booleanToken
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:98:1: booleanToken returns [string value] : ( AND | OR ) ;
    public string booleanToken() // throws RecognitionException [1]
    {   

        string value = null;
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:99:4: ( ( AND | OR ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:99:4: ( AND | OR )
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:99:4: ( AND | OR )
            	int alt13 = 2;
            	int LA13_0 = input.LA(1);
            	
            	if ( (LA13_0 == AND) )
            	{
            	    alt13 = 1;
            	}
            	else if ( (LA13_0 == OR) )
            	{
            	    alt13 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d13s0 =
            	        new NoViableAltException("99:4: ( AND | OR )", 13, 0, input);
            	
            	    throw nvae_d13s0;
            	}
            	switch (alt13) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:99:6: AND
            	        {
            	        	Match(input,AND,FOLLOW_AND_in_booleanToken567); 
            	        	 value =  "And"; 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:100:5: OR
            	        {
            	        	Match(input,OR,FOLLOW_OR_in_booleanToken575); 
            	        	 value =  "Or"; 
            	        
            	        }
            	        break;
            	
            	}

            
            }
    
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
        }
        finally 
    	{
        }
        return value;
    }
    // $ANTLR end booleanToken


   	protected DFA6 dfa6;
	private void InitializeCyclicDFAs()
	{
    	this.dfa6 = new DFA6(this);
	}

    static readonly short[] DFA6_eot = {
        -1, -1, -1, -1
        };
    static readonly short[] DFA6_eof = {
        1, -1, -1, -1
        };
    static readonly int[] DFA6_min = {
        5, 0, 17, 0
        };
    static readonly int[] DFA6_max = {
        22, 0, 22, 0
        };
    static readonly short[] DFA6_accept = {
        -1, 2, -1, 1
        };
    static readonly short[] DFA6_special = {
        -1, -1, -1, -1
        };
    
    static readonly short[] dfa6_transition_null = null;

    static readonly short[] dfa6_transition0 = {
    	1, 1, 1, -1, -1, -1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 1, 2
    	};
    static readonly short[] dfa6_transition1 = {
    	3, 3, 1, -1, 1, 2
    	};
    
    static readonly short[][] DFA6_transition = {
    	dfa6_transition0,
    	dfa6_transition_null,
    	dfa6_transition1,
    	dfa6_transition_null
        };
    
    protected class DFA6 : DFA
    {
        public DFA6(BaseRecognizer recognizer) 
        {
            this.recognizer = recognizer;
            this.decisionNumber = 6;
            this.eot = DFA6_eot;
            this.eof = DFA6_eof;
            this.min = DFA6_min;
            this.max = DFA6_max;
            this.accept     = DFA6_accept;
            this.special    = DFA6_special;
            this.transition = DFA6_transition;
        }
    
        override public string Description
        {
            get { return "()* loopback of 70:14: ( logic statement )*"; }
        }
    
    }
    
 

    public static readonly BitSet FOLLOW_RULEBASE_in_rulebase138 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rulebase140 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase142 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_rulebase144 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase146 = new BitSet(new ulong[]{0x00000000006800E0UL});
    public static readonly BitSet FOLLOW_rule_in_rulebase149 = new BitSet(new ulong[]{0x00000000006800E0UL});
    public static readonly BitSet FOLLOW_query_in_rulebase153 = new BitSet(new ulong[]{0x00000000006800E0UL});
    public static readonly BitSet FOLLOW_fact_in_rulebase157 = new BitSet(new ulong[]{0x00000000006800E0UL});
    public static readonly BitSet FOLLOW_ignored_in_rulebase161 = new BitSet(new ulong[]{0x00000000006800E0UL});
    public static readonly BitSet FOLLOW_EOF_in_rulebase165 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_fact176 = new BitSet(new ulong[]{0x0000000000280000UL});
    public static readonly BitSet FOLLOW_SPACE_in_fact179 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact181 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_fact183 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact185 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_fact189 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_statement_in_fact191 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_query201 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_query203 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query205 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_query207 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query209 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_query211 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_condition_in_query213 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_rule224 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rule226 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule228 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_rule230 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule232 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule234 = new BitSet(new ulong[]{0x0000000000400800UL});
    public static readonly BitSet FOLLOW_meta_in_rule236 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_IF_in_rule238 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule240 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_condition_in_rule242 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_action_in_rule244 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_priority_in_meta254 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_precondition_in_meta257 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_mutex_in_meta260 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_TAB_in_priority270 = new BitSet(new ulong[]{0x0000000000000100UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_priority272 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_priority274 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_priority276 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_priority278 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_precondition289 = new BitSet(new ulong[]{0x0000000000000200UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_precondition291 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_precondition293 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition295 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_precondition297 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition299 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_precondition301 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_mutex311 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_MUTEX_in_mutex313 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_mutex315 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex317 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_mutex319 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex321 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_mutex323 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_in_condition334 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_logic_in_condition337 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_statement_in_condition339 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_THEN_in_action349 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_SPACE_in_action351 = new BitSet(new ulong[]{0x000000000001E000UL});
    public static readonly BitSet FOLLOW_verb_in_action353 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_action355 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_statement_in_action357 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_statement368 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_words_in_statement370 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_statement372 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_logic382 = new BitSet(new ulong[]{0x0000000000060000UL});
    public static readonly BitSet FOLLOW_booleanToken_in_logic384 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_logic386 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEDUCT_in_verb403 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FORGET_in_verb411 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COUNT_in_verb419 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MODIFY_in_verb427 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_word_in_words438 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_SPACE_in_words441 = new BitSet(new ulong[]{0x000000000187FFF0UL});
    public static readonly BitSet FOLLOW_word_in_words443 = new BitSet(new ulong[]{0x0000000000080002UL});
    public static readonly BitSet FOLLOW_anyToken_in_word454 = new BitSet(new ulong[]{0x000000000187FFF2UL});
    public static readonly BitSet FOLLOW_CHAR_in_word458 = new BitSet(new ulong[]{0x000000000187FFF2UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_word462 = new BitSet(new ulong[]{0x000000000187FFF2UL});
    public static readonly BitSet FOLLOW_set_in_ignored472 = new BitSet(new ulong[]{0x0000000000680000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_ignored481 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_indent489 = new BitSet(new ulong[]{0x0000000000400002UL});
    public static readonly BitSet FOLLOW_RULEBASE_in_anyToken500 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_anyToken504 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_anyToken508 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_anyToken512 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_anyToken516 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_anyToken520 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUTEX_in_anyToken524 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IF_in_anyToken528 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_THEN_in_anyToken532 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEDUCT_in_anyToken536 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FORGET_in_anyToken540 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COUNT_in_anyToken544 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MODIFY_in_anyToken548 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_booleanToken_in_anyToken552 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_AND_in_booleanToken567 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_OR_in_booleanToken575 = new BitSet(new ulong[]{0x0000000000000002UL});

}
