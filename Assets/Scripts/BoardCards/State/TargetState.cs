using Berty.BoardCards.Behaviours;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.State
{
    /*public class TargetState : CardState
    {
        public TargetState(BoardCardBehaviour sprite) : base(sprite)
        {

        }

        public override void HandleClick()
        {
            Debug.Log("Target clicked!");
            card.Grid.ApplyPrincessBuff(card);
            card.Grid.Turn.SetMoveTime();
            card.Grid.Turn.ExecuteAutomaticOpponentTurn();
        }

        public override CardState SetActive() => new ActiveState(card);

        public override CardState SetTelecinetic() => new TelecineticState(card);

        public override CardState SetIdle => new IdleState(card);

        public override void EnableButtons() { }
    }*/
}