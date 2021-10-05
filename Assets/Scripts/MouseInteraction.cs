using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] GameObject basicTowerPrefab;

    [SerializeField] bool isPlaceable = default;
    public bool IsPlaceable => this.isPlaceable;

    private void OnMouseDown()
    {
        if (this.isPlaceable)
        {
            //Debug.Log(this.gameObject.transform.position);
            Vector3 placementPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.2f, this.gameObject.transform.position.z);
            Instantiate<GameObject>(this.basicTowerPrefab, placementPos, Quaternion.identity);
            this.isPlaceable = false;
        }        
    }
}
