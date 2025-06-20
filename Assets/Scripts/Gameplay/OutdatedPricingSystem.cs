using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay
{
    internal class OutdatedPricingSystem
    {
        private OutdatedCardManager cardManager;
        private int? cardPrice;

        public OutdatedPricingSystem(OutdatedCardManager cm)
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