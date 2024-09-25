using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatingCard : MonoBehaviour
{
    //Quaternion endingRotation;
    float rotatingAngle;
    float rotSpeed;
    Vector3 targetPosition;

    private void Start()
    {
        rotatingAngle = 0;
    }

    //private void Update()
    //{
        //if (rotatingAngle < float.Epsilon) return;
        //Debug.Log("RotatingFrame");
        //RotateFrame();
    //}

    public IEnumerator RotateObject(float angle, float speed)
    {
        SetRotation(angle, speed);
        yield return StartCoroutine(AnimateRotate());
    }

    public IEnumerator MoveToField(Field target, float speed)
    {
        Debug.Log($"Card initial coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
        SetTargetField(target, speed);
        Debug.Log($"Card target coordinates: {targetPosition.x}; {targetPosition.y}; {targetPosition.z}");
        yield return StartCoroutine(AnimateMove());
    }

    private void SetRotation(float angle, float speed)
    {
        if (rotatingAngle != 0) Debug.LogWarning("Rotating angles value set at " + rotatingAngle);
        rotatingAngle += angle;
        rotSpeed = speed;
        //endingRotation = Quaternion.Euler(0f, 0f, -rotatingAngle) * transform.rotation;
    }

    private void SetTargetField(Field target, float speed)
    {
        //targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        targetPosition = transform.position;
        targetPosition.x = target.transform.position.x;
        targetPosition.z = target.transform.position.z;
        rotSpeed = speed;
    }

    private IEnumerator AnimateRotate()
    {
        for (; rotatingAngle != 0; )
        {
            RotateFrame();
            yield return null;
        }
    }

    private IEnumerator AnimateMove()
    {
        for (; targetPosition != transform.position;)
        {
            MoveFrame();
            yield return null;
        }
        Debug.Log($"Card actual coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
    }

    private void RotateFrame()
    {
        float frameAngle = rotSpeed * Time.deltaTime;
        if (rotatingAngle == 0) throw new Exception("Rotating angle at 0!");
        else if (frameAngle > Mathf.Abs(rotatingAngle))
        {
            transform.Rotate(Vector3.forward, rotatingAngle);
            rotatingAngle = 0;
        }
        else if (rotatingAngle > 0)
        {
            transform.Rotate(Vector3.forward, frameAngle);
            rotatingAngle -= frameAngle;
        }
        else
        {
            transform.Rotate(Vector3.back, frameAngle);
            rotatingAngle += frameAngle;
        }
    }

    private void MoveFrame()
    {
        float frameDistance = rotSpeed * Time.deltaTime;
        if (Vector3.Distance(transform.position, targetPosition) < frameDistance) transform.position = targetPosition;
        else transform.position = Vector3.MoveTowards(transform.position, targetPosition, frameDistance);
    }
}
