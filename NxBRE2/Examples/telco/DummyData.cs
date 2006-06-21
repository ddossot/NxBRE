namespace org.nxbre.examples
{
	using System;
	using System.Collections;

	public class DummyData {
		// Singleton for generating dummy customers & transactions
		private static DummyData dd = null;
		
		private Random rnd = new Random();
			
		public static DummyData GetInstance() {
			if (dd == null) dd = new DummyData();
			return dd;
		}
		
		public ArrayList GetBusinessObjects(int nbDodecaCustomers) {
			ArrayList al = new ArrayList();
			
			for(int i=0; i<nbDodecaCustomers; i++) {
				// PriVaTe Callers Data
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "LOC", "PVT", "CC01", "PerSecond:0.001"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "LOC", "PVT", "CC01", "PerSecond:0.001"));
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "INT", "PVT", "CC02", "FirstMinuteFeeThenPerSecond:0.25;0.002"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "INT", "PVT", "CC02", "PerSecond:0.002"));
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "INT", "PVT", "CC99", "FirstMinuteFeeThenPerSecond:0.50;0.003"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "INT", "PVT", "CC99", "FirstMinuteFeeThenPerSecond:0.50;0.003"));
				// PROfessional Callers Data
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "LOC", "PRO", "CC01", "PerSecond:0.001"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "LOC", "PRO", "CC01", "PerSecond:0.001"));
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "INT", "PRO", "CC02", "FirstMinuteFeeThenPerSecond:0.15;0.0015"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "INT", "PRO", "CC02", "FirstMinuteFeeThenPerSecond:0.15;0.0015"));
				al.Add(new CallData(new DateTime(2004, 6, 1, 9+rnd.Next(10), rnd.Next(60), rnd.Next(60)), "INT", "PRO", "CC99", "FirstMinuteFeeThenPerSecond:0.15;0.0015"));
				al.Add(new CallData(new DateTime(2004, 6, 1, rnd.Next(7), rnd.Next(60), rnd.Next(60)), "INT", "PRO", "CC99", "FirstMinuteFeeThenPerSecond:0.15;0.0015"));
			}
			
			return al;
		}
	}
}
