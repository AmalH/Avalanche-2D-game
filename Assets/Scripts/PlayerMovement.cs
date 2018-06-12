using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public Camera playerCamera;
	
	public float speed = 0;
    public float jumpSpeed = 8.0F;
    public float gravity = 1.0F;
	Vector3 lastPosition = Vector3.zero;

	public bool grounded;
	public bool push;
	public bool playing;
	public bool gameOver;
	public bool paused;

	private float startTime;

	public Text scoreContent;
	public Text TimeUi;	
	public Text tapToPlayStatic;
	public Canvas gameoverCanvas;
	public Canvas inGameCanvas;
	public Canvas inPause;
	public Button pauseButton;
	public Text tap_to_restart_static;
	public GameObject bodyplayer;
	public Text Speed_text;

	public AudioSource intro;
	public AudioSource ski_sound;
	public AudioSource inGame;

	private Rigidbody rb;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

		push = false;
		grounded = false;

        playerCamera.transparencySortMode = TransparencySortMode.Orthographic;

		rb = GetComponent<Rigidbody>();

		//limit rotations
		rb.maxAngularVelocity = 3f;

		//add listeners
		pauseButton.GetComponent<Button>().onClick.AddListener(pauseGame);
    }

	void Awake(){
		startBlinking ();
		playing = false;
		gameOver = false;
		paused = false;
	}

	void taptoplay(){
		if(Input.touchCount > 0 || Input.GetMouseButton(0)) {
			stopBlinking ();
			GetComponent<Renderer> ().enabled = true;
			bodyplayer.GetComponent<Renderer>().enabled=true;
			rb.useGravity = true;
			playing = true;
			startTime = Time.time;
			inGameCanvas.enabled = true;
			inGame.Play ();
		}
	}

	void gameisOver(){
		Input.ResetInputAxes();
		if (gameoverCanvas.enabled != true) {
			inGame.Pause ();
			gameoverCanvas.enabled = true;
			startGameOverBlinking ();
			startTime = Time.time;
		}

		if((Input.touchCount > 0 || Input.GetMouseButton(0))&& Time.time-startTime>2.5f) {
			stopGameOverBlinking ();
			Application.LoadLevel (Application.loadedLevel);
			playing = false;
			intro.Play ();
			playing = true;
			startTime = Time.time;
			inGameCanvas.enabled = true;
		}
	}

    void Update()
    {    
		if (!playing)
			taptoplay ();
		else if (gameOver)
			gameisOver ();
		else if(!paused) {
			//check for intro playing(debug)
			if(intro.isPlaying)
				intro.Stop ();


			//calculate speed
			speed = (transform.position - lastPosition).magnitude;
			lastPosition = transform.position;
			//print ("speed : " + speed);

			//push when stopped
			if (push && grounded)
				rb.AddForce (new Vector3 (400, 0, 0));
			if (speed > 0.8f)
				push = false;
			if (speed < 0.4f)
				push = true;

			//jump
				//rb.velocity = new Vector3 (0, 30, 0);

			//After we move, adjust the camera to follow the player
			playerCamera.transform.position = new Vector3 (transform.position.x,
				transform.position.y,
				playerCamera.transform.position.z);

			//player controls
			controll ();

			//update ui
			ui_update ();
		}
	}

	void controll(){
		if (Input.touchCount > 0)
		{
			var touch = Input.GetTouch(0);
			if (touch.position.x < Screen.width/2)
			{
				rb.AddTorque(new Vector3(0,0,7000*Time.deltaTime));

			}
			else if (touch.position.x > Screen.width/2)
			{
				rb.AddTorque(new Vector3(0,0,-7000*Time.deltaTime));
			}
		}
	}

	void ui_update(){
		TimeUi.text = (Time.time-startTime).ToString ("0:0");
		scoreContent.text = ((int) ((Time.time-startTime)*0.3)).ToString();
		Speed_text.text = ((int)(speed * 20)).ToString ();
	}

	void OnCollisionEnter(Collision collision){
		grounded = true;
		ski_sound.Play ();
	}

	void OnCollisionExit(Collision collision){
		grounded = false;
		ski_sound.Pause ();
	}

	IEnumerator Blink_tap_to_play(){

		while (true) {
			switch (tapToPlayStatic.color.a.ToString ()) {
			case "0":
				tapToPlayStatic.color = new Color (tapToPlayStatic.color.r, tapToPlayStatic.color.g, tapToPlayStatic.color.b, 1);
				yield return new WaitForSeconds (0.5f);
				break;
			case "1":
				tapToPlayStatic.color = new Color (tapToPlayStatic.color.r, tapToPlayStatic.color.g, tapToPlayStatic.color.b, 0);
				yield return new WaitForSeconds (0.5f);
				break;

			}
		}

	}

	IEnumerator Blink_tap_to_restart(){

		while (true) {
			switch (tap_to_restart_static.color.a.ToString ()) {
			case "0":
				tap_to_restart_static.color = new Color (tap_to_restart_static.color.r, tap_to_restart_static.color.g, tap_to_restart_static.color.b, 1);
				yield return new WaitForSeconds (0.5f);
				break;
			case "1":
				tap_to_restart_static.color = new Color (tap_to_restart_static.color.r, tap_to_restart_static.color.g, tap_to_restart_static.color.b, 0);
				yield return new WaitForSeconds (0.5f);
				break;

			}
		}

	}

	void startBlinking(){
		StopCoroutine ("Blink_tap_to_play");
		StartCoroutine ("Blink_tap_to_play");
	}

	void stopBlinking(){
		StopCoroutine ("Blink_tap_to_play");
		tapToPlayStatic.color = new Color (tapToPlayStatic.color.r, tapToPlayStatic.color.g, tapToPlayStatic.color.b, 0);
	}

	void startGameOverBlinking(){
		StopCoroutine ("Blink_tap_to_restart");
		StartCoroutine ("Blink_tap_to_restart");
	}

	void stopGameOverBlinking(){
		StopCoroutine ("Blink_tap_to_restart");
		tap_to_restart_static.color = new Color (tap_to_restart_static.color.r, tap_to_restart_static.color.g, tap_to_restart_static.color.b, 0);
	}

	public void pauseGame(){
		paused = !paused;
		if (paused) {
			inPause.enabled = true;
			Time.timeScale = 0;

		} else {
			Time.timeScale = 1;
			inPause.enabled = false;
		}
			
	}

}
