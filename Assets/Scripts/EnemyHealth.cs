using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = default;
    [SerializeField] int healthIncreaseAfterDeath = default;
    private int currentHitPoints = 0;

    [SerializeField] AudioClip deathSFX;
    [SerializeField] AudioClip coinsEarnedSFX;

    [SerializeField] int balistaTowerDamage = default;
    [SerializeField] int cannonTowerDamage = default;
    [SerializeField] int catapultTowerDamage = default;

    [SerializeField] private Transform lifeBar;

    private void OnEnable()
    {
        this.currentHitPoints = maxHitPoints;

        this.lifeBar.transform.localScale = new Vector3(0.45f,
                                                        this.lifeBar.transform.localScale.y,
                                                        this.lifeBar.transform.localScale.z);
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

                case string name when name.Contains("Catapult"):
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
            AudioSource.PlayClipAtPoint(this.deathSFX, Camera.main.transform.position);
            AudioSource.PlayClipAtPoint(this.coinsEarnedSFX, Camera.main.transform.position);

            this.gameObject.GetComponent<Enemy>().RewardGold();
            this.maxHitPoints += this.healthIncreaseAfterDeath; this.gameObject.SetActive(false);
        }

        float subtractLifeBar = (this.currentHitPoints * 0.45f) / this.maxHitPoints;

        this.lifeBar.transform.localScale = new Vector3(subtractLifeBar,
                                                        this.lifeBar.transform.localScale.y,
                                                        this.lifeBar.transform.localScale.z);
    }
}
