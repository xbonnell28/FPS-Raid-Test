using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BasicEnemyRanged : BaseEnemy
{
    public float RunDistance = 4f;

    public override void Move(Vector3 direction)
    {
        Vector3 normalized = direction.normalized;
        Vector3 move = new(normalized.x, 0f, normalized.z);
        // move the enemy in the given direction
        if (direction.magnitude > StopDistance)
        {
            transform.position += speed * Time.deltaTime * move.normalized;
        } else if (direction.magnitude < RunDistance)
        {
            transform.position += speed * Time.deltaTime * -move.normalized;
        }
        // TODO make look at update less frequently to remove jittering
        // Remove vertical component of look at. If it's here then we get weird jittering due to player transform and enemy transform having slightly different z's
        Vector3 lookAtVector = new(playerPosition.x, transform.position.y, playerPosition.z);
        transform.LookAt(lookAtVector);
    }
}
