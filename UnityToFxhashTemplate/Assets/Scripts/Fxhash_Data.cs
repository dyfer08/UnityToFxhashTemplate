using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Fxhash_Data : ScriptableObject {	
	public bool usecustomHash = false;
	public string customHash = "ooS1USyYLinnBdKeaBfBEAwBnNPKVtXgWz1Vj5Jr8Tb9MBECw8i";

	public bool generatePreview = false;
	public int previewQuantity = 30;
	public int previewSuperSize = 2;
	
	public bool seedsHistory = false;
	public List<Seed> seeds = new List<Seed>();
}

[System.Serializable]
public class Seed {
	public string date = "";
	public string hash = "";
}
