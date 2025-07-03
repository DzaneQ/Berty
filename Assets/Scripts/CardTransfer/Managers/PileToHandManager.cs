using Berty.CardTransfer.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.CardTransfer.Managers
{
    public class PileToHandManager : ManagerSingleton<PileToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void PullCardsTo(int capacity)
        {
            Alignment align = game.CurrentAlignment;

            if (cardPile.PullCardsTo(capacity, align)) HandCardObjectManager.Instance.AddCardObjects();
            else TurnManager.Instance.EndTheGame();
        }
    }
}
