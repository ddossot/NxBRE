namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	
	using NxBRE.InferenceEngine;
	using NxBRE.InferenceEngine.IO;
		
	public class PhysicalObject {
		private int key;
		private int weight;
		private int gweight;
		private bool hasErrors;
		
		public int Key {get {return key;} set{key = value;}}
		public int Weight {get {return weight;}	set{weight = value;}}
		public int GWeight {get {return gweight;} set{gweight = value;}}
		public bool HasErrors {get {return hasErrors;} set{hasErrors = value;}}

		public PhysicalObject(int key, int weight, int gweight)
		{
			this.key = key;
			this.weight = weight;
			this.gweight = gweight;
			this.hasErrors = false;
		}

		public override string ToString() {
			return "PhysicalObject-"+key+((HasErrors)?"-E!":"");
		}
		
		public override int GetHashCode() {
			return key;
		}
		
	}

}
