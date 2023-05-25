using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bell : BaseMechanic
{
    public HiddenCodeBlock[] HiddentBlocks;
    public SpawnManager[] SpawnManagers;

    public float RingDuration;

    private bool _isRinging = false;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = gameObject.GetComponent<Renderer>();
        RevealHiddenBlocks(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(isActive && !_isRinging)
        {
            CheckSpawnManagers();
            StartCoroutine(BellRinging());
        }
    }

    private void CheckSpawnManagers()
    {
        foreach(SpawnManager spawnManager in SpawnManagers)
        {
            spawnManager.SpawnEntities();
        }
    }

    private IEnumerator BellRinging()
    {
        // Weaken All big enemies
        _isRinging = true;
        _renderer.material.color = Color.yellow;
        WeakenBigEnemy(true);
        RevealHiddenBlocks(true);
        yield return new WaitForSeconds(RingDuration);
        _isRinging = false;
        _renderer.material.color = Color.white;
        WeakenBigEnemy(false);
        RevealHiddenBlocks(false);
        StopCoroutine(BellRinging());
    }

    private void RevealHiddenBlocks(bool makeVisible)
    {
        if(HiddentBlocks.Length > 0)
        {
            foreach (HiddenCodeBlock hiddenBlock in HiddentBlocks)
            {
                hiddenBlock.MakeVisible(makeVisible);
            }
        }
    }

    private void WeakenBigEnemy(bool makeVulnerable)
    {
        BigMeleeEnemy[] bigMeleeEnemies = FindObjectsOfType<BigMeleeEnemy>();
        if(bigMeleeEnemies.Length > 0)
        {
            foreach (BigMeleeEnemy enemy in bigMeleeEnemies)
            {
                enemy.MakeVulnerable(makeVulnerable);
            }
        }
    }
}
