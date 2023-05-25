using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Bullet bulletPrefab;
    public float bulletSpeed;
    public float AttackRate;
    public float damage;
    public string validTarget;
    public Camera Camera;

    private float lastFireTime;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1")) {
            Attack();
        }
    }

    public void Attack()
    {
        if (Time.time - lastFireTime >= AttackRate)
        {

            // Create a ray from the camera going through the middle of your screen
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            // Check whether your are pointing to something so as to adjust the direction
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(1000);
            }

            FireProjectile(targetPoint);

            // Set last fire time to current time
            lastFireTime = Time.time;
        }
    }

    public virtual void FireProjectile(Vector3 targetPoint)
    {
        // Instantiate bullet prefab at the enemy's position
        Bullet bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.ValidTarget = validTarget;
        bullet.Damage = damage;

        // Set bullet velocity towards the player
        bullet.GetComponent<Rigidbody>().velocity = (targetPoint - transform.position).normalized * bulletSpeed;
    }
}
