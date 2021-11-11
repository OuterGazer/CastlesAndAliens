using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize;


    private Dictionary<Vector3Int, Node> gameGrid = new Dictionary<Vector3Int, Node>();
    public Node GetNode(Vector3Int coordinates)
    {
        if (this.gameGrid.ContainsKey(coordinates))
        {
            return this.gameGrid[coordinates];
        }
        else
        {
            // This just populates the console on edit mode so taking it out for the moment
            //Debug.LogWarning($"Coordinates {coordinates} are not part of the grid!");
        }
        return null;
    }

    private void Awake()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        for(int i = 0; i < gridSize.x; i++)
        {
            for(int j = 0; j < gridSize.y; j++)
            {
                for(int k = 0; k < gridSize.z; k++)
                {
                    Vector3Int curCoord = new Vector3Int(i, j, k);

                    this.gameGrid.Add(curCoord, new Node(curCoord, true));
                }
            }
        }
    }
}
