using Berty.BoardCards.Behaviours;
using Berty.UI.Card.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using UnityEngine;
using System;

namespace Berty.BoardCards.Listeners
{
    public class PaymentListener : MonoBehaviour
    {
        private BoardCardCore core;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
        }

        private void OnEnable()
        {
            EventManager.Instance.OnPaymentStart += HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm += HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel += HandlePaymentCancel;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnPaymentStart -= HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm -= HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel -= HandlePaymentCancel;
        }

        private void HandlePaymentStart(object sender, EventArgs args)
        {
            if (sender.Equals(core)) return;
            core.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            if (core.CardState == CardStateEnum.Attacking)
            {
                EventManager.Instance.RaiseOnDirectlyAttacked(core);
            }
            else if (core.CardState == CardStateEnum.NewCard)
            {
                EventManager.Instance.RaiseOnAttackNewStand(core);
            }    
            core.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (core.CardState == CardStateEnum.NewCard)
            {
                FieldToHandManager.Instance.RetrieveCardOnHold();
                core.DestroyCard();
                return;
            }
            core.SetMainState();
        }
    }
}