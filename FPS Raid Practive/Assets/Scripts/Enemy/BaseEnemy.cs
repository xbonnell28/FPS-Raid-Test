using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public float speed = 5.0f; // enemy movement speed
    public float Gravity = 20f;
    public float GroundCheckDistanceInAir = 1.1f;
    public float StopDistance = 1.5f;
    public float damage = 1f;
    public float AttackRate = 1f;

    protected Vector3 playerPosition; // player position
    protected Rigidbody rb;
    protected bool isGrounded;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Update()
    {
        // get the player's position
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
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
}
