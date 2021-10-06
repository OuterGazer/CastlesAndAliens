using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [Range(0f, 5f)] [SerializeField] float movementSpeed = default;

    // Start is called before the first frame update
    void OnEnable()
    {
        FindPath();
        ReturnToStart();
        this.StartCoroutine(FollowPath());
    }

    private void ReturnToStart()
    {
        this.gameObject.transform.position = this.path[0].gameObject.transform.position;
    }

    private void FindPath()
    {
        this.path.Clear();

        GameObject waypoints = GameObject.FindWithTag("Path");

        foreach(Transform item in waypoints.transform)
        {
            Waypoint waypoint = item.GetComponentInChildren<Waypoint>();

            if(waypoint != null)
                this.path.Add(waypoint);
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (Waypoint item in path)
        {
            Vector3 startPos = this.gameObject.transform.position;
            Vector3 finishPos = item.transform.position;
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
