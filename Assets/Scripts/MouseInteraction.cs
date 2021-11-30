using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    //[SerializeField] GameObject basicTowerPrefab;
    private GameObject towerPrefab;

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
        Messenger<GameObject>.AddListener("SetTowerType", SetTowerType);

        this.bank = GameObject.FindObjectOfType<Bank>();
        this.gridManager = GameObject.FindObjectOfType<GridManager>();
        //this.pathFinder = GameObject.FindObjectOfType<PathFinder>();
    }

    private void SetTowerType(GameObject towerType)
    {
        //if(this.isPlaceable)
            this.towerPrefab = towerType;
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
        if (this.towerPrefab == null) { return; }

        if (this.isPlaceable && this.bank.CurrentBalance > 0) // if(this.gridManager[this.tileCoordinates].IsWalkable && !this.pathFinder.WillBlockPath(this.tileCoordinates))
        {
            this.StartCoroutine(BuildTower());
        }        
    }

    private IEnumerator BuildTower()
    {
        Vector3 placementPos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
        
        GameObject tower = Instantiate<GameObject>(this.towerPrefab, placementPos, Quaternion.identity);
        DefenseTower defenseTower = tower.GetComponent<DefenseTower>();
        
        defenseTower.CanShoot = false;
        defenseTower.SetTileUnderneath(this);

        this.isPlaceable = false;
        //this.gridManager.BlockNode(this.tileCoordinates);
        //this.pathfinder.NotifyReceivers(); // For dynamic changing of path

        this.bank.Withdraw(defenseTower.GoldCost);

        //tower.transform.GetChild(1).gameObject.SetActive(false);
        foreach (Transform item in defenseTower.FinishedTower)
        {
            item.gameObject.SetActive(false);
        }

        defenseTower.Phase1.gameObject.SetActive(true);

        yield return new WaitForSeconds(defenseTower.BuildTime);

        defenseTower.Phase1.gameObject.SetActive(false);
        defenseTower.Phase2.gameObject.SetActive(true);

        yield return new WaitForSeconds(defenseTower.BuildTime);

        defenseTower.Phase3.gameObject.SetActive(true);

        yield return new WaitForSeconds(defenseTower.BuildTime);

        defenseTower.Phase2.gameObject.SetActive(false);
        defenseTower.Phase4.gameObject.SetActive(true);

        if (defenseTower.Phase5 != null)
        {
            yield return new WaitForSeconds(defenseTower.BuildTime);

            defenseTower.Phase5.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(defenseTower.BuildTime);

        defenseTower.Phase3.gameObject.SetActive(false);
        defenseTower.Phase4.gameObject.SetActive(false);

        if (defenseTower.Phase5 != null)
        {
            defenseTower.Phase5.gameObject.SetActive(false);
        }

        foreach (Transform item in defenseTower.FinishedTower)
        {
            item.gameObject.SetActive(true);
        }

        defenseTower.CanShoot = true;

        /*if (defenseTower != null)
        {
            tower.transform.GetChild(1).gameObject.SetActive(true);
            defenseTower.CanShoot = true;
        }*/
    }

    private void OnDestroy()
    {
        Messenger<GameObject>.RemoveListener("SetTowerType", SetTowerType);
    }
}
