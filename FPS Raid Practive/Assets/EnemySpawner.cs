using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public BaseEnemy EnemyPrefab;

    // Tracks whether this spawn point has an active enemy spawned
    private bool hasEctiveEnemy { get; set; }
    public void SpawnEnemy()
    {
        BaseEnemy enemy = Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        enemy.spawnPoint = this;
        hasEctiveEnemy = true;
    }

    public void EnemyDestroyed()
    {
        hasEctiveEnemy = false;
    }
}
