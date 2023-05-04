using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; set; }
    public bool destroyOnCollision = true;
    public string ValidTarget { get; set; }

    private bool hasCollided = false;
    private void OnTriggerEnter(Collider other)
    {
        if(!hasCollided)
        {
            if (other.gameObject.CompareTag(ValidTarget))
            {
                hasCollided = true;
                DestroyThisBullet();
                // Get player object and reduce health by bullet damage
                BaseEntity entity = other.GetComponent<BaseEntity>();
                entity.HandleDamage(Damage);
            }
            else if (other.gameObject.CompareTag("Wall"))
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
