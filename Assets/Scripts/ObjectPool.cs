using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Transform enemyPrefab;
    [SerializeField] float spawnTime = default;

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        WaitForSeconds timeBetweenEnemies = new WaitForSeconds(this.spawnTime);

        while (Application.isPlaying)
        {
            GameObject.Instantiate<Transform>(this.enemyPrefab, this.gameObject.transform);

            yield return timeBetweenEnemies;
        }
    }
}
