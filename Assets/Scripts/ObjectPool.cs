using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] Vector3Int[] pathStarts;
    [SerializeField] Vector3Int[] pathEnds;

    [Range(0, 50)][SerializeField] int poolSize = default;

    [SerializeField] Transform[] enemyPrefabs;
    [SerializeField] int numberOfEachType = default;

    [SerializeField] EnemyWave[] enemyWaves;
    private int curEnemyCount = 0;
    public int CurEnemyCount
    {
        get { return this.curEnemyCount; }
        set { this.curEnemyCount = value; }
    }

    [SerializeField] TextMeshProUGUI waveNumberText;
    private int waveNumber = 0;

    private Transform[] poolLeft;
    private Transform[] poolCenter;
    private Transform[] poolRight;

    private List<Transform[]> poolsList = new List<Transform[]>();

    private Pathfinder pathfinder;

    private bool isFirstTutorial = true;

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
        for(int i = 0; i < this.enemyWaves.Length; i++)
        {
            this.waveNumber++;
            this.waveNumberText.text = $"Wave Number: {this.waveNumber}";

            if(!this.isFirstTutorial)
                CheckForTutorial(i, 0);

            List<EnemyType> currentWave = this.enemyWaves[i].GetEnemies();
            float spawnTime = this.enemyWaves[i].GetSpawnTime();

            this.curEnemyCount = 0;

            for (int j = 0; j < currentWave.Count; j++)
            {
                string enemyType = currentWave[j].ToString();

                switch (enemyType)
                {
                    case string k when k.Contains("Left"):
                        ChooseEnemy(0, enemyType);
                        break;

                    case string k when k.Contains("Center"):
                        ChooseEnemy(1, enemyType);
                        break;

                    case string k when k.Contains("Right"):
                        ChooseEnemy(2, enemyType);
                        break;
                }

                this.curEnemyCount++;

                yield return new WaitForSeconds(spawnTime);

                if (this.isFirstTutorial)
                {
                    CheckForTutorial(i, 0);
                    this.isFirstTutorial = false;
                }
            }

            yield return new WaitUntil(() => this.curEnemyCount < 1);

            CheckIfTowerShouldBeUnlocked(i);

            CheckForTutorial(i, 1);
        }
    }

    private void CheckForTutorial(int waveIndex, int wavePosition)
    {
        string tutorialWord;

        if (wavePosition == 0)
            tutorialWord = this.enemyWaves[waveIndex].GetBeginningTutorialWord();
        else
            tutorialWord = this.enemyWaves[waveIndex].GetEndingTutorialWord();

        if (!string.IsNullOrWhiteSpace(tutorialWord))
        {
            Messenger.Broadcast(tutorialWord);
        }
    }

    private void CheckIfTowerShouldBeUnlocked(int i)
    {
        if (i != 7 && i != 18)
        {
            return;
        }
        else if (i == 7)
        {
            Messenger.Broadcast("UnlockCannonTower");
        }
        else if (i == 18)
        {
            Messenger.Broadcast("UnlockCatapultTower");
        }
    }

    private void ChooseEnemy(int spawnPoint, string enemyType)
    {
        switch (enemyType)
        {
            case string k when k.Contains("BasicRed"):
                SpawnEnemy(spawnPoint, 0);
                break;

            case string k when k.Contains("MediumRed"):
                SpawnEnemy(spawnPoint, 5);
                break;

            case string k when k.Contains("FastRed"):
                SpawnEnemy(spawnPoint, 10);
                break;

            case string k when k.Contains("BasicNormal"):
                SpawnEnemy(spawnPoint, 15);
                break;

            case string k when k.Contains("MediumNormal"):
                SpawnEnemy(spawnPoint, 20);
                break;

            case string k when k.Contains("FastNormal"):
                SpawnEnemy(spawnPoint, 25);
                break;

            case string k when k.Contains("Balista"):
                SpawnEnemy(spawnPoint, 30);
                break;

            case string k when k.Contains("Cannon"):
                SpawnEnemy(spawnPoint, 35);
                break;

            case string k when k.Contains("Missile"):
                SpawnEnemy(spawnPoint, 40);
                break;
        }
    }

    private void SpawnEnemy(int spawnPoint, int arrayPosition)
    {
        bool exitLoop = false;

        switch (spawnPoint)
        {
            case 0:
                LoopThroughInactiveEnemiesInPool(ref arrayPosition, ref exitLoop, this.poolLeft);
                break;

            case 1:
                LoopThroughInactiveEnemiesInPool(ref arrayPosition, ref exitLoop, this.poolCenter);
                break;

            case 2:
                LoopThroughInactiveEnemiesInPool(ref arrayPosition, ref exitLoop, this.poolRight);
                break;
        }
    }

    private void LoopThroughInactiveEnemiesInPool(ref int arrayPosition, ref bool exitLoop, Transform[] enemyPool)
    {
        do
        {
            if (!enemyPool[arrayPosition].gameObject.activeInHierarchy)
            {
                enemyPool[arrayPosition].gameObject.SetActive(true);
                exitLoop = true;
            }

            arrayPosition++;

        } while (!exitLoop);
    }
}
