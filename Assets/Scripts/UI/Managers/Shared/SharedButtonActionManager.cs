using Berty.BoardCards.Listeners;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Shared;
using Berty.Gameplay.Managers.Server;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.UI.Managers.Shared
{
    public class SharedButtonActionManager : SharedManagerSingleton<SharedButtonActionManager>
    {
        public void HandleCornerButtonClick(CornerButtonEnum buttonType)
        {
            switch (buttonType)
            {
                case CornerButtonEnum.EndTurn:
                    HandleEndTurn();
                    break;
                case CornerButtonEnum.Undo:
                    HandleUndo();
                    break;
                default:
                    throw new Exception("Unknown corner button type");
            }
        }

        private void HandleEndTurn()
        {
            SyncGameEntityToServer.Instance.Sync();
            EndTurnServerRpc();
        }

        private void HandleUndo()
        {
            PaymentManager.Instance.CancelPayment();
        }

        [ServerRpc]
        private void EndTurnServerRpc()
        {
            ServerTurnManager.Instance.EndTurn();
        }
    }
}