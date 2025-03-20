using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Structs
{
    public struct NewBar
    {
        public Transform bar;
        public SpriteRenderer rend;
        public Vector3 barLocation;
        public Vector2 rendSize;

        public NewBar(Transform targetBar, SpriteRenderer targetRenderer, Vector3 targetLocation, Vector2 targetSize)
        {
            bar = targetBar;
            rend = targetRenderer;
            barLocation = targetLocation;
            rendSize = targetSize;
        }
    }
}