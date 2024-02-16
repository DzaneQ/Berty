using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCard : SelectStatus
{
    private Vector3 unselectedPosition;
    private Transform returnTable;


    public SelectedCard(RectTransform transform, CardImage card) : base(transform, card)
    {
        unselectedPosition = cardTransform.position;
        Vector3 offset = new Vector3(0f, 25f, 0f);
        cardTransform.position += offset;
        //card.SelectPosition();
    }

    public override SelectStatus ChangePosition(bool CanSelect = true)
    {
        //Debug.Log("Attempting to deselect.");
        cardTransform.position = unselectedPosition;
        return new UnselectedCard(cardTransform, card);
    }

    public override bool IsCardSelected => true;

    public override void SetToBackup()
    {
        returnTable = cardTransform.parent;
    }

    public override Transform ReturnCard()
    {
        cardTransform.position = unselectedPosition;
        return returnTable;
    }

    //public override SelectStatus KillCard() // TODO: Remove this after fixing selection state.
    //{
    //    return new DeadCard(cardTransform, card);
    //}

    //override public SelectStatus SetUnselected(Transform cardTransform) => ChangePosition(true);
    public override SelectStatus SetUnselected()
    {
        return new UnselectedCard(cardTransform, card);
    }
}
