using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.transform.localRotation =  Quaternion.Euler(this.gameObject.transform.localRotation.eulerAngles.x,
                                                                    45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                    this.gameObject.transform.localRotation.eulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.localRotation = Quaternion.Euler(this.gameObject.transform.localRotation.eulerAngles.x,
                                                                   45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                   this.gameObject.transform.localRotation.eulerAngles.z);

        ChangePosOnRot();

        TiltZOnRamps();
    }

    private void ChangePosOnRot()
    {
        if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 270.0f))
        {
            this.gameObject.transform.localPosition = new Vector3(0.0f, 0.971f, -0.316f);
        }
        else if(Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 180.0f))
        {
            this.gameObject.transform.localPosition = new Vector3(-0.316f, 0.971f, 0.0f);
        }
        else if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 90.0f))
        {
            this.gameObject.transform.localPosition = new Vector3(0.176f, 0.74f, 0.129f);
        }
        else
        {
            this.gameObject.transform.localPosition = new Vector3(0.0f, 0.541f, -0.316f);
        }
    }

    private void TiltZOnRamps()
    {
        Vector3 curPos = this.gameObject.transform.localPosition;

        if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.x, 0.0f))
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(-40.0f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       0.0f);

            AdjustPosOnRamps(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
        }
        else if (this.parentTransform.localRotation.eulerAngles.x > 200f)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       2.80f);

            if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 270.0f))
                AdjustPosOnRamps(0.0f, 1.0f, -0.272f);
            else if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 180.0f))
            {
                this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       -2.80f);
                AdjustPosOnRamps(-0.346f, 1.0f, 0.043f);
            }
            else if(Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 90.0f))
            {
                this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       -2.80f);

                AdjustPosOnRamps(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
            }
            else
                AdjustPosOnRamps(0.0f, 0.541f, -0.316f);
        }
        else if (this.parentTransform.localRotation.eulerAngles.x > 0f)
        {
            this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       -2.80f);

            if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 270.0f))
                AdjustPosOnRamps(0.0f, 0.935f, -0.37f);
            else if (Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 180.0f))
            {
                AdjustPosOnRamps(-0.272f, 0.90f, 0.0f);
                this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       2.80f);
            }
            else if(Mathf.Approximately(this.parentTransform.localRotation.eulerAngles.y, 90.0f))
            {
                this.gameObject.transform.localRotation = Quaternion.Euler(-43.5f,
                                                                       45.7f - this.parentTransform.localRotation.eulerAngles.y,
                                                                       2.80f);

                AdjustPosOnRamps(this.gameObject.transform.localPosition.x, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);
            }                
            else
                AdjustPosOnRamps(0.0f, 0.541f, -0.272f);
        }
    }

    private void AdjustPosOnRamps(float x, float y, float z)
    {
        this.gameObject.transform.localPosition = new Vector3(x, y, z);
    }
}
