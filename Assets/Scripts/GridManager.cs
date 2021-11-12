using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector3Int gridSize;
    
    [Tooltip("World Grid Size - should match Unity snap settings.")]
    [SerializeField] int unityGridSize = 1;
    public int UnityGridSize => this.unityGridSize;
    [SerializeField] float unitydHeightSize = 0.1f;
    public float UnityHeightSize => this.unitydHeightSize;

    private Dictionary<Vector3Int, Node> gameGrid = new Dictionary<Vector3Int, Node>();
    public Dictionary<Vector3Int, Node> GameGrid
    {
        get { return this.gameGrid; }
    }
    /*public Node GetNode(Vector3Int coordinates)
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
    }*/

    private void Awake()
    {
        CreateGrid();
    }


    private void CreateGrid()
    {
        for(int i = 0; i <= gridSize.z; i++)
        {
            for(int j = 0; j <= gridSize.x; j++)
            {
                for(int k = 0; k <= gridSize.y; k++)
                {
                    Vector3Int curCoord = new Vector3Int(i, j, k);

                    this.gameGrid.Add(curCoord, new Node(curCoord, true));
                }
            }
        }
    }

    public void ResetNodes()
    {
        foreach(KeyValuePair<Vector3Int, Node> item in this.gameGrid)
        {
            item.Value.SetConnectedTo(null);
            item.Value.SetIsExplored(false);
            item.Value.SetIsPath(false);
        }
    }

    public void BlockNode(Vector3Int coordinates)
    {
        if (this.gameGrid.ContainsKey(coordinates))
        {
            this.gameGrid[coordinates].SetIsWalkable(false);
        }
    }

    public Vector3Int GetCoordsFromPos(Vector3 position)
    {
        Vector3Int coordinates = new Vector3Int();

        coordinates.x = Mathf.RoundToInt(position.z / this.unityGridSize); // UnityEditor.EditorSnapSettings.move.z
        coordinates.y = Mathf.RoundToInt(position.x / this.unityGridSize); // EditorSnapSettings.move.x
        coordinates.z = Mathf.RoundToInt(position.y / this.unitydHeightSize) -1 ; // EditorSnapSettings.move.y

        return coordinates;
    }

    public Vector3 GetPosFromCoords(Vector3Int coordinates)
    {
        Vector3 position = new Vector3();

        position.z = coordinates.x * this.unityGridSize; 
        position.x = coordinates.y * this.unityGridSize;
        position.y = ((float)coordinates.z + 1) * this.unitydHeightSize;

        return position;
    }
}
