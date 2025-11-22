using Berty.UI.Card.Entities;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System.Collections.Generic;
using Berty.BoardCards.ConfigData;

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
            CardPile.DiscardCards(selectedCards, Game.CurrentAlignment);
            HandCardObjectManager.Instance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
        }
    }
}
