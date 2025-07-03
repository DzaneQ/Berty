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
using Berty.BoardCards.ConfigData;

namespace Berty.CardTransfer.Managers
{
    public class HandToPileManager : ManagerSingleton<HandToPileManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void DiscardSelectedCardsFromHand()
        {
            List<CharacterConfig> selectedCards = HandCardSelectManager.Instance.SelectionSystem.SelectedCards;
            cardPile.DiscardCards(selectedCards, game.CurrentAlignment);
            HandCardSelectManager.Instance.SelectionSystem.ClearSelection();
            HandCardObjectManager.Instance.RemoveCardObjects();
        }
    }
}
