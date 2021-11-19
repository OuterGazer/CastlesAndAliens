using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    //[SerializeField] List<Waypoint> path = new List<Waypoint>();
    List<Node> path = new List<Node>();
    [Range(0f, 5f)] [SerializeField] float movementSpeed = default;

    private GridManager gridManager;
    private Pathfinder pathFinder;

    private void Awake()
    {
        this.gridManager = GameObject.FindObjectOfType<GridManager>();
        this.pathFinder = GameObject.FindObjectOfType<Pathfinder>();
    }

    void OnEnable()
    {
        ReturnToStart();
        FindPath(); //FindPath(true) // For dynamic pathfinding        
        this.StartCoroutine(FollowPath()); // For dynamic pathfinding we need to erase this line
    }

    private void ReturnToStart()
    {
        //this.gameObject.transform.position = this.path[0].gameObject.transform.position;
        this.gameObject.transform.position = this.gridManager.GetPosFromCoords(this.pathFinder.PathStart);
    }

    private void FindPath() // Change it to RecalculatePath(bool shouldPathBeReset) for dynamic changing of path
    {
        // This is for dynamic pathinding
        /*Vector3Int coordinates = new Vector3Int();
        if (shouldPathBeReset)
        {
            coordinates = this.pathFinder.PathStart;
        }
        else
        {
            coordinates = this.gridManager.GetCoordsFromPos(this.gameObject.transform.position);
        }*/

        //StopAllCoroutines() // Necessary for dynamic Pathfinding

        this.path.Clear();
        this.pathFinder.ClearChosenPath();

        this.path = this.pathFinder.FindPath();

        //this.StartCoroutine(FollowPath()); //Necessary for dynamic pathfinding
    }

    private IEnumerator FollowPath()
    {
        //foreach (Waypoint item in path)
        for(int i = 1; i < this.path.Count; i++)
        {
            Vector3 startPos = this.gameObject.transform.position;
            //Vector3 finishPos = item.transform.position;
            Vector3 finishPos = this.gridManager.GetPosFromCoords(this.path[i].Coordinates);
            float travelPercent = 0f;

            this.gameObject.transform.LookAt(finishPos);

            //this.gameObject.transform.position = item.transform.position; // Used for grid static movement
            //yield return new WaitForSeconds(this.waitTime);

            while (travelPercent < 1f)
            {
                travelPercent += this.movementSpeed * Time.deltaTime;

                this.gameObject.transform.position = Vector3.Lerp(startPos, finishPos, travelPercent);

                yield return new WaitForEndOfFrame();
            }

        }

        FinishPath();
    }

    private void FinishPath()
    {
        this.gameObject.GetComponent<Enemy>().StealGold();
        this.gameObject.SetActive(false);
    }
}
