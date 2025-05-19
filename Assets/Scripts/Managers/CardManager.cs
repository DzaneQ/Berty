using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Managers
{
    public class CardManager : ManagerSingleton<CardManager>
    {
        private Game game;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        private void PullCards()
        {
            int capacity = game.GameConfig.TableCapacity;
            Alignment align = game.CurrentAlignment;

            if (!game.CardPile.PullCardsTo(capacity, align)) TurnManager.Instance.EndTheGame();
        }
    }
}
