using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Vector3Int[] pathStarts;
    [SerializeField] Vector3Int[] pathEnds;

    [Range(0, 50)][SerializeField] int poolSize = default;

    [SerializeField] Transform[] enemyPrefabs;
    [SerializeField] int numberOfEachType = default;
    [Range(0.1f, 30f)][SerializeField] float spawnTime = default;

    [SerializeField] EnemyWave[] enemyWaves;

    private Transform[] poolLeft;
    private Transform[] poolCenter;
    private Transform[] poolRight;

    private List<Transform[]> poolsList = new List<Transform[]>();

    private Pathfinder pathfinder;

    private void Awake()
    {
        this.pathfinder = this.gameObject.GetComponent<Pathfinder>();

        PopulatePools();

        FindPaths();
    }

    private void PopulatePools()
    {
        this.poolsList.Add(this.poolLeft);
        this.poolsList.Add(this.poolCenter);
        this.poolsList.Add(this.poolRight);

        FillPoolWithEnemies(out this.poolLeft);
        FillPoolWithEnemies(out this.poolCenter);
        FillPoolWithEnemies(out this.poolRight);
    }

    private void FillPoolWithEnemies(out Transform[] pool)
    {
        pool = new Transform[this.poolSize];

        int enemyTypeCounter = 0;

        for (int i = 1; i <= pool.Length; i++)
        {
            pool[i - 1] = GameObject.Instantiate<Transform>(this.enemyPrefabs[enemyTypeCounter], this.gameObject.transform);

            //AssignPathsListToEnemy(pool[i-1]);

            pool[i - 1].gameObject.SetActive(false);

            if (i % this.numberOfEachType == 0)
                enemyTypeCounter++;
        }
    }

    private void FindPaths()
    {
        int poolCounter;

        for (int i = 0; i < this.pathStarts.Length; i++)
        {
            poolCounter = 0;

            for (int j = 0; j < this.pathEnds.Length; j++)
            {
                this.pathfinder.ClearChosenPath();
                List<List<Node>> tempNormal = new List<List<Node>>();
                tempNormal = this.pathfinder.FindPath(this.pathStarts[i], this.pathEnds[j], false);

                AssignPaths(tempNormal, i, poolCounter);
                poolCounter++;
            }

            this.pathfinder.ClearChosenPath();
            List<List<Node>> tempKamikaze = new List<List<Node>>();
            tempKamikaze = this.pathfinder.FindPath(this.pathStarts[i], this.pathEnds[1], true);

            AssignPaths(tempKamikaze, i, poolCounter);
        }        
    }

    private void AssignPaths(List<List<Node>> pathsList, int spawnIndex, int poolCounter)
    {
        switch (spawnIndex)
        {
            case 0:
                FindRightEnemyPoolAndAssignPaths(pathsList, this.poolLeft, poolCounter);                
                break;

            case 1:
                FindRightEnemyPoolAndAssignPaths(pathsList, this.poolCenter, poolCounter);
                break;

            case 2:
                FindRightEnemyPoolAndAssignPaths(pathsList, this.poolRight, poolCounter);
                break;
        }
    }

    private void FindRightEnemyPoolAndAssignPaths(List<List<Node>> pathsList, Transform[] enemyPool, int poolCounter)
    {
        switch (poolCounter)
        {
            case 0:
                for (int i = 0; i < 15; i++)
                {
                    EnemyMover enemy = enemyPool[i].gameObject.GetComponent<EnemyMover>();
                    enemy.FindPath(pathsList);
                }
                break;

            case 1:
                for (int i = 15; i < 30; i++)
                {
                    EnemyMover enemy = enemyPool[i].gameObject.GetComponent<EnemyMover>();
                    enemy.FindPath(pathsList);
                }
                break;

            case 2:
                for (int i = 30; i < 45; i++)
                {
                    EnemyMover enemy = enemyPool[i].gameObject.GetComponent<EnemyMover>();
                    enemy.FindPath(pathsList);
                }
                break;

        }
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        WaitForSeconds timeBetweenEnemies = new WaitForSeconds(this.spawnTime);

        while (Application.isPlaying)
        {
            for(int i = 0; i < this.poolCenter.Length; i++)
            {
                if (!this.poolCenter[i].gameObject.activeInHierarchy)
                {
                    this.poolCenter[i].gameObject.SetActive(true);

                    yield return timeBetweenEnemies;
                }

                if (i == this.poolCenter.Length)
                    i = 0;

                yield return null;
            }
        }
    }
}
