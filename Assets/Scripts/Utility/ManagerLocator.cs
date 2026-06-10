using Berty.Gameplay.Managers;
using Berty.Network.Managers.Shared;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Managers.Client;
using UnityEngine;

namespace Berty.Utility
{
    /// <summary>
    /// Locate managers that are used differently for singleplayer and multiplayer modes.
    /// </summary>
    public static class ManagerLocator
    {
        public static ITurnManager TurnManagerInstance;
        public static IPileToHandManager PileToHandManagerInstance;
        public static IHandCardObjectManager HandCardObjectManagerInstance;

        public static void InitializeSingleplayer()
        {
            TurnManagerInstance = TurnManager.Instance;
            PileToHandManagerInstance = PileToHandManager.Instance;
            HandCardObjectManagerInstance = HandCardObjectManager.Instance;
        }

        public static void InitializeMultiplayer()
        {
            TurnManagerInstance = SharedTurnManager.Instance;
            PileToHandManagerInstance = ClientPileToHandManager.Instance;
            HandCardObjectManagerInstance = ClientHandCardObjectManager.Instance;
        }
    }
}
