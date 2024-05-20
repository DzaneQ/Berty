using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransform : MonoBehaviour
{
    float edgeWidth = 10f;
    float rotManSpeed = 90f;
    float rotAutoMultiplier = 0.25f;

    void Update()
    {
        if (Input.mousePosition.x <= edgeWidth) RotateCameraCounterclockwise(rotManSpeed);
        else if (Input.mousePosition.x >= Screen.width - edgeWidth) RotateCameraClockwise(rotManSpeed);
        else RotateAutomatically();
    }

    private void RotateCameraCounterclockwise(float rotSpeed)
    {
        transform.Rotate(Vector3.back, rotSpeed * Time.deltaTime);
    }

    private void RotateCameraClockwise(float rotSpeed)
    {
        transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
    }

    private void RotateAutomatically()
    {
        float currentAngle = AngleValue();
        if (Mathf.Approximately(currentAngle % 90, 0)) return;
        float rightAngle = RightAngleValue();
        if (Mathf.Abs(currentAngle - rightAngle) < rotManSpeed * rotAutoMultiplier)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rightAngle);
        else
        {
            float targetAngle = (currentAngle - rightAngle) * (1 - rotAutoMultiplier) + rightAngle;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetAngle);
        }
    }

    public float RightAngleValue()
    { 
        float cameraRotation = AngleValue();
        //Debug.Log("Camera rotation: " + cameraRotation);
        return Mathf.Round(cameraRotation / 90) * 90;
    }

    private float AngleValue() => transform.localEulerAngles.z;
}
