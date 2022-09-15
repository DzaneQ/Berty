using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCard : SelectStatus
{
    private Vector3 unselectedPosition;

    public SelectedCard(CardImage cardImage) : base(cardImage)
    {

    }

    public override SelectStatus ChangePosition()
    {
        //Debug.Log("Attempting to deselect.");
        card.DeselectPosition();
        return new UnselectedCard(card);
    }

    override public bool IsCardSelected()
    {
        return true;
    }
}
