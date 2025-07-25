using Berty.Grid.Field.Behaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    interface IMoveCard
    {
        int CoroutineCount { get; }
        void ToField(FieldBehaviour field);
    }
}
