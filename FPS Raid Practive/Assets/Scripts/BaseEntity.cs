using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    protected float health;
    public abstract void HandleDamage(float damage);
}