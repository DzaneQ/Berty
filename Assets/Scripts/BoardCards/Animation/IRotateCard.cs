using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    interface IRotateCard
    {
        int CoroutineCount { get; }
        void ByAngleWithoutAnimation(int angle);
        void ByAngle(int angle);
    }
}
