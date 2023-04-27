using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


	[SerializeField] private float Speed = 4f;
	[SerializeField] private float SprintSpeedMultiplier = 1.5f;
	[SerializeField] private float CameraSensitivity = 2.0f;
	[SerializeField] private float Gravity = 20f;
	[SerializeField] private float JumpForce = 9f;

	public LayerMask GroundCheckLayers = 3;

	private CharacterController controller;
	private Camera playerCamera;
	private Vector3 playerVelocity = Vector3.zero;
	public float lookXLimit = 85.0f;
	private float rotationX = 0;
	private bool IsGrounded = false;
	private Vector3 GroundNormal;
	private float LastJumpTime;
	private float GroundCheckDistance = 0.05f;
	const float JumpGroundingPreventionTime = 0.2f;
	const float GroundCheckDistanceInAir = 0.07f;
	private void Start() {
		controller = GetComponent<CharacterController>();
		playerCamera = GetComponentInChildren<Camera>();

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update() {
		Debug.Log(IsGrounded);
		GroundCheck();
		HandlePlayerMovement();
		HandleJump();
		HandlePlayerCamera();
	}

	private void GroundCheck() {
		// Make sure that the ground check distance while already in air is very small, to prevent suddenly snapping to ground
		float chosenGroundCheckDistance =
			IsGrounded ? (controller.skinWidth + GroundCheckDistance) : GroundCheckDistanceInAir;

		IsGrounded = false;
		GroundNormal = Vector3.up;

		// only try to detect ground if it's been a short amount of time since last jump; otherwise we may snap to the ground instantly after we try jumping
		if (Time.time >= LastJumpTime + JumpGroundingPreventionTime) {
			// if we're grounded, collect info about the ground normal with a downward capsule cast representing our character capsule
			if (Physics.CapsuleCast(GetCapsuleBottomHemisphere(), GetCapsuleTopHemisphere(),
				controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance, -1,
				QueryTriggerInteraction.Ignore)) {
				// storing the upward direction for the surface found
				GroundNormal = hit.normal;

				// Only consider this a valid ground hit if the ground normal goes in the same direction as the character up
				// and if the slope angle is lower than the character controller's limit
				if (Vector3.Dot(hit.normal, transform.up) > 0f &&
					IsNormalUnderSlopeLimit(GroundNormal)) {
					IsGrounded = true;

					// handle snapping to the ground
					if (hit.distance > controller.skinWidth) {
						controller.Move(Vector3.down * hit.distance);
					}
				}
			}
		}
	}

	private Vector3 GetCapsuleTopHemisphere() {
		return transform.position + (transform.up * controller.height/2) - (transform.up * controller.radius);
	}

	private Vector3 GetCapsuleBottomHemisphere() {
		return transform.position - (transform.up * controller.height/2) + (transform.up * controller.radius);
	}

	// Returns true if the slope angle represented by the given normal is under the slope angle limit of the character controller
	bool IsNormalUnderSlopeLimit(Vector3 normal) {
		return Vector3.Angle(transform.up, normal) <= controller.slopeLimit;
	}

	private void HandlePlayerMovement() {
		if (controller.isGrounded && playerVelocity.y < 0) {
			playerVelocity.y = 0f;
		}

		Vector3 move = new(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		move = transform.TransformDirection(move);
		if (Input.GetKey(KeyCode.LeftShift)) {
			controller.Move(Speed * Time.deltaTime * move * SprintSpeedMultiplier);
		} else {
			controller.Move(Speed * Time.deltaTime * move);
		}
	}

	private void HandleJump() {
		// Apply jump("Space") if character is on ground
		if (Input.GetButtonDown("Jump") && IsGrounded) {
			// Make jump apply only an up force, unmodified by previous vertical velocity
			playerVelocity = new Vector3(playerVelocity.x, 0f, playerVelocity.z);
			playerVelocity += Vector3.up * JumpForce;
			IsGrounded = false;
			LastJumpTime = Time.time;
		}

		if (!IsGrounded) {
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
}
