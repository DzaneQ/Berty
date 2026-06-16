using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Network.Managers;

namespace Berty.UI.Card.Managers.Client
{
    public class ServerPileToHandManager : ServerManagerSingleton<ServerPileToHandManager>, IPileToHandManager
    {
        private Game game;
        private CardPile CardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void PullCards()
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            int capacity = game.GameConfig.TableCapacity;
            AlignmentEnum align = PlayerReadManager.Instance.MyAlignment;
            DebugManager instance = DebugManager.Instance;
            if (instance != null) instance.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (CardPile.PullCardsTo(capacity, align)) NetworkCardPileManager.Instance.AddCardObjectsClientRpc();
            else ManagerLocator.TurnManagerInstance.EndTheGame();
        }
    }
}