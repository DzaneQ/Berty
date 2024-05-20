using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLocker : MonoBehaviour
{
    private Quaternion rotation;

    void Start()
    {
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
