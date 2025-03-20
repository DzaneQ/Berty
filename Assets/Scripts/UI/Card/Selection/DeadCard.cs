/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadCard : SelectStatus
{
    public DeadCard(RectTransform transform, CardImage card) : base(transform, card)
    {

    }

    public override SelectStatus ChangePosition(bool CanSelect = true)
    {
        Debug.Log("Attempting to revive.");
        card.ReviveCard();
        return new UnselectedCard(cardTransform, card);
    }

    public override bool IsCardSelected => true;

    public override void SetToBackup()
    {
        returnTable = card.parent;
    }

    public override Transform ReturnCard()
    {
        card.position = unselectedPosition;
        return returnTable;
    }

    //override public SelectStatus SetUnselected(Transform cardTransform) => ChangePosition(true);
    public override SelectStatus SetUnselected()
    {
        return new UnselectedCard(card);
    }
}*/
