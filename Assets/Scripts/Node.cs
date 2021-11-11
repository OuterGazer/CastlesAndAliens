using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // It allows for a non-Monobehaviour class to be shown in the inspector, but [SerializedField] it still needs to be written
public class Node
{
    [SerializeField] private Vector2Int coordinates;
    public Vector2Int Coordinates => this.coordinates;

    [SerializeField] private bool isWalkable;
    public bool IsWalkable => this.isWalkable;

    [SerializeField] private bool isExplored = false;
    public bool IsExplored => this.isExplored;

    [SerializeField] private bool isPath = false;
    public bool IsPath => this.isPath;

    // The previous Node that this Node is connected to
    [SerializeField] private Node connectedTo;
    public Node ConnectedTo => this.connectedTo;


    public Node(Vector2Int coordinates, bool isWalkable)
    {
        this.coordinates = coordinates;

        this.isWalkable = isWalkable;
    }

}
