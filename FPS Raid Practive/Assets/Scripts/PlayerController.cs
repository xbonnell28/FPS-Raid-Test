using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float speed = 2f;
	[SerializeField] private float cameraSensitivity = 2.0f;
	[SerializeField] private float jumpHeight = 1.0f;
	[SerializeField] private float gravity = 9.8f;
	
	private CharacterController controller;
	private Camera playerCamera;
	private Vector3 moveDirection = Vector3.zero;
	private Vector3 playerVelocity = Vector3.zero;
	public float lookSpeed = 2.0f;
	public float lookXLimit = 85.0f;
	private float rotationX = 0;
	private bool groundedPlayer = false;

	private void Start() {
		controller = GetComponent<CharacterController>();
		playerCamera = GetComponentInChildren<Camera>();

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update() {
		Debug.Log(groundedPlayer);
		if (controller.isGrounded && playerVelocity.y < 0) {
			playerVelocity.y = 0f;
		}

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		move = transform.TransformDirection(move);
		controller.Move(move * Time.deltaTime * speed);

		// Apply jump("Space") if character is on ground
		if (Input.GetButtonDown("Jump") && groundedPlayer) {
			playerVelocity.y += Mathf.Sqrt(jumpHeight * gravity);
			groundedPlayer = false;
		}

		if (!groundedPlayer) {
			playerVelocity.y -= Mathf.Sqrt(gravity) * Time.deltaTime;
		}

		controller.Move(playerVelocity * Time.deltaTime);

		rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
		rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
	}

	private void FixedUpdate() {
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;

		if (hit.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			groundedPlayer = true;
		}
	}
}
