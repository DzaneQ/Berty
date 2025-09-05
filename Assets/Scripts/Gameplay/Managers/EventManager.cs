using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

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
        public event EventHandler OnNewCharacter;
        public event EventHandler OnMovedCharacter;
        public event EventHandler<ValueChangeEventArgs> OnValueChange;
        public event EventHandler<DirectAttackEventArgs> OnHighlightStart;
        public event Action OnHighlightEnd;

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
            OnAttackNewStand?.Invoke(defender, EventArgs.Empty);
        }

        public void RaiseOnNewCharacter(BoardCardCore newCard)
        {
            OnNewCharacter?.Invoke(newCard, EventArgs.Empty);
        }

        public void RaiseOnMovedCharacter(BoardCardCore movedCard)
        {
            OnMovedCharacter?.Invoke(movedCard, EventArgs.Empty);
        }

        public void RaiseOnValueChange(BoardCardCore statChangedCard, int value)
        {
            ValueChangeEventArgs args = new()
            {
                Delta = value
            };
            OnValueChange?.Invoke(statChangedCard, args);
        }

        public void RaiseOnHighlightStart(BoardCardCore focusedCard)
        {
            DirectAttackEventArgs args = new()
            {
                AttackedFields = game.Grid.GetFieldsInRange(focusedCard.BoardCard, focusedCard.BoardCard.CharacterConfig.AttackRange)
            };
            OnHighlightStart?.Invoke(focusedCard, args);
        }

        public void RaiseOnHighlightEnd()
        {
            OnHighlightEnd?.Invoke();
        }    
    }

    public class DirectAttackEventArgs : EventArgs
    {
        public List<BoardField> AttackedFields;
    }

    public class ValueChangeEventArgs : EventArgs
    {
        public int Delta;
    }
}
