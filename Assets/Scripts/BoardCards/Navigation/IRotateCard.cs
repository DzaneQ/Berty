using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Navigation
{
    interface IRotateCard
    {
        int CoroutineCount { get; }
        void ByAngle(int angle);
    }
}
