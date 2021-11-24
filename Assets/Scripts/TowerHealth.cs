using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefenseTower))]
public class TowerHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    private int currentHitPoints = 0;

    private void Start()
    {
        this.currentHitPoints = maxHitPoints;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Kamikaze"))
        {
            ProcessDamage();
        }
    }

    private void ProcessDamage()
    {
        this.currentHitPoints -= 1;

        if(this.currentHitPoints <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
