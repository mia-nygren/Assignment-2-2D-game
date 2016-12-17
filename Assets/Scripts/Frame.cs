using System;
using System.Collections.Generic;
using UnityEngine;

public class Frame  {   // Each frame property stores it's x position   // Holds the frame position for each frame	
		
	/* private properties */	
	private int frameNumber;
	private float frameWidth;
	private Vector3 framePosition;
	private Vector3 column1;
	private Vector3 column2;
	private List<GameObject> content = new List<GameObject>(); 
	private Obstacles Obstacles;

	public Frame(int frameNumber, Vector3 framePosition, float frameWidth) {
		Obstacles = GameObject.FindObjectOfType<Obstacles> ();
		this.frameNumber = frameNumber;
		this.framePosition = framePosition; // frame position is the center point of the frame
		
		// Set the column x values - they will be the center points for where to instert a prefab
		column1.x = framePosition.x - frameWidth/4;
		column2.x = framePosition.x + frameWidth/4;  
	}

	public int GetFrameNumber() {
		return this.frameNumber;
	}
		
	public Vector3 GetFramePosition() {
			return this.framePosition;
		}

	public void AddContent(string obstacle1, string obstacle2) {
		AddPrefab (obstacle1, 1);   // The second argument is defining the position (1 or 2) of what column to place it in the frame
		AddPrefab (obstacle2, 2); 
	}
		
	public void AddPrefabToColumn(GameObject prefab, int column) {

		GameObject clone = null;
		if (column == 1) {  
			column1.y = prefab.transform.position.y; // The y value will depend on what type of prefab we want to add- and it's defined in the prefab
			clone = (GameObject)GameObject.Instantiate (prefab, column1, Quaternion.identity); 
		}
		if (column == 2) {
			column2.y = prefab.transform.position.y;
			clone = (GameObject)GameObject.Instantiate (prefab, column2, Quaternion.identity); 
		}

		if(clone!=null)
			content.Add (clone);
	}

	public void ClearContent() {
		foreach (GameObject obj in content) {
			// Destroy the GameObject from the scene
			GameObject.Destroy (obj);
		}
			content.Clear ();  		
		}


	private void AddPrefab(string obstacle, int placement) {
		switch (obstacle) {
		case Obstacles.bat: 
			AddPrefabToColumn(Obstacles.Bat , placement);   
			break;
		case Obstacles.smallTree: 
			AddPrefabToColumn(Obstacles.SmallTree, placement); 
			break;
		case Obstacles.largeTree: 
			AddPrefabToColumn(Obstacles.LargeTree, placement); 
			break;
		case Obstacles.spider: 
			AddPrefabToColumn(Obstacles.Spider, placement); 
			break;
		default:
			break;
		}
	}
}
