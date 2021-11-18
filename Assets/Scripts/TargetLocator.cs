using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefenseTower))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform pedestal;
    [SerializeField] Transform weapon;
    [SerializeField] Transform bolt;
    [SerializeField] float range = default;
    public float Range => this.range;
    [SerializeField] float shootingCooldown = default;
    private float timeToNextShot = 0;

    [SerializeField] Transform target; // Only for debugging purposes as targets will be added programatically
    private DefenseTower defenseTower;

    private bool isTargetAcquired = false;

    private void Awake()
    {
        this.defenseTower = this.gameObject.GetComponent<DefenseTower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.defenseTower.CanShoot) { return; }

        FindClosestTarget();

        AimWeapon();

        ShootWeapon();
    }

    private void FindClosestTarget()
    {
        if (this.isTargetAcquired) { return; }

        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();

        if (enemies.Length == 0) { return; }

        Enemy enemyToAim = default;

        float distanceToClosestEnemy = Mathf.Infinity;

        for(int i = 0; i < enemies.Length; i++)
        {
            float distanceToCurrentEnemy = (enemies[i].transform.position - this.gameObject.transform.position).sqrMagnitude;

            if (distanceToCurrentEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = distanceToCurrentEnemy;
                enemyToAim = enemies[i];
            }
        }

        this.isTargetAcquired = true;
        enemyToAim.SetHasBeenTargeted(this);
        this.target = enemyToAim.transform;
    }

    private void AimWeapon()
    {
        /*LookAtHorizontal();
        LookAtVertical();*/

        if (!this.isTargetAcquired) { return; }

        AimToEnemy(this.pedestal, Vector3.up);
        AimToEnemy(this.weapon, this.weapon.right);
    }
    
    private void AimToEnemy(Transform objectToRotate, Vector3 axis)
    {
        //if (this.target == null) { return; }

        Vector3 newPos = this.target.position - objectToRotate.position;
        Vector3 newPosProj = Vector3.ProjectOnPlane(newPos, axis);

        float deltaAngle = Vector3.SignedAngle(objectToRotate.forward, newPosProj, axis);

        if(axis == Vector3.up)
            objectToRotate.Rotate(0, deltaAngle, 0);
        else
            objectToRotate.Rotate(deltaAngle, 0, 0);
    }

    /*private void LookAtHorizontal()
    {
        Vector3 newPos = this.target.position - this.pedestal.position;
        Vector3 newPosProj = Vector3.ProjectOnPlane(newPos, Vector3.up);

        float deltaAngle = Vector3.SignedAngle(this.pedestal.forward, newPosProj, Vector3.up);

        this.pedestal.Rotate(0, deltaAngle, 0);
    }

    private void LookAtVertical()
    {
        Vector3 newPos = this.target.position - this.weapon.position;
        Vector3 newPosProj = Vector3.ProjectOnPlane(newPos, this.weapon.right);

        float deltaAngle = Vector3.SignedAngle(this.weapon.forward, newPosProj, this.weapon.right);

        this.weapon.Rotate(deltaAngle, 0, 0);
    }*/

    private void ShootWeapon()
    {
        if (!this.isTargetAcquired) { return; }

        float distanceToEnemySqr = (this.target.transform.position - this.gameObject.transform.position).sqrMagnitude;

        if (distanceToEnemySqr > (this.range * this.range)) { ClearTarget(); return; }

        this.timeToNextShot -= Time.deltaTime;

        if(this.timeToNextShot <= 0)
        {
            Transform bolt = GameObject.Instantiate<Transform>(this.bolt, this.bolt.position, this.bolt.rotation);

            bolt.SetParent(null);

            bolt.GetComponent<BallistaBolt>().ShootBolt();
            bolt.GetComponent<BallistaBolt>().SetShotOrigin(this);

            this.bolt.gameObject.SetActive(false);

            this.timeToNextShot = this.shootingCooldown;
        }
    }

    public void ChargeNextBolt()
    {
        this.StartCoroutine(LoadBolt());
    }

    private IEnumerator LoadBolt()
    {
        yield return new WaitForSeconds(0.5f);

        this.bolt.gameObject.SetActive(true);
    }

    public void ClearTarget()
    {
        this.isTargetAcquired = false;
        this.target = default;
    }
}
