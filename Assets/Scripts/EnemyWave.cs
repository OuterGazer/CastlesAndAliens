using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    // TODO: how many enemies per spawn point
    // TODO: What enemy type(s)
    // TODO: how many of what enemy type (within the alloted max)
    // TODO: the order in which they spawn
    // TODO: the spawn time between single enemies
    // TODO: if enemies spawning from a new spawn point must wait till all enemies from a previous spawn point have spawned, or they can spawn at the same time.

    [SerializeField] int enemiesFromLeft = default;
    [SerializeField] int enemiesFromCenter = default;
    [SerializeField] int enemiesFromRight = default;

    [SerializeField] Dictionary<string, int> enemies;

    [SerializeField] float spawnTime = default;

    [SerializeField] bool canSpawnEnemiesFromMoreThanOnePoint = false;

    public int GetTotalAmountOfEnemies()
    {
        int totalAmountOfEnemies = this.enemiesFromLeft + this.enemiesFromCenter + this.enemiesFromRight;

        return totalAmountOfEnemies;
    }

    public float GetSpawnTime()
    {
        return this.spawnTime;
    }

    public bool GetCanSpawnAtTheSameTime()
    {
        return this.canSpawnEnemiesFromMoreThanOnePoint;
    }
}
