using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = default;
    [SerializeField] int goldPenalty = default;


    private List<TargetLocator> originTower = new List<TargetLocator>();
    private Bank bank;

    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocator originTower, bool isInRange)
    {
        this.hasBeenTargeted = isInRange;

        if (!isInRange)
        {
            this.originTower.Remove(originTower);
            return;
        }

        this.originTower.Add(originTower);
    }

    private void Start()
    {
        this.bank = GameObject.FindObjectOfType<Bank>();
    }

    // OnEnable is called every time the object is enabled
    void OnEnable()
    {
        this.originTower.Clear();

        //this.gameObject.GetComponentInChildren<MeshCollider>().enabled = true;
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
            foreach(TargetLocator item in this.originTower)
            {
                item.ClearTarget();
            }
            
        }

        //this.gameObject.GetComponentInChildren<MeshCollider>().enabled = false;
    }
}
