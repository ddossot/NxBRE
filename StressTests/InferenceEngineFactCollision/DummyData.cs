namespace NxBRE.StressTests
{
	using System;
	using System.Collections;
	
	public class DummyData {
		private static DummyData dd = null;
		
		private int nbDecaItems;
		private int generatedPhysicalObjects = 0;
			
		public static DummyData GetInstance() {
			if (dd == null) dd = new DummyData();
			return dd;
		}
		public IDictionary GetBusinessObjects(int nbDecaItems) {
			this.nbDecaItems = nbDecaItems;
			generatedPhysicalObjects = 0;
		
			IList physical_objects = GetPhysicalObjects();
			
			IDictionary businessObjects = new Hashtable();
			businessObjects.Add("PHYSICAL", physical_objects);
			return businessObjects;
		}

		private IList GetPhysicalObjects() 
		{
			ArrayList physical_objects = new ArrayList();
			
			for(int i=0; i<nbDecaItems; i++) 
			{
				physical_objects.Add(new PhysicalObject(1 + i*10, 1, 2));
				physical_objects.Add(new PhysicalObject(2 + i*10, 2, 3));
				physical_objects.Add(new PhysicalObject(3 + i*10, 3, 4));
				physical_objects.Add(new PhysicalObject(4 + i*10, 4, 5 ));
				physical_objects.Add(new PhysicalObject(5 + i*10, 5, 6));
				physical_objects.Add(new PhysicalObject(6 + i*10, 6, 7));
				physical_objects.Add(new PhysicalObject(7 + i*10, 7, 8));

				// Objects to fire the rule of weight
				physical_objects.Add(new PhysicalObject(8 + i*10, 9, 8));
				physical_objects.Add(new PhysicalObject(9 + i*10, 10, 9));
				physical_objects.Add(new PhysicalObject(10 + i*10, 11, 10));
			}
			
			generatedPhysicalObjects += physical_objects.Count;
			return physical_objects;
		}
	}

}
