using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
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
            if (game.CurrentAlignment == Core.ParentField.BoardField.Align)
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
            Core.Entity.ProgressTemporaryStats();
        }

        private void RegenerateDexterity()
        {
            if (!Core.BoardCard.IsTired) return;
            if (Core.BoardCard.Stats.Dexterity >= Core.BoardCard.CharacterConfig.Dexterity)
            {
                Core.BoardCard.MarkAsRested();
                return;
            }
            Entity.AdvanceDexterity(1, null);
            if (Core.BoardCard.Stats.Dexterity >= Core.BoardCard.CharacterConfig.Dexterity) Core.BoardCard.MarkAsRested();
        }

        private void EnableAttack()
        {
            Status providedStatus = game.GetStatusFromProviderOrNull(Core.BoardCard);
            if (providedStatus?.Name == StatusEnum.ExtraAttackCooldown) StatusManager.Instance.RemoveStatus(providedStatus);
            else Core.BoardCard.MarkAsHasNotAttacked();
        }

        private void HandleCharacterEffect()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.PapiezBertII:
                    EventManager.Instance.RaiseOnCharacterSpecialEffect(Core);
                    break;
            }
        }
    }
}