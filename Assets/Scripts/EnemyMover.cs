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
        FindPath();
        ReturnToStart();
        this.StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        //this.gameObject.transform.position = this.path[0].gameObject.transform.position;
        this.gameObject.transform.position = this.gridManager.GetPosFromCoords(this.pathFinder.PathStart);
    }

    private void FindPath()
    {
        this.path.Clear();

        this.path = this.pathFinder.FindPath();

        /*GameObject waypoints = GameObject.FindWithTag("Path");

        foreach(Transform item in waypoints.transform)
        {
            Waypoint waypoint = item.GetComponentInChildren<Waypoint>();

            if(waypoint != null)
                this.path.Add(waypoint);
        }*/
    }

    private IEnumerator FollowPath()
    {
        //foreach (Waypoint item in path)
        for(int i = 0; i < this.path.Count; i++)
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
