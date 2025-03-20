using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Structs
{
    public struct DefaultTransform
    {
        public Vector3 defaultPosition;
        public Quaternion defaultRotation;

        public DefaultTransform(Transform obj)
        {
            defaultPosition = obj.position;
            defaultRotation = obj.rotation;
        }
    }
}