using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    interface IRotateCard
    {
        int CoroutineCount { get; }
        void ByAngle(int angle);
    }
}
