using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMeleeEnemy : BasicMeleeEnemy
{
    public bool _isVulnerable = true;
    private Renderer _renderer;
    public override void Start()
    {
        base.Start();
    }

    // Need to handle damage here for when the BigEnemy is invulnerable
    public override void HandleDamage(float damage)
    {
        if(_isVulnerable)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void MakeVulnerable(bool makeVulnerable)
    {
        _renderer = gameObject.GetComponent<Renderer>();
        _renderer.material.color = Color.gray;
        _isVulnerable = makeVulnerable;
        if(_isVulnerable)
        {
            _renderer.material.color = Color.white;
        } else
        {
            _renderer.material.color = Color.grey;
        }
    }
}
