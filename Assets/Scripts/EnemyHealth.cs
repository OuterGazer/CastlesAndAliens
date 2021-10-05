using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int hitPoints = default;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player Weapon"))
        {
            ProcessDamage();
        }
    }

    private void ProcessDamage()
    {
        this.hitPoints -= 1;

        if(this.hitPoints <= 0)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
