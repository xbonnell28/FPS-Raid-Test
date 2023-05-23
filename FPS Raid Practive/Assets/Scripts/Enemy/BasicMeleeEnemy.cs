using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BasicMeleeEnemy : BaseEnemy
{
    public Bullet RightHand;
    private Collider RightHandCollider;

    private float lastFireTime;
    public float attackSpeed = 0.2f;
    public bool loop = false;

    public GameObject startPoint;
    public GameObject endPoint;
    private bool isAttacking = false;

    private NavMeshAgent agent;

    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = isRooted;
        RightHandCollider = RightHand.GetComponent<Collider>();
        RightHandCollider.enabled = false;
        RightHand.Damage = damage;
        RightHand.ValidTarget = validTarget;
        lastFireTime = Time.time;
    }

    public override void Attack(Vector3 direction)
    {
        if (agent.isStopped && Time.time - lastFireTime >= AttackRate)
        {
            if(!isAttacking)
            {
                isAttacking = true;
                lastFireTime = Time.time;
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        float elapsedTime = 0.0f;
        Vector3 start = RightHand.transform.position;
        RightHandCollider.enabled = true;
        while (elapsedTime < attackSpeed)
        {
            RightHand.transform.position = Vector3.Lerp(start, endPoint.transform.position, elapsedTime / attackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RightHand.transform.position = endPoint.transform.position;

        yield return new WaitForSeconds(0.1f);

        elapsedTime = 0.0f;
        start = RightHand.transform.position;

        while (elapsedTime < attackSpeed)
        {
            RightHand.transform.position = Vector3.Lerp(start, startPoint.transform.position, elapsedTime / attackSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RightHand.transform.position = startPoint.transform.position;
        RightHandCollider.enabled = false;
        isAttacking = false;
    }

    public override void Move(Vector3 direction)
    {
        if(!isRooted)
        {
            // move the enemy in the given direction
            if (direction.magnitude > agent.stoppingDistance)
            {
                agent.SetDestination(playerPosition);
                agent.isStopped = false;
            }
            else
            {
                transform.position = transform.position;
                rb.velocity = Vector3.zero;
                agent.isStopped = true;
            }
        }
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
