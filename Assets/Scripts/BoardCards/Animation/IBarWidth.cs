using UnityEngine;

namespace Berty.BoardCards.Animation
{
    internal interface IBarWidth
    {
        int CoroutineCount { get; }
        void SetVectorsWithoutAnimation(Vector3 targetLocation, Vector2 targetSize);
        void AdvanceToVectors(Vector3 targetLocation, Vector2 targetSize);
    }
}
