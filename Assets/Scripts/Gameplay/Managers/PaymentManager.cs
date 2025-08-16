using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.UI.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.Behaviours;

namespace Berty.Gameplay.Managers
{
    public class PaymentManager : ManagerSingleton<PaymentManager>
    {
        private SelectionAndPaymentSystem paymentSystem;

        protected override void Awake()
        {
            base.Awake();
            paymentSystem = CoreManager.Instance.SelectionAndPaymentSystem;
        }

        public void CallPayment(int price, BoardCardCore card)
        {
            paymentSystem.DemandPayment(price);
            ButtonObjectManager.Instance.DisplayUndoButton();
            EventManager.Instance.RaiseOnPaymentStart(card);
        }

        public void CancelPayment()
        {
            HandCardSelectManager.Instance.ClearSelection();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentCancel();
            paymentSystem.SetAsNotPaymentTime();
        }

        public void ConfirmPayment()
        {
            if (!paymentSystem.CheckOffer()) return;
            HandToPileManager.Instance.DiscardSelectedCardsFromHand();
            paymentSystem.SetAsNotPaymentTime();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentConfirm();
        }
    }
}
