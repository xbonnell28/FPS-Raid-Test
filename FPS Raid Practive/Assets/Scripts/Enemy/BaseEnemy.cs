using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float speed = 5.0f; // enemy movement speed
    public float Gravity = 20f;
    public float GroundCheckDistanceInAir = 1.1f;
    public float StopDistance = 1.5f;
    public float damage = 1f;

    protected Vector3 playerPosition; // player position
    private Rigidbody rb;
    private bool isGrounded;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Update()
    {
        // get the player's position
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        Move(TrackPlayer());
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

    public virtual void Move(Vector3 direction)
    {
        // move the enemy in the given direction
        if(direction.magnitude > StopDistance)
        {
            Vector3 normalized = direction.normalized;
            Vector3 move = new Vector3(normalized.x, 0f, normalized.z);
            transform.position += speed * Time.deltaTime * move.normalized;
        }
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new Vector3(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }

    public Vector3 TrackPlayer()
    {
        // return a vector from the enemy to the player
        return playerPosition - transform.position;
    }
}
