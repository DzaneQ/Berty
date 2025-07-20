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

        public void CallPayment(int price)
        {
            paymentSystem.DemandPayment(price);
            EventManager.Instance.RaiseOnPaymentStart();
        }

        public void CancelPayment()
        {
            HandCardSelectManager.Instance.ClearSelection();
            paymentSystem.SetAsNotPaymentTime();
            EventManager.Instance.RaiseOnPaymentCancel();
        }

        public void ConfirmPayment()
        {
            if (!paymentSystem.CheckOffer()) return;
            HandToPileManager.Instance.DiscardSelectedCardsFromHand();
            paymentSystem.SetAsNotPaymentTime();
            EventManager.Instance.RaiseOnPaymentConfirm();
        }
    }
}
