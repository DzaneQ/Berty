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
    public class TurnManager : ManagerSingleton<TurnManager>, ITurnManager
    {
        private Game game;
        public AlignmentEnum CurrentAlignment => game.CurrentAlignment;
        

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void EndTurn()
        {
            game.SwitchAlignment();
            EventManager.Instance.RaiseOnNewTurn();
            ManagerLocator.CheckpointManagerInstance.RequestCheckpoint();
        }

        public void EndTheGame()
        {
            AlignmentEnum winner = game.Grid.WinningSide();
            if (winner == AlignmentEnum.None) winner = CurrentAlignment;
            OverlayObjectManager.Instance.DisplayGameOverScreen(winner == AlignmentEnum.Player);
        }

        public bool IsItMyTurn()
        {
            return true;
        }
    }
}
