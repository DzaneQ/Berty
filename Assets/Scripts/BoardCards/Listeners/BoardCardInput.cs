using Berty.BoardCards;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Button;
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

        /*public void OnMouseOver()
        {
            //Debug.Log("OnMouseOver event trigger on: " + name);
            if (IsLeftClicked()) behaviour.State.HandleClick();
            else if (IsRightClicked()) behaviour.State.HandleSideClick();
        }

        private bool IsLeftClicked()
        {
            //Debug.Log($"Card {name} was left clicked. Is it locked? {IsLocked()}");
            if (!behaviour.IsLocked() && !behaviour.IsAnimating() && Input.GetMouseButtonDown(0)) return true;
            else return false;
        }

        private bool IsRightClicked()
        {
            if (!behaviour.IsLocked() && !behaviour.IsAnimating() && Input.GetMouseButtonDown(1)) return true;
            else return false;
        }*/

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
