using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCard : SelectStatus
{
    private Vector3 unselectedPosition;
    private Vector3 unselectedRotation;


    public SelectedCard(RectTransform transform, AnimatingCardImage animating) : base(transform, animating)
    {
        //Debug.Log("Selecting card...");
        unselectedPosition = cardTransform.position;
        unselectedRotation = cardTransform.eulerAngles;
        Vector3 posOffset = new Vector3(10f, 5f, 0f);
        Vector3 rotOffset = new Vector3(0f, 0f, -15f);
        if (animating == null || !cardTransform.parent.gameObject.activeSelf)
        {
            cardTransform.position += posOffset;
            cardTransform.eulerAngles += rotOffset;
        }
        else animating.MoveCard(unselectedPosition + posOffset, unselectedRotation + rotOffset, true, 0.15f);
    }

    public override SelectStatus ChangePosition(bool animate)
    {
        if (IsAnimating()) return this;
        Debug.Log("Attempting to deselect " + animating.name);
        Debug.Log("Unselect rotation before change: " + unselectedRotation);
        if (!animate || animating == null)
        {
            cardTransform.position = unselectedPosition;
            cardTransform.eulerAngles = unselectedRotation;
        }
        else animating.MoveCard(unselectedPosition, unselectedRotation, false, 0.15f);
        return new UnselectedCard(cardTransform, animating);
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

    public override SelectStatus UnselectAutomatically() => ChangePosition(false);
}
