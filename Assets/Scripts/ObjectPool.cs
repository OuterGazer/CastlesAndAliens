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

    [SerializeField] Transform[] enemyPrefabs;
    [SerializeField] int numberOfEachType = default;
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

        foreach(KeyValuePair<string, List<List<Node>>> item in this.fullPathsCollection)
        {
            Debug.Log(item.Key + " " + item.Value[0][0].Coordinates);
        }

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
            pool[i-1] = GameObject.Instantiate<Transform>(this.enemyPrefabs[enemyTypeCounter], this.gameObject.transform);

            AssignPathsListToEnemy(pool[i-1]);

            pool[i-1].gameObject.SetActive(false);

            if (i % this.numberOfEachType == 0)
                enemyTypeCounter++;
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
        if (enemy.gameObject.name.Contains("Red"))
            enemyPaths.FindPath(this.fullPathsCollection[forRed]);
        else if (enemy.gameObject.name.Contains("Normal"))
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
