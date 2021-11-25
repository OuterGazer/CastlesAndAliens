using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [Header("Building Parts")]
    [SerializeField] Transform[] finishedTower;
    public Transform[] FinishedTower => this.finishedTower;
    [SerializeField] Transform phase1;
    public Transform Phase1 => this.phase1;
    [SerializeField] Transform phase2;
    public Transform Phase2 => this.phase2;
    [SerializeField] Transform phase3;
    public Transform Phase3 => this.phase3;
    [SerializeField] Transform phase4;
    public Transform Phase4 => this.phase4;
    [SerializeField] Transform phase5;
    public Transform Phase5 => this.phase5;

    [Header("Tower Settings")]
    [SerializeField] int goldCost = default;
    public int GoldCost => this.goldCost;

    [SerializeField] float buildTime = default;
    public float BuildTime => this.buildTime;

    [SerializeField] int dangerLevel = default;
    public int DangerLevel => this.dangerLevel;

    private int dangerRange;

    private List<Waypoint> affectedPathtiles;


    private Rigidbody towerRB;
    private TargetLocator targetLocator;
    private List<TargetLocatorAlien> originAlien = new List<TargetLocatorAlien>();
    private LayerMask pathMask;
    private MouseInteraction tileUnderneath;

    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocatorAlien originAlien)
    {
        this.hasBeenTargeted = true;
        this.originAlien.Add(originAlien);
    }

    private bool canShoot = false;
    public bool CanShoot
    {
        get { return this.canShoot; }
        set { this.canShoot = value; }
    }

    private void Awake()
    {
        this.towerRB = this.gameObject.GetComponent<Rigidbody>();

        this.targetLocator = this.gameObject.GetComponent<TargetLocator>();
        this.dangerRange = (int)this.targetLocator.Range;

        this.pathMask = LayerMask.GetMask("Roads");

        //this.originAlien = default;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.affectedPathtiles = new List<Waypoint>();

        Collider[] tilesToAffect = Physics.OverlapSphere(this.gameObject.transform.position, this.dangerRange, this.pathMask);

        foreach(Collider item in tilesToAffect)
        {
            this.affectedPathtiles.Add(item.GetComponentInChildren<Waypoint>());
        }

        foreach(Waypoint item in this.affectedPathtiles)
        {
            item.IncreaseDangerLevel(this.dangerLevel);
        }
    }

    public void SetTileUnderneath(MouseInteraction tileUnderneath)
    {
        this.tileUnderneath = tileUnderneath;
    }



    private void OnDestroy()
    {
        this.tileUnderneath.SetIsPlaceable(true);

        foreach (Waypoint item in this.affectedPathtiles)
        {
            item.DecreaseDangerLevel(this.dangerLevel);
        }

        if (this.hasBeenTargeted)
        {
            this.hasBeenTargeted = false;
            foreach(TargetLocatorAlien item in this.originAlien)
            {
                item.ClearTarget();
            }
            
        }
    }
}
