using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    [SerializeField] int healthIncreaseAfterDeath = default;
    private int currentHitPoints = 0;

    [SerializeField] int balistaTowerDamage = default;
    [SerializeField] int cannonTowerDamage = default;
    [SerializeField] int catapultTowerDamage = default;

    private void OnEnable()
    {
        this.currentHitPoints = maxHitPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player Weapon"))
        {
            switch (other.name)
            {
                case string name when name.Contains("arrow"):
                    ProcessDamage(this.balistaTowerDamage);
                    break;

                case string name when name.Contains("Cannon"):
                    ProcessDamage(this.cannonTowerDamage);
                    break;

                case string name when name.Contains("rocket"):
                    ProcessDamage(this.catapultTowerDamage);
                    break;
            }
        }
    }

    public void ProcessDamage(int inDamage)
    {
        this.currentHitPoints -= inDamage;

        if(this.currentHitPoints <= 0)
        {
            this.gameObject.GetComponent<Enemy>().RewardGold();
            this.maxHitPoints += this.healthIncreaseAfterDeath;
            this.gameObject.SetActive(false);
        }
    }
}
