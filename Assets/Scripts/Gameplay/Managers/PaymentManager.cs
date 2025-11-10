using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using Berty.BoardCards.Behaviours;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class PaymentManager : ManagerSingleton<PaymentManager>
    {
        public void CallPayment(int price, BoardCardBehaviour card)
        {
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

        public void ConfirmPayment()
        {
            Debug.Log("Checking payment offer...");
            if (!SelectionManager.Instance.CheckOffer()) return;
            Debug.Log("Confirming payment.");
            HandToPileManager.Instance.DiscardSelectedCardsFromHand();
            SelectionManager.Instance.SetAsNotPaymentTime();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
        }
    }
}
