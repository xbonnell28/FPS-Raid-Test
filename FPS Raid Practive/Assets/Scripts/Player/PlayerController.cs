using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class PlayerController : BaseEntity
{
	// Weapons
	public PlayerWeapon primary;
	public PlayerWeapon secondary;
	public Grenade grenade;
	public float abilityCooldown;

	private float lastAbilityTime;

	private PlayerWeapon activeWeapon;

	[SerializeField] private float BaseSpeed = 4f;
	[SerializeField] private float SprintSpeedMultiplier = 1.5f;
	[SerializeField] private float CameraSensitivity;
	[SerializeField] private float Gravity = 20f;
	[SerializeField] private float JumpForce = 9f;

	public LayerMask GroundCheckLayers = 3;

	// Player Movement and Control vars
	private float rotationX = 0;
	private CharacterController controller;
	private Camera playerCamera;
    private Vector3 playerVelocity = Vector3.zero;
	private readonly float lookXLimit = 85.0f;
	private bool IsGrounded = false;
	private Vector3 GroundNormal;
	private float LastJumpTime;
	private readonly float GroundCheckDistance = 0.05f;
	private readonly float AirAcceleration = 3;

	// Player Status Vars
	public float HealthRegenDelay = 5;
	public float HealthRegenRate = 10;
	public float RegenAmount = 1;
	public float MaxHealth = 100;

	private float _heldCharge = 0;
	private float _lastTimeDamaged;
	private bool _isRegenerating;
    private Coroutine _regenCoroutine;

    // HUD Items
    public TextMeshProUGUI hp;
    public TextMeshProUGUI chargeText;

    const float JumpGroundingPreventionTime = 0.2f;
	const float GroundCheckDistanceInAir = 0.07f;
	private void Start() {
		controller = GetComponent<CharacterController>();
		playerCamera = GetComponentInChildren<Camera>();
		activeWeapon = primary;
		_lastTimeDamaged = Time.time;

		health = 100;
		hp.SetText("HP: " + health);
		chargeText.SetText("Charge: " + _heldCharge);

		// Lock cursor
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void Update()
    {
        GroundCheck();
        HandlePlayerMovement();
        HandleJump();
        HandlePlayerCamera();
        HandleWeapon();
		HandleAbility();
		HandleHealthRegen();
    }

    private void UpdateUI()
    {
        hp.SetText("HP: " + health);
        chargeText.SetText("Charge: " + _heldCharge);
    }
    public override void HandleDamage(float damage)
    {
        health -= damage;
        _lastTimeDamaged = Time.time;
		UpdateUI();
        if (health <= 0)
        {
            this.enabled = false;
            hp.SetText("Dead");
        }
		if (_isRegenerating)
		{
			StopRegeneration();
		}
    }

    private void HandleHealthRegen()
    {
		if(Time.time - _lastTimeDamaged > HealthRegenDelay 
			&& health < MaxHealth
			&& !_isRegenerating)
        {
            _isRegenerating = true;
            _regenCoroutine = StartCoroutine(RegenerateHealth());
		}
    }

    private IEnumerator RegenerateHealth()
    {
		while(health < MaxHealth)
        {
            health += RegenAmount;
            if (health >= MaxHealth)
            {
                health = MaxHealth;
            }
            UpdateUI();
            yield return new WaitForSeconds(HealthRegenRate);
		}
		StopRegeneration();
    }

    private void StopRegeneration()
    {
        _isRegenerating = false;
		StopCoroutine(_regenCoroutine);
    }

    private void HandleWeapon()
    {
        if (Input.GetButton("Fire1"))
        {
            Attack();
        }

        if (Input.GetButtonDown("Primary Weapon"))
        {
            activeWeapon = primary;
            primary.gameObject.SetActive(true);
            secondary.gameObject.SetActive(false);
        }
        else if (Input.GetButtonDown("Secondary Weapon"))
        {
            activeWeapon = secondary;
            primary.gameObject.SetActive(false);
            secondary.gameObject.SetActive(true);
        }
    }

	private void HandleAbility()
	{
		if (Input.GetButtonDown("Ability") && Time.time - lastAbilityTime >= abilityCooldown)
		{
			print("Grenade");
			Vector3 grenadeStart = transform.position;
			grenadeStart.z += 1;
			Instantiate(grenade, grenadeStart, Quaternion.identity);
			lastAbilityTime = Time.time;
		}
	}

    private void Attack()
    {
        activeWeapon.Attack();
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

    public float PullHeldCharge()
	{
        float tempCharge = _heldCharge;
		_heldCharge = 0;
        UpdateUI();
        return tempCharge;
	}

	public void AddToHeldCharge(float charge)
	{
		_heldCharge += charge;
        UpdateUI();
    }
}
