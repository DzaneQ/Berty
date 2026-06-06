using Berty.Gameplay.Managers;
using Berty.Network.Managers.Shared;

namespace Berty.Utility
{
    /// <summary>
    /// Locate managers that are used differently for singleplayer and multiplayer modes.
    /// </summary>
    public static class ManagerLocator
    {
        public static ITurnManager TurnManagerInstance;

        public static void InitializeSingleplayer()
        {
            TurnManagerInstance = TurnManager.Instance;
        }
        public static void InitializeMultiplayer()
        {
            TurnManagerInstance = SharedTurnManager.Instance;
        }
    }
}
