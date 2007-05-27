// $ANTLR 3.0 C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g 2007-05-27 13:56:57

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
		"FOR", 
		"IS", 
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

    public const int SPACE = 20;
    public const int PRIORITY = 10;
    public const int CHAR = 25;
    public const int IS = 7;
    public const int TAB = 23;
    public const int DEDUCT = 14;
    public const int QUERY = 8;
    public const int THEN = 13;
    public const int RULE = 9;
    public const int QUOTE = 21;
    public const int OR = 19;
    public const int NEWLINE = 22;
    public const int PRECONDITION = 11;
    public const int AND = 18;
    public const int RULEBASE = 4;
    public const int EOF = -1;
    public const int FORGET = 15;
    public const int COUNT = 16;
    public const int FACT = 5;
    public const int FOR = 6;
    public const int MODIFY = 17;
    public const int MUTEX = 12;
    public const int NUMERIC = 24;
    
    
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:38:1: rulebase : RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF ;
    public void rulebase() // throws RecognitionException [1]
    {   
        words_return words1 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:4: ( RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:4: RULEBASE SPACE QUOTE words QUOTE ( rule | query | fact | ignored )* EOF
            {
            	Match(input,RULEBASE,FOLLOW_RULEBASE_in_rulebase144); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rulebase146); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase148); 
            	PushFollow(FOLLOW_words_in_rulebase150);
            	words1 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rulebase152); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:37: ( rule | query | fact | ignored )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:38: rule
            			    {
            			    	PushFollow(FOLLOW_rule_in_rulebase155);
            			    	rule();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:45: query
            			    {
            			    	PushFollow(FOLLOW_query_in_rulebase159);
            			    	query();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:53: fact
            			    {
            			    	PushFollow(FOLLOW_fact_in_rulebase163);
            			    	fact();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 4 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:39:60: ignored
            			    {
            			    	PushFollow(FOLLOW_ignored_in_rulebase167);
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

            	Match(input,EOF,FOLLOW_EOF_in_rulebase171); 
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:1: fact : FACT ( SPACE QUOTE words QUOTE )? NEWLINE IS NEWLINE statement ;
    public void fact() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:8: ( FACT ( SPACE QUOTE words QUOTE )? NEWLINE IS NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:8: FACT ( SPACE QUOTE words QUOTE )? NEWLINE IS NEWLINE statement
            {
            	Match(input,FACT,FOLLOW_FACT_in_fact182); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:13: ( SPACE QUOTE words QUOTE )?
            	int alt2 = 2;
            	int LA2_0 = input.LA(1);
            	
            	if ( (LA2_0 == SPACE) )
            	{
            	    alt2 = 1;
            	}
            	switch (alt2) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:41:14: SPACE QUOTE words QUOTE
            	        {
            	        	Match(input,SPACE,FOLLOW_SPACE_in_fact185); 
            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact187); 
            	        	PushFollow(FOLLOW_words_in_fact189);
            	        	words();
            	        	followingStackPointer_--;

            	        	Match(input,QUOTE,FOLLOW_QUOTE_in_fact191); 
            	        
            	        }
            	        break;
            	
            	}

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_fact195); 
            	Match(input,IS,FOLLOW_IS_in_fact197); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_fact199); 
            	PushFollow(FOLLOW_statement_in_fact201);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:1: query : QUERY SPACE QUOTE words QUOTE NEWLINE FOR NEWLINE condition ;
    public void query() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:9: ( QUERY SPACE QUOTE words QUOTE NEWLINE FOR NEWLINE condition )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:43:9: QUERY SPACE QUOTE words QUOTE NEWLINE FOR NEWLINE condition
            {
            	Match(input,QUERY,FOLLOW_QUERY_in_query209); 
            	Match(input,SPACE,FOLLOW_SPACE_in_query211); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_query213); 
            	PushFollow(FOLLOW_words_in_query215);
            	words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_query217); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_query219); 
            	Match(input,FOR,FOLLOW_FOR_in_query221); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_query223); 
            	PushFollow(FOLLOW_condition_in_query225);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:1: rule : RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action ;
    public void rule() // throws RecognitionException [1]
    {   
        words_return words2 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:9: ( RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:45:9: RULE SPACE QUOTE words QUOTE NEWLINE meta 'if' NEWLINE condition action
            {
            	Match(input,RULE,FOLLOW_RULE_in_rule234); 
            	Match(input,SPACE,FOLLOW_SPACE_in_rule236); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule238); 
            	PushFollow(FOLLOW_words_in_rule240);
            	words2 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_rule242); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule244); 
            	PushFollow(FOLLOW_meta_in_rule246);
            	meta();
            	followingStackPointer_--;

            	Match(input,26,FOLLOW_26_in_rule248); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_rule250); 
            	PushFollow(FOLLOW_condition_in_rule252);
            	condition();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_action_in_rule254);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:1: meta : ( priority )? ( precondition )? ( mutex )* ;
    public void meta() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:8: ( ( priority )? ( precondition )? ( mutex )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:8: ( priority )? ( precondition )? ( mutex )*
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:8: ( priority )?
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
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:8: priority
            	        {
            	        	PushFollow(FOLLOW_priority_in_meta264);
            	        	priority();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:18: ( precondition )?
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
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:18: precondition
            	        {
            	        	PushFollow(FOLLOW_precondition_in_meta267);
            	        	precondition();
            	        	followingStackPointer_--;

            	        
            	        }
            	        break;
            	
            	}

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:32: ( mutex )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:47:32: mutex
            			    {
            			    	PushFollow(FOLLOW_mutex_in_meta270);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:49:1: priority : TAB PRIORITY SPACE NUMERIC NEWLINE ;
    public void priority() // throws RecognitionException [1]
    {   
        IToken NUMERIC3 = null;
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:50:4: ( TAB PRIORITY SPACE NUMERIC NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:50:4: TAB PRIORITY SPACE NUMERIC NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_priority280); 
            	Match(input,PRIORITY,FOLLOW_PRIORITY_in_priority282); 
            	Match(input,SPACE,FOLLOW_SPACE_in_priority284); 
            	NUMERIC3 = (IToken)input.LT(1);
            	Match(input,NUMERIC,FOLLOW_NUMERIC_in_priority286); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_priority288); 
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:52:1: precondition : TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE ;
    public void precondition() // throws RecognitionException [1]
    {   
        words_return words4 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:53:4: ( TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:53:4: TAB PRECONDITION SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_precondition299); 
            	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_precondition301); 
            	Match(input,SPACE,FOLLOW_SPACE_in_precondition303); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition305); 
            	PushFollow(FOLLOW_words_in_precondition307);
            	words4 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_precondition309); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_precondition311); 
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:55:1: mutex : TAB MUTEX SPACE QUOTE words QUOTE NEWLINE ;
    public void mutex() // throws RecognitionException [1]
    {   
        words_return words5 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:55:9: ( TAB MUTEX SPACE QUOTE words QUOTE NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:55:9: TAB MUTEX SPACE QUOTE words QUOTE NEWLINE
            {
            	Match(input,TAB,FOLLOW_TAB_in_mutex321); 
            	Match(input,MUTEX,FOLLOW_MUTEX_in_mutex323); 
            	Match(input,SPACE,FOLLOW_SPACE_in_mutex325); 
            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex327); 
            	PushFollow(FOLLOW_words_in_mutex329);
            	words5 = words();
            	followingStackPointer_--;

            	Match(input,QUOTE,FOLLOW_QUOTE_in_mutex331); 
            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_mutex333); 
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:57:1: condition : statement ( logic statement )* ;
    public void condition() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:4: ( statement ( logic statement )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:4: statement ( logic statement )*
            {
            	PushFollow(FOLLOW_statement_in_condition344);
            	statement();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:14: ( logic statement )*
            	do 
            	{
            	    int alt6 = 2;
            	    alt6 = dfa6.Predict(input);
            	    switch (alt6) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:58:15: logic statement
            			    {
            			    	PushFollow(FOLLOW_logic_in_condition347);
            			    	logic();
            			    	followingStackPointer_--;

            			    	PushFollow(FOLLOW_statement_in_condition349);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:60:1: action : THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement ;
    public void action() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:60:10: ( THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:60:10: THEN SPACE ( DEDUCT | FORGET | COUNT | MODIFY )+ NEWLINE statement
            {
            	Match(input,THEN,FOLLOW_THEN_in_action359); 
            	Match(input,SPACE,FOLLOW_SPACE_in_action361); 
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:60:21: ( DEDUCT | FORGET | COUNT | MODIFY )+
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
            			    	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_action363);    throw mse;
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

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_action380); 
            	PushFollow(FOLLOW_statement_in_action382);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:62:1: statement : indent words NEWLINE ;
    public void statement() // throws RecognitionException [1]
    {   
        words_return words6 = null;

        indent_return indent7 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:63:4: ( indent words NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:63:4: indent words NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_statement391);
            	indent7 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_words_in_statement393);
            	words6 = words();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_statement395); 
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:65:1: logic : indent booleanToken NEWLINE ;
    public void logic() // throws RecognitionException [1]
    {   
        indent_return indent8 = null;

        booleanToken_return booleanToken9 = null;
        
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:65:9: ( indent booleanToken NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:65:9: indent booleanToken NEWLINE
            {
            	PushFollow(FOLLOW_indent_in_logic405);
            	indent8 = indent();
            	followingStackPointer_--;

            	PushFollow(FOLLOW_booleanToken_in_logic407);
            	booleanToken9 = booleanToken();
            	followingStackPointer_--;

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_logic409); 
            	
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:1: words : word ( SPACE word )* ;
    public words_return words() // throws RecognitionException [1]
    {   
        words_return retval = new words_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:9: ( word ( SPACE word )* )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:9: word ( SPACE word )*
            {
            	PushFollow(FOLLOW_word_in_words419);
            	word();
            	followingStackPointer_--;

            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:14: ( SPACE word )*
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:81:15: SPACE word
            			    {
            			    	Match(input,SPACE,FOLLOW_SPACE_in_words422); 
            			    	PushFollow(FOLLOW_word_in_words424);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:1: word : ( anyToken | CHAR | NUMERIC )+ ;
    public void word() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:8: ( ( anyToken | CHAR | NUMERIC )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:8: ( anyToken | CHAR | NUMERIC )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:8: ( anyToken | CHAR | NUMERIC )+
            	int cnt9 = 0;
            	do 
            	{
            	    int alt9 = 4;
            	    switch ( input.LA(1) ) 
            	    {
            	    case RULEBASE:
            	    case FACT:
            	    case FOR:
            	    case IS:
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:9: anyToken
            			    {
            			    	PushFollow(FOLLOW_anyToken_in_word435);
            			    	anyToken();
            			    	followingStackPointer_--;

            			    
            			    }
            			    break;
            			case 2 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:20: CHAR
            			    {
            			    	Match(input,CHAR,FOLLOW_CHAR_in_word439); 
            			    
            			    }
            			    break;
            			case 3 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:83:27: NUMERIC
            			    {
            			    	Match(input,NUMERIC,FOLLOW_NUMERIC_in_word443); 
            			    
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:1: ignored : ( TAB | SPACE )* NEWLINE ;
    public void ignored() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:11: ( ( TAB | SPACE )* NEWLINE )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:11: ( TAB | SPACE )* NEWLINE
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:85:11: ( TAB | SPACE )*
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
            			    	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_ignored453);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    goto loop10;
            	    }
            	} while (true);
            	
            	loop10:
            		;	// Stops C# compiler whinging that label 'loop10' has no statements

            	Match(input,NEWLINE,FOLLOW_NEWLINE_in_ignored462); 
            
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:1: indent : ( TAB )+ ;
    public indent_return indent() // throws RecognitionException [1]
    {   
        indent_return retval = new indent_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:10: ( ( TAB )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:10: ( TAB )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:10: ( TAB )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:87:10: TAB
            			    {
            			    	Match(input,TAB,FOLLOW_TAB_in_indent470); 
            			    
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:89:1: anyToken : ( RULEBASE | FACT | FOR | IS | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) ;
    public void anyToken() // throws RecognitionException [1]
    {   
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:4: ( ( RULEBASE | FACT | FOR | IS | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:4: ( RULEBASE | FACT | FOR | IS | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:4: ( RULEBASE | FACT | FOR | IS | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )
            	int alt12 = 15;
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
            	case FOR:
            		{
            	    alt12 = 3;
            	    }
            	    break;
            	case IS:
            		{
            	    alt12 = 4;
            	    }
            	    break;
            	case QUERY:
            		{
            	    alt12 = 5;
            	    }
            	    break;
            	case RULE:
            		{
            	    alt12 = 6;
            	    }
            	    break;
            	case PRIORITY:
            		{
            	    alt12 = 7;
            	    }
            	    break;
            	case PRECONDITION:
            		{
            	    alt12 = 8;
            	    }
            	    break;
            	case MUTEX:
            		{
            	    alt12 = 9;
            	    }
            	    break;
            	case THEN:
            		{
            	    alt12 = 10;
            	    }
            	    break;
            	case DEDUCT:
            		{
            	    alt12 = 11;
            	    }
            	    break;
            	case FORGET:
            		{
            	    alt12 = 12;
            	    }
            	    break;
            	case COUNT:
            		{
            	    alt12 = 13;
            	    }
            	    break;
            	case MODIFY:
            		{
            	    alt12 = 14;
            	    }
            	    break;
            	case AND:
            	case OR:
            		{
            	    alt12 = 15;
            	    }
            	    break;
            		default:
            		    NoViableAltException nvae_d12s0 =
            		        new NoViableAltException("90:4: ( RULEBASE | FACT | FOR | IS | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | booleanToken )", 12, 0, input);
            	
            		    throw nvae_d12s0;
            	}
            	
            	switch (alt12) 
            	{
            	    case 1 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:5: RULEBASE
            	        {
            	        	Match(input,RULEBASE,FOLLOW_RULEBASE_in_anyToken481); 
            	        
            	        }
            	        break;
            	    case 2 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:16: FACT
            	        {
            	        	Match(input,FACT,FOLLOW_FACT_in_anyToken485); 
            	        
            	        }
            	        break;
            	    case 3 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:23: FOR
            	        {
            	        	Match(input,FOR,FOLLOW_FOR_in_anyToken489); 
            	        
            	        }
            	        break;
            	    case 4 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:29: IS
            	        {
            	        	Match(input,IS,FOLLOW_IS_in_anyToken493); 
            	        
            	        }
            	        break;
            	    case 5 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:34: QUERY
            	        {
            	        	Match(input,QUERY,FOLLOW_QUERY_in_anyToken497); 
            	        
            	        }
            	        break;
            	    case 6 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:42: RULE
            	        {
            	        	Match(input,RULE,FOLLOW_RULE_in_anyToken501); 
            	        
            	        }
            	        break;
            	    case 7 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:49: PRIORITY
            	        {
            	        	Match(input,PRIORITY,FOLLOW_PRIORITY_in_anyToken505); 
            	        
            	        }
            	        break;
            	    case 8 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:60: PRECONDITION
            	        {
            	        	Match(input,PRECONDITION,FOLLOW_PRECONDITION_in_anyToken509); 
            	        
            	        }
            	        break;
            	    case 9 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:75: MUTEX
            	        {
            	        	Match(input,MUTEX,FOLLOW_MUTEX_in_anyToken513); 
            	        
            	        }
            	        break;
            	    case 10 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:83: THEN
            	        {
            	        	Match(input,THEN,FOLLOW_THEN_in_anyToken517); 
            	        
            	        }
            	        break;
            	    case 11 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:90: DEDUCT
            	        {
            	        	Match(input,DEDUCT,FOLLOW_DEDUCT_in_anyToken521); 
            	        
            	        }
            	        break;
            	    case 12 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:99: FORGET
            	        {
            	        	Match(input,FORGET,FOLLOW_FORGET_in_anyToken525); 
            	        
            	        }
            	        break;
            	    case 13 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:108: COUNT
            	        {
            	        	Match(input,COUNT,FOLLOW_COUNT_in_anyToken529); 
            	        
            	        }
            	        break;
            	    case 14 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:116: MODIFY
            	        {
            	        	Match(input,MODIFY,FOLLOW_MODIFY_in_anyToken533); 
            	        
            	        }
            	        break;
            	    case 15 :
            	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:90:125: booleanToken
            	        {
            	        	PushFollow(FOLLOW_booleanToken_in_anyToken537);
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
    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:92:1: booleanToken : ( AND | OR ) ;
    public booleanToken_return booleanToken() // throws RecognitionException [1]
    {   
        booleanToken_return retval = new booleanToken_return();
        retval.start = input.LT(1);
    
        try 
    	{
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:4: ( ( AND | OR ) )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:4: ( AND | OR )
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
            	    RecoverFromMismatchedSet(input,mse,FOLLOW_set_in_booleanToken547);    throw mse;
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
        5, 0, 18, 0
        };
    static readonly int[] DFA6_max = {
        23, 0, 23, 0
        };
    static readonly short[] DFA6_accept = {
        -1, 2, -1, 1
        };
    static readonly short[] DFA6_special = {
        -1, -1, -1, -1
        };
    
    static readonly short[] dfa6_transition_null = null;

    static readonly short[] dfa6_transition0 = {
    	3, 3, 1, -1, 1, 2
    	};
    static readonly short[] dfa6_transition1 = {
    	1, -1, -1, 1, 1, -1, -1, -1, 1, -1, -1, -1, -1, -1, -1, 1, -1, 1, 2
    	};
    
    static readonly short[][] DFA6_transition = {
    	dfa6_transition1,
    	dfa6_transition_null,
    	dfa6_transition0,
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
            get { return "()* loopback of 58:14: ( logic statement )*"; }
        }
    
    }
    
 

    public static readonly BitSet FOLLOW_RULEBASE_in_rulebase144 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rulebase146 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase148 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_rulebase150 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rulebase152 = new BitSet(new ulong[]{0x0000000000D00320UL});
    public static readonly BitSet FOLLOW_rule_in_rulebase155 = new BitSet(new ulong[]{0x0000000000D00320UL});
    public static readonly BitSet FOLLOW_query_in_rulebase159 = new BitSet(new ulong[]{0x0000000000D00320UL});
    public static readonly BitSet FOLLOW_fact_in_rulebase163 = new BitSet(new ulong[]{0x0000000000D00320UL});
    public static readonly BitSet FOLLOW_ignored_in_rulebase167 = new BitSet(new ulong[]{0x0000000000D00320UL});
    public static readonly BitSet FOLLOW_EOF_in_rulebase171 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_fact182 = new BitSet(new ulong[]{0x0000000000500000UL});
    public static readonly BitSet FOLLOW_SPACE_in_fact185 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact187 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_fact189 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_fact191 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_fact195 = new BitSet(new ulong[]{0x0000000000000080UL});
    public static readonly BitSet FOLLOW_IS_in_fact197 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_fact199 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_statement_in_fact201 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_query209 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_query211 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query213 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_query215 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_query217 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_query219 = new BitSet(new ulong[]{0x0000000000000040UL});
    public static readonly BitSet FOLLOW_FOR_in_query221 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_query223 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_condition_in_query225 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_rule234 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_rule236 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule238 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_rule240 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_rule242 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule244 = new BitSet(new ulong[]{0x0000000004800000UL});
    public static readonly BitSet FOLLOW_meta_in_rule246 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_26_in_rule248 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_rule250 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_condition_in_rule252 = new BitSet(new ulong[]{0x0000000000002000UL});
    public static readonly BitSet FOLLOW_action_in_rule254 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_priority_in_meta264 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_precondition_in_meta267 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_mutex_in_meta270 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_TAB_in_priority280 = new BitSet(new ulong[]{0x0000000000000400UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_priority282 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_priority284 = new BitSet(new ulong[]{0x0000000001000000UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_priority286 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_priority288 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_precondition299 = new BitSet(new ulong[]{0x0000000000000800UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_precondition301 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_precondition303 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition305 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_precondition307 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_precondition309 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_precondition311 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_mutex321 = new BitSet(new ulong[]{0x0000000000001000UL});
    public static readonly BitSet FOLLOW_MUTEX_in_mutex323 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_mutex325 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex327 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_mutex329 = new BitSet(new ulong[]{0x0000000000200000UL});
    public static readonly BitSet FOLLOW_QUOTE_in_mutex331 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_mutex333 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_statement_in_condition344 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_logic_in_condition347 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_statement_in_condition349 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_THEN_in_action359 = new BitSet(new ulong[]{0x0000000000100000UL});
    public static readonly BitSet FOLLOW_SPACE_in_action361 = new BitSet(new ulong[]{0x000000000003C000UL});
    public static readonly BitSet FOLLOW_set_in_action363 = new BitSet(new ulong[]{0x000000000043C000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_action380 = new BitSet(new ulong[]{0x0000000000800000UL});
    public static readonly BitSet FOLLOW_statement_in_action382 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_statement391 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_words_in_statement393 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_statement395 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_indent_in_logic405 = new BitSet(new ulong[]{0x00000000000C0000UL});
    public static readonly BitSet FOLLOW_booleanToken_in_logic407 = new BitSet(new ulong[]{0x0000000000400000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_logic409 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_word_in_words419 = new BitSet(new ulong[]{0x0000000000100002UL});
    public static readonly BitSet FOLLOW_SPACE_in_words422 = new BitSet(new ulong[]{0x00000000030FFFF0UL});
    public static readonly BitSet FOLLOW_word_in_words424 = new BitSet(new ulong[]{0x0000000000100002UL});
    public static readonly BitSet FOLLOW_anyToken_in_word435 = new BitSet(new ulong[]{0x00000000030FFFF2UL});
    public static readonly BitSet FOLLOW_CHAR_in_word439 = new BitSet(new ulong[]{0x00000000030FFFF2UL});
    public static readonly BitSet FOLLOW_NUMERIC_in_word443 = new BitSet(new ulong[]{0x00000000030FFFF2UL});
    public static readonly BitSet FOLLOW_set_in_ignored453 = new BitSet(new ulong[]{0x0000000000D00000UL});
    public static readonly BitSet FOLLOW_NEWLINE_in_ignored462 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_TAB_in_indent470 = new BitSet(new ulong[]{0x0000000000800002UL});
    public static readonly BitSet FOLLOW_RULEBASE_in_anyToken481 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FACT_in_anyToken485 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FOR_in_anyToken489 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_IS_in_anyToken493 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_QUERY_in_anyToken497 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_RULE_in_anyToken501 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRIORITY_in_anyToken505 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_PRECONDITION_in_anyToken509 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MUTEX_in_anyToken513 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_THEN_in_anyToken517 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DEDUCT_in_anyToken521 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_FORGET_in_anyToken525 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_COUNT_in_anyToken529 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_MODIFY_in_anyToken533 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_booleanToken_in_anyToken537 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_set_in_booleanToken547 = new BitSet(new ulong[]{0x0000000000000002UL});

}
