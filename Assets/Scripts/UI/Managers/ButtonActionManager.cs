using Berty.BoardCards.Listeners;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.UI.Managers
{
    public class ButtonActionManager : ManagerSingleton<ButtonActionManager>
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
            //Debug.Log("Handling end turn");
            TurnManager.Instance.EndTurn();
        }

        private void HandleUndo()
        {
            PaymentManager.Instance.CancelPayment();
        }
    }
}