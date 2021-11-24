using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] PossibleDirections possibleRoadDirections;
    public PossibleDirections PossibleRoadDirections => this.possibleRoadDirections;

    private int dangerLevel = 0;
    public int DangerLevel => this.dangerLevel;

    private int lockingCounter = 0;
    public int LockingCounter
    {
        get { return this.lockingCounter; }
        set { this.lockingCounter = value; }
    }


    [SerializeField] bool shouldNeighboursBeLocked = true; // This is to prevent the 4 crossings near the castles from locking pathfinding into not repeating paths. This is necessary to be able to generate more than 2 different paths when enemies spawn.
    public bool ShouldNeighboursBeLocked { get { return this.shouldNeighboursBeLocked; } set { this.shouldNeighboursBeLocked = value; } }

    [SerializeField] bool hasLockingCounter = false;
    public bool HasLockingCounter => this.hasLockingCounter;

    
    public void IncreaseDangerLevel(int dangerModifier)
    {
        this.dangerLevel += dangerModifier;
        //Debug.Log($"I am {this.gameObject.name} and my danger level is {this.dangerLevel}!");
    }

    public void DecreaseDangerLevel(int dangerModifier)
    {
        this.dangerLevel -= dangerModifier;

        if (dangerLevel < 0)
            this.dangerLevel = 0;
    }
}
