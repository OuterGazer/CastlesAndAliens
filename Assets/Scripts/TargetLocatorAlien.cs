using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class TargetLocatorAlien : MonoBehaviour
{
    [SerializeField] Transform pedestal;
    [SerializeField] Transform weapon;
    [SerializeField] Transform bolt;
    [SerializeField] Transform extraBolt;
    private Transform shootingBolt;
    private Transform extraShootingBolt;
    [SerializeField] float range = default;
    public float Range => this.range;
    [SerializeField] float shootingCooldown = default;
    private float timeToNextShot = 0;

    [SerializeField] Transform target; // Only for debugging purposes as targets will be added programatically
    private LayerMask towerMask;
    
    private AudioSource audioSource;
    [SerializeField] AudioClip balistaShot;
    [SerializeField] AudioClip cannonShot;
    [SerializeField] AudioClip missileShot;

    private bool isTargetAcquired = false;

    private void Awake()
    {
        this.towerMask = LayerMask.GetMask("DefenseTower");
        this.audioSource = this.gameObject.GetComponent<AudioSource>();

        CreateBoltPool(this.bolt, out this.shootingBolt);

        if (this.extraBolt != null)
        {
            CreateBoltPool(this.extraBolt, out this.extraShootingBolt);
        }
    }

    private void CreateBoltPool(Transform bolt, out Transform shootingBolt)
    {
        shootingBolt = GameObject.Instantiate<Transform>(bolt, bolt.position, bolt.rotation);
        SetCorrectBoltScale(shootingBolt, true);
        shootingBolt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        this.timeToNextShot -= Time.deltaTime;

        FindClosestTarget();

        AimWeapon();

        ShootWeapon();
    }

    private void FindClosestTarget()
    {
        if (this.isTargetAcquired) { return; }

        //DefenseTower[] enemies = GameObject.FindObjectsOfType<DefenseTower>();

        Collider[] enemyColliders = Physics.OverlapSphere(this.gameObject.transform.position, this.range, this.towerMask);

        if (enemyColliders.Length == 0) { return; }

        DefenseTower[] enemies = new DefenseTower[enemyColliders.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemyColliders[i].GetComponentInParent<DefenseTower>();
        }

        DefenseTower enemyToAim = default;

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

        if(this.timeToNextShot <= 0)
        {
            //Transform bolt = GameObject.Instantiate<Transform>(this.bolt, this.bolt.position, this.bolt.rotation);
            SetShootingBoltToStandard(this.bolt, this.shootingBolt);

            EjectBolt(this.bolt, this.shootingBolt);
            PlayShootingSFX();

            this.timeToNextShot = this.shootingCooldown;

            if (this.extraBolt != null)
                this.StartCoroutine(ShootExtraBolt());
        }
    }

    private void PlayShootingSFX()
    {
        if (this.gameObject.name.Contains("Balista"))
            this.audioSource.PlayOneShot(this.balistaShot);
        else if (this.gameObject.name.Contains("Cannon"))
            this.audioSource.PlayOneShot(this.cannonShot);
        else if (this.gameObject.name.Contains("Missile"))
            this.audioSource.PlayOneShot(this.missileShot);
    }

    private void EjectBolt(Transform inBolt, Transform shootingBolt)
    {
        shootingBolt.SetParent(null);
        SetCorrectBoltScale(shootingBolt, true);

        BallistaBolt bolt = shootingBolt.GetComponent<BallistaBolt>();
        bolt.ShootBolt();
        bolt.SetShotOrigin(this);

        inBolt.gameObject.SetActive(false);
    }

    private IEnumerator ShootExtraBolt()
    {
        yield return new WaitForSeconds(0.15f);

        SetShootingBoltToStandard(this.extraBolt, this.extraShootingBolt);

        EjectBolt(this.extraBolt, this.extraShootingBolt);
    }

    private void SetShootingBoltToStandard(Transform bolt, Transform shootingBolt)
    {
        shootingBolt.gameObject.SetActive(true);
        SetCorrectBoltScale(shootingBolt, false);

        shootingBolt.SetParent(this.weapon);
        
        shootingBolt.position = bolt.position;
        shootingBolt.rotation = bolt.rotation;
    }

    private void SetCorrectBoltScale(Transform bolt, bool mustDecreaseSize)
    {
        Vector3 tempScale1 = this.gameObject.transform.GetChild(0).localScale;
        Vector3 tempScale2 = this.gameObject.transform.GetChild(0).GetChild(0).localScale;
        Vector3 tempScale3 = bolt.transform.localScale;
        //bolt.transform.localScale = new Vector3(0.395f, 0.395f, 0.1975f);

        if (mustDecreaseSize)
        {
            bolt.transform.localScale = new Vector3(tempScale1.x * tempScale2.x * tempScale3.x,
                                                tempScale1.y * tempScale2.y * tempScale3.y,
                                                tempScale1.z * tempScale2.z * tempScale3.z);
        }
        else
        {
            bolt.transform.localScale = new Vector3(tempScale3.x / tempScale2.x / tempScale1.x,
                                                tempScale3.y / tempScale2.y / tempScale1.y,
                                                tempScale3.z / tempScale2.z / tempScale1.z);
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

    private void OnDisable()
    {
        if (!this.bolt.gameObject.activeSelf)
            this.bolt.gameObject.SetActive(true);
    }
}
