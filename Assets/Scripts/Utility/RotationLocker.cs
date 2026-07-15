using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Utility
{
    public class RotationLocker : MonoBehaviour
    {
        private Quaternion rotation;
        private Vector3 setRotation = new Vector3(90, 0, 0);

        private void Start()
        {
            rotation = Quaternion.Euler(setRotation);
        }

        // Update is called once per frame
        private void LateUpdate()
        {
            transform.rotation = rotation;
        }
    }
}