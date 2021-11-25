using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRock : MonoBehaviour
{
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

    private bool canShoot = false;
    private bool canThrowRock = false;

    public void ShootRock()
    {
        this.canShoot = true;        
    }

    // Start is called before the first frame update
    void Awake()
    {
        this.rockRB = this.gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (this.canShoot)
        {
            this.catapult.Rotate(Vector3.right, this.throwSpeed * Time.deltaTime);

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
            if(this.catapult.localRotation.eulerAngles.x > this.minAngle)
                this.catapult.Rotate(-Vector3.right, this.recoilSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.CompareTag("Tower Base") && (this.gameObject.CompareTag("Kamikaze") || this.gameObject.CompareTag("Basic Enemy"))) ||
           (other.gameObject.CompareTag("Player Weapon") && (other.gameObject.CompareTag("Kamikaze") || other.gameObject.CompareTag("Basic Enemy"))) ||
           other.gameObject.CompareTag("Ground"))
        {
            //GameObject.Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        
    }

    private void OnDisable()
    {
        this.rockRB.isKinematic = true;
        this.rockRB.useGravity = false;

        if (this.shotOrigin != null)
            this.shotOrigin.SendMessage("ChargeNextBolt", SendMessageOptions.DontRequireReceiver);
    }
}
