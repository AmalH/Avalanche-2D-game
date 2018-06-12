using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax_scrolling : MonoBehaviour {
	Renderer rend;
	public bool paused;

	// Use this for initialization
	void Start () {
		rend = this.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {


	}
}
