using Berty.Utility;
using Berty.BoardCards.ConfigData;
using Berty.Network.Managers;

namespace Berty.UI.Card.Managers
{
    public class ClientFieldToHandManager : ClientManagerSingleton<ClientFieldToHandManager>, IFieldToHandManager
    {
        public void RetrievePendingCard()
        {
            CharacterConfig pendingCard = SelectionManager.Instance.GetPendingCardOrThrow();
            NetworkCardManager.Instance.RetrieveCard(pendingCard);
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjectFromConfig(pendingCard);
        }
    }
}
