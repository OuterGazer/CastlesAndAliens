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

    [SerializeField] private GameObject rocketFire;
    [SerializeField] private GameObject rocketExplosion;
    [SerializeField] private GameObject cannonExplosion;

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


    private float rocketCounter;

    private bool canShoot = false;
    public void ShootBolt()
    {
        this.canShoot = true;

        if (this.gameObject.name.Contains("rocket"))
        {
            this.rocketFire.SetActive(true);
            this.rocketCounter = 0;
        }
            
    }


    void Awake()
    {
        this.boltRB = this.gameObject.GetComponent<Rigidbody>();

        this.enemyMask = LayerMask.GetMask("Enemy");
    }

    void FixedUpdate()
    {
        if (this.canShoot)
        {
            this.boltRB.MovePosition(this.boltRB.position + this.boltRB.transform.forward * this.movementSpeed * Time.fixedDeltaTime);

            if (this.gameObject.name.Contains("rocket"))
            {
                this.rocketCounter += Time.fixedDeltaTime;

                if(this.rocketCounter > 2.0f)
                {
                    AudioSource.PlayClipAtPoint(this.missileAlienExplosion, Camera.main.transform.position);
                    GameObject.Instantiate<GameObject>(this.rocketExplosion, this.gameObject.transform.position, Quaternion.identity);
                    this.rocketCounter = 0;
                    this.gameObject.SetActive(false);
                }

            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("Tower Base") && (this.gameObject.CompareTag("Kamikaze") || this.gameObject.CompareTag("Basic Enemy"))) ||
           (this.gameObject.CompareTag("Player Weapon") && (other.gameObject.CompareTag("Kamikaze") || other.gameObject.CompareTag("Basic Enemy"))) ||
           other.gameObject.CompareTag("Ground")) 
        {
            if(this.gameObject.name.Contains("Cannon") && this.gameObject.CompareTag("Player Weapon"))
            {
                GameObject.Instantiate<GameObject>(this.cannonExplosion, this.gameObject.transform.position, Quaternion.identity);

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
                GameObject.Instantiate<GameObject>(this.rocketExplosion, this.gameObject.transform.position, Quaternion.identity);
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
            
            if (this.gameObject.name.Contains("arrow"))
                AudioSource.PlayClipAtPoint(this.balistaTowerBolt, Camera.main.transform.position);
        }
            

        if (this.shotOriginAlien != null)
        {
            this.shotOriginAlien.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);

            if (this.gameObject.name.Contains("arrow"))
                AudioSource.PlayClipAtPoint(this.balistaAlienBolt, Camera.main.transform.position);
        }
            
    }
}
