using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


	[SerializeField] private float BaseSpeed = 4f;
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
	private readonly float GroundCheckDistance = 0.05f;
	public float MovementSharpnessOnGround = 15;
	public float AirAcceleration = 3;
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
				controller.radius, Vector3.down, out RaycastHit hit, chosenGroundCheckDistance))
            {
                Debug.Log("Player Grounded");
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

		float playerVelocityY = playerVelocity.y;
		Vector3 move = new(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
		move = transform.TransformDirection(move);

		/*float Speed = BaseSpeed;

		// Attempts at adding MovementSharpness
		if(rigidbody.velocity.magnitude > 0.1f)
		{
			Speed = BaseSpeed * MovementSharpnessOnGround;
		} else
		{
			Speed = BaseSpeed * MovementSharpnessOnGround;
		}*/

		// Might need to consider max speed here
		if (Input.GetButton("Sprint")) {
			playerVelocity = BaseSpeed * SprintSpeedMultiplier * move;
		} else {
			playerVelocity = move * BaseSpeed;
		}
		// Attempted to Lerp the movement vector but it makes the character move extremely slow
		//Vector3 CharacterVelocity = Vector3.Lerp(move, playerVelocity, MovementSharpnessOnGround * Time.deltaTime);
		playerVelocity.y = playerVelocityY;
		controller.Move(playerVelocity * Time.deltaTime);


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
			playerVelocity = new Vector3(playerVelocity.x * AirAcceleration, playerVelocity.y, playerVelocity.z * AirAcceleration);
			playerVelocity += Gravity * Time.deltaTime * Vector3.down;
		}

		controller.Move(playerVelocity * Time.deltaTime);
	}

	// Gets a reoriented direction that is tangent to a given slope
	public Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal) {
		Vector3 directionRight = Vector3.Cross(direction, transform.up);
		return Vector3.Cross(slopeNormal, directionRight).normalized;
	}

	private void HandlePlayerCamera()
    {
        // Add the vertical mouse input to the camera's X rotation
        rotationX += -Input.GetAxis("Mouse Y") * CameraSensitivity;

        // Clamp the X rotation between a minimum and maximum angle
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

        // Set the camera's local rotation based on the X rotation
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

        // Add the horizontal mouse input to the player's Y rotation
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * CameraSensitivity, 0);
    }
}
