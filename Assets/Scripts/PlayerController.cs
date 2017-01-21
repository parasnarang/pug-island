using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	private Vector3 moveDirection = Vector3.zero;
	public GameObject pug;
	public Canvas canvas;
	public Canvas victoryScreen;
	public GameObject deathZone;
	public ParticleSystem plankShower;
	public int woodCount = 4;
	public Text txtWoodCount;

	private Rigidbody playerRigidBody;
	float distToGround;

	float timer;

	void Start () {
		playerRigidBody = gameObject.GetComponent<Rigidbody>();
		timer = 0f;
	}

	bool isGrounded() {
		bool hit = Physics.Raycast (transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.1f);
		return hit;
	}

	void FixedUpdate() {
		timer += Time.deltaTime;
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		moveDirection = moveDirection.normalized * speed;
		if (Input.GetButton ("Jump") && isGrounded()) {
			playerRigidBody.velocity = new Vector3(0, jumpSpeed * Time.deltaTime, 0);

		}
		playerRigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			pug.transform.forward = playerToMouse;
		}

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hitInfo;
			if(Physics.Raycast(transform.position, pug.transform.forward.normalized - new Vector3(0, 0.1f, 0), out hitInfo, 10.0f)) {
				if(hitInfo.collider.tag == "crate") {
					timer = 0f;
					Destroy (hitInfo.collider.transform.gameObject);
					plankShower.transform.position = hitInfo.collider.transform.position;
					plankShower.transform.gameObject.SetActive (true);
					woodCount++;
					txtWoodCount.text = string.Concat("Wood Collected: ", woodCount.ToString(), "/5");

					if (woodCount >= 5) {
						Debug.Log ("you win");
						victoryScreen.gameObject.SetActive (true);
						Time.timeScale = 0;
					}

				}
			}
		}

		if (timer >= 2f) {
			plankShower.transform.gameObject.SetActive (false);
		}
	}
}
