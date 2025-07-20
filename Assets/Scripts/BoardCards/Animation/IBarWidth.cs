using Berty.Structs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    interface IBarWidth
    {
        int CoroutineCount { get; }
        void SetVectorsWithoutAnimation(Vector3 targetLocation, Vector2 targetSize);
        void AdvanceToVectors(Vector3 targetLocation, Vector2 targetSize);
    }
}
