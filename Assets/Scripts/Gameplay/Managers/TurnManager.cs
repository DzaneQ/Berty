using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class TurnManager : ManagerSingleton<TurnManager>
    {
        private Game game;
        

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void EndTurn()
        {
            game.SwitchAlignment();
            HandCardObjectManager.Instance.SwitchTables();
            EventManager.Instance.RaiseOnNewTurn();
            CheckpointManager.Instance.RequestCheckpoint();
        }

        public void EndTheGame()
        {
            AlignmentEnum winner = game.Grid.WinningSide();
            if (winner == AlignmentEnum.None) winner = game.CurrentAlignment;
            OverlayObjectManager.Instance.DisplayGameOverScreen(winner);
        }
    }
}
