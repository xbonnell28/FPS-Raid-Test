using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Boss : BasicRangedEnemy
{
    public SpawnManager sm;
    public bool enraged;
    public float maxHealth;
    public TextMeshProUGUI bossHealth;

    public override void Update()
    {
        base.Update();
        bossHealth.text = health + "/" + maxHealth;
        if(enraged)
        {
            sm.SpawnEntities();
        }
    }
    public override void Attack(Vector3 direction)
    {
        if (agent.isStopped && Time.time - lastFireTime >= AttackRate)
        {
            // Instantiate bullet prefab at the enemy's position
            Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.transform.localScale *= 10;
            bullet.ValidTarget = validTarget;
            bullet.Damage = damage;
            // Set bullet velocity towards the player
            bullet.GetComponent<Rigidbody>().velocity = direction.normalized * bulletSpeed;

            // Set last fire time to current time
            lastFireTime = Time.time;
        }
    }
    public override void Die()
    {
        Destroy(gameObject);
        bossHealth.text = "You Win!";
    }

    public override void HandleDamage(float damage)
    {
        if(enraged)
        {
            base.HandleDamage(damage);
        }
    }
}
