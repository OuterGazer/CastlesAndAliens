using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRock : MonoBehaviour
{
    [SerializeField] private float blastRadius = default;
    [SerializeField] private float throwForce = default;
    [SerializeField] private float throwSpeed = default;
    [SerializeField] private float recoilSpeed = default;
    [SerializeField] private float minAngle = 15f;
    [SerializeField] private float maxAngle = 90f;
    [SerializeField] Transform catapult;

    private Rigidbody rockRB;
    private TargetLocator shotOrigin;
    private TargetLocatorAlien shotOriginAlien;
    public void SetShotOrigin(TargetLocator origin)
    {
        this.shotOrigin = origin;
    }
    private LayerMask enemyMask;

    private bool canShoot = false;
    private bool canThrowRock = false;
    private bool isRecoiling = false;
    public bool IsRecoiling => this.isRecoiling;

    public void ShootRock()
    {
        this.canShoot = true;
        this.isRecoiling = true;
    }

    // Start is called before the first frame update
    void Awake()
    {
        this.rockRB = this.gameObject.GetComponent<Rigidbody>();

        this.enemyMask = LayerMask.GetMask("Enemy");
    }

    private void Update()
    {
        if (this.canShoot)
        {
            ActivateCatapult(this.maxAngle, this.throwSpeed);

            //this.catapult.Rotate(Vector3.right, this.throwSpeed * Time.deltaTime);

            if (this.catapult.localRotation.eulerAngles.x >= this.maxAngle)
                this.canThrowRock = true;

            if (this.canThrowRock)
            {
                this.gameObject.transform.SetParent(null);

                this.rockRB.isKinematic = false;
                this.rockRB.useGravity = true;
                this.rockRB.velocity = Vector3.zero;

                this.rockRB.AddRelativeForce(Vector3.forward * this.throwForce, ForceMode.Impulse);
                this.canThrowRock = false;
                this.canShoot = false;
            }
        }
        else
        {
            //if (this.catapult.localRotation.eulerAngles.x - this.minAngle <= 1f) { this.isRecoiling = false; return; }

            ActivateCatapult(this.minAngle, this.recoilSpeed);

            /*if (this.catapult.localRotation.eulerAngles.x >= this.minAngle)
                this.catapult.Rotate(-Vector3.right, this.recoilSpeed * Time.deltaTime);
            else
                this.catapult.localRotation = Quaternion.Euler(this.minAngle, this.catapult.localRotation.eulerAngles.y, this.catapult.localRotation.eulerAngles.z);*/
        }
    }

    private void ActivateCatapult(float targetAngle, float rotSpeed)
    {
        Vector3 curRot = new Vector3(targetAngle, this.catapult.localRotation.eulerAngles.y,
                                     this.catapult.localRotation.eulerAngles.z);

        Quaternion rotToApply = Quaternion.RotateTowards(this.catapult.localRotation, Quaternion.Euler(curRot), rotSpeed * Time.deltaTime);
        this.catapult.localRotation = rotToApply;
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("Tower Base") && (this.gameObject.CompareTag("Kamikaze") || this.gameObject.CompareTag("Basic Enemy"))) ||
           (other.gameObject.CompareTag("Player Weapon") && (other.gameObject.CompareTag("Kamikaze") || other.gameObject.CompareTag("Basic Enemy"))) ||
           other.gameObject.CompareTag("Ground"))
        {
            Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, this.blastRadius, this.enemyMask);

            if (enemies.Length > 0)
            {
                foreach (Collider item in enemies)
                {
                    item.GetComponentInParent<EnemyHealth>().ProcessDamage(2);
                }
            }            

            this.gameObject.SetActive(false);
        }
        
    }

    private void OnDisable()
    {
        this.rockRB.isKinematic = true;
        this.rockRB.useGravity = false;
        this.isRecoiling = false;

        if (this.shotOrigin != null)
            this.shotOrigin.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);
    }
}
