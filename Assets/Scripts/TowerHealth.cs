using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefenseTower))]
public class TowerHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    private int currentHitPoints = 0;

    [SerializeField] AudioClip deathSFX;

    [SerializeField] int balistaEnemyDamage = default;
    [SerializeField] int cannonEnemyDamage = default;
    [SerializeField] int missileEnemyDamage = default;

    [SerializeField] private Transform lifeBar;

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
            AudioSource.PlayClipAtPoint(this.deathSFX, Camera.main.transform.position);

            GameObject.Destroy(this.gameObject);
        }

        float subtractLifeBar = (this.currentHitPoints * 1.20f) / this.maxHitPoints;

        this.lifeBar.transform.localScale = new Vector3(subtractLifeBar,
                                                        this.lifeBar.transform.localScale.y,
                                                        this.lifeBar.transform.localScale.z);
    }
}
