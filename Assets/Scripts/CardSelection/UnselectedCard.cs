using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedCard : SelectStatus
{
    public UnselectedCard(CardImage cardImage) : base(cardImage)
    {

    }

    public override SelectStatus ChangePosition()
    {
        //Debug.Log("Attempting to select.");
        if (card.CanSelect())
        {
            card.SelectPosition();
            return new SelectedCard(card);
        }
        return this;
    }

    override public bool IsCardSelected()
    {
        return false;
    }
}
