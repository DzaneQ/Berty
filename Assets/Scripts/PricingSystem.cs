using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PricingSystem
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
        if (cardManager.SelectedCards().Count == cardPrice) return true;
        return false;
    }
}
