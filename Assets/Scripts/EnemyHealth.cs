using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    private int currentHitPoints = 0;

    private void OnEnable()
    {
        this.currentHitPoints = maxHitPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player Weapon"))
        {
            ProcessDamage();
        }
    }

    private void ProcessDamage()
    {
        this.currentHitPoints -= 1;

        if(this.currentHitPoints <= 0)
        {
            this.gameObject.GetComponent<Enemy>().RewardGold();
            this.gameObject.SetActive(false);
        }
    }
}
