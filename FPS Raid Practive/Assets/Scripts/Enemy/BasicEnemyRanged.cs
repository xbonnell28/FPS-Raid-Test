using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicEnemyRanged : BaseEnemy
{
    public float RunDistance = 4f;
    public Bullet bulletPrefab;
    private bool stopped;
    private float bulletSpeed = 2f;
    private float lastFireTime;

    public override void Start()
    {
        lastFireTime = Time.time;
    }

    public override void Attack(Vector3 direction)
    {
        if (stopped && Time.time - lastFireTime >= AttackRate)
        {
            // Instantiate bullet prefab at the enemy's position
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.damage = damage;
            // Set bullet velocity towards the player
            bullet.GetComponent<Rigidbody>().velocity = direction * bulletSpeed;

            // Set last fire time to current time
            lastFireTime = Time.time;
        }
    }

    public override void Move(Vector3 direction)
    {
        Vector3 normalized = direction.normalized;
        Vector3 move = new(normalized.x, 0f, normalized.z);
        stopped = true;
        // move the enemy in the given direction
        if (direction.magnitude > StopDistance)
        {
            transform.position += speed * Time.deltaTime * move.normalized;
            stopped = false;
        } else if (direction.magnitude < RunDistance)
        {
            transform.position += speed * Time.deltaTime * -move.normalized;
            stopped = false;
        }
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
