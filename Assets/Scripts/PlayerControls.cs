using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
	//-----------------------0-----1-----2-------3--------4------5---------6-------7----
	float sTime_Spd;
	//float spd_IncTime = 1.5f; // X seconds to increase speed while running
	//float spd_DecTime = 0.5f; // X seconds to decrease speed while running

	public float[] spd_IncTimeSet = new float[2]; //0 = Time to Fast2, 1 = Time to Fast3
	public float[] spd_DecTimeSet = new float[2]; //0 = Time to Fast2, 1 = Time to Fast1
	public float[] spdSet = new float[3]; //0 = Fast1, 1 = Fast2, 2 = Fast3
	public int currSpd;

	float hAxis, vAxis, defSpd, maxSpd, angle;
	Animator anim;
	public float stickDZone = 0.5f;
	public string myName;
	public bool isMoving = false;
	Vector3 currPos, lastPos;
	public Direction myDir = Direction.Up;

	void Start () {
		lastPos = transform.position;
		currSpd = 0;
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate () {
		Move ();
		SpeedManagement ();
		anim.SetInteger ("Player Direction", (int)myDir);
		anim.SetBool ("isMoving", isMoving);
	}

	void Move (){
		//-------CHARACTER ROTATION-------//
		/*
		 *  ---- NOT NEEDED UNLESS WE'RE DOING TOP-DOWN SPRITE ROTATION ----
		 * if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
			angle = Mathf.Atan2 (Input.GetAxis ("Horizontal"), Input.GetAxis ("Vertical")) * Mathf.Rad2Deg;
		}
		transform.rotation = Quaternion.Euler(Vector3.forward * angle);
		*/

		SpriteRotation();

		//-------CHARACTER MOVEMENT-------//
		currPos = transform.position;
		if (currPos != lastPos) {
			if (!isMoving) {
				sTime_Spd = Time.time;
			}
			isMoving = true;
		} else {
			isMoving = false;
			currSpd = 0;
		}

		/*
		if (Mathf.Abs(Input.GetAxis ("Horizontal")) >= stickDZone|| Mathf.Abs(Input.GetAxis ("Vertical")) >= stickDZone) {
			if (!isMoving) { sTime_Spd = Time.time; }
			if (currPos != lastPos) {
				isMoving = true;
			}
		} else {
			if (isMoving) { sTime_Spd = Time.time; }
			isMoving = false;
			currSpd = 0;
		}

		hAxis = Input.GetAxis ("Horizontal") * spdSet[currSpd];
		vAxis = Input.GetAxis ("Vertical") * spdSet[currSpd];
		*/
		transform.position = new Vector3 (transform.position.x + hAxis * spdSet[currSpd], transform.position.y + vAxis * spdSet[currSpd]);
	}

	void SpriteRotation(){
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
	}
		
	void SpeedManagement(){
		if (isMoving) {
			/*
			if (Time.time > sTime_Spd + spd_IncTime) {
				sTime_Spd += spd_IncTime;
				if (spd < maxSpd) {
					spd += spdInc;
				} else {
					spd = maxSpd;
				}
			}
		} else {
			if (Time.time > sTime_Spd + spd_DecTime) {
				sTime_Spd += spd_DecTime;
				if (spd > defSpd) {
					spd -= spdDec;
				} else {
					spd = defSpd;
				}
			}
			*/

			if (Time.time > sTime_Spd + spd_IncTimeSet[Mathf.Clamp(currSpd-1, 0, spdSet.Length)] && currSpd < 2){
				sTime_Spd += spd_IncTimeSet[Mathf.Clamp(currSpd-1, 0, spdSet.Length)];
				if (currSpd < 2) {
					currSpd++;
				} else {
					currSpd = 2;
				}
			}
		}

		vAxis = 0f;
		hAxis = 0f;

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
			vAxis = 1f;
			hAxis = -1f;
		}
		if (myDir == Direction.UpRight) {
			vAxis = 1f;
			hAxis = 1f;
		}
		if (myDir == Direction.DownLeft) {
			vAxis = -1f;
			hAxis = -1f;
		}
		if (myDir == Direction.DownRight) {
			vAxis = -1f;
			hAxis = 1f;
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "NonMovable") {
			Debug.Log ("nooooooo");
		}
	}
}
