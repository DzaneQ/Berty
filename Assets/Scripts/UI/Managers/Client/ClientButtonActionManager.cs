using Berty.BoardCards.Listeners;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Network.Managers.Shared;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.UI.Managers.Client
{
    public class ClientButtonActionManager : ClientManagerSingleton<ClientButtonActionManager>
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
            //SyncGameEntityToServer.Instance.Sync(); // NOTE: May cause race conditions. Maybe remove.
            SharedTurnManager.Instance.EndTurn();
        }

        private void HandleUndo()
        {
            PaymentManager.Instance.CancelPayment();
        }
    }
}