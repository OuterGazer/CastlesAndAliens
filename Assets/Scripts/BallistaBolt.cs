using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    [SerializeField] private float blastRadius = default;

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

    // Start is called before the first frame update
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
           (other.gameObject.CompareTag("Player Weapon") && (other.gameObject.CompareTag("Kamikaze") || other.gameObject.CompareTag("Basic Enemy"))) ||
           other.gameObject.CompareTag("Ground"))
        {
            if(this.gameObject.name.Contains("Cannon"))
            {
                Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.blastRadius, this.enemyMask);

                if (enemies.Length > 0)
                {
                    foreach (Collider item in enemies)
                    {
                        item.GetComponentInParent<EnemyHealth>().ProcessDamage(1);
                    }
                }
            }

            this.gameObject.SetActive(false);
        }
        
    }

    private void OnDisable()
    {
        this.canShoot = false;

        if (this.shotOrigin != null)
            this.shotOrigin.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);

        if (this.shotOriginAlien != null)
            this.shotOriginAlien.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);
    }
}
