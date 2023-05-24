using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : BaseEntity
{
    public float speed = 5.0f; // enemy movement speed
    public float Gravity = 20f;
    public float GroundCheckDistanceInAir = 1.1f;
    public float StopDistance = 1.5f;
    public float damage = 1f;
    public float AttackRate = 1f;
    public string validTarget;
    public bool isRooted = false;
    public bool ShouldSpawnCharges;

    public float HeldCharge;
    public Charge ChargeToDrop;

    protected Vector3 playerPosition; // player position
    protected Rigidbody rb;
    protected bool isGrounded;

    public EnemySpawner spawnPoint { get; set; }

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Update()
    {
        // get the player's position
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        // TODO We should be passing around a normalized vector for player direction
        // full magnitude vectors cause wonky behaviour when far away
        // For now most places already handle this, but if you come back here to work more on enemies fix this
        // **Think about this, whould we be passing around full magnitude or normalized**
        // Full mag, always know player location in relation to object, have to remember to normalize
        // Normalized, safer, low to no chance of issue when object and player far away but have to get player in the scene if distance is important for the object
        Vector3 playerVector = TrackPlayer();
        Move(playerVector);
        Attack(playerVector);
    }

    void FixedUpdate()
    {
        // Check if the enemy is touching the ground
        // This vector is centered in the capsule putting the origin at y=1 so the ground check distance needs to take this into account
        isGrounded = Physics.Raycast(transform.position, Vector3.down, GroundCheckDistanceInAir);
        // Apply custom gravity to the enemy if it's not on the ground
        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * Gravity, ForceMode.Acceleration);
        }
    }

    public abstract void Attack(Vector3 direction);

    public abstract void Move(Vector3 direction);

    public Vector3 TrackPlayer()
    {
        // return a vector from the enemy to the player
        return playerPosition - transform.position;
    }

    public override void HandleDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if(ShouldSpawnCharges)
        {
            Transform currentTransform = this.transform;
            Charge dropCharge = Instantiate(ChargeToDrop, currentTransform.position, Quaternion.identity);
            dropCharge.ChargeAmount = HeldCharge;
        }
        spawnPoint.EnemyDestroyed();
        Destroy(this.gameObject);
    }
}
