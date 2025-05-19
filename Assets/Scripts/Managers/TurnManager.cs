using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Managers
{
    public class TurnManager : ManagerSingleton<TurnManager>
    {
        private Game game;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void EndTheGame()
        {
            Alignment winner = game.Grid.WinningSide();
            if (winner == Alignment.None) winner = game.CurrentAlignment;
            GameObjectManager.Instance.DisplayGameOverScreen(winner);
            /*if (winner == Alignment.Player) endingMessage.text = "Wygrana!";
            if (winner == Alignment.Opponent) endingMessage.text = "Przegrana!";
            DisableInteractions();
            GameOverText.SetActive(true);*/
        }
    }
}
