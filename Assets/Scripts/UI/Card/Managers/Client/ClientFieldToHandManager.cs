using Berty.Utility;
using Berty.BoardCards.ConfigData;

namespace Berty.UI.Card.Managers
{
    public class ClientFieldToHandManager : ClientManagerSingleton<ClientFieldToHandManager>, IFieldToHandManager
    {
        public void RetrievePendingCard()
        {
            CharacterConfig pendingCard = SelectionManager.Instance.GetPendingCardOrThrow();
            // NOTE: Analogically to the opposite action: do we really need to bother card pile manipulation?
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjectFromConfig(pendingCard);
        }
    }
}
