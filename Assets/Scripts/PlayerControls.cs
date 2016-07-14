using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
	//-----------------------0-----1-----2-------3--------4------5---------6-------7----

	float sTime_Spd, sTime_Slide, sTime_inputIgnore;
	public float[] spd_IncTimeSet = new float[2]; //0 = Time to Fast2, 1 = Time to Fast3
	public float[] spd_DecTimeSet = new float[2]; //0 = Time to Fast2, 1 = Time to Fast1
	public float[] spdSet = new float[3]; //0 = Fast1, 1 = Fast2, 2 = Fast3
	float mySpd;
	public int currSpd;

	float hAxis, vAxis, defSpd, maxSpd, angle;
	Animator anim;
	public float stickDZone = 0.5f;
	public float slide_length, inputIgnore_length;
	public string myName;
	public bool isMoving = false;
	public bool isSliding = false;
	public bool canMove = true;
	bool ignoreInput = false;
	Vector3 currPos, lastPos;
	public Direction myDir = Direction.Up;
	Direction wrongDir;

	void Start () {
		lastPos = transform.position;
		currSpd = 0;
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate () {
		Move ();
		SpeedManagement ();
		PlayerActions ();
		anim.SetInteger ("Player Direction", (int)myDir);
		anim.SetBool ("isMoving", isMoving);
		anim.SetBool ("isSliding", isSliding);
	}

	void Move (){
		//-------CHARACTER ROTATION-------//
		SpriteRotation();

		//-------CHARACTER MOVEMENT-------//
		currPos = transform.position;
		if (canMove) {
			if (currPos != lastPos) {
				if (!isMoving) {
					sTime_Spd = Time.time;
				}
				isMoving = true;
			} else {
				isMoving = false;
				currSpd = 0;
			}
		}
		if (canMove) {
			mySpd = spdSet [currSpd];
		}
		if (Time.time > sTime_inputIgnore + inputIgnore_length) {
			ignoreInput = false;
		}
		transform.position = new Vector3 (transform.position.x + hAxis * mySpd, transform.position.y + vAxis * mySpd);
	}

	void PlayerActions(){
		//Sliding
		if (Input.GetButtonUp ("Fire1")) {
			if (!isSliding) {
				sTime_Slide = Time.time;
			}
			isSliding = true;
		}
		if (Time.time > sTime_Slide + slide_length){
			if (isSliding) {
				sTime_Spd = Time.time;
				currSpd = 0;
			}
			isSliding = false;
		}
	}

	void SpriteRotation(){
		if (!isSliding) {
			if (!ignoreInput) {
				if (Input.GetAxis ("Horizontal") > 0) {
					myDir = Direction.Right;
					GetComponent<SpriteRenderer> ().flipX = false;
				} else if (Input.GetAxis ("Horizontal") < 0) {
					myDir = Direction.Left;
					GetComponent<SpriteRenderer> ().flipX = true;
				}

				if (Input.GetAxis ("Vertical") > 0) {
					if (Input.GetAxis ("Horizontal") > 0) {
						myDir = Direction.UpRight;
					} else if (Input.GetAxis ("Horizontal") < 0) {
						myDir = Direction.UpLeft;
					} else {
						myDir = Direction.Up;
					}
				} else if (Input.GetAxis ("Vertical") < 0) {
					if (Input.GetAxis ("Horizontal") > 0) {
						myDir = Direction.DownRight;
					} else if (Input.GetAxis ("Horizontal") < 0) {
						myDir = Direction.DownLeft;
					} else {
						myDir = Direction.Down;
					}				
				}
				ignoreInput = true;
				sTime_inputIgnore = Time.time;
			}

			if (!canMove && myDir != wrongDir) {
				canMove = true;
				isMoving = true;
				mySpd = spdSet [currSpd];
			}
		}
	}
		
	void SpeedManagement(){
		if (isMoving) {
			if (Time.time > sTime_Spd + spd_IncTimeSet[Mathf.Clamp(currSpd-1, 0, spdSet.Length)] && currSpd < 2){
				sTime_Spd += spd_IncTimeSet[Mathf.Clamp(currSpd-1, 0, spdSet.Length)];
				if (currSpd < 2) {
					currSpd++;
				} else {
					currSpd = 2;
				}
			}
		}
		vAxis = 0f; hAxis = 0f;

		if (myDir == Direction.Up) {
			vAxis = 1f;
		} else if (myDir == Direction.Down) {
			vAxis = -1f;
		} 

		if (myDir == Direction.Left) {
			hAxis = -1f;
		} else if (myDir == Direction.Right) {
			hAxis = 1f;
		} 

		if (myDir == Direction.UpLeft) {
			vAxis = 1f; hAxis = -1f;
		}
		if (myDir == Direction.UpRight) {
			vAxis = 1f; hAxis = 1f;
		}
		if (myDir == Direction.DownLeft) {
			vAxis = -1f; hAxis = -1f;
		}
		if (myDir == Direction.DownRight) {
			vAxis = -1f; hAxis = 1f;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "NonMovable") {
			//Debug.Log ("nooooooo");
			wrongDir = myDir;
			canMove = false;
			isMoving = false;
			mySpd = 0f;
		}
	}
}
