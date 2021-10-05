using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallistaBolt : MonoBehaviour
{
    [SerializeField] private float movementSpeed = default;

    private Rigidbody boltRB;
    private TargetLocator shotOrigin;
    public void SetShotOrigin(TargetLocator origin)
    {
        this.shotOrigin = origin;
    }

    private bool canShoot = false;
    public void ShootBolt()
    {
        this.canShoot = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.boltRB = this.gameObject.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if(this.canShoot)
            this.boltRB.MovePosition(this.boltRB.position + this.boltRB.transform.forward * this.movementSpeed * Time.fixedDeltaTime);
    }
    

    private void OnTriggerEnter(Collider other)
    {

        GameObject.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        if (this.shotOrigin != null)
            this.shotOrigin.SendMessage("ChargeNextBolt");
    }
}
