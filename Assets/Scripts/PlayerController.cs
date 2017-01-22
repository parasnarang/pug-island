using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	public bool controlEnabled = true;
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	private Vector3 moveDirection = Vector3.zero;
	public GameObject pug;
	public Canvas canvas;
	public Canvas startScreen;
	public Canvas scoreScreen;
	public Canvas raftReadyScreen;
	public GameObject deathZone;
	public ParticleSystem plankShower;
	public int woodCount = 0;
	public Text txtWoodCount;
	public Button btnPlayAgain;
	public Button btnPlayAgain2;
	public AudioClip dogBark;
	public AudioClip woodBreak;
	public GameObject[] raftStages;
	public GameObject RaftParent;
	public Button btnStartGame;
	public GameObject fire;

	private AudioSource _audio;
	private Rigidbody playerRigidBody;
	float distToGround;

	private Animator anim;

	float timer;

	void Start () {
		startScreen.gameObject.SetActive (true);
		showAndroidControls (false);
		playerRigidBody = gameObject.GetComponent<Rigidbody>();
		timer = 0f;
		btnPlayAgain.onClick.AddListener(PlayAgain);
		btnPlayAgain2.onClick.AddListener(PlayAgain);
		btnStartGame.onClick.AddListener(StartGame);
		_audio = gameObject.GetComponent<AudioSource> ();
		anim = transform.FindChild ("pug").GetComponent<Animator> ();
		Time.timeScale = 0.0f;
		RaftParent.GetComponent<SphereCollider> ().enabled = false;
	}

	void showAndroidControls(bool show){
		Renderer[] renderers = GameObject.Find ("MobileSingleStickControl").GetComponentsInChildren<Renderer>();
		foreach (Renderer r in renderers){
			r.enabled = show;
		}
	}

	bool isGrounded() {
		bool hit = Physics.Raycast (transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.5f);
		return hit;
	}

	void StartGame() {
		startScreen.gameObject.SetActive (false);
		scoreScreen.gameObject.SetActive (true);
		fire.GetComponent<AudioSource> ().Play ();
		Time.timeScale = 1.0f;
		if (Application.platform == RuntimePlatform.Android) {
			showAndroidControls (true);
		} else {
			showAndroidControls (false);
		}
	}

	void PlayAgain() {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		controlEnabled = true;
		Time.timeScale = 1.0f;
	}

	private GameObject[] raftPcs = new GameObject[5];
	void FixedUpdate() {
		timer += Time.deltaTime;
	
		if (controlEnabled) {
			if (Application.platform == RuntimePlatform.Android) {
				moveDirection = new Vector3 (CrossPlatformInputManager.GetAxisRaw ("Horizontal"), 0, CrossPlatformInputManager.GetAxisRaw ("Vertical"));
			} else {
				moveDirection = new Vector3 (Input.GetAxisRaw ("Horizontal"), 0, Input.GetAxisRaw ("Vertical"));
			}
	
			moveDirection = moveDirection.normalized * speed;
			if (moveDirection != Vector3.zero) {
				pug.transform.forward = Vector3.Lerp(pug.transform.forward, moveDirection, Time.deltaTime);
			}

			if (((Application.platform == RuntimePlatform.Android && CrossPlatformInputManager.GetButton ("Jump")) || Input.GetButton ("Jump")) && isGrounded ()) {
				playerRigidBody.velocity = new Vector3 (0, jumpSpeed * Time.deltaTime, 0);
				_audio.PlayOneShot (dogBark);

			}

			playerRigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);
		}

		if (!isGrounded ()) {
			anim.SetBool ("isJumping", true);
		} else {
			anim.SetBool ("isJumping", false);
		}

		if (moveDirection != Vector3.zero) {
			anim.SetBool ("isRunning", true);
		} else {
			anim.SetBool ("isRunning", false);
		}

		if (controlEnabled && Input.GetMouseButtonDown (0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit;
			if (controlEnabled && Physics.Raycast (camRay, out floorHit, 100f)) {
				Vector3 playerToMouse = floorHit.point - transform.position;
				playerToMouse.y = 0f;
				pug.transform.forward = playerToMouse;
			}

			RaycastHit hitInfo;
			if(Physics.Raycast(transform.position, pug.transform.forward.normalized - new Vector3(0, 0.1f, 0), out hitInfo, 7.0f)) {
				if(hitInfo.collider.tag == "crate") {
					timer = 0f;
					_audio.PlayOneShot (woodBreak, 0.1f);
					Destroy (hitInfo.collider.transform.gameObject);
					plankShower.transform.position = hitInfo.collider.transform.position;
					plankShower.transform.gameObject.SetActive (true);
					GameObject raftPc = Instantiate (raftStages [woodCount], RaftParent.transform.position, Quaternion.identity) as GameObject;
					raftPc.transform.parent = RaftParent.transform;
					raftPcs [woodCount] = raftPc;
					if (woodCount > 0) {
						Destroy (raftPcs [woodCount - 1]);
					}
					woodCount++;
					txtWoodCount.text = string.Concat("Wood Collected: ", woodCount.ToString(), "/5");


					if (woodCount >= 5) {
//						Debug.Log ("you win");
//						victoryScreen.gameObject.SetActive (true);
						raftReadyScreen.gameObject.SetActive(true);
						RaftParent.GetComponent<SphereCollider> ().enabled = true;
						timer = 0f;
//						Time.timeScale = 0;
					}

				}
			}
		}

		if (timer >= 2f) {
			plankShower.transform.gameObject.SetActive (false);
		}

		if (timer >= 5f) {
			raftReadyScreen.gameObject.SetActive(false);
		}
	}
}
