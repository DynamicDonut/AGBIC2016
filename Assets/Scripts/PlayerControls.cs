using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	float hAxis, vAxis;
	public float spd = 0.1f;
	public string myName;
	public bool isMoving = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Move ();
	}

	void Move (){
		if (Input.GetAxis ("Horizontal") != 0f || Input.GetAxis ("Vertical") != 0f) {
			isMoving = true;
		} else {
			isMoving = false;
		}

		hAxis = Input.GetAxis ("Horizontal") * spd;
		vAxis = Input.GetAxis ("Vertical") * spd;
		transform.position = new Vector3 (transform.position.x + hAxis, transform.position.y + vAxis);
	}
}
