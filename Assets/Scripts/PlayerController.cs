using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : ExtendedBehaviour {

	/* Private properties */ 
	private float forwardSpeed = 5.5f;  // Used to move forward left -> right motion
	private float thrust = 40f; 
	private	float distance = 0f;
	private float timer = 0f;
	private float timer2 = 0f;
	private bool playerStartedGame = false;
	private bool pressedKey;
	private bool collision = false;
	private bool gameOver = false;
	private BoxCollider2D boxCollider2D;
	private Rigidbody2D rb;
	private FramesController framesController; 

	/* Public properties */
	public Animator animator;
	public Vector3 velocity = Vector3.zero;
	public Transform respawn;
	public Text distanceText;
	public Text recordText;

	void Awake() {		
		framesController = GameObject.FindGameObjectWithTag ("FramesContainer").GetComponent<FramesController> ();
	}
		
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		boxCollider2D = GetComponent<BoxCollider2D> ();
		rb.gravityScale = 0;  // don't let the player be affected by gravity just yet
		animator = transform.GetComponentInChildren<Animator>();
	}

	// Graphic and input updates should be done here.
	void Update () {
		if (collision) {   // Only true if the player did collide with something.			
			timer2 -= Time.deltaTime;
			if (timer2 < 0) // If the player get's stuck on something - wait a bit then initialize 
				{	
					Initialize ();   
				}
			else if (transform.position.y < -10f) {
					Initialize (); 
			} 
		} else {
			// Check for input here
			if (Input.GetKey (KeyCode.Space) ) {   // With GetKey (instead of GetKeyDown) it also moves up when the space-key is held down
				// Player pressed space - hence started the game 
				if (!playerStartedGame) {
					playerStartedGame = true;
					gameOver = false; 
				}
				pressedKey = true;
			}
		}
	}
	// Physics engine updates in here. 
	void FixedUpdate () {   // Is going to be run multiple times per second around 50 times. 

		if (playerStartedGame && !gameOver) {
			rb.gravityScale = 1; // set the players gravity to 1
			velocity.x = forwardSpeed; // Velocity is the amount of distance the player is going to move in one second.

			if (pressedKey) {
				pressedKey = false;							
				rb.AddForce (Vector2.up *thrust);
				animator.SetTrigger ("Flying");
			}
				
			transform.position += velocity * Time.deltaTime; 
			UpdateDistanceAndSpeed ();

			// Check if the players position reaches the loop-end. 
			LoopPlayer ();
		}
	}

	void OnTriggerEnter2D(Collider2D other) {

		// Check if the polygon collider2D for the player is actually touching the collider2D.
		if (boxCollider2D.IsTouching (other)) {  

			if (other.CompareTag (Obstacles.Tags.Top) || other.CompareTag (Obstacles.Tags.Bottom) || other.CompareTag (Obstacles.Tags.Obstacle)) {
				// Should note be able to press a key anymore hence game over
				gameOver = true;
				collision = true;
				animator.SetBool ("Death", true);
				ResetTimer ();  
			}
		}
	}


	/* Private help methods */ 

	private void Initialize() {
		collision = false;  // Reset collision
		UpdateTexts();
		distance = 0; 
		ResetPlayer (); // Reset the player position to the respawn 
		ResetTimer (); 
		rb.gravityScale = 0; 
		animator.SetBool ("Death", false);
		Wait(1, InitializeDelay );  // So that the user doesn't se a millisecond of framecontent being rendered before the camera moves back to it's startposition
	}	
	private void InitializeDelay() {  
		framesController.Initialize();   // In the canvasControllers initialize method the frames are cleared and new objects are added
		playerStartedGame = false;
		pressedKey = false;
		SetForwardSpeed(5.5f);
		timer = 0f; // I need two timers atm - one for increasing the speed, and one if the player get's stuck.  I know it looks ugly - TODO - refactor or find a better solution!
	}
	
	private void ResetTimer () {
		timer2 =2f;	
	}
	private void SetForwardSpeed(float newSpeed) {  // TODO - forwardspeed is not completaly consistent
		forwardSpeed = newSpeed;
	}

	private void LoopPlayer() {
		// If camera reaches a certain position - move the player to the startposition of the loop ( the loop is defined in frameConrol )

		if (Camera.main.transform.position.x >= framesController.GetLoopEndPositionX ()) {  // TODO - refactor looppositions?
			Vector3 tmp = transform.position;
			tmp.x = framesController.DefineLoopStartPositionX (transform.position); // When the player's position has reached the end of the loop - move it back exactly 3 frames ...
			transform.position = tmp;   // Changing the player's position since the camera follows the player, and not the other way around...
		}  
	}

	public void ResetPlayer() {  
		rb.velocity = Vector3.zero; // Resets the force  
		rb.angularVelocity = 0f;
		transform.rotation = Quaternion.identity;
		transform.position = respawn.position;  // Resets the player's position to the respawn 
	}

	public void UpdateTexts() {
		if(distance > int.Parse(recordText.text))
			recordText.text = distance.ToString();  // Make a new string
		distanceText.text = "Distance: 0";		
	}

	public void UpdateDistanceAndSpeed(){   
		timer += Time.deltaTime;

		if(timer > 0.05f) //http://answers.unity3d.com/answers/467794/view.html
		{
			distance += 1;	
			distanceText.text = "Distance: " + distance.ToString();
			timer -= 0.05f; 

			// Change the speed depending on how many points     
			if (forwardSpeed < 8) {   
				SetForwardSpeed(forwardSpeed + 0.002f );  // Increase the speed a liiittle bit all the time until it reaches a maximum speed
			}
		}
	}
}


	



