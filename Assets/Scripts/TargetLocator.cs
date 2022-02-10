using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DefenseTower))]
public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform pedestal;
    [SerializeField] Transform weapon;
    [SerializeField] Transform bolt;
    private Transform shootingBolt;
    [SerializeField] float range = default;
    public float Range => this.range;
    [SerializeField] float closeRangeLimit = default;
    public float CloseRangeLimit => this.closeRangeLimit;
    [SerializeField] float shootingCooldown = default;
    private float timeToNextShot = 0;

    private string towerName;

    Transform target;
    private DefenseTower defenseTower;
    private LayerMask enemyMask;
    private CatapultRock currentInstance;

    private AudioSource audioSource;
    [SerializeField] AudioClip balistaShot;
    [SerializeField] AudioClip cannonShot;
    [SerializeField] AudioClip catapultShot;

    private bool isTargetAcquired = false;

    private void Awake()
    {
        this.defenseTower = this.gameObject.GetComponent<DefenseTower>();
        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.enemyMask = LayerMask.GetMask("Enemy");

        this.shootingBolt = GameObject.Instantiate<Transform>(this.bolt, this.bolt.position, this.bolt.rotation);
        this.shootingBolt.gameObject.SetActive(false);

        this.towerName = this.gameObject.name;
        this.currentInstance = this.shootingBolt.GetComponent<CatapultRock>();
    }

    private void Start()
    {
        ClearTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.defenseTower.CanShoot) { return; }

        this.timeToNextShot -= Time.deltaTime;

        FindClosestTarget();

        CheckIfTargetIsInRange();

        AimWeapon();

        ShootWeapon();
    }

    private void FindClosestTarget()
    {
        if (this.isTargetAcquired) { return; }

        //Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Collider[] enemyColliders = Physics.OverlapSphere(this.gameObject.transform.position, this.range, this.enemyMask);

        if (enemyColliders.Length == 0) { ClearTarget(); return; }

        Enemy[] enemies = new Enemy[enemyColliders.Length];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemies[i] = enemyColliders[i].GetComponentInParent<Enemy>();
        }        

        Enemy enemyToAim = default;

        float distanceToClosestEnemy = Mathf.Infinity;

        for(int i = 0; i < enemies.Length; i++)
        {
            float distanceToCurrentEnemy = (enemies[i].transform.position - this.gameObject.transform.position).sqrMagnitude;

            if ((distanceToCurrentEnemy < distanceToClosestEnemy) &&
                distanceToCurrentEnemy > (this.closeRangeLimit * this.closeRangeLimit))
            {
                distanceToClosestEnemy = distanceToCurrentEnemy;
                enemyToAim = enemies[i];
            }
        }

        if(enemyToAim != null)
        {
            this.isTargetAcquired = true;
            enemyToAim.SetHasBeenTargeted(this, true);
            this.target = enemyToAim.transform;
        }        
    }

    private void CheckIfTargetIsInRange()
    {
        if (!this.isTargetAcquired) { return; }

        float distanceToCurrentEnemy = (this.target.transform.position - this.gameObject.transform.position).sqrMagnitude;

        if ((distanceToCurrentEnemy > (this.range * this.range)) ||
            (distanceToCurrentEnemy < (this.closeRangeLimit * this.closeRangeLimit)))
        {
            this.target.gameObject.GetComponent<Enemy>().SetHasBeenTargeted(this, false);
            ClearTarget();
        }
    }

    private void AimWeapon()
    {
        /*LookAtHorizontal();
        LookAtVertical();*/

        if (!this.isTargetAcquired) { return; }

        if (this.towerName.Equals("Catapult Tower(Clone)") && this.currentInstance.IsRecoiling) { return; }

        AimToEnemy(this.pedestal, Vector3.up);

        if(this.towerName.Equals("Catapult Tower(Clone)")) { return; }

        AimToEnemy(this.weapon, this.weapon.right);
    }
    
    private void AimToEnemy(Transform objectToRotate, Vector3 axis)
    {
        //if (this.target == null) { return; }

        Vector3 newPosCorrected = new Vector3(this.target.transform.position.x, this.target.transform.position.y + 0.3f, this.target.transform.position.z);

        Vector3 newPos = newPosCorrected - objectToRotate.position;
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
            SetShootingBoltToStandard();

            if(!this.towerName.Equals("Catapult Tower(Clone)"))
            {
                this.shootingBolt.SetParent(null);

                BallistaBolt bolt = this.shootingBolt.GetComponent<BallistaBolt>();
                bolt.ShootBolt();
                bolt.SetShotOrigin(this);
            }
            else
            {
                CatapultRock rock = this.shootingBolt.GetComponent<CatapultRock>();
                this.currentInstance = rock;
                rock.ShootRock();
                rock.SetShotOrigin(this);
            }

            PlayShootingSFX();
            

            this.bolt.gameObject.SetActive(false);

            this.timeToNextShot = this.shootingCooldown;
        }
    }

    private void PlayShootingSFX()
    {
        if (this.gameObject.name.Contains("Balista"))
            this.audioSource.PlayOneShot(this.balistaShot);
        else if (this.gameObject.name.Contains("Cannon"))
            this.audioSource.PlayOneShot(this.cannonShot);
        else if (this.gameObject.name.Contains("Catapult"))
            this.audioSource.PlayOneShot(this.catapultShot);
    }

    private void SetShootingBoltToStandard()
    {
        this.shootingBolt.gameObject.SetActive(true);
        this.shootingBolt.SetParent(this.weapon);
        this.shootingBolt.position = this.bolt.position;
        this.shootingBolt.rotation = this.bolt.rotation;
    }

    public void ChargeNextBolt()
    {
        this.StartCoroutine(LoadBolt());
    }

    private IEnumerator LoadBolt()
    {
        yield return new WaitForSeconds(this.shootingCooldown - 0.5f);

        this.bolt.gameObject.SetActive(true);
    }

    public void ClearTarget()
    {
        this.isTargetAcquired = false;
        this.target = default;
    }

    private void OnDestroy()
    {
        if(this.shootingBolt != null)
            GameObject.Destroy(this.shootingBolt.gameObject);
    }
}
