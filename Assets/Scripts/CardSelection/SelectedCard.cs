using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCard : SelectStatus
{
    private Vector3 unselectedPosition;
    private Vector3 unselectedRotation;


    public SelectedCard(RectTransform transform, CardImage card) : base(transform, card)
    {
        //Debug.Log("Selecting card...");
        unselectedPosition = cardTransform.position;
        unselectedRotation = cardTransform.eulerAngles;
        Vector3 posOffset = new Vector3(10f, 5f, 0f);
        Vector3 rotOffset = new Vector3(0f, 0f, -15f);
        cardTransform.position += posOffset;
        cardTransform.eulerAngles += rotOffset;
    }

    public override SelectStatus ChangePosition(bool CanSelect = true)
    {
        //Debug.Log("Attempting to deselect.");
        cardTransform.position = unselectedPosition;
        cardTransform.eulerAngles = unselectedRotation;
        return new UnselectedCard(cardTransform, card);
    }

    public override bool IsCardSelected => true;

    //public override void SetToBackup()
    //{
    //    
    //}

    //public override Transform ReturnCard()
    //{
    //    cardTransform.position = unselectedPosition;
    //    return returnTable;
    //}

    public override SelectStatus SetUnselected()
    {
        return ChangePosition();
    }
}
