using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] float waitTime = default;

    // Start is called before the first frame update
    void Start()
    {
        this.StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        foreach(Waypoint item in path)
        {
            Vector3 startPos = this.gameObject.transform.position;
            Vector3 finishPos = item.transform.position;
            float travelPercent = 0f;

            //this.gameObject.transform.position = item.transform.position;
            //yield return new WaitForSeconds(this.waitTime);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime;

                this.gameObject.transform.position = Vector3.Lerp(startPos, finishPos, travelPercent);

                yield return new WaitForEndOfFrame();
            }
            
        }
    }
}
