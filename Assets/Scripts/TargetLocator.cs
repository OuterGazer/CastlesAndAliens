using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform pedestal;
    [SerializeField] Transform weapon;

    [SerializeField] Transform target; // Only for debugging purposes as targets will be added programatically

    private Vector3 lastFramePos;

    // Start is called before the first frame update
    void Start()
    {
        this.target = GameObject.FindObjectOfType<EnemyMover>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {
        /*LookAtHorizontal();
        LookAtVertical();*/

        AimToEnemy(this.pedestal, Vector3.up);
        AimToEnemy(this.weapon, this.weapon.right);
    }
    
    private void AimToEnemy(Transform objectToRotate, Vector3 axis)
    {
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
}
