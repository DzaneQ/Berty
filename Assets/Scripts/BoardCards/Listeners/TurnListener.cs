using Berty.BoardCards.Behaviours;
using Berty.UI.Card.Managers;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using System;
using UnityEngine;
using Berty.BoardCards.Managers;

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
            ProgressTemporaryStats();
            if (game.CurrentAlignment != core.ParentField.BoardField.Align) return;
            RegenerateDexterity();
            EnableAttack();
            core.SetMainState();
        }

        private void ProgressTemporaryStats()
        {
            StatChangeManager.Instance.ProgressTemporaryStats(core);
        }

        private void RegenerateDexterity()
        {
            if (!core.BoardCard.IsTired) return;
            if (core.BoardCard.Stats.Dexterity >= core.BoardCard.CharacterConfig.Dexterity)
            {
                core.BoardCard.MarkAsRested();
                return;
            }
            StatChangeManager.Instance.AdvanceDexterity(core, 1);
            if (core.BoardCard.Stats.Dexterity >= core.BoardCard.CharacterConfig.Dexterity) core.BoardCard.MarkAsRested();
        }

        private void EnableAttack()
        {
            core.BoardCard.MarkAsHasNotAttacked();
        }
    }
}