using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class BasicRangedEnemy : BaseEnemy
{
    public float RunDistance = 4f;
    public Bullet bulletPrefab;
    public float bulletSpeed = 5f;
    private float lastFireTime;

    private NavMeshAgent agent;
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = isRooted;
        lastFireTime = Time.time;
    }

    public override void Attack(Vector3 direction)
    {
        if (agent.isStopped && Time.time - lastFireTime >= AttackRate)
        {
            // Instantiate bullet prefab at the enemy's position
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.ValidTarget = validTarget;
            bullet.Damage = damage;
            // Set bullet velocity towards the player
            bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;

            // Set last fire time to current time
            lastFireTime = Time.time;
        }
    }

    public override void Move(Vector3 direction)
    {
        Vector3 normalized = direction.normalized;
        Vector3 move = new(normalized.x, 0f, normalized.z);
        agent.isStopped = true;
        if (!isRooted) {
            // move the enemy in the given direction
            if (direction.magnitude > agent.stoppingDistance)
            {
                agent.SetDestination(playerPosition);
                agent.isStopped = false;
            }
            else if (direction.magnitude < RunDistance)
            {
                transform.position += speed * Time.deltaTime * -move.normalized;
                agent.isStopped = false;
            }
        }
        
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
