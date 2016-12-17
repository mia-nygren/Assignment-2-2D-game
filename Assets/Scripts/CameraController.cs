using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour {
	private Transform playerTransform;
	private float offsetX;

	void Start () {
		playerTransform = GameObject.FindGameObjectWithTag ("Player").transform;
		offsetX = transform.position.x - playerTransform.position.x;
	}

	void Update () {
		Vector3 updatedCameraPosition = transform.position;
		updatedCameraPosition.x = playerTransform.position.x + offsetX;
		transform.position = updatedCameraPosition;
	}

	public float GetCameraWidth(){   // TODO - figure out how to access camera in another way...?
		// Calculate how wide the camera is in Units
		float camHeight = 2f * Camera.main.orthographicSize;
		float camWidth = camHeight * Camera.main.aspect;
		return camWidth; 
	}

	public float GetCameraHeight() {
		// returns how high the camera is in Units
		return 2f * Camera.main.orthographicSize;
	}

	public float GetCameraOffset() {
		return this.offsetX;
	}

	public bool IsOnFrame(Frame frame) {
		if (this.transform.position.x >= frame.GetFramePosition().x && this.transform.position.x < frame.GetFramePosition().x + GetCameraWidth() /2 )
			return true;
		return false;
	}
}
