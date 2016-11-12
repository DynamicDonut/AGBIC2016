using UnityEngine;
using System.Collections;

public class GM : MonoBehaviour {

	public float[] spd_IncTimeSet = new float[2]; //0 = Spd1 to Spd2 time, 1 = Spd2 to Spd3 time
	public float[] spd_DecTimeSet = new float[2]; //0 = Spd3 to Spd2 time, 1 = Spd2 to Spd1 time
	public float[] spdSet = new float[3]; //0 = Spd1, 1 = Spd2, 2 = Spd3

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
