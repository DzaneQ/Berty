using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
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
            EventManager.Instance.RaiseOnNewTurn();
        }

        public void EndTheGame()
        {
            Alignment winner = game.Grid.WinningSide();
            if (winner == Alignment.None) winner = game.CurrentAlignment;
            OverlayObjectManager.Instance.DisplayGameOverScreen(winner);
        }
    }
}
