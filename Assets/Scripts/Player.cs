using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// configurables
	public float speed = 7f;
	public float acceleration = 0.75f;
	public float jump = 10f;
	public float inputBuffer = 0.05f;
	public bool canDoubleJump = true;
	public bool mirrorWhenTurning = true;

	// physics
	private Rigidbody2D body;
	public Transform groundCheck;
	public LayerMask groundLayer;
	public float groundCheckRadius = 0.2f;
	public float wallCheckDistance = 1f;
	public bool checkForEdges = false;
	private float groundAngle = 0;

	// flags
	private bool canControl = true;
	private bool running = false;
	private bool grounded = false;
	private bool doubleJumped = false;

	// misc
	private float jumpBufferedFor = 0;

	// particles
	public GameObject jumpParticles, landParticles;

	// sound stuff
	private AudioSource audioSource;
	public AudioClip jumpClip, landClip;

	// animations
	private Animator anim;
	public Transform face;
	private Vector3 facePos;

	private Machine machine, saveMachine;
	public GameObject[] letters;

	private RoomCamera cam;

	// ###############################################################

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D> ();
		audioSource = GetComponent<AudioSource> ();
		anim = GetComponentInChildren<Animator> ();

		cam = Camera.main.GetComponent<RoomCamera> ();

		facePos = face.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown (KeyCode.Escape)) {
			Die ();
		}

		float faceMove = 0.1f;
		float fx = Mathf.PerlinNoise (Time.time * 0.5f, 0f) - 0.5f;
		float fy = Mathf.PerlinNoise (Time.time * 0.5f, 1000f) - 0.5f;
		face.localPosition = facePos + new Vector3 (fx * faceMove, fy * faceMove, 0);

		LetterSpawning ();

		bool wasGrounded = grounded;

		if (!checkForEdges) {
			grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);

			// draw debug lines
			Color debugLineColor = grounded ? Color.green : Color.red;
			Debug.DrawLine (transform.position, groundCheck.position, debugLineColor, 0.2f);
			Debug.DrawLine (groundCheck.position + Vector3.left * groundCheckRadius, groundCheck.position + Vector3.right * groundCheckRadius, debugLineColor, 0.2f);
		} else {
			grounded = Physics2D.Raycast (transform.position, Vector2.down, 1f);

			// draw debug lines
			Color debugLineColor = grounded ? Color.green : Color.red;
			Debug.DrawRay (transform.position, Vector2.down, debugLineColor, 0.2f);
		}

		// just landed
		if (!wasGrounded && grounded) {
			Land ();
		}

		// just left the ground
		if (wasGrounded && !grounded) {
			groundAngle = 0;
		}

		// jump buffer timing
		if (jumpBufferedFor > 0) {
			jumpBufferedFor -= Time.deltaTime;
		}

		// controls
		if (canControl) {

			float inputDirection = Input.GetAxis("Horizontal");

			// jump
			if ((grounded || (canDoubleJump && !doubleJumped)) && (Input.GetButtonDown("Jump") || jumpBufferedFor > 0)) {

				body.velocity = new Vector2 (body.velocity.x, 0); // reset vertical speed

				if (!grounded) {
					doubleJumped = true;
				}

				jumpBufferedFor = 0;

				AudioManager.Instance.PlayEffectAt(0, transform.position);

				// jump sounds
				if (audioSource && jumpClip) {
					audioSource.PlayOneShot (jumpClip);
				}

				// jump particles
				if (jumpParticles) {

					GameObject go = Instantiate (jumpParticles, transform);
					ParticleSystem ps = go.GetComponent<ParticleSystem> ();
					ParticleSystem.MainModule mm = ps.main;
					ParticleSystem.ShapeModule sm = ps.shape;

					mm.startRotationZ = inputDirection * 0.4f;
					sm.radius = Random.Range (0.3f, 0.7f); 
					sm.radiusSpread = Random.Range(0.1f, 0.4f);
				}

				// animation
				if (anim) {
					anim.speed = 1f;
					anim.SetTrigger ("jump");
					anim.ResetTrigger ("land");
				}

				body.AddForce (Vector2.up * jump, ForceMode2D.Impulse);

			} else if (canControl && Input.GetButtonDown("Jump")) {
			
				// jump command buffering
				jumpBufferedFor = 0.2f;
			}

			// moving
			Vector2 moveVector = new Vector2 (speed * inputDirection, body.velocity.y);

			if (Mathf.Sign (body.velocity.x) == Mathf.Sign (moveVector.x)) {
				body.velocity = Vector2.MoveTowards (body.velocity, moveVector, acceleration);
			} else {
				body.velocity = moveVector;
			}

			// direction
			if (mirrorWhenTurning && Mathf.Abs(inputDirection) > inputBuffer) {

				float dir = Mathf.Sign (inputDirection);
				transform.localScale = new Vector2 (dir, 1);

//				Transform sprite = transform.Find("Character");
//				Vector3 scl = sprite.localScale;
//				scl.x = dir;
//				sprite.localScale = scl;

//				transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 90f - dir * 90f, transform.localEulerAngles.z);
			}

			Vector2 p = transform.position + Vector3.right * inputDirection * wallCheckDistance + Vector3.down * 0.5f;
			bool wallHug = Physics2D.OverlapCircle (p, groundCheckRadius, groundLayer);
			Color hugLineColor = grounded ? Color.green : Color.red;
			Debug.DrawLine (transform.position, p, hugLineColor, 0.2f);

			running = inputDirection < -inputBuffer || inputDirection > inputBuffer;

			if (wallHug && !checkForEdges) {
				body.velocity = new Vector2 (0, body.velocity.y);
				running = false;
			}

			if (Physics2D.OverlapCircle (transform.position + Vector3.up * 0.5f, groundCheckRadius, groundLayer)) {
				Die ();
			}

			if (!grounded) {
				running = false; 
			}

			if (anim) {

				anim.SetBool ("running", running);

				if (running) {
					anim.speed = Mathf.Abs (body.velocity.x * 0.18f);
					anim.SetFloat ("speed", Mathf.Abs(body.velocity.x));
				} else {
					anim.speed = 1f;
					anim.SetFloat ("speed", 0);
				}
			}
		}
	}

	private void Land() {

		doubleJumped = false;

		AudioManager.Instance.PlayEffectAt(1, transform.position);

		// landing sound
		if (audioSource && landClip) {
			audioSource.PlayOneShot (landClip);
		}

		// landing particles
		if (landParticles) {
			Instantiate (landParticles, groundCheck.position + Vector3.down * 0.5f, Quaternion.identity);
		}

		// animation
		if (anim) {
			anim.speed = 1f;
			anim.SetTrigger ("land");
		}
	}

	public bool IsGrounded() {
		return grounded;
	}

	void OnCollisionStay2D(Collision2D coll) {
		groundAngle = Mathf.Atan2(coll.contacts [0].normal.y, coll.contacts [0].normal.x) * Mathf.Rad2Deg - 90;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		groundAngle = Mathf.Atan2(coll.contacts [0].normal.y, coll.contacts [0].normal.x) * Mathf.Rad2Deg - 90;

		if (coll.gameObject.tag == "DeathBall") {

			Die ();

			EffectManager.Instance.AddEffect (0, coll.transform.position);
			Destroy (coll.gameObject);
		}

		if (Mathf.Abs(coll.relativeVelocity.y) > 12f) {
			cam.Shake (-coll.relativeVelocity * 0.01f);
		}
	}

	private void Die() {

		cam.Chromate (0.05f);
		cam.Shake (0.2f, 0.1f);

		EffectManager.Instance.AddEffect (1, transform.position);
		EffectManager.Instance.AddEffect (3, transform.position);

		AudioManager.Instance.PlayEffectAt(5, transform.position);
		AudioManager.Instance.PlayEffectAt(13, transform.position, 0.3f);
		TryRespawn ();
	}

	public float GetGroundAngle() {
		if (Mathf.Abs (groundAngle) > 90) {
			groundAngle = 0;
		}
		return groundAngle;
	}

	void OnTriggerEnter2D(Collider2D trigger) {
		if (trigger.tag == "Room") {
			trigger.GetComponent<Room> ().Focus ();
		}

		if (trigger.tag == "Machine") {

			if (machine) {
				machine.Deactivate ();
			}

			machine = trigger.GetComponent<Machine> ();
			machine.Activate ();

			if (machine.isCheckpoint) {
				saveMachine = machine;
			}
		}

		if (trigger.tag == "Water") {
			cam.Shake (0.15f, 0.1f);
			cam.Chromate (0.04f);

			EffectManager.Instance.AddEffect (2, transform.position);

			AudioManager.Instance.PlayEffectAt(4, transform.position);
			AudioManager.Instance.PlayEffectAt(12, transform.position, 0.3f);
			TryRespawn ();
		}
	}

	void TryRespawn() {
		if (saveMachine) {

			gameObject.SetActive (false);

			if (saveMachine.respawns > 0) {
				saveMachine.respawns--;
				saveMachine.ShowNumber ();
				Invoke ("Respawn", 1f);

				GameManager.Instance.colorValue = saveMachine.respawns * 0.1f;

			} else {
				GameManager.Instance.ShowEnd ("GAME OVER");
			}
		}
	}

	void Respawn() {
		gameObject.SetActive (true);
		transform.position = saveMachine.transform.position + Vector3.up * 2f;

		EffectManager.Instance.AddEffect (6, transform.position);
		AudioManager.Instance.PlayEffectAt(3, transform.position);
	}

	void LetterSpawning() {

		bool didSpawn = false;
		
		if (machine && Input.GetKeyDown (KeyCode.A)) {
			didSpawn = machine.SpawnLetter (letters [0]);
		}

		if (machine && Input.GetKeyDown (KeyCode.B)) {
			didSpawn = machine.SpawnLetter (letters [1]);
		}

		if (machine && Input.GetKeyDown (KeyCode.C)) {
			didSpawn = machine.SpawnLetter (letters [2]);
		}

		if (machine && Input.GetKeyDown (KeyCode.D)) {
			didSpawn = machine.SpawnLetter (letters [3]);
		}

		if (machine && Input.GetKeyDown (KeyCode.E)) {
			didSpawn = machine.SpawnLetter (letters [4]);
		}

		if (machine && Input.GetKeyDown (KeyCode.F)) {
			didSpawn = machine.SpawnLetter (letters [5]);
		}

		if (machine && Input.GetKeyDown (KeyCode.G)) {
			didSpawn = machine.SpawnLetter (letters [6]);
		}

		if (machine && Input.GetKeyDown (KeyCode.H)) {
			didSpawn = machine.SpawnLetter (letters [7]);
		}

		if (machine && Input.GetKeyDown (KeyCode.I)) {
			didSpawn = machine.SpawnLetter (letters [8]);
		}

		if (machine && Input.GetKeyDown (KeyCode.J)) {
			didSpawn = machine.SpawnLetter (letters [9]);
		}

		if (machine && Input.GetKeyDown (KeyCode.K)) {
			didSpawn = machine.SpawnLetter (letters [10]);
		}

		if (machine && Input.GetKeyDown (KeyCode.L)) {
			didSpawn = machine.SpawnLetter (letters [11]);
		}

		if (machine && Input.GetKeyDown (KeyCode.M)) {
			didSpawn = machine.SpawnLetter (letters [12]);
		}

		if (machine && Input.GetKeyDown (KeyCode.N)) {
			didSpawn = machine.SpawnLetter (letters [13]);
		}

		if (machine && Input.GetKeyDown (KeyCode.O)) {
			didSpawn = machine.SpawnLetter (letters [14]);
		}

		if (machine && Input.GetKeyDown (KeyCode.P)) {
			didSpawn = machine.SpawnLetter (letters [15]);
		}

		if (machine && Input.GetKeyDown (KeyCode.Q)) {
			didSpawn = machine.SpawnLetter (letters [16]);
		}

		if (machine && Input.GetKeyDown (KeyCode.R)) {
			didSpawn = machine.SpawnLetter (letters [17]);
		}

		if (machine && Input.GetKeyDown (KeyCode.S)) {
			didSpawn = machine.SpawnLetter (letters [18]);
		}

		if (machine && Input.GetKeyDown (KeyCode.T)) {
			didSpawn = machine.SpawnLetter (letters [19]);
		}

		if (machine && Input.GetKeyDown (KeyCode.U)) {
			didSpawn = machine.SpawnLetter (letters [20]);
		}

		if (machine && Input.GetKeyDown (KeyCode.V)) {
			didSpawn = machine.SpawnLetter (letters [21]);
		}

		if (machine && Input.GetKeyDown (KeyCode.W)) {
			didSpawn = machine.SpawnLetter (letters [22]);
		}

		if (machine && Input.GetKeyDown (KeyCode.X)) {
			didSpawn = machine.SpawnLetter (letters [23]);
		}

		if (machine && Input.GetKeyDown (KeyCode.Y)) {
			didSpawn = machine.SpawnLetter (letters [24]);
		}

		if (machine && Input.GetKeyDown (KeyCode.Z)) {
			didSpawn = machine.SpawnLetter (letters [25]);
		}

		if (didSpawn) {
			anim.ResetTrigger ("cast");
			anim.SetTrigger ("cast");
			EffectManager.Instance.AddEffect (6, transform.position);
		}
	}
}
