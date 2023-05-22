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

    public override void HandleDamage(float damage)
    {
        if(_isVulnerable)
        {
            health -= damage;
            Debug.Log("Taking Damage");
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
