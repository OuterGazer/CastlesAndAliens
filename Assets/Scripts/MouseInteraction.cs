using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] bool isPlaceable = default;

    private void OnMouseDown()
    {
        if (this.isPlaceable)
        {
            Debug.Log(this.gameObject.transform.position);
        }        
    }
}
