using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();

    // Start is called before the first frame update
    void Start()
    {
        PrintWaypointNames();
    }

    private void PrintWaypointNames()
    {
        foreach(Waypoint item in path)
        {
            Debug.Log(item.name);
        }
    }
}
