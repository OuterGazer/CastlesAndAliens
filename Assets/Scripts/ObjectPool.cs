using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Vector3Int[] pathStarts;
    [SerializeField] Vector3Int[] pathEnds;

    private Dictionary<string, List<List<Node>>> fullPathsCollection = new Dictionary<string, List<List<Node>>>();
    private Queue<string> pathType = new Queue<string>(); 

    [Range(0, 50)][SerializeField] int poolSize = default;
    private int poolCounter = 0;

    [SerializeField] Transform[] enemyPrefabsLeft;
    [SerializeField] Transform[] enemyPrefabsCenter;
    [SerializeField] Transform[] enemyPrefabsRight;
    [Range(0.1f, 30f)][SerializeField] float spawnTime = default;

    private Transform[] poolLeft;
    private Transform[] poolCenter;
    private Transform[] poolRight;
    private Pathfinder pathfinder;

    private void Awake()
    {
        this.pathfinder = this.gameObject.GetComponent<Pathfinder>();

        AddPathTypes();

        FindPaths();

        PopulatePools();
    }

    private void AddPathTypes()
    {
        this.pathType.Enqueue("RedLeft");
        this.pathType.Enqueue("LeftNormal");
        this.pathType.Enqueue("LeftKamikaze");
        this.pathType.Enqueue("RedCenter");
        this.pathType.Enqueue("CenterNormal");
        this.pathType.Enqueue("CenterKamikaze");
        this.pathType.Enqueue("RedRight");
        this.pathType.Enqueue("RightNormal");
        this.pathType.Enqueue("RightKamikaze");
    }

    private void FindPaths()
    {
        for(int i = 0; i < this.pathStarts.Length; i++)
        {
            for (int j = 0; j < this.pathEnds.Length; j++)
            {
                this.pathfinder.ClearChosenPath();
                List<List<Node>> tempNormal = this.pathfinder.FindPath(this.pathStarts[i], this.pathEnds[j], false);

                this.fullPathsCollection.Add(this.pathType.Dequeue(), tempNormal);               
            }

            this.pathfinder.ClearChosenPath();
            List<List<Node>> tempKamikaze = this.pathfinder.FindPath(this.pathStarts[i], this.pathEnds[1], true);

            this.fullPathsCollection.Add(this.pathType.Dequeue(), tempKamikaze);           
        }
    }

    private void PopulatePools()
    {
        FillPoolWithEnemies(this.poolLeft, this.enemyPrefabsLeft);
        FillPoolWithEnemies(this.poolCenter, this.enemyPrefabsCenter);
        FillPoolWithEnemies(this.poolRight, this.enemyPrefabsRight);
    }

    private void FillPoolWithEnemies(Transform[] pool, Transform[] EnemyPrefabs)
    {
        pool = new Transform[this.poolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = GameObject.Instantiate<Transform>(EnemyPrefabs[i], this.gameObject.transform);

            AssignPathsListToEnemy(pool[i]);

            pool[i].gameObject.SetActive(false);
        }

        this.poolCounter++;
    }

    private void AssignPathsListToEnemy(Transform enemy)
    {
        EnemyMover enemyPaths = enemy.gameObject.GetComponent<EnemyMover>();

        switch (this.poolCounter)
        {
            case 0:
                FindProperPathsList(enemy, enemyPaths, "RedLeft", "LeftNormal", "LeftKamikaze");
                break;

            case 1:
                FindProperPathsList(enemy, enemyPaths, "RedCenter", "CenterNormal", "CenterKamikaze");
                break;

            case 2:
                FindProperPathsList(enemy, enemyPaths, "RedRight", "RightNormal", "RightKamikaze");
                break;
        }
    }

    private void FindProperPathsList(Transform enemy, EnemyMover enemyPaths, string forRed, string forNormal, string forKamikaze)
    {
        if (enemy.name.Contains("Red"))
            enemyPaths.FindPath(this.fullPathsCollection[forRed]);
        else if (enemy.name.Contains("Normal"))
            enemyPaths.FindPath(this.fullPathsCollection[forNormal]);
        else
            enemyPaths.FindPath(this.fullPathsCollection[forKamikaze]);
    }

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        WaitForSeconds timeBetweenEnemies = new WaitForSeconds(this.spawnTime);

        while (Application.isPlaying)
        {
            for(int i = 0; i < this.poolLeft.Length; i++)
            {
                if (!poolLeft[i].gameObject.activeInHierarchy)
                {
                    poolLeft[i].gameObject.SetActive(true);

                    yield return timeBetweenEnemies;
                }

                if (i == this.poolLeft.Length)
                    i = 0;

                yield return null;
            }
        }
    }
}
