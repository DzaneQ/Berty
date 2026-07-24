using Berty.Characters.Managers;
using Berty.Gameplay.Managers;
using Berty.Network.Managers;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Managers.Client;
using Berty.UI.Card.Managers.Server;

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
        public static IHandToFieldManager HandToFieldManagerInstance;
        public static IFieldToHandManager FieldToHandManagerInstance;
        public static IConfirmPaymentManager ConfirmPaymentManagerInstance;
        public static ICheckpointManager CheckpointManagerInstance;
        public static IApplyManualEffectManager ApplyManualEffectManagerInstance;

        public static void InitializeSingleplayer()
        {
            TurnManagerInstance = TurnManager.Instance;
            PileToHandManagerInstance = PileToHandManager.Instance;
            HandCardObjectManagerInstance = HandCardObjectManager.Instance;
            HandToFieldManagerInstance = HandToFieldManager.Instance;
            FieldToHandManagerInstance = FieldToHandManager.Instance;
            ConfirmPaymentManagerInstance = PaymentManager.Instance;
            CheckpointManagerInstance = CheckpointManager.Instance;
            ApplyManualEffectManagerInstance = ApplyManualEffectManager.Instance;
        }

        public static void InitializeMultiplayer()
        {
            TurnManagerInstance = NetworkTurnManager.Instance;
            PileToHandManagerInstance = ServerPileToHandManager.Instance;
            HandCardObjectManagerInstance = ClientHandCardObjectManager.Instance;
            HandToFieldManagerInstance = ClientHandToFieldManager.Instance;
            FieldToHandManagerInstance = ClientFieldToHandManager.Instance;
            ConfirmPaymentManagerInstance = NetworkConfirmPaymentManager.Instance;
            CheckpointManagerInstance = NetworkCheckpointManager.Instance;
            ApplyManualEffectManagerInstance = NetworkApplyManualEffectManager.Instance;
        }
    }
}
