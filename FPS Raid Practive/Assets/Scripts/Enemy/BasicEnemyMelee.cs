using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class BasicEnemyMelee : BaseEnemy
{
    public float reach = 1f;

    public Bullet RightHand;
    private Collider RightHandCollider;

    private float lastFireTime;
    private float attackSpeed = 3f;
    public bool loop = true;

    public GameObject startPoint;
    public GameObject endPoint;

    public override void Start()
    {
        RightHandCollider = RightHand.GetComponent<Collider>();
        RightHandCollider.enabled = false;
        RightHand.damage = damage;
        lastFireTime = Time.time;
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

    public override void Attack(Vector3 direction)
    {
        if (direction.magnitude <= StopDistance)
        {
            StartCoroutine(AttackCoroutine(direction));
        }
    }

    IEnumerator AttackCoroutine(Vector3 direction)
    {
        while(true)
        {
            float elapsedTime = 0.0f;
            Vector3 start = RightHand.transform.position;
            while (elapsedTime < AttackRate)
            {
                RightHand.transform.position = Vector3.Lerp(start, endPoint.transform.position, elapsedTime/AttackRate);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            RightHand.transform.position = endPoint.transform.position;

            yield return new WaitForSeconds(0.5f);

            elapsedTime = 0.0f;
            start = RightHand.transform.position;

            while (elapsedTime < AttackRate)
            {
                RightHand.transform.position = Vector3.Lerp(start, startPoint.transform.position, elapsedTime / AttackRate);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            RightHand.transform.position = startPoint.transform.position;

            if (!loop) // Stop moving if loop is false
            {
                yield break;
            }
        }
    }

    public override void Move(Vector3 direction)
    {
        // move the enemy in the given direction
        if (direction.magnitude > StopDistance)
        {
            Vector3 normalized = direction.normalized;
            Vector3 move = new(normalized.x, 0f, normalized.z);
            RightHandCollider.enabled = true;
            transform.position += speed * Time.deltaTime * move.normalized;
            RightHandCollider.enabled = false;
        }
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
