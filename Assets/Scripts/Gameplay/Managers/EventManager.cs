using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Berty.BoardCards.Behaviours;
using Berty.Grid.Field.Entities;
using Berty.Gameplay.Entities;

namespace Berty.Gameplay.Managers
{
    public class EventManager : ManagerSingleton<EventManager>
    {
        private Game game;

        public event Action OnNewTurn;
        public event EventHandler OnPaymentStart;
        public event Action OnPaymentConfirm;
        public event Action OnPaymentCancel;
        public event EventHandler<DirectAttackEventArgs> OnDirectlyAttacked;
        public event EventHandler OnAttackNewStand;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void RaiseOnNewTurn()
        {
            OnNewTurn?.Invoke();
        }

        public void RaiseOnPaymentStart(BoardCardCore card)
        {
            OnPaymentStart?.Invoke(card, EventArgs.Empty);
        }

        public void RaiseOnPaymentConfirm()
        {
            OnPaymentConfirm?.Invoke();
        }

        public void RaiseOnPaymentCancel()
        {
            OnPaymentCancel?.Invoke();
        }

        public void RaiseOnDirectlyAttacked(BoardCardCore attacker)
        {
            DirectAttackEventArgs args = new()
            {
                AttackedFields = game.Grid.GetFieldsInRange(attacker.BoardCard, attacker.BoardCard.CharacterConfig.AttackRange)
            };
            OnDirectlyAttacked?.Invoke(attacker, args);
        }

        public void RaiseOnAttackNewStand(BoardCardCore defender)
        {
            Debug.Log($"Attacking {defender.name} for being new stand");
            OnAttackNewStand?.Invoke(defender, EventArgs.Empty);
        }
    }

    public class DirectAttackEventArgs : EventArgs
    {
        public List<BoardField> AttackedFields;
    }
}
