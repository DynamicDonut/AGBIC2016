using UnityEngine;
using System.Collections;

public class NewControlSetup : MonoBehaviour {
	//boundNum = distance of the boundary for the Clamp
	public float boundNum, deadZone, spd;
	public string myName = "P1";
	public Color myColor = Color.red;

	//sTime = start time, lTime = length time
	float sTime_Slide;
	public float lTime_Slide;

	Vector3 lastDir; Animator anim; GM myGM;
	public bool isMoving, isSliding;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = myColor;
		myGM = GameObject.Find ("GManager").GetComponent<GM> ();

		if (myName.EndsWith (1.ToString())) {
			lastDir = Vector3.down;
		} else if (myName.EndsWith (2.ToString())) {
			lastDir = Vector3.left;
		} else if (myName.EndsWith (3.ToString())) {
			lastDir = Vector3.right;
		} else if (myName.EndsWith (4.ToString())) {
			lastDir = Vector3.up;
		} 
		isSliding = isMoving = false;
		anim = GetComponent<Animator> ();
	}

	void Update(){
		Movement ();
		anim.SetFloat ("DirX", lastDir.x);
		anim.SetFloat ("DirY", lastDir.y);
		anim.SetBool ("isMoving", isMoving);
		anim.SetBool ("isSliding", isSliding);
	}

	// Update is called once per frame
	void Movement() {
		Mathf.Clamp (transform.position.x, -boundNum, boundNum);
		Mathf.Clamp (transform.position.y, -boundNum, boundNum);

		if (lastDir == Vector3.zero) {
			isMoving = false;
		} else {
			isMoving = true;
		}
		//Digital Movement
		Vector3 moveDir = Vector3.zero;
		if (Input.GetAxis ("Horizontal") > deadZone) {
			moveDir += Vector3.right;
			GetComponent<SpriteRenderer> ().flipX = false;
		} else if (Input.GetAxis ("Horizontal") < -deadZone) {
			moveDir -= Vector3.right;
			GetComponent<SpriteRenderer> ().flipX = true;
		}
		if (Input.GetAxis ("Vertical") > deadZone) {
			moveDir += Vector3.up;
		} else if (Input.GetAxis ("Vertical") < -deadZone) {
			moveDir -= Vector3.up;
		}

		if (Input.GetAxis ("Horizontal") == 0 && Input.GetAxis ("Vertical") == 0) {
			transform.position += lastDir.normalized * (spd/10);
		}
		if (moveDir != Vector3.zero){
			lastDir = moveDir;
		}
		transform.position += moveDir.normalized * (spd/10);

		//Sliding
		if (Input.GetButtonDown ("Fire1")) {
			if (!isSliding) {
				sTime_Slide = Time.time;
				isSliding = true;
			}
		}

		if (Time.time > sTime_Slide + lTime_Slide) {
			isSliding = false;
		}
	}

	void SpeedManagement(){
		if (isMoving) {
			
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "NonMovable") {
			lastDir = Vector3.zero;
			isSliding = false;
		}
	}
}
