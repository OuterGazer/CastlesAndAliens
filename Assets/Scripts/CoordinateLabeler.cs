using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defColor = Color.red;
    [SerializeField] Color placedColor = Color.grey;
    [SerializeField] Color exploredColor = Color.blue;
    [SerializeField] Color pathColor = Color.green;

    //private MouseInteraction waypoint;
    private GridManager gridManager;
    private TextMeshPro coordinateLabel;
    private Vector3Int coordinates = new Vector3Int();


    private void Awake()
    {
        this.coordinateLabel = this.gameObject.GetComponent<TextMeshPro>();
        this.coordinateLabel.enabled = false;

        //this.waypoint = this.gameObject.GetComponentInParent<MouseInteraction>();
        this.gridManager = GameObject.FindObjectOfType<GridManager>();

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

        SetWaypointColor();
        ToggleCoordinates();
    }

    private void DisplayCoordinates()
    {
        // Using UnityEditor.EditorSnapSettings prevents from creating a build and forces us to move this script into an Editor folder in the assets folder
        // (or really any property/method from UnityEditor namespace)

        if (this.gridManager == null) { return; }

        this.coordinates.x = Mathf.RoundToInt(this.gameObject.transform.parent.position.z / this.gridManager.UnityGridSize); // UnityEditor.EditorSnapSettings.move.z
        this.coordinates.y = Mathf.RoundToInt(this.gameObject.transform.parent.position.x / this.gridManager.UnityGridSize); // EditorSnapSettings.move.x
        this.coordinates.z = Mathf.RoundToInt(this.gameObject.transform.parent.position.y / this.gridManager.UnityHeightSize) - 1; // EditorSnapSettings.move.y

        this.coordinateLabel.text = $"{this.coordinates.x}, {this.coordinates.y}\n{this.coordinates.z}";
    }

    private void UpdateObjectName()
    {
        this.gameObject.transform.parent.name = this.coordinates.ToString();
    }

    private void SetWaypointColor()
    {
        if (this.gridManager == null) { return; }
        //Node node = this.gridManager.GetNode(this.coordinates);

        Node node = null;
        if (this.gridManager.GameGrid.ContainsKey(this.coordinates))
            node = this.gridManager.GameGrid[this.coordinates];

        if (node == null) { return; }

        if (!node.IsWalkable)
            this.coordinateLabel.color = this.placedColor;
        else if (node.IsPath)
            this.coordinateLabel.color = this.pathColor;
        else if (node.IsExplored)
            this.coordinateLabel.color = this.exploredColor;        
        else
            this.coordinateLabel.color = this.defColor;

        /*if (this.waypoint.IsPlaceable)
            this.coordinateLabel.color = this.defColor;
        else
            this.coordinateLabel.color = this.placedColor;*/
    }

    private void ToggleCoordinates()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            this.coordinateLabel.enabled = !this.coordinateLabel.IsActive();
        }
    }
}
