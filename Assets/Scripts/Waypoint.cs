using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] PossibleDirections possibleRoadDirections;
    public PossibleDirections PossibleRoadDirections => this.possibleRoadDirections;
}
