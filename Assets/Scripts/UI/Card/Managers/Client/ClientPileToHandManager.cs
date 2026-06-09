/*using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging.Managers;
using Berty.Gameplay.Managers.Client;

namespace Berty.UI.Card.Managers.Client
{
    public class ClientPileToHandManager : ClientManagerSingleton<ClientPileToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void PullCardsTo(int capacity)
        {
            if (PlayerReadManager.Instance.IsItNotMyTurn()) return;
            AlignmentEnum align = game.CurrentAlignment;
            DebugManager.Instance?.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (cardPile.PullCardsTo(capacity, align)) ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
            else ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}*/
