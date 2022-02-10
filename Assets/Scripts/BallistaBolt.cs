using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    [SerializeField] private float blastRadius = default;

    [SerializeField] private AudioClip balistaTowerBolt;
    [SerializeField] private AudioClip balistaAlienBolt;
    [SerializeField] private AudioClip cannonTowerExplosion;
    [SerializeField] private AudioClip cannonAlienHit;
    [SerializeField] private AudioClip missileAlienExplosion;

    private Rigidbody boltRB;
    private LayerMask enemyMask;
    private TargetLocator shotOrigin;
    private TargetLocatorAlien shotOriginAlien;
    public void SetShotOrigin(TargetLocator origin)
    {
        this.shotOrigin = origin;
    }

    public void SetShotOrigin(TargetLocatorAlien origin)
    {
        this.shotOriginAlien = origin;
    }

    private bool canShoot = false;
    public void ShootBolt()
    {
        this.canShoot = true;
    }


    void Awake()
    {
        this.boltRB = this.gameObject.GetComponent<Rigidbody>();

        this.enemyMask = LayerMask.GetMask("Enemy");
    }

    void FixedUpdate()
    {
        if(this.canShoot)
            this.boltRB.MovePosition(this.boltRB.position + this.boltRB.transform.forward * this.movementSpeed * Time.fixedDeltaTime);
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("Tower Base") && (this.gameObject.CompareTag("Kamikaze") || this.gameObject.CompareTag("Basic Enemy"))) ||
           (this.gameObject.CompareTag("Player Weapon") && (other.gameObject.CompareTag("Kamikaze") || other.gameObject.CompareTag("Basic Enemy"))) ||
           other.gameObject.CompareTag("Ground")) 
        {
            if(this.gameObject.name.Contains("Cannon") && this.gameObject.CompareTag("Player Weapon"))
            {
                Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.blastRadius, this.enemyMask);

                if (enemies.Length > 0)
                {
                    foreach (Collider item in enemies)
                    {
                        item.GetComponentInParent<EnemyHealth>().ProcessDamage(1);
                    }
                }

                AudioSource.PlayClipAtPoint(this.cannonTowerExplosion, Camera.main.transform.position);
            }

            if(other.gameObject.CompareTag("Tower Base") && this.gameObject.name.Contains("Cannon"))
            {
                AudioSource.PlayClipAtPoint(this.cannonAlienHit, Camera.main.transform.position);
            }
            else if(other.gameObject.CompareTag("Tower Base") && this.gameObject.name.Contains("rocket"))
            {
                AudioSource.PlayClipAtPoint(this.missileAlienExplosion, Camera.main.transform.position);
            }

            this.gameObject.SetActive(false);
        }        
    }

    private void OnDisable()
    {
        this.canShoot = false;

        if (this.shotOrigin != null)
        {
            this.shotOrigin.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);
            
            if (!this.gameObject.name.Contains("Cannon"))
                AudioSource.PlayClipAtPoint(this.balistaTowerBolt, Camera.main.transform.position);
        }
            

        if (this.shotOriginAlien != null)
        {
            this.shotOriginAlien.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);
            AudioSource.PlayClipAtPoint(this.balistaAlienBolt, Camera.main.transform.position);
        }
            
    }
}
