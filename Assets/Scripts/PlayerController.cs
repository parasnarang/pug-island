using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	CharacterController controller;
	public float speed = 6.0F;
	public float jumpSpeed = 8.0F;
	public float gravity = 10.0f;
	private Vector3 moveDirection = Vector3.zero;
	public GameObject pug;
	public Canvas canvas;
	public GameObject deathZone;

	void Start () {
		controller = GetComponent<CharacterController>();
	}
	
	void Update () {
		if (controller.isGrounded) {
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection *= speed;
			if (moveDirection != Vector3.zero) {
				pug.transform.forward = moveDirection;
			}

			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;

		}

		moveDirection.y -= gravity * Time.deltaTime;
		controller.Move(moveDirection * Time.deltaTime);
	}
}
