using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage { get; set; }
    public bool destroyOnCollision = true;
    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DestroyThisBullet();
            // Get player object and reduce health by bullet damage
            PlayerController player = other.GetComponent<PlayerController>();
            player.HandleDamage(damage);
        } else if (!other.gameObject.CompareTag("Enemy"))
        {
            DestroyThisBullet();
        }
    }

    private void DestroyThisBullet()
    {
        if (destroyOnCollision)
        {
            Destroy(this.gameObject);
        }
    }
}
