// $ANTLR 3.0 C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g 2007-05-29 21:23:23

using System.Collections.Generic;


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



public class InferenceRules_ENParser : Parser 
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
		"CHAR", 
		"'if'"
    };

    public const int SPACE = 18;
    public const int PRIORITY = 8;
    public const int CHAR = 23;
    public const int TAB = 21;
    public const int DEDUCT = 12;
    public const int QUERY = 6;
    public const int THEN = 11;
    public const int RULE = 7;
    public const int QUOTE = 19;
    public const int OR = 17;
    public const int NEWLINE = 20;
    public const int PRECONDITION = 9;
    public const int AND = 16;
    public const int RULEBASE = 4;
    public const int EOF = -1;
    public const int FORGET = 13;
    public const int COUNT = 14;
    public const int FACT = 5;
    public const int MODIFY = 15;
    public const int MUTEX = 10;
    public const int NUMERIC = 22;
    
    
        public InferenceRules_ENParser(ITokenStream input) 
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
		get { return "C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g"; }
	}

    
    IDictionary<int, string> logicBlocks = new Dictionary<int, string>();


    
    // $ANTLR start rulebase
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:36:1: rulebase : RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF ;
    public void rulebase() // throws RecognitionException [1]
    {   
        words_return words1 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:4: ( RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:4: RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF
            {
            	Match(input,RULEBASE,FOLLOW_RULEBASE_in_rulebase132); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rulebase134); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase136); 
            	PushFollow(FOLLOW_words_in_rulebase138);
            	words1 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase140); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:37: ( rule | query | fact | ignored )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:38: rule
            			    {
            			    	PushFollow(FOLLOW_rule_in_rulebase143);
            			    	rule();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:45: query
            			    {
            			    	PushFollow(FOLLOW_query_in_rulebase147);
            			    	query();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:53: fact
            			    {
            			    	PushFollow(FOLLOW_fact_in_rulebase151);
            			    	fact();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 4 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:37:60: ignored
            			    {
            			    	PushFollow(FOLLOW_ignored_in_rulebase155);
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

            	Match(input,EOF,FOLLOW_EOF_in_rulebase159); 
            	Console.WriteLine("rulebase label: "+input.ToString(words1.start,words1.stop));
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:1: fact : FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement ;
    public void fact() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:8: ( FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:8: FACT ( SPACE QUOTE words QUOTE )? NEWLINE statement
            {
            	Match(input,FACT,FOLLOW_FACT_in_fact170); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:13: ( SPACE QUOTE words QUOTE )?
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);
            	
            	if ( (LA2_0 == SPACE) )
            	{
            	    alt2 = 1;
            	}
            	switch (alt2) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:14: SPACE QUOTE words QUOTE
            	        {
            	        	Match(input,SPACE,FOLLOW_SPACE_in_fact173); 
            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact175); 
            	        	PushFollow(FOLLOW_words_in_fact177);
            	        	words();
            	        	followingStackPointer_--;

            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact179); 
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_fact183); 
            	PushFollow(FOLLOW_statement_in_fact185);
            	statement();
            	followingStackPointer_--;

            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:1: query : QUERY SPACE QUOTE words QUOTE NEWLINE condition ;
    public void query() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:9: ( QUERY SPACE QUOTE words QUOTE NEWLINE condition )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:9: QUERY SPACE QUOTE words QUOTE NEWLINE condition
            {
            	Match(input,QUERY,FOLLOW_QUERY_in_query193); 
            	Match(input,SPACE,FOLLOW_SPACE_in_query195); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_query197); 
            	PushFollow(FOLLOW_words_in_query199);
            	words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_query201); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_query203); 
            	PushFollow(FOLLOW_condition_in_query205);
            	condition();
            	followingStackPointer_--;

            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:1: rule : RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action ;
    public void rule() // throws RecognitionException [1]
    {   
        words_return words2 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:9: ( RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:9: RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action
            {
            	Match(input,RULE,FOLLOW_RULE_in_rule214); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rule216); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule218); 
            	PushFollow(FOLLOW_words_in_rule220);
            	words2 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule222); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule224); 
            	PushFollow(FOLLOW_meta_in_rule226);
            	meta();
            	followingStackPointer_--;

            	Match(input,24,FOLLOW_24_in_rule228); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule230); 
            	PushFollow(FOLLOW_condition_in_rule232);
            	condition();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_action_in_rule234);
            	action();
            	followingStackPointer_--;

            	Console.WriteLine("rule label: "+input.ToString(words2.start,words2.stop));
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:1: meta : ( priority )? ( precondition )? ( mutex )* ;
    public void meta() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:8: ( ( priority )? ( precondition )? ( mutex )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:8: ( priority )? ( precondition )? ( mutex )*
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:8: ( priority )?
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
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:8: priority
            	        {
            	        	PushFollow(FOLLOW_priority_in_meta244);
            	        	priority();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:18: ( precondition )?
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
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:18: precondition
            	        {
            	        	PushFollow(FOLLOW_precondition_in_meta247);
            	        	precondition();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:32: ( mutex )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:32: mutex
            			    {
            			    	PushFollow(FOLLOW_mutex_in_meta250);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:1: priority : TAB PRIORITY SPACE NUMERIC NEWLINE ;
    public void priority() // throws RecognitionException [1]
    {   
        IToken NUMERIC3 = null;
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:48:4: ( TAB PRIORITY SPACE NUMERIC NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:48:4: TAB PRIORITY SPACE NUMERIC NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_priority260); 
            	Match(input,PRIORITY,FOLLOW_PRIORITY_in_priority262); 
            	Match(input,SPACE,FOLLOW_SPACE_in_priority264); 
            	NUMERIC3 = (IToken)input.LT(1);
            	Match(input,NUMERIC,FOLLOW_NUMERIC_in_priority266); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_priority268); 
            	Console.WriteLine("priority: "+NUMERIC3.Text);
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:50:1: precondition : TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE ;
    public void precondition() // throws RecognitionException [1]
    {   
        words_return words4 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:51:4: ( TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:51:4: TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_precondition279); 
            	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_precondition281); 
            	Match(input,SPACE,FOLLOW_SPACE_in_precondition283); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition285); 
            	PushFollow(FOLLOW_words_in_precondition287);
            	words4 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition289); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_precondition291); 
            	Console.WriteLine("precondition: "+input.ToString(words4.start,words4.stop));
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:53:1: mutex : TAB MUTEX SPACE QUOTE words QUOTE NEWLINE ;
    public void mutex() // throws RecognitionException [1]
    {   
        words_return words5 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:53:9: ( TAB MUTEX SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:53:9: TAB MUTEX SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_mutex301); 
            	Match(input,MUTEX,FOLLOW_MUTEX_in_mutex303); 
            	Match(input,SPACE,FOLLOW_SPACE_in_mutex305); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex307); 
            	PushFollow(FOLLOW_words_in_mutex309);
            	words5 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex311); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_mutex313); 
            	Console.WriteLine("mutex: "+input.ToString(words5.start,words5.stop));
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:55:1: condition : statement ( logic statement )* ;
    public void condition() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:56:4: ( statement ( logic statement )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:56:4: statement ( logic statement )*
            {
            	PushFollow(FOLLOW_statement_in_condition324);
            	statement();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:56:14: ( logic statement )*
            	do 
            	{
            	    int alt6 = 2;
            	    alt6 = dfa6.Predict(input);
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:56:15: logic statement
            			    {
            			    	PushFollow(FOLLOW_logic_in_condition327);
            			    	logic();
            			    	followingStackPointer_--;

            			    	PushFollow(FOLLOW_statement_in_condition329);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:1: action : THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement ;
    public void action() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:10: ( THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:10: THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement
            {
            	Match(input,THEN,FOLLOW_THEN_in_action339); 
            	Match(input,SPACE,FOLLOW_SPACE_in_action341); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:21: ( DEDUCT | FORGET | COUNT | MODIFY )+
            	int cnt7 = 0;
            	do 
            	{
            	    int alt7 = 2;
            	    int LA7_0 = input.LA(1);
            	    
            	    if ( ((LA7_0 >= DEDUCT && LA7_0 <= MODIFY)) )
            	    {
            	        alt7 = 1;
            	    }
            	    
            	
            	    switch (alt7) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:
            			    {
            			    	if ( (input.LA(1) >= DEDUCT && input.LA(1) <= MODIFY) ) 
            			    	{
            			    	    input.Consume();
            			    	    errorRecovery = false;
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_action343);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt7 >= 1 ) goto loop7;
            		            EarlyExitException eee =
            		                new EarlyExitException(7, input);
            		            throw eee;
            	    }
            	    cnt7++;
            	} while (true);
            	
            	loop7:
            		;	// Stops C# compiler whinging that label 'loop7' has no statements

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_action360); 
            	PushFollow(FOLLOW_statement_in_action362);
            	statement();
            	followingStackPointer_--;

            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:60:1: statement : indent words NEWLINE ;
    public void statement() // throws RecognitionException [1]
    {   
        words_return words6 = null;

        indent_return indent7 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:61:4: ( indent words NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:61:4: indent words NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_statement371);
            	indent7 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_words_in_statement373);
            	words6 = words();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_statement375); 
            	Console.WriteLine("depth of: '"+input.ToString(words6.start,words6.stop)+"' is: "+input.ToString(indent7.start,indent7.stop).Length);
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:63:1: logic : indent booleanToken NEWLINE ;
    public void logic() // throws RecognitionException [1]
    {   
        indent_return indent8 = null;

        booleanToken_return booleanToken9 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:63:9: ( indent booleanToken NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:63:9: indent booleanToken NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_logic385);
            	indent8 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_booleanToken_in_logic387);
            	booleanToken9 = booleanToken();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_logic389); 
            	
            				int depth = input.ToString(indent8.start,indent8.stop).Length;
            				string newOperator = input.ToString(booleanToken9.start,booleanToken9.stop);
            				
            				Console.WriteLine("depth of op.: '{0}' is: {1}", newOperator, depth);
            				
            				string existingOperator;
            				
            				if (logicBlocks.TryGetValue(depth, out existingOperator)) {
            					if (!newOperator.Equals(existingOperator)) throw new Exception("Operator mismatch at depth: " + depth);
            				}
            				else {
            					logicBlocks.Add(depth, newOperator);
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
    // $ANTLR end logic

    public class words_return : ParserRuleReturnScope 
    {
    };
    
    // $ANTLR start words
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:79:1: words : word ( SPACE word )* ;
    public words_return words() // throws RecognitionException [1]
    {   
        words_return retval = new words_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:79:9: ( word ( SPACE word )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:79:9: word ( SPACE word )*
            {
            	PushFollow(FOLLOW_word_in_words399);
            	word();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:79:14: ( SPACE word )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:79:15: SPACE word
            			    {
            			    	Match(input,SPACE,FOLLOW_SPACE_in_words402); 
            			    	PushFollow(FOLLOW_word_in_words404);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:1: word : ( anyToken | CHAR | NUMERIC )+ ;
    public void word() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:8: ( ( anyToken | CHAR | NUMERIC )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:8: ( anyToken | CHAR | NUMERIC )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:8: ( anyToken | CHAR | NUMERIC )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:9: anyToken
            			    {
            			    	PushFollow(FOLLOW_anyToken_in_word415);
            			    	anyToken();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:20: CHAR
            			    {
            			    	Match(input,CHAR,FOLLOW_CHAR_in_word419); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:27: NUMERIC
            			    {
            			    	Match(input,NUMERIC,FOLLOW_NUMERIC_in_word423); 
            			    
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:1: ignored : ( TAB | SPACE )* NEWLINE ;
    public void ignored() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:11: ( ( TAB | SPACE )* NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:11: ( TAB | SPACE )* NEWLINE
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:11: ( TAB | SPACE )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:
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
            			    	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_ignored433);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop10;
            	    }
            	} while (true);
            	
            	loop10:
            		;	// Stops C# compiler whinging that label 'loop10' has no statements

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_ignored442); 
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:1: indent : ( TAB )+ ;
    public indent_return indent() // throws RecognitionException [1]
    {   
        indent_return retval = new indent_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:10: ( ( TAB )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:10: ( TAB )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:10: ( TAB )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:10: TAB
            			    {
            			    	Match(input,TAB,FOLLOW_TAB_in_indent450); 
            			    
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:1: anyToken : ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) ;
    public void anyToken() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:4: ( ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            	int alt12 = 13;
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
            	case THEN:
            		{
            	    alt12 = 8;
            	    }
            	    break;
            	case DEDUCT:
            		{
            	    alt12 = 9;
            	    }
            	    break;
            	case FORGET:
            		{
            	    alt12 = 10;
            	    }
            	    break;
            	case COUNT:
            		{
            	    alt12 = 11;
            	    }
            	    break;
            	case MODIFY:
            		{
            	    alt12 = 12;
            	    }
            	    break;
            	case AND:
            	case OR:
            		{
            	    alt12 = 13;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d12s0 =
            		        new NoViableAltException("88:4: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )", 12, 0, input);
            	
            		    throw nvae_d12s0;
            	}
            	
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:5: RULEBASE
            	        {
            	        	Match(input,RULEBASE,FOLLOW_RULEBASE_in_anyToken461); 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:16: FACT
            	        {
            	        	Match(input,FACT,FOLLOW_FACT_in_anyToken465); 
            	        
            	        }
            	        break;
            	    case 3 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:23: QUERY
            	        {
            	        	Match(input,QUERY,FOLLOW_QUERY_in_anyToken469); 
            	        
            	        }
            	        break;
            	    case 4 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:31: RULE
            	        {
            	        	Match(input,RULE,FOLLOW_RULE_in_anyToken473); 
            	        
            	        }
            	        break;
            	    case 5 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:38: PRIORITY
            	        {
            	        	Match(input,PRIORITY,FOLLOW_PRIORITY_in_anyToken477); 
            	        
            	        }
            	        break;
            	    case 6 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:49: PRECONDITION
            	        {
            	        	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_anyToken481); 
            	        
            	        }
            	        break;
            	    case 7 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:64: MUTEX
            	        {
            	        	Match(input,MUTEX,FOLLOW_MUTEX_in_anyToken485); 
            	        
            	        }
            	        break;
            	    case 8 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:72: THEN
            	        {
            	        	Match(input,THEN,FOLLOW_THEN_in_anyToken489); 
            	        
            	        }
            	        break;
            	    case 9 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:79: DEDUCT
            	        {
            	        	Match(input,DEDUCT,FOLLOW_DEDUCT_in_anyToken493); 
            	        
            	        }
            	        break;
            	    case 10 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:88: FORGET
            	        {
            	        	Match(input,FORGET,FOLLOW_FORGET_in_anyToken497); 
            	        
            	        }
            	        break;
            	    case 11 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:97: COUNT
            	        {
            	        	Match(input,COUNT,FOLLOW_COUNT_in_anyToken501); 
            	        
            	        }
            	        break;
            	    case 12 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:105: MODIFY
            	        {
            	        	Match(input,MODIFY,FOLLOW_MODIFY_in_anyToken505); 
            	        
            	        }
            	        break;
            	    case 13 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:88:114: booleanToken
            	        {
            	        	PushFollow(FOLLOW_booleanToken_in_anyToken509);
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

    public class booleanToken_return : ParserRuleReturnScope 
    {
    };
    
    // $ANTLR start booleanToken
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:1: booleanToken : ( AND | OR ) ;
    public booleanToken_return booleanToken() // throws RecognitionException [1]
    {   
        booleanToken_return retval = new booleanToken_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:91:4: ( ( AND | OR ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:91:4: ( AND | OR )
            {
            	if ( (input.LA(1) >= AND && input.LA(1) <= OR) ) 
            	{
            	    input.Consume();
            	    errorRecovery = false;
            	}
            	else 
            	{
            	    MismatchedSetException mse =
            	        new MismatchedSetException(null,input);
            	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_booleanToken519);    throw mse;
            	}

            
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
        5, 0, 16, 0
        };
    static readonly int[] DFA6_max = {
        21, 0, 21, 0
        };
    static readonly short[] DFA6_accept = {
        -1, 2, -1, 1
        };
    static readonly short[] DFA6_special = {
        -1, -1, -1, -1
        };
    
    static readonly short[] dfa6_transition_null = null;

    static readonly short[] dfa6_transition0 = {
    	1, 1, 1, -1, -1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 1, 2
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
            get { return "()* loopback of 56:14: ( logic statement )*"; }
        }
    
    }
    
 

    public static readonly BitSet FOLLOW_RULEBASE_in_rulebase132 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rulebase134 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase136 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_rulebase138 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase140 = new BitSet(new ulong[]{0x00000000003400E0UL});
    public static readonly BitSet FOLLOW_rule_in_rulebase143 = new BitSet(new ulong[]{0x00000000003400E0UL});
    public static readonly BitSet FOLLOW_query_in_rulebase147 = new BitSet(new ulong[]{0x00000000003400E0UL});
    public static readonly BitSet FOLLOW_fact_in_rulebase151 = new BitSet(new ulong[]{0x00000000003400E0UL});
    public static readonly BitSet FOLLOW_ignored_in_rulebase155 = new BitSet(new ulong[]{0x00000000003400E0UL});
    public static readonly BitSet FOLLOW_EOF_in_rulebase159 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_fact170 = new BitSet(new ulong[]{0x0000000000140000UL});
    public static readonly BitSet FOLLOW_SPACE_in_fact173 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact175 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_fact177 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact179 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_fact183 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_statement_in_fact185 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_query193 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_query195 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query197 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_query199 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query201 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_query203 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_condition_in_query205 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_rule214 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rule216 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule218 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_rule220 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule222 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule224 = new BitSet(new ulong[]{0x0000000001200000UL});
    public static readonly BitSet FOLLOW_meta_in_rule226 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_24_in_rule228 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule230 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_condition_in_rule232 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_action_in_rule234 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_priority_in_meta244 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_precondition_in_meta247 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_mutex_in_meta250 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_TAB_in_priority260 = new BitSet(new ulong[]{0x0000000000000100UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_priority262 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_priority264 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_priority266 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_priority268 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_precondition279 = new BitSet(new ulong[]{0x0000000000000200UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_precondition281 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_precondition283 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition285 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_precondition287 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition289 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_precondition291 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_mutex301 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_MUTEX_in_mutex303 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_mutex305 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex307 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_mutex309 = new BitSet(new ulong[]{0x0000000000080000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex311 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_mutex313 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_in_condition324 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_logic_in_condition327 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_statement_in_condition329 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_THEN_in_action339 = new BitSet(new ulong[]{0x0000000000040000UL});
    public static readonly BitSet FOLLOW_SPACE_in_action341 = new BitSet(new ulong[]{0x000000000000F000UL});
    public static readonly BitSet FOLLOW_set_in_action343 = new BitSet(new ulong[]{0x000000000010F000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_action360 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_statement_in_action362 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_statement371 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_words_in_statement373 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_statement375 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_logic385 = new BitSet(new ulong[]{0x0000000000030000UL});
    public static readonly BitSet FOLLOW_booleanToken_in_logic387 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_logic389 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_word_in_words399 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_SPACE_in_words402 = new BitSet(new ulong[]{0x0000000000C3FFF0UL});
    public static readonly BitSet FOLLOW_word_in_words404 = new BitSet(new ulong[]{0x0000000000040002UL});
    public static readonly BitSet FOLLOW_anyToken_in_word415 = new BitSet(new ulong[]{0x0000000000C3FFF2UL});
    public static readonly BitSet FOLLOW_CHAR_in_word419 = new BitSet(new ulong[]{0x0000000000C3FFF2UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_word423 = new BitSet(new ulong[]{0x0000000000C3FFF2UL});
    public static readonly BitSet FOLLOW_set_in_ignored433 = new BitSet(new ulong[]{0x0000000000340000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_ignored442 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_indent450 = new BitSet(new ulong[]{0x0000000000200002UL});
    public static readonly BitSet FOLLOW_RULEBASE_in_anyToken461 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_anyToken465 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_anyToken469 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_anyToken473 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_anyToken477 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_anyToken481 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUTEX_in_anyToken485 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_THEN_in_anyToken489 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEDUCT_in_anyToken493 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FORGET_in_anyToken497 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COUNT_in_anyToken501 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MODIFY_in_anyToken505 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_booleanToken_in_anyToken509 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_booleanToken519 = new BitSet(new ulong[]{0x0000000000000002UL});

}
