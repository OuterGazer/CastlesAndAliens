using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
    private TextMeshPro coordinateLabel;
    private Vector3Int coordinates = new Vector3Int();

    private void Awake()
    {
        this.coordinateLabel = this.gameObject.GetComponent<TextMeshPro>();

        DisplayCoordinates(); //will display coordinates entering play mode
    }

    // Update is called once per frame
    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();

            UpdateObjectName();
        }
    }

    private void DisplayCoordinates()
    {
        this.coordinates.x = Mathf.RoundToInt(this.gameObject.transform.parent.position.z / EditorSnapSettings.move.z);
        this.coordinates.y = Mathf.RoundToInt(this.gameObject.transform.parent.position.x / EditorSnapSettings.move.x);
        this.coordinates.z = Mathf.RoundToInt(this.gameObject.transform.parent.position.y / EditorSnapSettings.move.y) - 1;

        this.coordinateLabel.text = $"{this.coordinates.x}, {this.coordinates.y}\n{this.coordinates.z}";
    }

    private void UpdateObjectName()
    {
        this.gameObject.transform.parent.name = $"({this.coordinates.x}, {this.coordinates.y}, {this.coordinates.z})";
    }
}
