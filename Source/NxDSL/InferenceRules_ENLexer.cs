// $ANTLR 3.0 C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g 2007-05-29 21:23:23




using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;

namespace NxDSL
{

public class InferenceRules_ENLexer : Lexer 
{
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
    public const int Tokens = 25;
    public const int MODIFY = 15;
    public const int MUTEX = 10;
    public const int T24 = 24;
    public const int NUMERIC = 22;

    public InferenceRules_ENLexer() 
    {
		InitializeCyclicDFAs();
    }
    public InferenceRules_ENLexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDFAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g";} 
    }

    // $ANTLR start RULEBASE 
    public void mRULEBASE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RULEBASE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:10:12: ( 'rulebase' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:10:12: 'rulebase'
            {
            	Match("rulebase"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RULEBASE

    // $ANTLR start FACT 
    public void mFACT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = FACT;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:11:8: ( 'fact' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:11:8: 'fact'
            {
            	Match("fact"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end FACT

    // $ANTLR start QUERY 
    public void mQUERY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = QUERY;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:12:9: ( 'query' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:12:9: 'query'
            {
            	Match("query"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end QUERY

    // $ANTLR start RULE 
    public void mRULE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RULE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:13:8: ( 'rule' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:13:8: 'rule'
            {
            	Match("rule"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end RULE

    // $ANTLR start PRIORITY 
    public void mPRIORITY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = PRIORITY;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:14:12: ( 'priority' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:14:12: 'priority'
            {
            	Match("priority"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end PRIORITY

    // $ANTLR start PRECONDITION 
    public void mPRECONDITION() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = PRECONDITION;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:15:16: ( 'precondition' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:15:16: 'precondition'
            {
            	Match("precondition"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end PRECONDITION

    // $ANTLR start MUTEX 
    public void mMUTEX() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MUTEX;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:16:9: ( 'mutex' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:16:9: 'mutex'
            {
            	Match("mutex"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MUTEX

    // $ANTLR start THEN 
    public void mTHEN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = THEN;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:17:8: ( 'then' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:17:8: 'then'
            {
            	Match("then"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end THEN

    // $ANTLR start DEDUCT 
    public void mDEDUCT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = DEDUCT;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:18:10: ( 'deduct' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:18:10: 'deduct'
            {
            	Match("deduct"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end DEDUCT

    // $ANTLR start FORGET 
    public void mFORGET() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = FORGET;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:19:10: ( 'forget' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:19:10: 'forget'
            {
            	Match("forget"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end FORGET

    // $ANTLR start COUNT 
    public void mCOUNT() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = COUNT;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:20:9: ( 'count' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:20:9: 'count'
            {
            	Match("count"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end COUNT

    // $ANTLR start MODIFY 
    public void mMODIFY() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = MODIFY;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:21:10: ( 'modify' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:21:10: 'modify'
            {
            	Match("modify"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end MODIFY

    // $ANTLR start AND 
    public void mAND() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = AND;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:22:7: ( 'and' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:22:7: 'and'
            {
            	Match("and"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end AND

    // $ANTLR start OR 
    public void mOR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = OR;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:23:6: ( 'or' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:23:6: 'or'
            {
            	Match("or"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end OR

    // $ANTLR start T24 
    public void mT24() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = T24;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:24:7: ( 'if' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:24:7: 'if'
            {
            	Match("if"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end T24

    // $ANTLR start NUMERIC 
    public void mNUMERIC() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NUMERIC;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:11: ( ( '0' .. '9' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:11: ( '0' .. '9' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:11: ( '0' .. '9' )+
            	int cnt1 = 0;
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);
            	    
            	    if ( ((LA1_0 >= '0' && LA1_0 <= '9')) )
            	    {
            	        alt1 = 1;
            	    }
            	    
            	
            	    switch (alt1) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:93:12: '0' .. '9'
            			    {
            			    	MatchRange('0','9'); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt1 >= 1 ) goto loop1;
            		            EarlyExitException eee =
            		                new EarlyExitException(1, input);
            		            throw eee;
            	    }
            	    cnt1++;
            	} while (true);
            	
            	loop1:
            		;	// Stops C# compiler whinging that label 'loop1' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NUMERIC

    // $ANTLR start CHAR 
    public void mCHAR() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = CHAR;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:94:8: ( ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:94:8: ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:94:8: ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+
            	int cnt2 = 0;
            	do 
            	{
            	    int alt2 = 2;
            	    int LA2_0 = input.LA(1);
            	    
            	    if ( (LA2_0 == '!' || (LA2_0 >= '#' && LA2_0 <= '/') || (LA2_0 >= ':' && LA2_0 <= '\u00FF')) )
            	    {
            	        alt2 = 1;
            	    }
            	    
            	
            	    switch (alt2) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:
            			    {
            			    	if ( input.LA(1) == '!' || (input.LA(1) >= '#' && input.LA(1) <= '/') || (input.LA(1) >= ':' && input.LA(1) <= '\u00FF') ) 
            			    	{
            			    	    input.Consume();
            			    	
            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse =
            			    	        new MismatchedSetException(null,input);
            			    	    Recover(mse);    throw mse;
            			    	}

            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt2 >= 1 ) goto loop2;
            		            EarlyExitException eee =
            		                new EarlyExitException(2, input);
            		            throw eee;
            	    }
            	    cnt2++;
            	} while (true);
            	
            	loop2:
            		;	// Stops C# compiler whinging that label 'loop2' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end CHAR

    // $ANTLR start NEWLINE 
    public void mNEWLINE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NEWLINE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:11: ( ( ( '\\r' )? '\\n' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:11: ( ( '\\r' )? '\\n' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:11: ( ( '\\r' )? '\\n' )+
            	int cnt4 = 0;
            	do 
            	{
            	    int alt4 = 2;
            	    int LA4_0 = input.LA(1);
            	    
            	    if ( (LA4_0 == '\n' || LA4_0 == '\r') )
            	    {
            	        alt4 = 1;
            	    }
            	    
            	
            	    switch (alt4) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:12: ( '\\r' )? '\\n'
            			    {
            			    	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:12: ( '\\r' )?
            			    	int alt3 = 2;
            			    	int LA3_0 = input.LA(1);
            			    	
            			    	if ( (LA3_0 == '\r') )
            			    	{
            			    	    alt3 = 1;
            			    	}
            			    	switch (alt3) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:95:12: '\\r'
            			    	        {
            			    	        	Match('\r'); 
            			    	        
            			    	        }
            			    	        break;
            			    	
            			    	}

            			    	Match('\n'); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt4 >= 1 ) goto loop4;
            		            EarlyExitException eee =
            		                new EarlyExitException(4, input);
            		            throw eee;
            	    }
            	    cnt4++;
            	} while (true);
            	
            	loop4:
            		;	// Stops C# compiler whinging that label 'loop4' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end NEWLINE

    // $ANTLR start SPACE 
    public void mSPACE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = SPACE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:96:9: ( ( ' ' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:96:9: ( ' ' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:96:9: ( ' ' )+
            	int cnt5 = 0;
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);
            	    
            	    if ( (LA5_0 == ' ') )
            	    {
            	        alt5 = 1;
            	    }
            	    
            	
            	    switch (alt5) 
            		{
            			case 1 :
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:96:9: ' '
            			    {
            			    	Match(' '); 
            			    
            			    }
            			    break;
            	
            			default:
            			    if ( cnt5 >= 1 ) goto loop5;
            		            EarlyExitException eee =
            		                new EarlyExitException(5, input);
            		            throw eee;
            	    }
            	    cnt5++;
            	} while (true);
            	
            	loop5:
            		;	// Stops C# compiler whinging that label 'loop5' has no statements

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end SPACE

    // $ANTLR start TAB 
    public void mTAB() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = TAB;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:97:7: ( '\\t' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:97:7: '\\t'
            {
            	Match('\t'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end TAB

    // $ANTLR start QUOTE 
    public void mQUOTE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = QUOTE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:98:9: ( '\"' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:98:9: '\"'
            {
            	Match('\"'); 
            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end QUOTE

    override public void mTokens() // throws RecognitionException 
    {
        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:10: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | AND | OR | T24 | NUMERIC | CHAR | NEWLINE | SPACE | TAB | QUOTE )
        int alt6 = 21;
        switch ( input.LA(1) ) 
        {
        case 'r':
        	{
            int LA6_1 = input.LA(2);
            
            if ( (LA6_1 == 'u') )
            {
                int LA6_18 = input.LA(3);
                
                if ( (LA6_18 == 'l') )
                {
                    int LA6_31 = input.LA(4);
                    
                    if ( (LA6_31 == 'e') )
                    {
                        switch ( input.LA(5) ) 
                        {
                        case 'b':
                        	{
                            int LA6_57 = input.LA(6);
                            
                            if ( (LA6_57 == 'a') )
                            {
                                int LA6_69 = input.LA(7);
                                
                                if ( (LA6_69 == 's') )
                                {
                                    int LA6_78 = input.LA(8);
                                    
                                    if ( (LA6_78 == 'e') )
                                    {
                                        int LA6_84 = input.LA(9);
                                        
                                        if ( (LA6_84 == '!' || (LA6_84 >= '#' && LA6_84 <= '/') || (LA6_84 >= ':' && LA6_84 <= '\u00FF')) )
                                        {
                                            alt6 = 17;
                                        }
                                        else 
                                        {
                                            alt6 = 1;}
                                    }
                                    else 
                                    {
                                        alt6 = 17;}
                                }
                                else 
                                {
                                    alt6 = 17;}
                            }
                            else 
                            {
                                alt6 = 17;}
                            }
                            break;
                        case '!':
                        case '#':
                        case '$':
                        case '%':
                        case '&':
                        case '\'':
                        case '(':
                        case ')':
                        case '*':
                        case '+':
                        case ',':
                        case '-':
                        case '.':
                        case '/':
                        case ':':
                        case ';':
                        case '<':
                        case '=':
                        case '>':
                        case '?':
                        case '@':
                        case 'A':
                        case 'B':
                        case 'C':
                        case 'D':
                        case 'E':
                        case 'F':
                        case 'G':
                        case 'H':
                        case 'I':
                        case 'J':
                        case 'K':
                        case 'L':
                        case 'M':
                        case 'N':
                        case 'O':
                        case 'P':
                        case 'Q':
                        case 'R':
                        case 'S':
                        case 'T':
                        case 'U':
                        case 'V':
                        case 'W':
                        case 'X':
                        case 'Y':
                        case 'Z':
                        case '[':
                        case '\\':
                        case ']':
                        case '^':
                        case '_':
                        case '`':
                        case 'a':
                        case 'c':
                        case 'd':
                        case 'e':
                        case 'f':
                        case 'g':
                        case 'h':
                        case 'i':
                        case 'j':
                        case 'k':
                        case 'l':
                        case 'm':
                        case 'n':
                        case 'o':
                        case 'p':
                        case 'q':
                        case 'r':
                        case 's':
                        case 't':
                        case 'u':
                        case 'v':
                        case 'w':
                        case 'x':
                        case 'y':
                        case 'z':
                        case '{':
                        case '|':
                        case '}':
                        case '~':
                        case '\u007F':
                        case '\u0080':
                        case '\u0081':
                        case '\u0082':
                        case '\u0083':
                        case '\u0084':
                        case '\u0085':
                        case '\u0086':
                        case '\u0087':
                        case '\u0088':
                        case '\u0089':
                        case '\u008A':
                        case '\u008B':
                        case '\u008C':
                        case '\u008D':
                        case '\u008E':
                        case '\u008F':
                        case '\u0090':
                        case '\u0091':
                        case '\u0092':
                        case '\u0093':
                        case '\u0094':
                        case '\u0095':
                        case '\u0096':
                        case '\u0097':
                        case '\u0098':
                        case '\u0099':
                        case '\u009A':
                        case '\u009B':
                        case '\u009C':
                        case '\u009D':
                        case '\u009E':
                        case '\u009F':
                        case '\u00A0':
                        case '\u00A1':
                        case '\u00A2':
                        case '\u00A3':
                        case '\u00A4':
                        case '\u00A5':
                        case '\u00A6':
                        case '\u00A7':
                        case '\u00A8':
                        case '\u00A9':
                        case '\u00AA':
                        case '\u00AB':
                        case '\u00AC':
                        case '\u00AD':
                        case '\u00AE':
                        case '\u00AF':
                        case '\u00B0':
                        case '\u00B1':
                        case '\u00B2':
                        case '\u00B3':
                        case '\u00B4':
                        case '\u00B5':
                        case '\u00B6':
                        case '\u00B7':
                        case '\u00B8':
                        case '\u00B9':
                        case '\u00BA':
                        case '\u00BB':
                        case '\u00BC':
                        case '\u00BD':
                        case '\u00BE':
                        case '\u00BF':
                        case '\u00C0':
                        case '\u00C1':
                        case '\u00C2':
                        case '\u00C3':
                        case '\u00C4':
                        case '\u00C5':
                        case '\u00C6':
                        case '\u00C7':
                        case '\u00C8':
                        case '\u00C9':
                        case '\u00CA':
                        case '\u00CB':
                        case '\u00CC':
                        case '\u00CD':
                        case '\u00CE':
                        case '\u00CF':
                        case '\u00D0':
                        case '\u00D1':
                        case '\u00D2':
                        case '\u00D3':
                        case '\u00D4':
                        case '\u00D5':
                        case '\u00D6':
                        case '\u00D7':
                        case '\u00D8':
                        case '\u00D9':
                        case '\u00DA':
                        case '\u00DB':
                        case '\u00DC':
                        case '\u00DD':
                        case '\u00DE':
                        case '\u00DF':
                        case '\u00E0':
                        case '\u00E1':
                        case '\u00E2':
                        case '\u00E3':
                        case '\u00E4':
                        case '\u00E5':
                        case '\u00E6':
                        case '\u00E7':
                        case '\u00E8':
                        case '\u00E9':
                        case '\u00EA':
                        case '\u00EB':
                        case '\u00EC':
                        case '\u00ED':
                        case '\u00EE':
                        case '\u00EF':
                        case '\u00F0':
                        case '\u00F1':
                        case '\u00F2':
                        case '\u00F3':
                        case '\u00F4':
                        case '\u00F5':
                        case '\u00F6':
                        case '\u00F7':
                        case '\u00F8':
                        case '\u00F9':
                        case '\u00FA':
                        case '\u00FB':
                        case '\u00FC':
                        case '\u00FD':
                        case '\u00FE':
                        case '\u00FF':
                        	{
                            alt6 = 17;
                            }
                            break;
                        	default:
                            	alt6 = 4;
                            	break;}
                    
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'f':
        	{
            switch ( input.LA(2) ) 
            {
            case 'o':
            	{
                int LA6_19 = input.LA(3);
                
                if ( (LA6_19 == 'r') )
                {
                    int LA6_32 = input.LA(4);
                    
                    if ( (LA6_32 == 'g') )
                    {
                        int LA6_46 = input.LA(5);
                        
                        if ( (LA6_46 == 'e') )
                        {
                            int LA6_59 = input.LA(6);
                            
                            if ( (LA6_59 == 't') )
                            {
                                int LA6_70 = input.LA(7);
                                
                                if ( (LA6_70 == '!' || (LA6_70 >= '#' && LA6_70 <= '/') || (LA6_70 >= ':' && LA6_70 <= '\u00FF')) )
                                {
                                    alt6 = 17;
                                }
                                else 
                                {
                                    alt6 = 10;}
                            }
                            else 
                            {
                                alt6 = 17;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
                }
                break;
            case 'a':
            	{
                int LA6_20 = input.LA(3);
                
                if ( (LA6_20 == 'c') )
                {
                    int LA6_33 = input.LA(4);
                    
                    if ( (LA6_33 == 't') )
                    {
                        int LA6_47 = input.LA(5);
                        
                        if ( (LA6_47 == '!' || (LA6_47 >= '#' && LA6_47 <= '/') || (LA6_47 >= ':' && LA6_47 <= '\u00FF')) )
                        {
                            alt6 = 17;
                        }
                        else 
                        {
                            alt6 = 2;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
                }
                break;
            	default:
                	alt6 = 17;
                	break;}
        
            }
            break;
        case 'q':
        	{
            int LA6_3 = input.LA(2);
            
            if ( (LA6_3 == 'u') )
            {
                int LA6_21 = input.LA(3);
                
                if ( (LA6_21 == 'e') )
                {
                    int LA6_34 = input.LA(4);
                    
                    if ( (LA6_34 == 'r') )
                    {
                        int LA6_48 = input.LA(5);
                        
                        if ( (LA6_48 == 'y') )
                        {
                            int LA6_61 = input.LA(6);
                            
                            if ( (LA6_61 == '!' || (LA6_61 >= '#' && LA6_61 <= '/') || (LA6_61 >= ':' && LA6_61 <= '\u00FF')) )
                            {
                                alt6 = 17;
                            }
                            else 
                            {
                                alt6 = 3;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'p':
        	{
            int LA6_4 = input.LA(2);
            
            if ( (LA6_4 == 'r') )
            {
                switch ( input.LA(3) ) 
                {
                case 'e':
                	{
                    int LA6_35 = input.LA(4);
                    
                    if ( (LA6_35 == 'c') )
                    {
                        int LA6_49 = input.LA(5);
                        
                        if ( (LA6_49 == 'o') )
                        {
                            int LA6_62 = input.LA(6);
                            
                            if ( (LA6_62 == 'n') )
                            {
                                int LA6_72 = input.LA(7);
                                
                                if ( (LA6_72 == 'd') )
                                {
                                    int LA6_80 = input.LA(8);
                                    
                                    if ( (LA6_80 == 'i') )
                                    {
                                        int LA6_85 = input.LA(9);
                                        
                                        if ( (LA6_85 == 't') )
                                        {
                                            int LA6_88 = input.LA(10);
                                            
                                            if ( (LA6_88 == 'i') )
                                            {
                                                int LA6_90 = input.LA(11);
                                                
                                                if ( (LA6_90 == 'o') )
                                                {
                                                    int LA6_91 = input.LA(12);
                                                    
                                                    if ( (LA6_91 == 'n') )
                                                    {
                                                        int LA6_92 = input.LA(13);
                                                        
                                                        if ( (LA6_92 == '!' || (LA6_92 >= '#' && LA6_92 <= '/') || (LA6_92 >= ':' && LA6_92 <= '\u00FF')) )
                                                        {
                                                            alt6 = 17;
                                                        }
                                                        else 
                                                        {
                                                            alt6 = 6;}
                                                    }
                                                    else 
                                                    {
                                                        alt6 = 17;}
                                                }
                                                else 
                                                {
                                                    alt6 = 17;}
                                            }
                                            else 
                                            {
                                                alt6 = 17;}
                                        }
                                        else 
                                        {
                                            alt6 = 17;}
                                    }
                                    else 
                                    {
                                        alt6 = 17;}
                                }
                                else 
                                {
                                    alt6 = 17;}
                            }
                            else 
                            {
                                alt6 = 17;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                    }
                    break;
                case 'i':
                	{
                    int LA6_36 = input.LA(4);
                    
                    if ( (LA6_36 == 'o') )
                    {
                        int LA6_50 = input.LA(5);
                        
                        if ( (LA6_50 == 'r') )
                        {
                            int LA6_63 = input.LA(6);
                            
                            if ( (LA6_63 == 'i') )
                            {
                                int LA6_73 = input.LA(7);
                                
                                if ( (LA6_73 == 't') )
                                {
                                    int LA6_81 = input.LA(8);
                                    
                                    if ( (LA6_81 == 'y') )
                                    {
                                        int LA6_86 = input.LA(9);
                                        
                                        if ( (LA6_86 == '!' || (LA6_86 >= '#' && LA6_86 <= '/') || (LA6_86 >= ':' && LA6_86 <= '\u00FF')) )
                                        {
                                            alt6 = 17;
                                        }
                                        else 
                                        {
                                            alt6 = 5;}
                                    }
                                    else 
                                    {
                                        alt6 = 17;}
                                }
                                else 
                                {
                                    alt6 = 17;}
                            }
                            else 
                            {
                                alt6 = 17;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                    }
                    break;
                	default:
                    	alt6 = 17;
                    	break;}
            
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'm':
        	{
            switch ( input.LA(2) ) 
            {
            case 'o':
            	{
                int LA6_23 = input.LA(3);
                
                if ( (LA6_23 == 'd') )
                {
                    int LA6_37 = input.LA(4);
                    
                    if ( (LA6_37 == 'i') )
                    {
                        int LA6_51 = input.LA(5);
                        
                        if ( (LA6_51 == 'f') )
                        {
                            int LA6_64 = input.LA(6);
                            
                            if ( (LA6_64 == 'y') )
                            {
                                int LA6_74 = input.LA(7);
                                
                                if ( (LA6_74 == '!' || (LA6_74 >= '#' && LA6_74 <= '/') || (LA6_74 >= ':' && LA6_74 <= '\u00FF')) )
                                {
                                    alt6 = 17;
                                }
                                else 
                                {
                                    alt6 = 12;}
                            }
                            else 
                            {
                                alt6 = 17;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
                }
                break;
            case 'u':
            	{
                int LA6_24 = input.LA(3);
                
                if ( (LA6_24 == 't') )
                {
                    int LA6_38 = input.LA(4);
                    
                    if ( (LA6_38 == 'e') )
                    {
                        int LA6_52 = input.LA(5);
                        
                        if ( (LA6_52 == 'x') )
                        {
                            int LA6_65 = input.LA(6);
                            
                            if ( (LA6_65 == '!' || (LA6_65 >= '#' && LA6_65 <= '/') || (LA6_65 >= ':' && LA6_65 <= '\u00FF')) )
                            {
                                alt6 = 17;
                            }
                            else 
                            {
                                alt6 = 7;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
                }
                break;
            	default:
                	alt6 = 17;
                	break;}
        
            }
            break;
        case 't':
        	{
            int LA6_6 = input.LA(2);
            
            if ( (LA6_6 == 'h') )
            {
                int LA6_25 = input.LA(3);
                
                if ( (LA6_25 == 'e') )
                {
                    int LA6_39 = input.LA(4);
                    
                    if ( (LA6_39 == 'n') )
                    {
                        int LA6_53 = input.LA(5);
                        
                        if ( (LA6_53 == '!' || (LA6_53 >= '#' && LA6_53 <= '/') || (LA6_53 >= ':' && LA6_53 <= '\u00FF')) )
                        {
                            alt6 = 17;
                        }
                        else 
                        {
                            alt6 = 8;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'd':
        	{
            int LA6_7 = input.LA(2);
            
            if ( (LA6_7 == 'e') )
            {
                int LA6_26 = input.LA(3);
                
                if ( (LA6_26 == 'd') )
                {
                    int LA6_40 = input.LA(4);
                    
                    if ( (LA6_40 == 'u') )
                    {
                        int LA6_54 = input.LA(5);
                        
                        if ( (LA6_54 == 'c') )
                        {
                            int LA6_67 = input.LA(6);
                            
                            if ( (LA6_67 == 't') )
                            {
                                int LA6_76 = input.LA(7);
                                
                                if ( (LA6_76 == '!' || (LA6_76 >= '#' && LA6_76 <= '/') || (LA6_76 >= ':' && LA6_76 <= '\u00FF')) )
                                {
                                    alt6 = 17;
                                }
                                else 
                                {
                                    alt6 = 9;}
                            }
                            else 
                            {
                                alt6 = 17;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'c':
        	{
            int LA6_8 = input.LA(2);
            
            if ( (LA6_8 == 'o') )
            {
                int LA6_27 = input.LA(3);
                
                if ( (LA6_27 == 'u') )
                {
                    int LA6_41 = input.LA(4);
                    
                    if ( (LA6_41 == 'n') )
                    {
                        int LA6_55 = input.LA(5);
                        
                        if ( (LA6_55 == 't') )
                        {
                            int LA6_68 = input.LA(6);
                            
                            if ( (LA6_68 == '!' || (LA6_68 >= '#' && LA6_68 <= '/') || (LA6_68 >= ':' && LA6_68 <= '\u00FF')) )
                            {
                                alt6 = 17;
                            }
                            else 
                            {
                                alt6 = 11;}
                        }
                        else 
                        {
                            alt6 = 17;}
                    }
                    else 
                    {
                        alt6 = 17;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'a':
        	{
            int LA6_9 = input.LA(2);
            
            if ( (LA6_9 == 'n') )
            {
                int LA6_28 = input.LA(3);
                
                if ( (LA6_28 == 'd') )
                {
                    int LA6_42 = input.LA(4);
                    
                    if ( (LA6_42 == '!' || (LA6_42 >= '#' && LA6_42 <= '/') || (LA6_42 >= ':' && LA6_42 <= '\u00FF')) )
                    {
                        alt6 = 17;
                    }
                    else 
                    {
                        alt6 = 13;}
                }
                else 
                {
                    alt6 = 17;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'o':
        	{
            int LA6_10 = input.LA(2);
            
            if ( (LA6_10 == 'r') )
            {
                int LA6_29 = input.LA(3);
                
                if ( (LA6_29 == '!' || (LA6_29 >= '#' && LA6_29 <= '/') || (LA6_29 >= ':' && LA6_29 <= '\u00FF')) )
                {
                    alt6 = 17;
                }
                else 
                {
                    alt6 = 14;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'i':
        	{
            int LA6_11 = input.LA(2);
            
            if ( (LA6_11 == 'f') )
            {
                int LA6_30 = input.LA(3);
                
                if ( (LA6_30 == '!' || (LA6_30 >= '#' && LA6_30 <= '/') || (LA6_30 >= ':' && LA6_30 <= '\u00FF')) )
                {
                    alt6 = 17;
                }
                else 
                {
                    alt6 = 15;}
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case '0':
        case '1':
        case '2':
        case '3':
        case '4':
        case '5':
        case '6':
        case '7':
        case '8':
        case '9':
        	{
            alt6 = 16;
            }
            break;
        case '!':
        case '#':
        case '$':
        case '%':
        case '&':
        case '\'':
        case '(':
        case ')':
        case '*':
        case '+':
        case ',':
        case '-':
        case '.':
        case '/':
        case ':':
        case ';':
        case '<':
        case '=':
        case '>':
        case '?':
        case '@':
        case 'A':
        case 'B':
        case 'C':
        case 'D':
        case 'E':
        case 'F':
        case 'G':
        case 'H':
        case 'I':
        case 'J':
        case 'K':
        case 'L':
        case 'M':
        case 'N':
        case 'O':
        case 'P':
        case 'Q':
        case 'R':
        case 'S':
        case 'T':
        case 'U':
        case 'V':
        case 'W':
        case 'X':
        case 'Y':
        case 'Z':
        case '[':
        case '\\':
        case ']':
        case '^':
        case '_':
        case '`':
        case 'b':
        case 'e':
        case 'g':
        case 'h':
        case 'j':
        case 'k':
        case 'l':
        case 'n':
        case 's':
        case 'u':
        case 'v':
        case 'w':
        case 'x':
        case 'y':
        case 'z':
        case '{':
        case '|':
        case '}':
        case '~':
        case '\u007F':
        case '\u0080':
        case '\u0081':
        case '\u0082':
        case '\u0083':
        case '\u0084':
        case '\u0085':
        case '\u0086':
        case '\u0087':
        case '\u0088':
        case '\u0089':
        case '\u008A':
        case '\u008B':
        case '\u008C':
        case '\u008D':
        case '\u008E':
        case '\u008F':
        case '\u0090':
        case '\u0091':
        case '\u0092':
        case '\u0093':
        case '\u0094':
        case '\u0095':
        case '\u0096':
        case '\u0097':
        case '\u0098':
        case '\u0099':
        case '\u009A':
        case '\u009B':
        case '\u009C':
        case '\u009D':
        case '\u009E':
        case '\u009F':
        case '\u00A0':
        case '\u00A1':
        case '\u00A2':
        case '\u00A3':
        case '\u00A4':
        case '\u00A5':
        case '\u00A6':
        case '\u00A7':
        case '\u00A8':
        case '\u00A9':
        case '\u00AA':
        case '\u00AB':
        case '\u00AC':
        case '\u00AD':
        case '\u00AE':
        case '\u00AF':
        case '\u00B0':
        case '\u00B1':
        case '\u00B2':
        case '\u00B3':
        case '\u00B4':
        case '\u00B5':
        case '\u00B6':
        case '\u00B7':
        case '\u00B8':
        case '\u00B9':
        case '\u00BA':
        case '\u00BB':
        case '\u00BC':
        case '\u00BD':
        case '\u00BE':
        case '\u00BF':
        case '\u00C0':
        case '\u00C1':
        case '\u00C2':
        case '\u00C3':
        case '\u00C4':
        case '\u00C5':
        case '\u00C6':
        case '\u00C7':
        case '\u00C8':
        case '\u00C9':
        case '\u00CA':
        case '\u00CB':
        case '\u00CC':
        case '\u00CD':
        case '\u00CE':
        case '\u00CF':
        case '\u00D0':
        case '\u00D1':
        case '\u00D2':
        case '\u00D3':
        case '\u00D4':
        case '\u00D5':
        case '\u00D6':
        case '\u00D7':
        case '\u00D8':
        case '\u00D9':
        case '\u00DA':
        case '\u00DB':
        case '\u00DC':
        case '\u00DD':
        case '\u00DE':
        case '\u00DF':
        case '\u00E0':
        case '\u00E1':
        case '\u00E2':
        case '\u00E3':
        case '\u00E4':
        case '\u00E5':
        case '\u00E6':
        case '\u00E7':
        case '\u00E8':
        case '\u00E9':
        case '\u00EA':
        case '\u00EB':
        case '\u00EC':
        case '\u00ED':
        case '\u00EE':
        case '\u00EF':
        case '\u00F0':
        case '\u00F1':
        case '\u00F2':
        case '\u00F3':
        case '\u00F4':
        case '\u00F5':
        case '\u00F6':
        case '\u00F7':
        case '\u00F8':
        case '\u00F9':
        case '\u00FA':
        case '\u00FB':
        case '\u00FC':
        case '\u00FD':
        case '\u00FE':
        case '\u00FF':
        	{
            alt6 = 17;
            }
            break;
        case '\n':
        case '\r':
        	{
            alt6 = 18;
            }
            break;
        case ' ':
        	{
            alt6 = 19;
            }
            break;
        case '\t':
        	{
            alt6 = 20;
            }
            break;
        case '\"':
        	{
            alt6 = 21;
            }
            break;
        	default:
        	    NoViableAltException nvae_d6s0 =
        	        new NoViableAltException("1:1: Tokens : ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | THEN | DEDUCT | FORGET | COUNT | MODIFY | AND | OR | T24 | NUMERIC | CHAR | NEWLINE | SPACE | TAB | QUOTE );", 6, 0, input);
        
        	    throw nvae_d6s0;
        }
        
        switch (alt6) 
        {
            case 1 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:10: RULEBASE
                {
                	mRULEBASE(); 
                
                }
                break;
            case 2 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:19: FACT
                {
                	mFACT(); 
                
                }
                break;
            case 3 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:24: QUERY
                {
                	mQUERY(); 
                
                }
                break;
            case 4 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:30: RULE
                {
                	mRULE(); 
                
                }
                break;
            case 5 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:35: PRIORITY
                {
                	mPRIORITY(); 
                
                }
                break;
            case 6 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:44: PRECONDITION
                {
                	mPRECONDITION(); 
                
                }
                break;
            case 7 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:57: MUTEX
                {
                	mMUTEX(); 
                
                }
                break;
            case 8 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:63: THEN
                {
                	mTHEN(); 
                
                }
                break;
            case 9 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:68: DEDUCT
                {
                	mDEDUCT(); 
                
                }
                break;
            case 10 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:75: FORGET
                {
                	mFORGET(); 
                
                }
                break;
            case 11 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:82: COUNT
                {
                	mCOUNT(); 
                
                }
                break;
            case 12 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:88: MODIFY
                {
                	mMODIFY(); 
                
                }
                break;
            case 13 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:95: AND
                {
                	mAND(); 
                
                }
                break;
            case 14 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:99: OR
                {
                	mOR(); 
                
                }
                break;
            case 15 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:102: T24
                {
                	mT24(); 
                
                }
                break;
            case 16 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:106: NUMERIC
                {
                	mNUMERIC(); 
                
                }
                break;
            case 17 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:114: CHAR
                {
                	mCHAR(); 
                
                }
                break;
            case 18 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:119: NEWLINE
                {
                	mNEWLINE(); 
                
                }
                break;
            case 19 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:127: SPACE
                {
                	mSPACE(); 
                
                }
                break;
            case 20 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:133: TAB
                {
                	mTAB(); 
                
                }
                break;
            case 21 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_EN.g:1:137: QUOTE
                {
                	mQUOTE(); 
                
                }
                break;
        
        }
    
    }


	private void InitializeCyclicDFAs()
	{
	}

 
    
}

}
