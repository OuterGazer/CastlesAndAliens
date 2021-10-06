using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = default;
    [SerializeField] int goldPenalty = default;


    private TargetLocator originTower;
    private Bank bank;

    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocator originTower)
    {
        this.hasBeenTargeted = true;
        this.originTower = originTower;
    }

    private void Start()
    {
        this.bank = GameObject.FindObjectOfType<Bank>();
    }

    // OnEnable is called every time the object is enabled
    void OnEnable()
    {
        this.originTower = default;
    }

    public void RewardGold()
    {
        if (this.bank != null)
            this.bank.Deposit(this.goldReward);
    }

    public void StealGold()
    {
        if (this.bank != null)
            this.bank.Withdraw(this.goldPenalty);
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
