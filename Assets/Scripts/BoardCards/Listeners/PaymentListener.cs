using Berty.BoardCards.Behaviours;
using Berty.CardTransfer.Managers;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using UnityEngine;

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

        private void HandlePaymentStart()
        {
            if (core.IsForPay()) return;
            core.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            core.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (core.CardState == CardStateEnum.NewCard)
            {
                Destroy(gameObject);
                return;
            }
            core.SetMainState();
        }
    }
}