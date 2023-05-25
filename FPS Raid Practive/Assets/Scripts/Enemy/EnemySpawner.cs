using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BaseEnemy EnemyPrefab;

    // Tracks whether this spawn point has an active enemy spawned
    public bool hasActiveEnemy { get; set; }
    public void SpawnEnemy()
    {
        if(!hasActiveEnemy)
        {
            BaseEnemy enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
            enemy.spawnPoint = this;
            hasActiveEnemy = true;
        }
    }

    public void EnemyDestroyed()
    {
        hasActiveEnemy = false;
    }
}
