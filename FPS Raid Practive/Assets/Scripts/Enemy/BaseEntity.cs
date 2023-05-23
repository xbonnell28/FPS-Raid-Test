using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public float health;
    public abstract void HandleDamage(float damage);
}