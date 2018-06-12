using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class body_script : MonoBehaviour {

	public GameObject skate;
	public AudioSource crash;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		Physics.gravity = new Vector3(0, -10.0F, 0);
		GetComponent<Rigidbody>().centerOfMass = new Vector3 (0, -5f, 0);
	}

	void OnCollisionEnter(Collision collision)
	{
		if(!crash.isPlaying)
			crash.Play ();
		skate = GameObject.Find ("eskimo_snowboard");
		PlayerMovement ps = (PlayerMovement) skate.GetComponent (typeof(PlayerMovement));
		ps.gameOver = true;
	}
}
