using Berty.BoardCards.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System.Collections.Generic;

namespace Berty.UI.Card.Managers
{
    public class HandToPileManager : ManagerSingleton<HandToPileManager>
    {
        private Game Game { get; set; }
        private CardPile CardPile => Game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        public void DiscardSelectedCardsFromHand()
        {
            IReadOnlyList<CharacterConfig> selectedCards = SelectionManager.Instance.SelectedCards;
            CardPile.DiscardCards(selectedCards, ManagerLocator.TurnManagerInstance.CurrentAlignment);
            ManagerLocator.HandCardObjectManagerInstance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
        }
    }
}
