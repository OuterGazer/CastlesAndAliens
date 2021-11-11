using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    private GridManager gridManager;
    private Dictionary<Vector3Int, Node> gameGrid = new Dictionary<Vector3Int, Node>();

    [SerializeField] Vector3Int pathStart;
    [SerializeField] Vector3Int pathEnd;

    private Node startNode;
    private Node endNode;
    private Node currentSearchNode;

    Dictionary<Vector3Int, Node> reached = new Dictionary<Vector3Int, Node>();
    Queue<Node> frontier = new Queue<Node>();

    // This array influences the path depending on the order of directions
    Vector3Int[] directions = { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
    Vector3Int[] height = { Vector3Int.back, Vector3Int.forward };


    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

        if (this.gridManager != null)
            this.gameGrid = this.gridManager.GameGrid;

        this.startNode = new Node(this.pathStart, true);
        this.endNode = new Node(this.pathEnd, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        BreadthFirstSearch();
    }

    private void ExploreNeighbours()
    {
        List<Node> neighboursList = new List<Node>();

        for(int i = 0; i < this.directions.Length; i++)
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

                foreach(Node item in neighboursList)
                {
                    if(!this.reached.ContainsKey(item.Coordinates) && item.IsWalkable)
                    {
                        this.reached.Add(item.Coordinates, item);
                        this.frontier.Enqueue(item);
                    }
                }
            }
                
        }

    }

    private void BreadthFirstSearch()
    {
        bool isRunning = true;

        this.frontier.Enqueue(this.startNode);
        this.reached.Add(this.pathStart, this.startNode);

        while((this.frontier.Count > 0) && (isRunning == true))
        {
            this.currentSearchNode = this.frontier.Dequeue();
            this.currentSearchNode.SetIsExplored(true);

            ExploreNeighbours();

            if (this.currentSearchNode.Coordinates == this.pathEnd)
                isRunning = false;
        }
    }
}
