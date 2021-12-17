using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefenseTower))]
public class TowerHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    private int currentHitPoints = 0;

    [SerializeField] int balistaEnemyDamage = default;
    [SerializeField] int cannonEnemyDamage = default;
    [SerializeField] int missileEnemyDamage = default;

    private void Start()
    {
        this.currentHitPoints = maxHitPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kamikaze"))
        {
            switch (other.name)
            {
                case string name when name.Contains("arrow"):
                    ProcessDamage(this.balistaEnemyDamage);
                    break;

                case string name when name.Contains("Cannon"):
                    ProcessDamage(this.cannonEnemyDamage);
                    break;

                case string name when name.Contains("rocket"):
                    ProcessDamage(this.missileEnemyDamage);
                    break;
            }
        }
    }

    private void ProcessDamage(int inDamage)
    {
        this.currentHitPoints -= inDamage;

        if(this.currentHitPoints <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
