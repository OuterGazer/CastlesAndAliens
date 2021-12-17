using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyType
{
    LeftBasicRed,
    LeftMediumRed,
    LeftFastRed,
    LeftBasicNormal,
    LeftMediumNormal,
    LeftFastNormal,
    LeftBalista,
    LeftCannon,
    LeftMissile,
    CenterBasicRed,
    CenterMediumRed,
    CenterFastRed,
    CenterBasicNormal,
    CenterMediumNormal,
    CenterFastNormal,
    CenterBalista,
    CenterCannon,
    CenterMissile,
    RightBasicRed,
    RightMediumRed,
    RightFastRed,
    RightBasicNormal,
    RightMediumNormal,
    RightFastNormal,
    RightBalista,
    RightCannon,
    RightMissile,
}

[CreateAssetMenuAttribute(menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    // TODO: how many enemies per spawn point
    // TODO: What enemy type(s)
    // TODO: how many of what enemy type (within the alloted max)
    // TODO: the order in which they spawn
    // TODO: the spawn time between single enemies
    // TODO: if enemies spawning from a new spawn point must wait till all enemies from a previous spawn point have spawned, or they can spawn at the same time.

    [SerializeField] List<EnemyType> waveEnemyList;

    [SerializeField] float spawnTime = default;

    public List<EnemyType> GetEnemies()
    {
        return this.waveEnemyList;
    }

    public int GetTotalAmountOfEnemies()
    {
        return this.waveEnemyList.Count;
    }

    public float GetSpawnTime()
    {
        return this.spawnTime;
    }
}
