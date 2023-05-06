using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : PlayerWeapon
{
    // Shotgun blast variables
    public int numPellets = 9;
    public float spreadAngle = 20f;
    public override void FireProjectile(Vector3 targetPoint)
    {

        for (int i = 0; i < numPellets; i++)
        {
            // Calculate the direction for each pellet with some spread
            Vector3 spreadDirection = Quaternion.Euler(Random.Range(-spreadAngle, spreadAngle), Random.Range(-spreadAngle, spreadAngle), 0) * (targetPoint - transform.position).normalized;

            // Instantiate bullet prefab at the enemy's position
            Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.ValidTarget = validTarget;
            bullet.Damage = damage;

            // Set bullet velocity towards the spread direction
            bullet.GetComponent<Rigidbody>().velocity = spreadDirection * bulletSpeed;
        }
    }
}
