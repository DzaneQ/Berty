using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class TurnListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnNewTurn += HandleNewTurn;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
        }

        private void HandleNewTurn()
        {
            if (StateMachine.IsForPay()) throw new Exception($"Board card {name} detected for pay when switching turns.");
            if (ManagerLocator.TurnManagerInstance.CurrentAlignment == ParentField.BoardField.Align)
            {
                HandleCharacterEffect();
                ProgressTemporaryStats();
                RegenerateDexterity();
                EnableAttack();
            }
            StateMachine.SetMainState();
        }

        private void ProgressTemporaryStats()
        {
            EntityHandler.ProgressTemporaryStats();
        }

        private void RegenerateDexterity()
        {
            if (!BoardCard.IsTired) return;
            if (BoardCard.Stats.Dexterity >= BoardCard.CharacterConfig.Dexterity)
            {
                BoardCard.MarkAsRested();
                return;
            }
            EntityHandler.AdvanceDexterity(1, null);
            if (BoardCard.Stats.Dexterity >= BoardCard.CharacterConfig.Dexterity) BoardCard.MarkAsRested();
        }

        private void EnableAttack()
        {
            Status providedStatus = game.GetStatusFromProviderOrNull(BoardCard);
            if (providedStatus?.Name == StatusEnum.ExtraAttackCooldown) StatusManager.Instance.RemoveStatus(providedStatus);
            else BoardCard.MarkAsHasNotAttacked();
        }

        private void HandleCharacterEffect()
        {
            switch (BoardCard.GetSkill())
            {
                case CharacterEnum.PapiezBertII:
                    EventManager.Instance.RaiseOnCharacterSpecialEffect(this);
                    break;
            }
        }
    }
}