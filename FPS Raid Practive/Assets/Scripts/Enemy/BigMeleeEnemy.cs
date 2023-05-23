using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigMeleeEnemy : BasicMeleeEnemy
{
    public Bell Bell { get; private set; }

    private bool _isVulnerable = false;
    private Renderer _renderer;
    public override void Start()
    {
        base.Start();
        _renderer = gameObject.GetComponent<Renderer>();
        _renderer.material.color = Color.gray;
    }

    // Need to handle damage here for when the BigEnemy is invulnerable
    public override void HandleDamage(float damage)
    {
        if(_isVulnerable)
        {
            health -= damage;
            if (health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void MakeVulnerable(bool makeVulnerable)
    {
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
