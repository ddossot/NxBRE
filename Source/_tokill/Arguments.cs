/*
* The Apache Software License, Version 1.1
*
*
* Copyright (c) 1999, 2000  The Apache Software Foundation.  All rights 
* reserved.
*
* Redistribution and use in source and binary forms, with or without
* modification, are permitted provided that the following conditions
* are met:
*
* 1. Redistributions of source code must retain the above copyright
*    notice, this list of conditions and the following disclaimer. 
*
* 2. Redistributions in binary form must reproduce the above copyright
*    notice, this list of conditions and the following disclaimer in
*    the documentation and/or other materials provided with the
*    distribution.
*
* 3. The end-user documentation included with the redistribution,
*    if any, must include the following acknowledgment:  
*       "This product includes software developed by the
*        Apache Software Foundation (http://www.apache.org/)."
*    Alternately, this acknowledgment may appear in the software itself,
*    if and wherever such third-party acknowledgments normally appear.
*
* 4. The names "Xerces" and "Apache Software Foundation" must
*    not be used to endorse or promote products derived from this
*    software without prior written permission. For written 
*    permission, please contact apache@apache.org.
*
* 5. Products derived from this software may not be called "Apache",
*    nor may "Apache" appear in their name, without prior written
*    permission of the Apache Software Foundation.
*
* THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
* WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
* DISCLAIMED.  IN NO EVENT SHALL THE APACHE SOFTWARE FOUNDATION OR
* ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
* USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
* OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
* OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
* SUCH DAMAGE.
* ====================================================================
*
* This software consists of voluntary contributions made by many
* individuals on behalf of the Apache Software Foundation and was
* originally based on software copyright (c) 1999, International
* Business Machines, Inc., http://www.apache.org.  For more
* information on the Apache Software Foundation, please see
* <http://www.apache.org/>.
*/


namespace net.ideaity.util
{
	using System;
/// <summary> This is 100&#37; Jeff's.  I just repackaged it so I could keep in 
/// synchronised
/// --
/// Sloan Seaman
/// </summary>	
	public class Arguments
	{
		private void  InitBlock()
		{
			queueOfSwitches = new Queue(this, 20);
			queueStringParameters = new Queue(this, 20);
			queueOfOtherStringParameters = new Queue(this, 20);
		}
		/// <summary> 
		/// </summary>
		/// <returns> 
		/// 
		/// </returns>
		virtual public int getArguments()
		{
				if (this.fDbug)
				{
					queueOfSwitches.print();
				}
				
				return queueOfSwitches.empty()?- 1:((System.Int32) queueOfSwitches.pop());
		}
		/// <summary> 
		/// </summary>
		/// <returns> 
		/// 
		/// </returns>
		virtual public string StringParameter
		{
			get
			{
				string s = (System.String) queueStringParameters.pop();
				if (this.fDbug)
				{
					queueStringParameters.print();
				}
				if (this.fDbug)
				{
					System.Console.Out.WriteLine("string par = " + s);
				}
				return s;
			}
			
		}
		virtual public System.String[] Usage
		{
			set
			{
				messageArray = value;
			}
			
		}
		private bool fDbug = false;
		private Queue queueOfSwitches;
		private Queue queueStringParameters;
		private Queue queueOfOtherStringParameters;
		private System.String[] messageArray = null;
		
		public Arguments()
		{
			InitBlock();
		}
		
		/// <summary> Takes the array of standard Args passed
		/// from main method and parses the '-' and
		/// the characters after arg.
		/// <P>
		/// - The value -1 is a special flag that is
		/// used to indicate the beginning of the queue
		/// of flags and it is also to tell the end of
		/// a group of switches.
		/// </P>
		/// <P>
		/// This method will generate 3 internal queues.
		/// - A queue that has the switch flag arguments.
		/// e.g. 
		/// -dvV
		/// will hold  d, v, V, -1.
		/// </P>
		/// <P>
		/// - A queue holding the string arguments needed by
		/// the switch flag arguments.
		/// If character -p requires a string argument.
		/// The string argument is saved in the string argument
		/// queue.
		/// </P>
		/// <P>
		/// - A queue holding a list of files string parameters
		/// not associated with a switch flag.
		/// -a -v -p myvalue  test.xml test1.xml
		/// this queue will containt test.xml test1.xml
		/// </P>
		/// </summary>
		/// <param name="arguments">
		/// </param>
		/// <param name="argsWithOptions">
		/// </param>
		public virtual void  parseArgumentTokens(System.String[] arguments, char[] argsWithOptions)
		{
			int lengthOfToken = 0;
			char[] bufferOfToken = null;			
			int argLength = arguments.Length;
			
			for (int i = 0; i < argLength; i++)
			{
				bufferOfToken = arguments[i].ToCharArray();
				lengthOfToken = bufferOfToken.Length;
				if (bufferOfToken[0] == '-')
				{
					int token;
					for (int j = 1; j < lengthOfToken; j++)
					{
						token = bufferOfToken[j];
						queueOfSwitches.push((System.Object) token);
						for (int k = 0; k < argsWithOptions.Length; k++)
						{
							if (token == argsWithOptions[k])
							{
								if (this.fDbug)
								{
									System.Console.Out.WriteLine("token = " + token);
								}
								queueStringParameters.push(arguments[++i]);
								goto outer;
							}
						}
					}
					
					if (i + 1 < argLength)
					{
						if (!(arguments[i + 1][0] == '-'))
						//next argument not start '-'
							queueOfSwitches.push((System.Object) (- 1));
						//put -1 marker 
					}
				}
				else
				{
					queueOfOtherStringParameters.push(arguments[i]);
				}
				outer: ;
			}
			
			
			if (this.fDbug)
			{
				queueOfSwitches.print();
				queueStringParameters.print();
				queueOfOtherStringParameters.print();
			}
		}
		

		public virtual string getListFiles()
		{
			
			if (this.fDbug)
			{
				queueOfOtherStringParameters.print();
			}
			
			string s = (System.String) queueOfOtherStringParameters.pop();
			return s;
		}
		
		
		
		public virtual int stringParameterLeft()
		{
			return queueStringParameters.size();
		}
		
		
		
		public virtual void  printUsage()
		{
			for (int i = 0; i < messageArray.Length; i++)
			{
				System.Console.Error.WriteLine(messageArray[i]);
			}
		}
		
		// Private methods
		// Private inner classes
		private const int maxIncrement = 10;
		
		private class Queue
		{
			private void  InitBlock(Arguments enclosingInstance)
			{
				this.enclosingInstance = enclosingInstance;
			}
			private Arguments enclosingInstance;
			public Arguments Enclosing_Instance
			{
				get
				{
					return enclosingInstance;
				}
				
			}
			
			private System.Object[] queue;
			private int max;
			private int front;
			private int rear;
			private int items;
			
			
			public Queue(Arguments enclosingInstance, int size)
			{
				InitBlock(enclosingInstance);
				queue = new System.Object[size];
				front = 0;
				rear = - 1;
				items = 0;
				max = size;
			}
			public virtual void  push(object token)
			{
				try
				{
					queue[++rear] = token;
					items++;
				}
				catch (System.IndexOutOfRangeException)
				{
					System.Object[] holdQueue = new System.Object[max + net.ideaity.util.Arguments.maxIncrement];
					Array.Copy(queue, 0, holdQueue, 0, max);
					queue = holdQueue;
					max += net.ideaity.util.Arguments.maxIncrement;
					queue[rear] = token;
					items++;
				}
				
			}
			public virtual object pop()
			{
				object token = null;
				if (items != 0)
				{
					token = queue[front++];
					items--;
				}
				return token;
			}
			public virtual bool empty()
			{
				return (items == 0);
			}
			
			public virtual int size()
			{
				return items;
			}
			
			public virtual void  clear()
			{
				front = 0;
				rear = - 1;
				items = 0;
			}
			
			
			public virtual void  print()
			{
				for (int i = front; i <= rear; i++)
				{
					System.Console.Out.WriteLine("token[ " + i + "] = " + queue[i]);
				}
			}
		}
	}
}
