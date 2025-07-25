using Berty.BoardCards;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
using Berty.BoardCards.Managers;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class BoardCardInput : MonoBehaviour
    {
        private BoardCardCore behaviour;

        void Awake()
        {
            behaviour = GetComponent<BoardCardCore>();
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }

        void OnEnable()
        {
            EnableButtons();
        }

        void OnDisable()
        {
            DisableButtons();
        }

        public void OnMouseOver()
        {
            //Debug.Log("OnMouseOver event trigger on: " + name);
            if (IsLeftClicked()) HandleClick();
            //else if (IsRightClicked()) HandleSideClick();
        }

        private bool IsLeftClicked()
        {
            //Debug.Log($"Card {name} was left clicked.");
            return Input.GetMouseButtonDown(0);
        }

        /*private bool IsRightClicked()
        {
            if (!behaviour.IsLocked() && !behaviour.IsAnimating() && Input.GetMouseButtonDown(1)) return true;
            else return false;
        }*/

        private void HandleClick()
        {
            switch (behaviour.CardState)
            {
                case CardStateEnum.Active:
                    BoardCardActionManager.Instance.PrepareToAttack(behaviour);
                    break;
                case CardStateEnum.NewTransform:
                case CardStateEnum.NewCard:
                case CardStateEnum.Attacking:
                    BoardCardActionManager.Instance.ConfirmPayment(behaviour);
                    break;
                case CardStateEnum.Idle:
                case CardStateEnum.Telekinetic:
                    break;
                default:
                    throw new Exception("Clicked on card of an unknown state");
            }
        }

        private void DisableButtons()
        {
            foreach (CardButton button in behaviour.CardNavigation.Buttons) button.DisableButton();
        }

        private void EnableButtons()
        {
            foreach (CardButton button in behaviour.CardNavigation.Buttons) button.EnableButton();
        }
    }
}
