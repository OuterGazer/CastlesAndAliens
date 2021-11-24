using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
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
    private TargetLocatorAlien originAlien;
    private LayerMask pathMask;

    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocatorAlien originAlien)
    {
        this.hasBeenTargeted = true;
        this.originAlien = originAlien;
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

        this.originAlien = default;
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

    private void OnDestroy()
    {
        foreach (Waypoint item in this.affectedPathtiles)
        {
            item.DecreaseDangerLevel(this.dangerLevel);
        }

        if (this.hasBeenTargeted)
        {
            this.hasBeenTargeted = false;
            this.originAlien.ClearTarget();
        }
    }
}
