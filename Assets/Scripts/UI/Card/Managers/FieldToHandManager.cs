using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.BoardCards.ConfigData;

namespace Berty.UI.Card.Managers
{
    public class FieldToHandManager : ManagerSingleton<FieldToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void RetrievePendingCard()
        {
            AlignmentEnum align = game.CurrentAlignment;
            CharacterConfig pendingCard = SelectionManager.Instance.GetPendingCardOrThrow();
            cardPile.RetrieveCard(pendingCard, align);
            HandCardObjectManager.Instance.AddCardObjectFromConfigForTable(pendingCard, align);
        }
    }
}
