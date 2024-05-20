using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedCard : SelectStatus
{
    public UnselectedCard(RectTransform transform, CardImage card) : base(transform, card)
    {
        Debug.Log("Unselecting card: " + card.gameObject.name + "; rotation: " + card.transform.eulerAngles.z);
    }

    public override SelectStatus ChangePosition(bool canSelect)
    {
        if (!canSelect) return this;
        return new SelectedCard(cardTransform, card);
        
    }

    public override bool IsCardSelected => false;

    //public override void SetToBackup()
    //{
    //    returnTable = cardTransform.parent;
    //}

    //public override SelectStatus KillCard()
    //{
    //    return new DeadCard(cardTransform, card);
    //}

    //public override Transform ReturnCard()
    //{
    //    return returnTable;
    //}


    public override SelectStatus SetUnselected() // TODO: Remove this after fixing selection state.
    {
        Debug.LogWarning("Unselecting unselected card!");
        return this;
    }
}
