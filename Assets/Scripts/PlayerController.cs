using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
//	CharacterController controller;
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 10.0f;
	private Vector3 moveDirection = Vector3.zero;
	public GameObject pug;
	public Canvas canvas;
	public GameObject deathZone;

	Collider coll;
//	private Rigidbody playerRigidBody;
	float distToGround;
	void Start () {
//		controller = GetComponent<CharacterController>();
//		playerRigidBody = gameObject.GetComponent<Rigidbody>();
		coll = gameObject.GetComponent<Collider>();
		distToGround = coll.bounds.extents.y;
		Debug.Log (distToGround);
	}
	
//	void Update () {
//		if (controller.isGrounded) {
//			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//			moveDirection *= speed;
////			if (moveDirection != Vector3.zero) {
////				pug.transform.forward = moveDirection;
////			}
//
//			RaycastHit hit = new RaycastHit();
//
//
//			if (Input.GetButton("Jump"))
//				moveDirection.y = jumpSpeed;
//
//			if (Input.GetMouseButtonDown(0)) {
////				Debug.Log ("click");
////				Debug.Log (Input.mousePosition);
//				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
////				Debug.DrawRay (transform.position, ray.direction * 20, Color.red, 4.0f);
//				pug.transform.forward = ray.direction;
//				if(Physics.Raycast(ray, out hit, 20.0f)) {
//					Debug.Log(hit.collider.transform);
//				}
//			}
//
//		}
//
//		moveDirection.y -= gravity * Time.deltaTime;
//		controller.Move(moveDirection * Time.deltaTime);
//	}

	bool isGrounded() {
		bool hit = Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
		Debug.Log (hit);
		return hit;
	}

	void FixedUpdate() {
		moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		moveDirection *= speed;
		if (Input.GetButton ("Jump") && isGrounded()) {
			moveDirection.y = jumpSpeed;

		}
		transform.position += moveDirection * Time.deltaTime;

		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;
		if (Physics.Raycast (camRay, out floorHit, 100f)) {
			Vector3 playerToMouse = floorHit.point - transform.position;
			playerToMouse.y = 0f;
			pug.transform.forward = playerToMouse;
		}
	}
}
