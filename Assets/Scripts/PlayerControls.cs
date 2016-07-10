using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public enum Direction { Up, UpLeft, Left, DownLeft, Down, DownRight, Right, UpRight }
	//-----------------------0-----1-----2-------3--------4------5---------6-------7----
	float sTime_Spd;
	float spd_IncTime = 1.5f; // X seconds to increase speed while running
	float spd_DecTime = 0.5f; // X seconds to decrease speed while running

	float hAxis, vAxis, defSpd, maxSpd, angle;
	Animator anim;
	public float spd = 0.1f;
	public float spdInc = 0.045f;
	public float spdDec = 0.045f;
	public float stickDZone = 0.5f;
	public string myName;
	public bool isMoving = false;
	Vector3 currPos, lastPos;
	public Direction myDir = Direction.Up;

	// Use this for initialization
	void Start () {
		defSpd = spd; // Base speed the player starts at.
		maxSpd = 1.2f;
		lastPos = transform.position;
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
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
		if (Mathf.Abs(Input.GetAxis ("Horizontal")) >= stickDZone|| Mathf.Abs(Input.GetAxis ("Vertical")) >= stickDZone) {
			if (!isMoving) { sTime_Spd = Time.time; }
			if (currPos != lastPos) {
				isMoving = true;
			}
		} else {
			if (isMoving) { sTime_Spd = Time.time; }
			isMoving = false;
		}

		hAxis = Input.GetAxis ("Horizontal") * spd;
		vAxis = Input.GetAxis ("Vertical") * spd;
		transform.position = new Vector3 (transform.position.x + hAxis, transform.position.y + vAxis);
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
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "NonMovable") {
			Debug.Log ("nooooooo");
		}
	}
}
