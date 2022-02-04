using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = default;
    //[SerializeField] int goldPenalty = default;


    private List<TargetLocator> originTower = new List<TargetLocator>();
    private Bank bank;

    private bool hasBeenTargeted = false;
    public bool HasBeenTargeted => this.hasBeenTargeted;
    public void SetHasBeenTargeted(TargetLocator originTower, bool isInRange)
    {
        if (!isInRange)
        {
            this.originTower.Remove(originTower);
            return;
        }
        else
        {
            this.hasBeenTargeted = true;
            this.originTower.Add(originTower);
        }
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

    /*public void StealGold()
    {
        if (this.bank != null)
            this.bank.Withdraw(this.goldPenalty);
    }*/

    public void AnnoyRoyalty()
    {
        Messenger<string>.Broadcast("AnnoyRoyalty", this.gameObject.name, MessengerMode.DONT_REQUIRE_LISTENER);
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

        if(GameObject.FindObjectOfType<ObjectPool>() != null)
            GameObject.FindObjectOfType<ObjectPool>().CurEnemyCount--;
    }
}
