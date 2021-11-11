using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // It allows for a non-Monobehaviour class to be shown in the inspector, but [SerializedField] it still needs to be written
public class Node
{
    [SerializeField] private Vector3Int coordinates;
    public Vector3Int Coordinates => this.coordinates;

    [SerializeField] private bool isWalkable;
    public bool IsWalkable => this.isWalkable;

    [SerializeField] private bool isExplored = false;
    public bool IsExplored => this.isExplored;
    public void SetIsExplored(bool isExplored)
    {
        this.isExplored = isExplored;
    }

    [SerializeField] private bool isPath = false;
    public bool IsPath => this.isPath;
    public void SetIsPath(bool isPath)
    {
        this.isPath = isPath;
    }

    // The previous Node that this Node is connected to
    [SerializeField] private Node connectedTo;
    public Node ConnectedTo => this.connectedTo;


    public Node(Vector3Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;

        this.isWalkable = isWalkable;
    }

}
