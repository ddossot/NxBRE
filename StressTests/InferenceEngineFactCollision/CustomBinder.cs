namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
	
	public class CustomBinder:AbstractBinder {
		public CustomBinder():base(BindingTypes.BeforeAfter) {}
		
		public override void BeforeProcess() {
			foreach(PhysicalObject physical_object in (ArrayList)BusinessObjects["PHYSICAL"])
			{
				IEF.AssertNewFactOrFail("Weight", new object[]{physical_object,physical_object.Weight});
				IEF.AssertNewFactOrFail("GWeight", new object[]{physical_object,physical_object.GWeight});
			}
		}

		public override void AfterProcess() {}
		
		public override NewFactEvent OnNewFact {
			get {
				return new NewFactEvent(NewFactHandler);
			}
		}
		
		private void NewFactHandler(NewFactEventArgs nfea) {
			if (nfea.Fact.Type == "WeightError")
			{	
				((PhysicalObject)nfea.Fact.GetPredicateValue(0)).HasErrors = true;
			}
		}
	}
	
}
