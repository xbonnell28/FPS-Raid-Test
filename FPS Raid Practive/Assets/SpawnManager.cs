using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public EnemySpawner[] enemySpawners;

    public void SpawnEntities()
    {
        if (ShouldSpawnEnemies())
        {
            for (int i = 0; i < enemySpawners.Length; i++)
            {
                enemySpawners[i].SpawnEnemy();
            }
        }
    }

    private bool ShouldSpawnEnemies()
    {
        float totalSpawnedEnemies = 0;
        for (int i = 0; i < enemySpawners.Length; i++)
        {
            if (enemySpawners[i].hasActiveEnemy)
            {
                totalSpawnedEnemies++;
            }
        }
        return totalSpawnedEnemies <= enemySpawners.Length/2;
    }
}
