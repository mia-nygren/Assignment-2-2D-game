using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowCamera : MonoBehaviour {
	//public List<GameObject> objects= new List<GameObject>();
	public GameObject[] objects;

	// Update is called once per frame
	void Update () {
		foreach (GameObject obj in objects) {   // note to self - om de hackar testa FixedUpdate
			UpdateTransform (obj);
		}
	}

	private void UpdateTransform(GameObject obj){
		Vector3 tmp = obj.transform.position;
		tmp.x = this.transform.position.x;
		obj.transform.position = tmp; 
	}
}