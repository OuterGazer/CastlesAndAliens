using System;
using System.Collections;
using System.Collections.Generic;
//using System.Linq;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;
    private Dictionary<Vector3Int, Node> gameGrid = new Dictionary<Vector3Int, Node>();

    [SerializeField] Vector3Int pathStart;
    public Vector3Int PathStart => this.pathStart;
    [SerializeField] Vector3Int pathEnd;
    public Vector3Int PathEnd => this.pathEnd;

    private Node startNode;
    private Node endNode;
    private Node currentSearchNode;

    Dictionary<Vector3Int, Node> reached = new Dictionary<Vector3Int, Node>();
    Queue<Node> frontier = new Queue<Node>();

    // This array influences the path depending on the order of directions
    //Vector3Int[] directions; // = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    List<Vector3Int> directions = new List<Vector3Int>();
    Vector3Int[] height = { Vector3Int.back, 2 * Vector3Int.back, Vector3Int.forward, 2 * Vector3Int.forward };


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

        if (this.gridManager != null)
        {
            this.gameGrid = this.gridManager.GameGrid;
            this.startNode = this.gameGrid[this.pathStart];
            this.endNode = this.gameGrid[this.pathEnd];            
        }
                    
    }

    // Start is called before the first frame update
    void Start()
    {
        //FindPath();
    }

    public List<Node> FindPath()
    {
        this.gridManager.ResetNodes();        

        BreadthFirstSearch();

        return CreatePath();
    }

    private bool ExploreNeighbours()
    {      
        List<Node> neighboursList = new List<Node>();

        for(int i = 0; i < this.directions.Count; i++)
        {
            Vector3Int curNeighbourCoords = this.currentSearchNode.Coordinates + this.directions[i];

            if (this.gameGrid.ContainsKey(curNeighbourCoords)) 
            {
                if (GameObject.Find(curNeighbourCoords.ToString()) == null)
                {                    
                    for (int j = 0; j < this.height.Length; j++)
                    {
                        curNeighbourCoords = curNeighbourCoords + this.height[j];

                        if (GameObject.Find(curNeighbourCoords.ToString()) != null)
                        {
                            break;
                        }

                        curNeighbourCoords = curNeighbourCoords - this.height[j];
                    }
                }
                neighboursList.Add(this.gameGrid[curNeighbourCoords]);

                Node lastNodeAdded = neighboursList[neighboursList.Count - 1];

                //Debug.Log("Start Node: " + startNode.IsWalkable);
                //Debug.Log("End Node: " + endNode.IsWalkable);

                if (!this.reached.ContainsKey(curNeighbourCoords) && lastNodeAdded.IsWalkable)
                {
                    lastNodeAdded.SetConnectedTo(this.currentSearchNode);

                    this.reached.Add(lastNodeAdded.Coordinates, lastNodeAdded);

                    if (this.currentSearchNode.Coordinates == this.pathEnd)
                        return true;

                    this.frontier.Enqueue(lastNodeAdded);
                }

                /*foreach(Node item in neighboursList)
                {
                    if(!this.reached.ContainsKey(item.Coordinates) && item.IsWalkable)
                    {
                        item.SetConnectedTo(this.currentSearchNode);

                        this.reached.Add(item.Coordinates, item);
                        this.frontier.Enqueue(item);
                    }
                }*/
            }
                
        }

        return false;
    }

    private void BreadthFirstSearch()
    {
        //this.startNode.SetIsWalkable(true);
        //this.endNode.SetIsWalkable(true);

        this.reached.Clear();
        this.frontier.Clear();

        this.frontier.Enqueue(this.startNode);
        this.reached.Add(this.pathStart, this.startNode);

        while(this.frontier.Count > 0)
        {
            this.currentSearchNode = this.frontier.Dequeue();
            this.currentSearchNode.SetIsExplored(true);

            AssignSearchDirections();

            if (ExploreNeighbours() == true)
                break;
        }
    }

    private List<Node> CreatePath()
    {
        List<Node> path = new List<Node>();
        Node currentNode = this.endNode;

        path.Add(currentNode);
        currentNode.SetIsPath(true);

        while(true)
        {
            currentNode = currentNode.ConnectedTo;

            if (currentNode == null)
                break;

            path.Add(currentNode);
            currentNode.SetIsPath(true);
        }

        path.Reverse();

        return path;
    }

    private void AssignSearchDirections()
    {
        this.directions.Clear();

        GameObject currentTile = GameObject.Find(this.currentSearchNode.Coordinates.ToString());
        Waypoint currentTileWaypoint = currentTile.GetComponent<Waypoint>();
        PossibleDirections directionsForBFS = currentTileWaypoint.PossibleRoadDirections;

        //PossibleDirections directionsForBFS = GameObject.Find(this.currentSearchNode.Coordinates.ToString()).GetComponent<Waypoint>().PossibleRoadDirections;

        string[] directions = {"Right", "Left", "Forward", "Backward"};
        
        foreach(string item in directions)
        {
            if (directionsForBFS.ToString().Contains(item))
            {
                // Here right = +z, left = -z, up = +x, down = -x
                switch (item)
                {
                    case "Right":
                        this.directions.Add(Vector3Int.right);
                        break;
                    case "Left":
                        this.directions.Add(Vector3Int.left);
                        break;
                    case "Forward":
                        this.directions.Add(Vector3Int.up);
                        break;
                    case "Backward":
                        this.directions.Add(Vector3Int.down);
                        break;

                }
            }
        }

    }

    public bool WillBlockPath(Vector3Int tileCoords)
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
}
