using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    //[SerializeField] List<Waypoint> path = new List<Waypoint>();
    private List<Node> path = new List<Node>();
    [Range(0f, 5f)] [SerializeField] float movementSpeed = default;

    private List<List<Node>> possiblePaths;// = new List<List<Node>>();
    private Dictionary<List<Node>, int> pathsWithDangerLevel = new Dictionary<List<Node>, int>();

    private int spawnCount = 0;


    private GridManager gridManager;


    private bool isKamikaze = false;
    private bool isReturning = false;
    private bool arePathsReversed = false;


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

        this.isKamikaze = this.gameObject.CompareTag("Kamikaze");
    }

    void OnEnable()
    {
        if(this.spawnCount < 1) { this.spawnCount++; return; }

        if (this.isReturning)
            this.isReturning = false;

        AssignPath();

        ReturnToStart();

        //FindPath(); //FindPath(true) // For dynamic pathfinding        
        this.StartCoroutine(FollowPath()); // For dynamic pathfinding we need to erase this line
    }

    private void ReturnToStart()
    {
        //this.gameObject.transform.position = this.path[0].gameObject.transform.position;
        this.gameObject.transform.position = this.gridManager.GetPosFromCoords(this.path[0].Coordinates);
    }

    public void FindPath(List<List<Node>> pathsList) // Change it to RecalculatePath(bool shouldPathBeReset) for dynamic changing of path
    {
        if (!this.gameObject.CompareTag("Kamikaze"))
        {
            this.possiblePaths = new List<List<Node>>(pathsList);
        }
        else
        {
            this.possiblePaths = new List<List<Node>>();

            foreach(List<Node> item in pathsList)
            {
                List<Node> tempList = new List<Node>();

                foreach(Node n in item)
                {
                    Node tempNode = new Node(n.Coordinates, n.IsWalkable);
                    tempList.Add(tempNode);
                }

                this.possiblePaths.Add(tempList);
            }
        }


        // This is for dynamic pathinding
        /*Vector3Int coordinates = new Vector3Int();
        if (shouldPathBeReset)
        {
            coordinates = this.pathFinder.PathStart;
        }
        else
        {
            coordinates = this.gridManager.GetCoordsFromPos(this.gameObject.transform.position);
        }*/

        //StopAllCoroutines() // Necessary for dynamic Pathfinding        

        //this.path = this.pathFinder.FindPath(this.possiblePaths);

        //this.StartCoroutine(FollowPath()); //Necessary for dynamic pathfinding
    }

    private void AssignPath()
    {
        this.path.Clear();

        AdjustPathIfIsKamikaze();

        ChooseCorrectPath();

        IOrderedEnumerable<KeyValuePair<List<Node>, int>> sortedPaths;

        if (!this.isKamikaze)
        {
            sortedPaths = this.pathsWithDangerLevel.OrderBy(x => x.Value).ThenBy(x => x.Key.Count); // Regular enemies avoid danger
        }
        else
        {
            sortedPaths = this.pathsWithDangerLevel.OrderByDescending(x => x.Value).ThenBy(x => x.Key.Count); // Kamikazes will prioritize paths with most danger
        }

        foreach (KeyValuePair<List<Node>, int> item in sortedPaths)
        {
            List<Node> chosenPath = new List<Node>();

            foreach(Node n in item.Key)
            {
                chosenPath.Add(new Node(n.Coordinates, n.IsWalkable));
                this.path = chosenPath;
            }

            this.pathsWithDangerLevel.Clear();
            break;
        }
    }

    private void AdjustPathIfIsKamikaze()
    {
        if (this.isKamikaze)
        {
            if (!this.isReturning)
            {
                if (this.arePathsReversed)
                {
                    foreach (List<Node> item in this.possiblePaths)
                    {
                        item.Reverse();
                    }

                    this.arePathsReversed = false;
                }
            }
            else
            {
                foreach (List<Node> item in this.possiblePaths)
                {
                    item.Reverse();
                }

                this.arePathsReversed = true;
            }
        }
    }

    private void ChooseCorrectPath()
    {
        foreach (List<Node> item in this.possiblePaths)
        {
            int pathDangerLevel = 0;

            foreach (Node tile in item)
            {
                Waypoint curNodeWayp = this.gridManager.TileList.Find(x => x.name == tile.Coordinates.ToString()).GetComponent<Waypoint>();
                pathDangerLevel += curNodeWayp.DangerLevel;
            }

            this.pathsWithDangerLevel.Add(item, pathDangerLevel);
        }
    }

    private IEnumerator FollowPath()
    {
        //foreach (Waypoint item in path)
        for(int i = 1; i < this.path.Count; i++)
        {
            Vector3 startPos = this.gameObject.transform.position;
            //Vector3 finishPos = item.transform.position;
            Vector3 finishPos = this.gridManager.GetPosFromCoords(this.path[i].Coordinates);
            //Debug.Log(this.path[i].Coordinates);
            float travelPercent = 0f;

            this.gameObject.transform.LookAt(finishPos);

            //this.gameObject.transform.position = item.transform.position; // Used for grid static movement
            //yield return new WaitForSeconds(this.waitTime);

            while (travelPercent < 1f)
            {
                travelPercent += this.movementSpeed * Time.deltaTime;

                this.gameObject.transform.position = Vector3.Lerp(startPos, finishPos, travelPercent);

                yield return new WaitForEndOfFrame();
            }

        }

        FinishPath();
    }

    private void FinishPath()
    {
        if (!this.isKamikaze)
        {
            this.gameObject.GetComponent<Enemy>().StealGold();
            this.gameObject.SetActive(false);
        }
        else
        {
            this.isReturning = !this.isReturning;
            AssignPath();
            this.StartCoroutine(FollowPath());
        }
        
    }
}
