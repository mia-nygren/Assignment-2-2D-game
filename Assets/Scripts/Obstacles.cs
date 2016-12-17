using UnityEngine;
using System.Collections;

public class Obstacles:MonoBehaviour {
		// A collection of constant strings that are used in the application 
		// I think it is better to have them in a separate place like this since it is easer to rename them and detect where they are being used. 
		public const string bat = "Bat";   // They have to be constants since I use them in a swich statement....
		public const string spider = "Spider";
		public const string smallTree = "SmallTree";
		public const string largeTree = "LargeTree";
		
		public GameObject Bat;
		public GameObject Spider;
		public GameObject SmallTree;
		public GameObject LargeTree;

	public static class Tags {
		public static string Top { get { return "Top"; } }
		public static string Bottom { get { return "Bottom"; } }
		public static string Obstacle { get { return "Obstacle"; } }
	}
}
