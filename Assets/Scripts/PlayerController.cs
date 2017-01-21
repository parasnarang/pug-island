using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {
//	CharacterController controller;
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
//	public float gravity = 10.0f;
	private Vector3 moveDirection = Vector3.zero;
	public GameObject pug;
	public Canvas canvas;
	public GameObject deathZone;

//	Collider coll;
	private Rigidbody playerRigidBody;
	float distToGround;

	void Start () {
//		controller = GetComponent<CharacterController>();
		playerRigidBody = gameObject.GetComponent<Rigidbody>();
//		coll = gameObject.GetComponent<Collider>();
	}

	bool isGrounded() {
		bool hit = Physics.Raycast (transform.position + new Vector3(0, 0.1f, 0), Vector3.down, 0.1f);
		return hit;
	}

	void FixedUpdate() {
		moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		moveDirection = moveDirection.normalized * speed;
		if (Input.GetButton ("Jump") && isGrounded()) {
			//moveDirection.y = jumpSpeed;
			playerRigidBody.velocity = new Vector3(0, jumpSpeed * Time.deltaTime, 0);
//			playerRigidBody.velocity.y = jumpSpeed * Time.deltaTime;

		}
//		transform.position += moveDirection * Time.deltaTime;
		playerRigidBody.MovePosition(transform.position + moveDirection * Time.deltaTime);

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			pug.transform.forward = playerToMouse;
		}
	}
}
