using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using Berty.BoardCards.Behaviours;
using System;

namespace Berty.Gameplay.Managers
{
    public class PaymentManager : ManagerSingleton<PaymentManager>, IConfirmPaymentManager
    {
        public void CallPayment(int price, BoardCardBehaviour card)
        {
            if (card == null) throw new Exception($"Calling to pay {price} cards for a null card");
            SelectionManager.Instance.DemandPayment(price);
            ButtonObjectManager.Instance.DisplayUndoButton();
            EventManager.Instance.RaiseOnPaymentStart(card);
        }

        public void CancelPayment()
        {
            HandCardSelectManager.Instance.ClearSelection();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentCancel();
            SelectionManager.Instance.SetAsNotPaymentTime();
        }

        public void ConfirmPayment(BoardCardBehaviour card)
        {
            if (!SelectionManager.Instance.CheckOffer()) return;
            HandToPileManager.Instance.DiscardSelectedCardsFromHand();
            SelectionManager.Instance.SetAsNotPaymentTime();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentConfirm();
            ManagerLocator.CheckpointManagerInstance.RequestCheckpoint();
        }
    }
}
