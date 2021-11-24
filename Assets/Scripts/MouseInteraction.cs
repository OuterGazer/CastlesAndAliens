using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] GameObject basicTowerPrefab;

    [SerializeField] bool isPlaceable = default;
    public bool IsPlaceable => this.isPlaceable;
    public void SetIsPlaceable(bool isPlaceable)
    {
        this.isPlaceable = isPlaceable;
    }
    [SerializeField] bool isRoadTile = default;
    public bool IsRoadTile => this.isRoadTile;

    [SerializeField] bool isRiverTile = default;
    public bool IsRiverTile => this.isRiverTile;

    private Bank bank;
    private GridManager gridManager;
    //private PathFinder pathFinder;
    Vector3Int tileCoordinates = new Vector3Int();

    private void Awake()
    {
        this.bank = GameObject.FindObjectOfType<Bank>();
        this.gridManager = GameObject.FindObjectOfType<GridManager>();
        //this.pathFinder = GameObject.FindObjectOfType<PathFinder>();
    }

    private void Start()
    {
        if(this.gridManager != null)
        {
            this.tileCoordinates = this.gridManager.GetCoordsFromPos(this.gameObject.transform.GetChild(0).position);

            if (this.isPlaceable || (!this.isPlaceable && !this.isRoadTile))
            {
                this.gridManager.BlockNode(this.tileCoordinates);
            }

           /* if (this.isPlaceable)
                Debug.Log("I'm placeable!");*/
        }
    }

    private void OnMouseDown()
    {
        if (this.isPlaceable && this.bank.CurrentBalance > 0) // if(this.gridManager[this.tileCoordinates].IsWalkable && !this.pathFinder.WillBlockPath(this.tileCoordinates))
        {
            this.StartCoroutine(BuildTower());
        }        
    }

    private IEnumerator BuildTower()
    {
        Vector3 placementPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
        
        GameObject tower = Instantiate<GameObject>(this.basicTowerPrefab, placementPos, Quaternion.identity);
        DefenseTower defenseTower = tower.GetComponent<DefenseTower>();
        
        tower.transform.GetChild(1).gameObject.SetActive(false);
        defenseTower.CanShoot = false;
        defenseTower.SetTileUnderneath(this);

        this.isPlaceable = false;
        //this.gridManager.BlockNode(this.tileCoordinates);
        //this.pathfinder.NotifyReceivers(); // For dynamic changing of path

        this.bank.Withdraw(defenseTower.GoldCost);

        yield return new WaitForSeconds(defenseTower.BuildTime);

        if (tower == null) { this.StopAllCoroutines(); }

        tower.transform.GetChild(1).gameObject.SetActive(true);
        defenseTower.CanShoot = true;
    }
}
