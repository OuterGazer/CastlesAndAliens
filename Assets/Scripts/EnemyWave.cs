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
