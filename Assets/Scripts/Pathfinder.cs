using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;
    private Dictionary<Vector3Int, Node> gameGrid = new Dictionary<Vector3Int, Node>();

    private Vector3Int pathStart;
    private Vector3Int pathEnd;
    [SerializeField] private int maxNumberOfPathsToCalculate = default;

    private Node startNode;
    private Node endNode;
    private Node currentSearchNode;

    private Dictionary<Vector3Int, Node> reached = new Dictionary<Vector3Int, Node>();
    private Queue<Node> frontier = new Queue<Node>();
    private List<List<Node>> possiblePaths = new List<List<Node>>();
    private List<List<Node>> pathsForEnemy = new List<List<Node>>();
    public void ClearChosenPath()
    {
        this.pathStart = Vector3Int.zero;
        this.pathEnd = Vector3Int.zero;
        this.pathsForEnemy.Clear();
        this.gridManager.ResetChosenNodesToBeElegibleAgain();
    }


    // This array influences the path depending on the order of directions
    //Vector3Int[] directions; // = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    List<Vector3Int> directions = new List<Vector3Int>();
    Vector3Int[] height = { Vector3Int.forward, Vector3Int.back, 2 * Vector3Int.back, 2 * Vector3Int.forward };


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

        if (this.gridManager != null)
        {
            this.gameGrid = this.gridManager.GameGrid;
            //this.startNode = this.gameGrid[this.pathStart];
            //this.endNode = this.gameGrid[this.pathEnd];            
        }
                    
    }

    public List<List<Node>> FindPath(Vector3Int pathStart, Vector3Int pathEnd, bool isKamikaze)
    {
        this.gridManager.ResetNodes();

        this.pathStart = pathStart;
        this.pathEnd = pathEnd;

        this.startNode = this.gameGrid[this.pathStart];
        this.endNode = this.gameGrid[this.pathEnd];

        BreadthFirstSearch(isKamikaze);

        if (this.possiblePaths.Count < 1)
        {
            return this.pathsForEnemy;
        }

        if(this.pathsForEnemy.Count < 1)
        {
            foreach(List<Node> item in this.possiblePaths)
            {
                this.pathsForEnemy.Add(item);
            }

            this.possiblePaths.Clear();
        }

        return null;//return CreatePath();

        //return FindPath(this.pathStart) // For dynamic pathfinding. This should be the only line in the method
    }

    // Overloaded method for dynamic pathfinding
    /*public List<Node> FindPath(Vector3Int enemyCurCoords)
    {
        this.gridManager.ResetNodes();

        BreadthFirstSearch(enemyCurCoords);

        return CreatePath();
    }*/

    private void ExploreNeighbours(bool isKamikaze)
    {      
        List<Node> neighboursList = new List<Node>();

        for (int i = 0; i < this.directions.Count; i++)
        {
            Vector3Int curNeighbourCoords = this.currentSearchNode.Coordinates + this.directions[i];

            if (this.gameGrid.ContainsKey(curNeighbourCoords))
            {
                curNeighbourCoords = CheckNodesAboveOrBelow(curNeighbourCoords);

                neighboursList.Add(this.gameGrid[curNeighbourCoords]);

                Node lastNodeAdded = neighboursList[neighboursList.Count - 1];

                if (!this.reached.ContainsKey(curNeighbourCoords) && lastNodeAdded.ShouldBeChosenAgain && lastNodeAdded.IsWalkable)
                {
                    lastNodeAdded.SetConnectedTo(this.currentSearchNode);

                    if (lastNodeAdded.Coordinates.Equals(this.pathEnd))
                    {
                        CreatePath(isKamikaze);

                        return;
                    }

                    if ((this.directions.Count > 2) &&
                        this.gridManager.TileList.Find(x => x.name == this.currentSearchNode.Coordinates.ToString())
                                                 .GetComponent<Waypoint>().ShouldNeighboursBeLocked)
                    {
                        lastNodeAdded.SetHasBeenChosen(true);
                    }
                            

                    this.reached.Add(lastNodeAdded.Coordinates, lastNodeAdded);

                    this.frontier.Enqueue(lastNodeAdded);
                }
            }

        }
    }

    private Vector3Int CheckNodesAboveOrBelow(Vector3Int curNeighbourCoords)
    {
        if (this.gridManager.TileList.Find(x => x.name == curNeighbourCoords.ToString()) == null)
        {
            for (int j = 0; j < this.height.Length; j++)
            {
                curNeighbourCoords = curNeighbourCoords + this.height[j];

                if(this.gridManager.TileList.Find(x => x.name == curNeighbourCoords.ToString()) != null)
                {
                    break;
                }

                curNeighbourCoords = curNeighbourCoords - this.height[j];
            }
        }

        return curNeighbourCoords;
    }

    private void BreadthFirstSearch(bool isKamikaze) // private void BreadthFirstSearch(Vector3Int coordinates) // To calcualte dynamically from enemy current position
    {
        this.reached.Clear();
        this.frontier.Clear();
        
        this.frontier.Enqueue(this.startNode); // this.frontier.Enqueue(this.gameGrid[coordinates]); // for the dynamic pathfinding
        this.reached.Add(this.pathStart, this.startNode); // this.reached.Add(coordinates, this.gameGrid[coordinates]); // for the dynamic pathfinding

        while (this.frontier.Count > 0)
        {
            this.currentSearchNode = this.frontier.Dequeue();

            GameObject currentPathTile = this.gridManager.TileList.Find(x => x.name == this.currentSearchNode.Coordinates.ToString());
            bool isRiverTile = currentPathTile.transform.parent.gameObject.GetComponentInChildren<MouseInteraction>().IsRiverTile;

            if (isKamikaze && isRiverTile)
                continue;

            this.currentSearchNode.SetIsExplored(true);

            AssignSearchDirections();

            ExploreNeighbours(isKamikaze);
        }
    }

    private void CreatePath(bool isKamikaze) // private List<Node> CreatePath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = this.endNode;

        path.Add(currentNode);
        currentNode.SetIsPath(true);

        while(true)
        {
            currentNode = currentNode.ConnectedTo;

            if (currentNode == null)
            {
                break;
            }

            path.Add(currentNode);
            currentNode.SetIsPath(true);

            Waypoint curTile = this.gridManager.TileList.Find(x => x.name == currentNode.Coordinates.ToString())
                                                 .GetComponent<Waypoint>();

            if (curTile.HasLockingCounter) // Lock certain paths if they have been used twice
            {
                curTile.LockingCounter++;

                if (curTile.LockingCounter > 2)
                    currentNode.SetShouldBeChosenAgain(false);
            }

            if (currentNode.HasBeenChosen)
                currentNode.SetShouldBeChosenAgain(false);
        }

        path.Reverse();

        this.possiblePaths.Add(path);

        if(this.possiblePaths.Count < this.maxNumberOfPathsToCalculate)
        {
            FindPath(this.pathStart, this.pathEnd, isKamikaze);
        }
    }

    private void AssignSearchDirections()
    {
        this.directions.Clear();

        GameObject currentTile = this.gridManager.TileList.Find(x => x.name == this.currentSearchNode.Coordinates.ToString());

        if (currentTile == null)
        {
            Debug.Log(this.currentSearchNode.Coordinates + " doesn't have a Waypoint script. Fix!");
            return;
        }

        Waypoint currentTileWaypoint = currentTile.GetComponent<Waypoint>();
        PossibleDirections directionsForBFS = currentTileWaypoint.PossibleRoadDirections;

        string[] directions = {"Right", "Left", "Forward", "Backward"};

        string directionsForBFSString = directionsForBFS.ToString();

        for(int i = 0; i < directions.Length; i++)
        {
            if (String.IsNullOrWhiteSpace(directionsForBFSString)) { break; }

            if (directionsForBFSString.Contains(directions[i]))
            {
                // Here right = +z, left = -z, up = +x, down = -x
                switch (directions[i])
                {
                    case "Right":
                        this.directions.Add(Vector3Int.right);
                        directionsForBFSString.Replace("Right", "");
                        break;
                    case "Left":
                        this.directions.Add(Vector3Int.left);
                        directionsForBFSString.Replace("Left", "");
                        break;
                    case "Forward":
                        this.directions.Add(Vector3Int.up);
                        directionsForBFSString.Replace("Forward", "");
                        break;
                    case "Backward":
                        this.directions.Add(Vector3Int.down);
                        directionsForBFSString.Replace("Backward", "");
                        break;

                }
            }
        }

        // Knuth Shuffle Algorithm to randomly shuffle a collection
        /*if(this.directions.Count > 2)
        {
            for(int i = 0; i < this.directions.Count; i++)
            {
                Vector3Int tmp = this.directions[i];
                int r = UnityEngine.Random.Range(i, this.directions.Count);

                this.directions[i] = this.directions[r];
                this.directions[r] = tmp;
            }
        }*/

    }

    /*public bool WillBlockPath(Vector3Int tileCoords)
    {
        if (this.gameGrid.ContainsKey(tileCoords))
        {
            Node tempNode = this.gameGrid[tileCoords];
            bool previousState = tempNode.IsWalkable;

            tempNode.SetIsWalkable(false);
            List<Node> newPath = FindPath();
            tempNode.SetIsWalkable(previousState);

            if(newPath.Count <= 1)
            {
                FindPath();

                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        //this.BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }*/
}
