using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FramesController : MonoBehaviour {
	// I Use Frames as an empty object that has the CamvasController script attached to it and it decides where the all the obstacles should be instantiated

	/* Private properties */
	private CameraController cameraController; 
	private Dictionary<int, Frame> FrameDictionary = new Dictionary<int, Frame>();  // Using Dictionary since it has O(1) lookup-time, and I have to retrieve a value fast!
	private int numberOfFrames = 5;  
	private float loopStartPosX;
	private float loopEndPosX; 
	private float cameraStartPosX = 0;  
	private List<string> obstacleList;  // Using a list because I have to remove elements from it's copy later, and it is easier to do that with a list than an array. 
	private List<string> listClone;
	private Obstacles Obstacles;
	private bool addContent = true;

	void Awake() {
		Obstacles = GameObject.FindObjectOfType<Obstacles> ();
		// Create a list with 8 game objects - 2 of each kind
		obstacleList = new List<string> {Obstacles.bat, Obstacles.largeTree, Obstacles.spider, Obstacles.smallTree, Obstacles.bat, Obstacles.spider, Obstacles.smallTree, Obstacles.largeTree} ;  

		// Access the CameraController script 
		cameraController = Camera.main.GetComponent<CameraController> ();
		CreateFrames();  // This is only done if there is no frames already. 
		CreateFramesContainer ();
		AddFrameContents (); 
	}

	void Start () {
		// Define where the loop ends in the x-axis
		loopEndPosX = FrameDictionary [numberOfFrames].GetFramePosition().x - cameraController.GetCameraOffset(); 
	}

	// Update is called once per frame
	void Update () {
		if (FrameDictionary.Count > 0) { 
			// Do this in update:
			if (cameraController.IsOnFrame (FrameDictionary [3])) {  
				if (addContent) {
					// reassign the listClone
					listClone = new List<string> (obstacleList);   // Runs if entered frame 3  - also duplicated code - refactor!
					ShuffleElements (listClone); 

					// Clear frame 5
					FrameDictionary [numberOfFrames].ClearContent ();

					// Add content to frame 5
					string obstacle1 = SubtractObstacleFromList ();
					string obstacle2 = SubtractObstacleFromList ();
					FrameDictionary [numberOfFrames].AddContent (obstacle1, obstacle2); 

					// Add exactly the same content to frame 2 ( so frame 5 and to are identical )   
					FrameDictionary [2].ClearContent ();
					FrameDictionary [2].AddContent (obstacle1, obstacle2);
					addContent = false;
				}
			}

			if (cameraController.IsOnFrame (FrameDictionary [numberOfFrames])) {  // If the camera is on the 5th frame add new content to frame 3 and 4
				// Clear and add new content to frame 3 and 4
				for (int i = 3; i < 4; i++) {
					FrameDictionary [i].ClearContent (); 
					FrameDictionary [i].AddContent (SubtractObstacleFromList (), SubtractObstacleFromList ());
				}
				addContent = true; 
			}
		}
		// TODO - if I have time - Animate bat forward if the distance between it and the player is less than frameswidth / 2 or something like that...? 
	}

	public void Initialize() {
		// Clear all frames
		if (FrameDictionary.Count > 0) { // Will be true only if frames have been added
			foreach (Frame frame in FrameDictionary.Values) {
				frame.ClearContent ();
			}
		} 
		AddFrameContents (); 
		addContent = true;
	}

	private void CreateFramesContainer() {   // This doesn't really have any other function other than that the canvas (imo) should be of proper size instead of just a small rectangle. 
			// Set the size to the same as the main camera.
			Vector3 tmp = Vector3.zero;
			tmp.x = cameraController.GetCameraWidth ();
			tmp.y = cameraController.GetCameraHeight ();
			tmp.x *= 5;  // Make it 5 times wider 
			transform.localScale = tmp;

			Vector3 tmp2 = Vector3.zero;
			tmp2.x = cameraController.GetCameraWidth () * 2; // Move it 2 camerawidths to the right. 
			transform.position = tmp2; 
	}

	private void CreateFrames(){   
		if (FrameDictionary.Count == 0) { // Only create frames if thye do not exist
			
			float width = cameraController.GetCameraWidth (); 
			Vector3 framePosition = Vector3.zero;
			framePosition.x = cameraStartPosX;   // The first frame will have the position of 0. 

			for (int i = 1; i < numberOfFrames + 1; i++) { 
				Frame frame = new Frame (i, framePosition, width);   
				FrameDictionary.Add (i, frame);   // Add the frame to the Dictionary that holds the frames. 
				framePosition.x += width;  
			}
		}
	}

	private void AddFrameContents() {
		// Clone the original list, so that we can modify the new one without losing data in the original
		listClone =  new List<string>(obstacleList); 

		// Shuffle the list so the objects get a random placement 
		ShuffleElements (listClone);  // TODO - does this even shuffle properly?!

		// Add tree to the first frame 
		if (FrameDictionary.ContainsKey (1)) {
			//FrameDictionary [1].AddContent(Obstacles.smallTree, 2);   
		}

		// Add obstacles to frame 2, 3 and 4.  - the last frame will be added separately later in update
		for(int i = 2; i < FrameDictionary.Count; i++){
			if (FrameDictionary.ContainsKey (i)) {  // If the frame exists in the dictionary
				FrameDictionary[i].AddContent(SubtractObstacleFromList (), SubtractObstacleFromList ());
			}
		}
	}
		
	private string SubtractObstacleFromList() {
		string obstacle = listClone [0].ToString (); // Have to make it another string so we can use it in the switch statement after it get's deleted from the list. 
		listClone.RemoveAt (0);
		return obstacle;
	}

	public float DefineLoopStartPositionX(Vector3 playerTransform) { 
		return playerTransform.x  - (cameraController.GetCameraWidth () * 3f) ;  // Move the player back 3 frames when the player reaches frame 5
	}

	public float GetLoopEndPositionX() {
		return this.loopEndPosX;
	}

	private void ShuffleElements(List<string> list) {  // http://stackoverflow.com/a/5383533/4178864
		System.Random rnd = new System.Random ();
		int n = list.Count;
		while (n > 1) {
			int k = (rnd.Next(0, n) % n);
			n--;
			string value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}