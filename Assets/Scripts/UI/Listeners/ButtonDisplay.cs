using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using TMPro;
using UnityEngine;

namespace Berty.UI.Listeners
{
    public class ButtonDisplay : MonoBehaviour
    {
        public CornerButtonEnum ButtonType { get; private set; }

        private TMP_Text label;

        private void Awake()
        {
            label = transform.GetComponentInChildren<TMP_Text>();
            if (label == null) throw new Exception("TMP_Text component not found in button:" + name);

            EventManager.Instance.OnNewTurn += HandleNewTurn;
            EventManager.Instance.OnPaymentStart += HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm += HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel += HandlePaymentCancel;
            EventManager.Instance.OnStatusUpdated += HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved += HandleStatusRemoved;

            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewTurn -= HandleNewTurn;
            EventManager.Instance.OnPaymentStart -= HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm -= HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel -= HandlePaymentCancel;
            EventManager.Instance.OnStatusUpdated -= HandleStatusUpdated;
            EventManager.Instance.OnStatusRemoved -= HandleStatusRemoved;
        }

        private void HandleNewTurn()
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) HideCornerButton();
            else DisplayButton(CornerButtonEnum.EndTurn);
        }

        private void HandlePaymentStart(object sender, EventArgs args)
        {
            BoardCardBehaviour cardFocus = (BoardCardBehaviour)sender;
            if (cardFocus.StateMachine.HasState(CardStateEnum.NewTransform)) HideCornerButton();
            else DisplayButton(CornerButtonEnum.Undo);
        }

        private void HandlePaymentConfirm()
        {
            DisplayButton(CornerButtonEnum.EndTurn);
        }

        private void HandlePaymentCancel()
        {
            DisplayButton(CornerButtonEnum.EndTurn);
        }

        private void HandleStatusUpdated(object sender, EventArgs args)
        {
            Status status = (Status)sender;
            switch (status.Name)
            {
                case StatusEnum.ClickToApplyEffect:
                    HideCornerButton();
                    break;
            }
        }

        private void HandleStatusRemoved(object sender, StatusEventArgs args)
        {
            switch (args.StatusName)
            {
                case StatusEnum.ClickToApplyEffect:
                    DisplayButton(CornerButtonEnum.EndTurn);
                    break;
            }
        }

        private void DisplayButton(CornerButtonEnum buttonType)
        {
            ButtonType = buttonType;
            UpdateButtonLabel();
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            gameObject.SetActive(true);
        }

        private void HideCornerButton()
        {
            gameObject.SetActive(false);
        }

        private void UpdateButtonLabel()
        {
            label.text = ButtonType switch
            {
                CornerButtonEnum.EndTurn => GameLanguageManager.Instance.GetTextFromKey("end_turn"),
                CornerButtonEnum.Undo => GameLanguageManager.Instance.GetTextFromKey("undo"),
                _ => throw new Exception("Unknown button type.")
            };
        }
    }
}
