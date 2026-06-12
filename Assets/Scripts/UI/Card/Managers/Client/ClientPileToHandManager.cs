using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging.Managers;
using Berty.Gameplay.Managers.Client;

namespace Berty.UI.Card.Managers.Client
{
    public class ClientPileToHandManager : ClientManagerSingleton<ClientPileToHandManager>, IPileToHandManager
    {
        private Game game;
        private CardPile CardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game; // TODO: Change it so it's not used from local variable
        }

        public void PullCardsTo(int capacity)
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            AlignmentEnum align = PlayerReadManager.Instance.MyAlignment;
            DebugManager.Instance?.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (CardPile.PullCardsTo(capacity, align)) ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
            else ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}