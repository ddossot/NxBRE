namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	using NxBRE.InferenceEngine.Rules;
	
	public class WeightError 
	{
		private string ruleBaseFile;
		private int nbDecaItems;
		
		public void PerformProcess(IBinder binder)	
		{
			// generate dummy business objects
			IDictionary businessObjects = DummyData.GetInstance().GetBusinessObjects(nbDecaItems);

			// instantiate an inference engine, bind my data and process the rules
			IEImpl ie = new IEImpl(binder);
			//ie.LogHandlers += new DispatchLog(HandleLogEvent);
			ie.LoadRuleBase(new RuleML09NafDatalogAdapter(ruleBaseFile,System.IO.FileAccess.Read));
			ie.Process(businessObjects);
			
			// processing is done, let's analyze the results
			IList<IList<Fact>> qrs = ie.RunQuery("Item with weight errors");
			
			if (qrs.Count != (3 * nbDecaItems)) throw new Exception("Collision!");
			
			Console.WriteLine("No collision!");
		}
		
		public WeightError(int nbDecaItems, string ruleBaseFile)
		{
			this.nbDecaItems = nbDecaItems;
			this.ruleBaseFile = ruleBaseFile;
		}
	}

}
