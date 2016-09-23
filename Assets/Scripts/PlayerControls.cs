using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

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

	void Start () {
		lastPos = transform.position;
		currSpd = 0;
		anim = GetComponent<Animator> ();
	}
	
	void FixedUpdate () {
		Move ();
		SpeedManagement ();
		PlayerActions ();
		anim.SetBool ("isMoving", isMoving);
		anim.SetBool ("isSliding", isSliding);
		Debug.Log (Input.GetAxis ("Horizontal"));
		Debug.Log (Input.GetAxis ("Vertical"));
	}

	void Move(){
		Vector3 moveDir = Vector3.zero;
		if (Input.GetAxis ("Horizontal") > 0) {
			moveDir += Vector3.right;
			GetComponent<SpriteRenderer> ().flipX = false;
		} else if (Input.GetAxis ("Horizontal") < 0) {
			moveDir -= Vector3.right;
			GetComponent<SpriteRenderer> ().flipX = true;
		}

		if (Input.GetAxis ("Vertical") > 0) {
			moveDir += Vector3.up;
		} else if (Input.GetAxis ("Vertical") < 0) {
			moveDir -= Vector3.up;
		}
		moveDir.Normalize();

		if (canMove) {
			float lastDirX = Input.GetAxis ("Horizontal");
			float lastDirY = Input.GetAxis ("Vertical");

			if (lastDirX != 0 || lastDirY != 0) {
				if (!isMoving) {
					sTime_Spd = Time.time;
				}
				isMoving = true;
			} else {
				isMoving = false;
				currSpd = 0;
			}
			mySpd = spdSet [currSpd];
			MoveAnim (lastDirX, lastDirY);
		}
		transform.position += moveDir * mySpd;
	}

	void PlayerActions(){
		//Sliding
		if (Input.GetButtonDown ("Fire1")) {
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

	void MoveAnim(float myX, float myY) {
		if (myX > 0) {
			anim.SetFloat ("DirX", 1f);
		} else if (myX < 0) {
			anim.SetFloat ("DirX", -1f);
		} else {
			anim.SetFloat ("DirX", 0f);
		}

		if (myY > 0) {
			anim.SetFloat ("DirY", 1f);
		} else if (myY < 0) {
			anim.SetFloat ("DirY", -1f);
		} else {
			anim.SetFloat ("DirY", 0f);
		}

		if (myX <= -0.1f) {
			anim.SetFloat ("LastDirX", -1f);
		} else if (myX >= 0.1f) {
			anim.SetFloat ("LastDirX", 1f);
		} else if (myX == 0 && myY != 0) {
			anim.SetFloat ("LastDirX", 0f);
		}

		if (myY <= -0.1f) {
			anim.SetFloat ("LastDirY", -1f);
		} else if (myY >= 0.1f) {
			anim.SetFloat ("LastDirY", 1f);
		} else if (myY == 0 && myX != 0) {
			anim.SetFloat ("LastDirY", 0f);
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
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "NonMovable") {
			//Debug.Log ("nooooooo");

			canMove = false;
			isMoving = false;
			mySpd = 0f;
		}
	}
}
