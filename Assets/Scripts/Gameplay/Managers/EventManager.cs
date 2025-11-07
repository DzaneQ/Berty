using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
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
        public event EventHandler OnCharacterDeath;
        public event EventHandler OnCharacterSpecialEffect;
        public event EventHandler<ValueChangeEventArgs> OnValueChange;
        public event EventHandler OnStatusUpdated;
        public event EventHandler<StatusEventArgs> OnStatusRemoved;
        public event EventHandler<DirectAttackEventArgs> OnHighlightStart;
        public event Action OnHighlightEnd;
        public event EventHandler<ValidateOutputEventArgs> OnCheckpointRequest;
        public event Action OnVolumeChanged;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void RaiseOnNewTurn()
        {
            OnNewTurn?.Invoke();
        }

        public void RaiseOnPaymentStart(BoardCardBehaviour card)
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

        public void RaiseOnDirectlyAttacked(BoardCardBehaviour attacker)
        {
            DirectAttackEventArgs args = new()
            {
                AttackedFields = game.Grid.GetFieldsInRange(attacker.BoardCard, attacker.BoardCard.CharacterConfig.AttackRange)
            };
            OnDirectlyAttacked?.Invoke(attacker, args);
        }

        public void RaiseOnAttackNewStand(BoardCardBehaviour defender)
        {
            OnAttackNewStand?.Invoke(defender, EventArgs.Empty);
        }

        public void RaiseOnNewCharacter(BoardCardBehaviour newCard)
        {
            OnNewCharacter?.Invoke(newCard, EventArgs.Empty);
        }

        public void RaiseOnMovedCharacter(BoardCardBehaviour movedCard)
        {
            OnMovedCharacter?.Invoke(movedCard, EventArgs.Empty);
        }

        public void RaiseOnCharacterDeath(BoardCardBehaviour dyingCard)
        {
            OnCharacterDeath?.Invoke(dyingCard, EventArgs.Empty);
        }

        public void RaiseOnCharacterSpecialEffect(BoardCardBehaviour specialCard)
        {
            OnCharacterSpecialEffect?.Invoke(specialCard, EventArgs.Empty);
        }

        public void RaiseOnValueChange(BoardCardBehaviour statChangedCard, int value)
        {
            ValueChangeEventArgs args = new()
            {
                Delta = value
            };
            OnValueChange?.Invoke(statChangedCard, args);
        }

        public void RaiseOnStatusUpdated(Status status)
        {
            OnStatusUpdated?.Invoke(status, EventArgs.Empty);
        }

        public void RaiseOnStatusRemoved(StatusEnum name, AlignmentEnum align)
        {
            StatusEventArgs args = new()
            {
                StatusName = name,
                Alignment = align
            };
            OnStatusRemoved?.Invoke(null, args);
        }

        public void RaiseOnHighlightStart(BoardCardBehaviour focusedCard)
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

        public bool RaiseOnCheckpointRequest()
        {
            ValidateOutputEventArgs args = new()
            {
                IsRestricted = false
            };
            OnCheckpointRequest?.Invoke(null, args);
            return args.IsRestricted;
        }

        public void RaiseOnVolumeChanged()
        {
            OnVolumeChanged?.Invoke();
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

    public class StatusEventArgs : EventArgs
    {
        public StatusEnum StatusName;
        public AlignmentEnum Alignment;
    }

    public class ValidateOutputEventArgs : EventArgs
    {
        public bool IsRestricted;
    }
}
