// $ANTLR 3.0 C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g 2007-07-21 21:02:45




using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



internal class InferenceRules_FRLexer : Lexer 
{
    public const int MODIFY = 16;
    public const int DEDUCT = 13;
    public const int RULE = 7;
    public const int RULEBASE = 4;
    public const int CHAR = 24;
    public const int TAB = 22;
    public const int COUNT = 15;
    public const int NUMERIC = 23;
    public const int AND = 17;
    public const int Tokens = 25;
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
    public const int FORGET = 14;
    public const int QUERY = 6;
    public const int FACT = 5;

    public InferenceRules_FRLexer() 
    {
		InitializeCyclicDFAs();
    }
    public InferenceRules_FRLexer(ICharStream input) 
		: base(input)
	{
		InitializeCyclicDFAs();
    }
    
    override public string GrammarFileName
    {
    	get { return "C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g";} 
    }

    // $ANTLR start RULEBASE 
    public void mRULEBASE() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = RULEBASE;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:10:12: ( 'base_de_règles' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:10:12: 'base_de_règles'
            {
            	Match("base_de_règles"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:11:8: ( 'fait' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:11:8: 'fait'
            {
            	Match("fait"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:12:9: ( 'requête' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:12:9: 'requête'
            {
            	Match("requête"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:13:8: ( 'règle' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:13:8: 'règle'
            {
            	Match("règle"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:14:12: ( 'priorité' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:14:12: 'priorité'
            {
            	Match("priorité"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:15:16: ( 'précondition' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:15:16: 'précondition'
            {
            	Match("précondition"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:16:9: ( 'mutex' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:16:9: 'mutex'
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

    // $ANTLR start IF 
    public void mIF() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = IF;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:17:6: ( 'si' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:17:6: 'si'
            {
            	Match("si"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end IF

    // $ANTLR start THEN 
    public void mTHEN() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = THEN;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:18:8: ( 'alors' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:18:8: 'alors'
            {
            	Match("alors"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:19:10: ( 'déduis' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:19:10: 'déduis'
            {
            	Match("déduis"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:20:10: ( 'oublis' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:20:10: 'oublis'
            {
            	Match("oublis"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:21:9: ( 'compte' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:21:9: 'compte'
            {
            	Match("compte"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:22:10: ( 'modifie' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:22:10: 'modifie'
            {
            	Match("modifie"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:23:7: ( 'et' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:23:7: 'et'
            {
            	Match("et"); 

            
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:24:6: ( 'ou' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:24:6: 'ou'
            {
            	Match("ou"); 

            
            }
    
            this.type = _type;
        }
        finally 
    	{
        }
    }
    // $ANTLR end OR

    // $ANTLR start NUMERIC 
    public void mNUMERIC() // throws RecognitionException [2]
    {
        try 
    	{
            int _type = NUMERIC;
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:102:11: ( ( '0' .. '9' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:102:11: ( '0' .. '9' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:102:11: ( '0' .. '9' )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:102:12: '0' .. '9'
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:103:8: ( ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:103:8: ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:103:8: ( '!' | '\\u0023' .. '\\u002F' | '\\u003A' .. '\\u00FF' )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:11: ( ( ( '\\r' )? '\\n' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:11: ( ( '\\r' )? '\\n' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:11: ( ( '\\r' )? '\\n' )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:12: ( '\\r' )? '\\n'
            			    {
            			    	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:12: ( '\\r' )?
            			    	int alt3 = 2;
            			    	int LA3_0 = input.LA(1);
            			    	
            			    	if ( (LA3_0 == '\r') )
            			    	{
            			    	    alt3 = 1;
            			    	}
            			    	switch (alt3) 
            			    	{
            			    	    case 1 :
            			    	        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:104:12: '\\r'
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:105:9: ( ( ' ' )+ )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:105:9: ( ' ' )+
            {
            	// C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:105:9: ( ' ' )+
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
            			    // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:105:9: ' '
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:106:7: ( '\\t' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:106:7: '\\t'
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
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:107:9: ( '\"' )
            // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:107:9: '\"'
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
        // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:10: ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | AND | OR | NUMERIC | CHAR | NEWLINE | SPACE | TAB | QUOTE )
        int alt6 = 21;
        switch ( input.LA(1) ) 
        {
        case 'b':
        	{
            int LA6_1 = input.LA(2);
            
            if ( (LA6_1 == 'a') )
            {
                int LA6_18 = input.LA(3);
                
                if ( (LA6_18 == 's') )
                {
                    int LA6_31 = input.LA(4);
                    
                    if ( (LA6_31 == 'e') )
                    {
                        int LA6_46 = input.LA(5);
                        
                        if ( (LA6_46 == '_') )
                        {
                            int LA6_58 = input.LA(6);
                            
                            if ( (LA6_58 == 'd') )
                            {
                                int LA6_70 = input.LA(7);
                                
                                if ( (LA6_70 == 'e') )
                                {
                                    int LA6_81 = input.LA(8);
                                    
                                    if ( (LA6_81 == '_') )
                                    {
                                        int LA6_89 = input.LA(9);
                                        
                                        if ( (LA6_89 == 'r') )
                                        {
                                            int LA6_94 = input.LA(10);
                                            
                                            if ( (LA6_94 == '\u00E8') )
                                            {
                                                int LA6_97 = input.LA(11);
                                                
                                                if ( (LA6_97 == 'g') )
                                                {
                                                    int LA6_99 = input.LA(12);
                                                    
                                                    if ( (LA6_99 == 'l') )
                                                    {
                                                        int LA6_101 = input.LA(13);
                                                        
                                                        if ( (LA6_101 == 'e') )
                                                        {
                                                            int LA6_103 = input.LA(14);
                                                            
                                                            if ( (LA6_103 == 's') )
                                                            {
                                                                int LA6_105 = input.LA(15);
                                                                
                                                                if ( (LA6_105 == '!' || (LA6_105 >= '#' && LA6_105 <= '/') || (LA6_105 >= ':' && LA6_105 <= '\u00FF')) )
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
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'f':
        	{
            int LA6_2 = input.LA(2);
            
            if ( (LA6_2 == 'a') )
            {
                int LA6_19 = input.LA(3);
                
                if ( (LA6_19 == 'i') )
                {
                    int LA6_32 = input.LA(4);
                    
                    if ( (LA6_32 == 't') )
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
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'r':
        	{
            switch ( input.LA(2) ) 
            {
            case 'e':
            	{
                int LA6_20 = input.LA(3);
                
                if ( (LA6_20 == 'q') )
                {
                    int LA6_33 = input.LA(4);
                    
                    if ( (LA6_33 == 'u') )
                    {
                        int LA6_48 = input.LA(5);
                        
                        if ( (LA6_48 == '\u00EA') )
                        {
                            int LA6_60 = input.LA(6);
                            
                            if ( (LA6_60 == 't') )
                            {
                                int LA6_71 = input.LA(7);
                                
                                if ( (LA6_71 == 'e') )
                                {
                                    int LA6_82 = input.LA(8);
                                    
                                    if ( (LA6_82 == '!' || (LA6_82 >= '#' && LA6_82 <= '/') || (LA6_82 >= ':' && LA6_82 <= '\u00FF')) )
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
                else 
                {
                    alt6 = 17;}
                }
                break;
            case '\u00E8':
            	{
                int LA6_21 = input.LA(3);
                
                if ( (LA6_21 == 'g') )
                {
                    int LA6_34 = input.LA(4);
                    
                    if ( (LA6_34 == 'l') )
                    {
                        int LA6_49 = input.LA(5);
                        
                        if ( (LA6_49 == 'e') )
                        {
                            int LA6_61 = input.LA(6);
                            
                            if ( (LA6_61 == '!' || (LA6_61 >= '#' && LA6_61 <= '/') || (LA6_61 >= ':' && LA6_61 <= '\u00FF')) )
                            {
                                alt6 = 17;
                            }
                            else 
                            {
                                alt6 = 4;}
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
        case 'p':
        	{
            int LA6_4 = input.LA(2);
            
            if ( (LA6_4 == 'r') )
            {
                switch ( input.LA(3) ) 
                {
                case '\u00E9':
                	{
                    int LA6_35 = input.LA(4);
                    
                    if ( (LA6_35 == 'c') )
                    {
                        int LA6_50 = input.LA(5);
                        
                        if ( (LA6_50 == 'o') )
                        {
                            int LA6_62 = input.LA(6);
                            
                            if ( (LA6_62 == 'n') )
                            {
                                int LA6_73 = input.LA(7);
                                
                                if ( (LA6_73 == 'd') )
                                {
                                    int LA6_83 = input.LA(8);
                                    
                                    if ( (LA6_83 == 'i') )
                                    {
                                        int LA6_91 = input.LA(9);
                                        
                                        if ( (LA6_91 == 't') )
                                        {
                                            int LA6_95 = input.LA(10);
                                            
                                            if ( (LA6_95 == 'i') )
                                            {
                                                int LA6_98 = input.LA(11);
                                                
                                                if ( (LA6_98 == 'o') )
                                                {
                                                    int LA6_100 = input.LA(12);
                                                    
                                                    if ( (LA6_100 == 'n') )
                                                    {
                                                        int LA6_102 = input.LA(13);
                                                        
                                                        if ( (LA6_102 == '!' || (LA6_102 >= '#' && LA6_102 <= '/') || (LA6_102 >= ':' && LA6_102 <= '\u00FF')) )
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
                        int LA6_51 = input.LA(5);
                        
                        if ( (LA6_51 == 'r') )
                        {
                            int LA6_63 = input.LA(6);
                            
                            if ( (LA6_63 == 'i') )
                            {
                                int LA6_74 = input.LA(7);
                                
                                if ( (LA6_74 == 't') )
                                {
                                    int LA6_84 = input.LA(8);
                                    
                                    if ( (LA6_84 == '\u00E9') )
                                    {
                                        int LA6_92 = input.LA(9);
                                        
                                        if ( (LA6_92 == '!' || (LA6_92 >= '#' && LA6_92 <= '/') || (LA6_92 >= ':' && LA6_92 <= '\u00FF')) )
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
                        int LA6_52 = input.LA(5);
                        
                        if ( (LA6_52 == 'f') )
                        {
                            int LA6_64 = input.LA(6);
                            
                            if ( (LA6_64 == 'i') )
                            {
                                int LA6_75 = input.LA(7);
                                
                                if ( (LA6_75 == 'e') )
                                {
                                    int LA6_85 = input.LA(8);
                                    
                                    if ( (LA6_85 == '!' || (LA6_85 >= '#' && LA6_85 <= '/') || (LA6_85 >= ':' && LA6_85 <= '\u00FF')) )
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
                        int LA6_53 = input.LA(5);
                        
                        if ( (LA6_53 == 'x') )
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
        case 's':
        	{
            int LA6_6 = input.LA(2);
            
            if ( (LA6_6 == 'i') )
            {
                int LA6_25 = input.LA(3);
                
                if ( (LA6_25 == '!' || (LA6_25 >= '#' && LA6_25 <= '/') || (LA6_25 >= ':' && LA6_25 <= '\u00FF')) )
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
            break;
        case 'a':
        	{
            int LA6_7 = input.LA(2);
            
            if ( (LA6_7 == 'l') )
            {
                int LA6_26 = input.LA(3);
                
                if ( (LA6_26 == 'o') )
                {
                    int LA6_40 = input.LA(4);
                    
                    if ( (LA6_40 == 'r') )
                    {
                        int LA6_54 = input.LA(5);
                        
                        if ( (LA6_54 == 's') )
                        {
                            int LA6_66 = input.LA(6);
                            
                            if ( (LA6_66 == '!' || (LA6_66 >= '#' && LA6_66 <= '/') || (LA6_66 >= ':' && LA6_66 <= '\u00FF')) )
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
            break;
        case 'd':
        	{
            int LA6_8 = input.LA(2);
            
            if ( (LA6_8 == '\u00E9') )
            {
                int LA6_27 = input.LA(3);
                
                if ( (LA6_27 == 'd') )
                {
                    int LA6_41 = input.LA(4);
                    
                    if ( (LA6_41 == 'u') )
                    {
                        int LA6_55 = input.LA(5);
                        
                        if ( (LA6_55 == 'i') )
                        {
                            int LA6_67 = input.LA(6);
                            
                            if ( (LA6_67 == 's') )
                            {
                                int LA6_78 = input.LA(7);
                                
                                if ( (LA6_78 == '!' || (LA6_78 >= '#' && LA6_78 <= '/') || (LA6_78 >= ':' && LA6_78 <= '\u00FF')) )
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
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'o':
        	{
            int LA6_9 = input.LA(2);
            
            if ( (LA6_9 == 'u') )
            {
                switch ( input.LA(3) ) 
                {
                case 'b':
                	{
                    int LA6_42 = input.LA(4);
                    
                    if ( (LA6_42 == 'l') )
                    {
                        int LA6_56 = input.LA(5);
                        
                        if ( (LA6_56 == 'i') )
                        {
                            int LA6_68 = input.LA(6);
                            
                            if ( (LA6_68 == 's') )
                            {
                                int LA6_79 = input.LA(7);
                                
                                if ( (LA6_79 == '!' || (LA6_79 >= '#' && LA6_79 <= '/') || (LA6_79 >= ':' && LA6_79 <= '\u00FF')) )
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
                    	alt6 = 15;
                    	break;}
            
            }
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'c':
        	{
            int LA6_10 = input.LA(2);
            
            if ( (LA6_10 == 'o') )
            {
                int LA6_29 = input.LA(3);
                
                if ( (LA6_29 == 'm') )
                {
                    int LA6_44 = input.LA(4);
                    
                    if ( (LA6_44 == 'p') )
                    {
                        int LA6_57 = input.LA(5);
                        
                        if ( (LA6_57 == 't') )
                        {
                            int LA6_69 = input.LA(6);
                            
                            if ( (LA6_69 == 'e') )
                            {
                                int LA6_80 = input.LA(7);
                                
                                if ( (LA6_80 == '!' || (LA6_80 >= '#' && LA6_80 <= '/') || (LA6_80 >= ':' && LA6_80 <= '\u00FF')) )
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
            else 
            {
                alt6 = 17;}
            }
            break;
        case 'e':
        	{
            int LA6_11 = input.LA(2);
            
            if ( (LA6_11 == 't') )
            {
                int LA6_30 = input.LA(3);
                
                if ( (LA6_30 == '!' || (LA6_30 >= '#' && LA6_30 <= '/') || (LA6_30 >= ':' && LA6_30 <= '\u00FF')) )
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
        case 'g':
        case 'h':
        case 'i':
        case 'j':
        case 'k':
        case 'l':
        case 'n':
        case 'q':
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
        	        new NoViableAltException("1:1: Tokens : ( RULEBASE | FACT | QUERY | RULE | PRIORITY | PRECONDITION | MUTEX | IF | THEN | DEDUCT | FORGET | COUNT | MODIFY | AND | OR | NUMERIC | CHAR | NEWLINE | SPACE | TAB | QUOTE );", 6, 0, input);
        
        	    throw nvae_d6s0;
        }
        
        switch (alt6) 
        {
            case 1 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:10: RULEBASE
                {
                	mRULEBASE(); 
                
                }
                break;
            case 2 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:19: FACT
                {
                	mFACT(); 
                
                }
                break;
            case 3 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:24: QUERY
                {
                	mQUERY(); 
                
                }
                break;
            case 4 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:30: RULE
                {
                	mRULE(); 
                
                }
                break;
            case 5 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:35: PRIORITY
                {
                	mPRIORITY(); 
                
                }
                break;
            case 6 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:44: PRECONDITION
                {
                	mPRECONDITION(); 
                
                }
                break;
            case 7 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:57: MUTEX
                {
                	mMUTEX(); 
                
                }
                break;
            case 8 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:63: IF
                {
                	mIF(); 
                
                }
                break;
            case 9 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:66: THEN
                {
                	mTHEN(); 
                
                }
                break;
            case 10 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:71: DEDUCT
                {
                	mDEDUCT(); 
                
                }
                break;
            case 11 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:78: FORGET
                {
                	mFORGET(); 
                
                }
                break;
            case 12 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:85: COUNT
                {
                	mCOUNT(); 
                
                }
                break;
            case 13 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:91: MODIFY
                {
                	mMODIFY(); 
                
                }
                break;
            case 14 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:98: AND
                {
                	mAND(); 
                
                }
                break;
            case 15 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:102: OR
                {
                	mOR(); 
                
                }
                break;
            case 16 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:105: NUMERIC
                {
                	mNUMERIC(); 
                
                }
                break;
            case 17 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:113: CHAR
                {
                	mCHAR(); 
                
                }
                break;
            case 18 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:118: NEWLINE
                {
                	mNEWLINE(); 
                
                }
                break;
            case 19 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:126: SPACE
                {
                	mSPACE(); 
                
                }
                break;
            case 20 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:132: TAB
                {
                	mTAB(); 
                
                }
                break;
            case 21 :
                // C:\\Documents and Settings\\David Dossot\\My Documents\\NxBRE\\SVN\\trunk\\NxDSL\\Source\\Grammars\\InferenceRules_FR.g:1:136: QUOTE
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
