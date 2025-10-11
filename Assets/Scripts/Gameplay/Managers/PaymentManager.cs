using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using Berty.BoardCards.Behaviours;

namespace Berty.Gameplay.Managers
{
    public class PaymentManager : ManagerSingleton<PaymentManager>
    {
        public void CallPayment(int price, BoardCardCore card)
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
            if (!SelectionManager.Instance.CheckOffer()) return;
            HandToPileManager.Instance.DiscardSelectedCardsFromHand();
            SelectionManager.Instance.SetAsNotPaymentTime();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
        }
    }
}
