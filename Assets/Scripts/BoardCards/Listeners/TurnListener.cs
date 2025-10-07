using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class TurnListener : MonoBehaviour
    {
        private BoardCardCore core;
        private Game game;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
        }

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
            if (core.IsForPay()) throw new Exception($"Board card {name} detected for pay when switching turns.");
            if (game.CurrentAlignment == core.ParentField.BoardField.Align)
            {
                HandleCharacterEffect();
                ProgressTemporaryStats();
                RegenerateDexterity();
                EnableAttack();
            }
            core.SetMainState();
        }

        private void ProgressTemporaryStats()
        {
            core.StatChange.ProgressTemporaryStats();
        }

        private void RegenerateDexterity()
        {
            if (!core.BoardCard.IsTired) return;
            if (core.BoardCard.Stats.Dexterity >= core.BoardCard.CharacterConfig.Dexterity)
            {
                core.BoardCard.MarkAsRested();
                return;
            }
            core.StatChange.AdvanceDexterity(1, null);
            if (core.BoardCard.Stats.Dexterity >= core.BoardCard.CharacterConfig.Dexterity) core.BoardCard.MarkAsRested();
        }

        private void EnableAttack()
        {
            Status providedStatus = game.GetStatusFromProviderOrNull(core.BoardCard);
            if (providedStatus?.Name == StatusEnum.ExtraAttackCooldown) StatusManager.Instance.RemoveStatus(providedStatus);
            else core.BoardCard.MarkAsHasNotAttacked();
        }

        private void HandleCharacterEffect()
        {
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.PapiezBertII:
                    EventManager.Instance.RaiseOnCharacterSpecialEffect(core);
                    break;
            }
        }
    }
}