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

namespace Berty.BoardCards.Listeners
{
    public class TurnListener : MonoBehaviour
    {
        private BoardCardCore core;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
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
            core.SetMainState();
        }
    }
}