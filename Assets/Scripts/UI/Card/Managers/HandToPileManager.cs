using Berty.UI.Card.Entities;
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
using Berty.BoardCards.ConfigData;

namespace Berty.UI.Card.Managers
{
    public class HandToPileManager : ManagerSingleton<HandToPileManager>
    {
        private Game Game { get; set; }
        private CardPile CardPile => Game.CardPile;
        private SelectionAndPaymentSystem SelectionSystem { get; set; }

        protected override void Awake()
        {
            InitializeSingleton();
            Game = CoreManager.Instance.Game;
            SelectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
        }

        public void DiscardSelectedCardsFromHand()
        {
            List<CharacterConfig> selectedCards = SelectionSystem.SelectedCards;
            CardPile.DiscardCards(selectedCards, Game.CurrentAlignment);
            HandCardObjectManager.Instance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
        }
    }
}
