using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : MonoBehaviour
{
    public BigMeleeEnemy[] BigMeleeEnemies;
    public float RingDuration;

    private bool _isRinging = false;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
    }
    private void FixedUpdate()
    {
        if(_isRinging) return;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!_isRinging)
        {
            StartCoroutine(BellRinging());
        }
    }

    private IEnumerator BellRinging()
    {
        _isRinging = true;
        _renderer.material.color = Color.yellow;
        WeakenBigEnemy(true);
        yield return new WaitForSeconds(RingDuration);
        _isRinging = false;
        _renderer.material.color = Color.white;
        WeakenBigEnemy(false);  
    }

    private void WeakenBigEnemy(bool makeVulnerable)
    {
        foreach(BigMeleeEnemy enemy in BigMeleeEnemies) 
        {
            enemy.MakeVulnerable(makeVulnerable);
        }
    }
}
