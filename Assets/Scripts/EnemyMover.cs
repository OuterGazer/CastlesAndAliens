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
            //Debug.Log(item.name);
            this.gameObject.transform.position = item.transform.position;

            yield return new WaitForSeconds(this.waitTime);
        }
    }
}
