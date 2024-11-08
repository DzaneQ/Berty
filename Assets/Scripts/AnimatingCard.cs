using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatingCard : MonoBehaviour
{
    //Quaternion endingRotation;
    float rotatingAngle;
    //float rotSpeed;
    float durationSeconds;
    Vector3 targetPosition;
    //Vector3 backupPosition;
    int coroutineCount;
    //CardSprite backupCard;

    public int CoroutineCount
    {
        get => coroutineCount;
        private set
        {
            coroutineCount = value;
            if (coroutineCount < 0) throw new Exception("Negative coroutine count!");
        }
    }

    private void Start()
    {
        rotatingAngle = 0;
        coroutineCount = 0;
    }

    //private void Update()
    //{
        //if (rotatingAngle < float.Epsilon) return;
        //Debug.Log("RotatingFrame");
        //RotateFrame();
    //}

    public IEnumerator RotateObject(float angle, float duration)
    {
        SetRotation(angle, duration);
        yield return StartCoroutine(AnimateRotate());
    }

    public IEnumerator MoveToField(Field target, float duration)
    {
        //Debug.Log($"Card initial coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
        SetTargetField(target, duration);
        //Debug.Log($"Card target coordinates: {targetPosition.x}; {targetPosition.y}; {targetPosition.z}");
        yield return StartCoroutine(AnimateMove());
    }

    private void SetRotation(float angle, float duration)
    {
        if (rotatingAngle != 0) Debug.LogWarning("Rotating angles value set at " + rotatingAngle);
        rotatingAngle += angle;
        durationSeconds = duration;
        //endingRotation = Quaternion.Euler(0f, 0f, -rotatingAngle) * transform.rotation;
    }

    private void SetTargetField(Field target, float duration)
    {
        //targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        targetPosition = transform.position;
        targetPosition.x = target.transform.position.x;
        targetPosition.z = target.transform.position.z;
        durationSeconds = duration;
    }

    private IEnumerator AnimateRotate()
    {
        CoroutineCount++;
        float currentTime = 0;
        for (; rotatingAngle != 0; )
        {
            RotateFrame(ref currentTime);
            yield return null;
        }
        CoroutineCount--;
    }

    private IEnumerator AnimateMove()
    {
        coroutineCount++;
        float currentTime = 0;
        for (; targetPosition != transform.position;)
        {
            MoveFrame(ref currentTime);
            yield return null;
        }
        coroutineCount--;
    }

    private void RotateFrame(ref float currentTime)
    {
        float frameTime = Time.deltaTime;
        if (rotatingAngle == 0) throw new Exception("Rotating angle at 0!");
        else if (frameTime + currentTime >= durationSeconds)
        {
            transform.Rotate(Vector3.forward, rotatingAngle);
            rotatingAngle = 0;
        }
        else
        {
            float step = frameTime / (durationSeconds - currentTime);
            float frameAngle = rotatingAngle * step;
            transform.Rotate(Vector3.forward, frameAngle);
            rotatingAngle -= frameAngle;
        }
        currentTime += frameTime;
    }

    private void MoveFrame(ref float currentTime)
    {
        float frameTime = Time.deltaTime;
        if (currentTime + frameTime >= durationSeconds) transform.position = targetPosition;
        else
        {
            float step = frameTime / (durationSeconds - currentTime);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        currentTime += frameTime;
    }
}
