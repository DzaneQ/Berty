using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay
{
    internal class PricingSystem
    {
        private CardManager cardManager;
        private int cardPrice;

        public PricingSystem(CardManager cm)
        {
            cardManager = cm;
        }

        public void DemandPayment(int price)
        {
            cardPrice = price;
        }

        public bool CheckOffer()
        {
            return cardManager.SelectedCards().Count == cardPrice;
        }
    }
}