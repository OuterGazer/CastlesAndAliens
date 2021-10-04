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
        this.lastFramePos = this.target.position - this.pedestal.position;
    }

    // Update is called once per frame
    void Update()
    {
        AimWeapon();
    }

    private void AimWeapon()
    {
        Vector3 newPos = this.target.position - this.pedestal.position;

        Vector3 lastPosProj = Vector3.ProjectOnPlane(this.lastFramePos, Vector3.up);
        Vector3 newPosProj = Vector3.ProjectOnPlane(newPos, Vector3.up);

        float angleToRotate = Vector3.Angle(lastPosProj, newPosProj);

        this.pedestal.Rotate(Vector3.up, angleToRotate);

        this.lastFramePos = this.target.position - this.pedestal.position;


        /*this.pedestal.LookAt(this.target);
        this.pedestal.rotation = Quaternion.Euler(0, this.pedestal.rotation.y, 0);

        this.weapon.LookAt(this.target);
        this.weapon.rotation = Quaternion.Euler(this.weapon.rotation.x, 0, 0);*/
    }
}
