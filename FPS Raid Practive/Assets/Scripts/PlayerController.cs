using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


	[SerializeField] private float Speed = 4f;
	[SerializeField] private float CameraSensitivity = 2.0f;
	[SerializeField] private float Gravity = 20f;
	[SerializeField] private float JumpForce = 9f;

	private CharacterController controller;
	private Camera playerCamera;
	private Vector3 playerVelocity = Vector3.zero;
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
		HandlePlayerMovement();
		HandleJump();
		HandlePlayerCamera();
	}

	private void HandlePlayerMovement() {
		if (controller.isGrounded && playerVelocity.y < 0) {
			playerVelocity.y = 0f;
		}

		Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		move = transform.TransformDirection(move);
		controller.Move(Speed * Time.deltaTime * move);
	}

	private void HandleJump() {
		// Apply jump("Space") if character is on ground
		if (Input.GetButtonDown("Jump") && groundedPlayer) {
			// Make jump apply only an up force, unmodified by previous vertical velocity
			playerVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);
			playerVelocity += Vector3.up * JumpForce;
			groundedPlayer = false;
		}

		if (!groundedPlayer) {
			// Add air accleration to make air movement feel nicer

			playerVelocity += Vector3.down * Gravity * Time.deltaTime;
		}

		controller.Move(playerVelocity * Time.deltaTime);
	}

	private void HandlePlayerCamera() {
		rotationX += -Input.GetAxis("Mouse Y") * CameraSensitivity;
		rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * CameraSensitivity, 0);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			groundedPlayer = true;
		}
	}
}
