using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private TargetLocator originTower;


    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocator originTower)
    {
        this.hasBeenTargeted = true;
        this.originTower = originTower;
    }

    // OnEnable is called every time the object is enabled
    void OnEnable()
    {
        this.originTower = default;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDisable()
    {
        if (this.hasBeenTargeted)
        {
            this.hasBeenTargeted = false;
            this.originTower.ClearTarget();
        }            
    }
}
