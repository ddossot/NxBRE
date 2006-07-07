// For useful output, start with: Discount -l 2 -e 1 discount.xml

namespace NxBRE.Examples
{
	using System;

	using net.ideaity.util;
	using net.ideaity.util.events;

	public class MainClass
	{
		public static bool SHOWSTACK = false;
		public static int LOGLEVEL = 99;
		public static int ERRORLEVEL = 99;
		
		public static void Main(string[] args)
		{
			Arguments argOpt = new Arguments();
			argOpt.Usage = new string[]{"Usage",
																	"\tTest (options) [xmlfile]",
																	"",
																	"\toptions:",
																	"\t  -s | -S  Turn ON/OFF RuleContext dump",
																	"\t  -e [num]  Set the error level to num",
																	"\t\t  num=[" + ExceptionEventImpl.FATAL + "," + ExceptionEventImpl.ERROR + "," + ExceptionEventImpl.WARN + "]",
																	"\t  -l [num]  Set the log level to num",
																	"\t\t  num=[" + LogEventImpl.DEBUG + "," + LogEventImpl.FATAL + "," + LogEventImpl.ERROR + "," + LogEventImpl.WARN + "," + LogEventImpl.INFO + "]",
																	"\t  -h       This message"};
			
			if (args.Length == 0)
			{
				argOpt.printUsage();
				System.Environment.Exit(1);
			}
			
			argOpt.parseArgumentTokens(args, new char[]{'e', 'l'});
						
			int c;
			while ((c = argOpt.getArguments()) != - 1)
			{
				switch (c)
				{
					
					case 's': 
						SHOWSTACK = true;
						break;
					
					case 'S': 
						SHOWSTACK = false;
						break;
					
					case 'e': 
						ERRORLEVEL = Int32.Parse(argOpt.StringParameter);
						break;
					
					case 'l': 
						LOGLEVEL = Int32.Parse(argOpt.StringParameter);
						break;
					
					case 'h': 
						argOpt.printUsage();
						System.Environment.Exit(0);
						break;
					
					default: 
						break;
					
				}
			}
			
			string xmlFile = argOpt.getListFiles();
			CalculateTotal calculator = new CalculateTotal(xmlFile, ERRORLEVEL, LOGLEVEL);
			
			if (calculator.IsValid) {
				Console.Out.WriteLine("\nOrder #1: Calculated discounted total={0} (expected: {1})\n",
				                      calculator.GetTotal(new Order(5, 25, "A")),
				                      25);
				
				Console.Out.WriteLine("\nOrder #2: Calculated discounted total={0} (expected: {1})\n",
				                      calculator.GetTotal(new Order(50, 250, "B")),
				                      225);
				
				Console.Out.WriteLine("\nOrder #3: Calculated discounted total={0} (expected: {1})\n",
				                      calculator.GetTotal(new Order(20, 200, "C")),
				                      160);
				
				Console.Out.WriteLine("\nOrder #4: Calculated discounted total={0} (expected: {1})\n",
				                      calculator.GetTotal(new Order(50, 500, "D")),
				                      350);
			}
		}
	}
}
