using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage { get; set; }
    public bool destroyOnCollision = true;

    private bool hasCollided = false;
    private void Start()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!hasCollided)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                hasCollided = true;
                DestroyThisBullet();
                // Get player object and reduce health by bullet damage
                PlayerController player = other.GetComponent<PlayerController>();
                player.HandleDamage(damage);
            }
            else if (!other.gameObject.CompareTag("Enemy"))
            {
                DestroyThisBullet();
            }
        }
    }
        

    private void OnTriggerExit(Collider other)
    {
        hasCollided = false;
    }

    private void DestroyThisBullet()
    {
        if (destroyOnCollision)
        {
            Destroy(this.gameObject);
        }
    }
}
