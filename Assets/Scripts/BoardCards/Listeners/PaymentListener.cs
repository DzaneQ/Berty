using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Managers;
using System;
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

        private void HandlePaymentStart(object sender, EventArgs args)
        {
            if (sender.Equals(core)) return;
            core.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            if (core.CardState == CardStateEnum.Attacking)
            {
                SoundManager.Instance.AttackSound(core.SoundSource, core.BoardCard.CharacterConfig.AttackSound);
                EventManager.Instance.RaiseOnDirectlyAttacked(core);
            }
            else if (core.CardState == CardStateEnum.NewCard)
            {
                SoundManager.Instance.ConfirmSound(core.SoundSource);
                EventManager.Instance.RaiseOnAttackNewStand(core);
            }    
            core.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (core.CardState == CardStateEnum.NewCard)
            {
                FieldToHandManager.Instance.RetrieveCardOnHold();
                SoundManager.Instance.TakeSound(core.transform);
                core.DestroyCard();
                return;
            }
            core.SetMainState();
        }
    }
}