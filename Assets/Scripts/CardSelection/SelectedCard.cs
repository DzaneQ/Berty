using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCard : SelectStatus
{
    private Vector3 unselectedPosition;
    private Transform returnTable;


    public SelectedCard(RectTransform transform) : base(transform)
    {
        unselectedPosition = card.position;
        Vector3 offset = new Vector3(0f, 25f, 0f);
        card.position += offset;
        //card.SelectPosition();
    }

    public override SelectStatus ChangePosition(bool CanSelect = true)
    {
        //Debug.Log("Attempting to deselect.");
        card.position = unselectedPosition;
        return new UnselectedCard(card);
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
}
