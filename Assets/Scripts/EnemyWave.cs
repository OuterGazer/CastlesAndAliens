using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "Enemy Wave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField] Transform[] enemies;

    [SerializeField] float spawnTime = default;

    // TODO: have the scriptable object differentiate enemies from different pools from ObjectPool

    [SerializeField] bool canSpawnEnemiesFromMoreThanOnePoint = false;
}
