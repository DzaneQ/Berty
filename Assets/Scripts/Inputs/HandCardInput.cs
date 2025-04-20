using Berty.Gameplay;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Inputs
{
    public class HandCardInput : MonoBehaviour
    {
        private CardImage behaviour;
        private CardManager cardManager;

        void Awake()
        {
            behaviour = GetComponent<CardImage>();
        }

        void Start()
        {
            cardManager = FindObjectOfType<CardManager>();
        }
        public void CardClick()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            if (transform.parent.name.Contains("Dead")) behaviour.ReviveCard();
            else behaviour.ChangeSelection();
        }

        public void CardFocusOn()
        {
            cardManager.ShowLookupCard(behaviour.Sprite);
        }

        public void CardFocusOff()
        {
            cardManager.HideLookupCard();
        }
    }
}
