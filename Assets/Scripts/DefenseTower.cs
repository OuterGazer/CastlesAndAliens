using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseTower : MonoBehaviour
{
    [SerializeField] int goldCost = default;
    public int GoldCost => this.goldCost;

    [SerializeField] float buildTime = default;
    public float BuildTime => this.buildTime;

    private bool canShoot = false;
    public bool CanShoot
    {
        get { return this.canShoot; }
        set { this.canShoot = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
