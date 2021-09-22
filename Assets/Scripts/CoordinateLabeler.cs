using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    private TextMeshPro coordinateLabel;
    private Vector3Int coordinates = new Vector3Int();

    private void Awake()
    {
        this.coordinateLabel = this.gameObject.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
        }
    }

    private void DisplayCoordinates()
    {
        this.coordinates.x = Mathf.RoundToInt(this.gameObject.transform.parent.position.z / 10);
        this.coordinates.y = Mathf.RoundToInt(this.gameObject.transform.parent.position.x / 10);
        this.coordinates.z = Mathf.RoundToInt(this.gameObject.transform.parent.position.y / 2) - 1;

        this.coordinateLabel.text = $"{this.coordinates.x}, {this.coordinates.y}\n{this.coordinates.z}";
    }
}
