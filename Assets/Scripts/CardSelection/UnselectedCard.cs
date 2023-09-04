using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedCard : SelectStatus
{
    public UnselectedCard(RectTransform transform) : base(transform)
    {
        //Debug.Log("Unselecting card: " + card.gameObject.name);
    }

    public override SelectStatus ChangePosition(bool canSelect)
    {
        if (!canSelect) return this;
        return new SelectedCard(card);
        
    }

    public override bool IsCardSelected => false;

    public override void SetToBackup()
    {
        Debug.LogWarning("No backup for unselected card: " + card.name);
    }

    //public override SelectStatus SetUnselected(Transform cardTransform)
    //{
    //    return this;
    //}
}
