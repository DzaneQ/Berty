using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedCard : SelectStatus
{
    public UnselectedCard(RectTransform transform, CardImage card) : base(transform, card)
    {
        //Debug.Log("Unselecting card: " + card.gameObject.name);
    }

    public override SelectStatus ChangePosition(bool canSelect)
    {
        if (!canSelect) return this;
        return new SelectedCard(cardTransform, card);
        
    }

    public override bool IsCardSelected => false;

    public override void SetToBackup()
    {
        Debug.LogWarning("No backup for unselected card: " + cardTransform.name);
    }

    //public override SelectStatus KillCard()
    //{
    //    return new DeadCard(cardTransform, card);
    //}

    public override SelectStatus SetUnselected() // TODO: Remove this after fixing selection state.
    {
        Debug.LogWarning("Unselecting unselected card!");
        return this;
    }
}
