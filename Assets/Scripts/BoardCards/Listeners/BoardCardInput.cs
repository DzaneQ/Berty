using Berty.BoardCards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class BoardCardInput : MonoBehaviour
    {
        private CardSpriteBehaviour behaviour;

        void Awake()
        {
            behaviour = GetComponent<CardSpriteBehaviour>();
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }
        public void OnMouseOver()
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
        }
    }
}
