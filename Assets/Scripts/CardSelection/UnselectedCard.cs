using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnselectedCard : SelectStatus
{
    public UnselectedCard(RectTransform transform, AnimatingCardImage animating) : base(transform, animating)
    {
        //Debug.Log("Unselecting card: " + card.gameObject.name + "; rotation: " + card.transform.eulerAngles.z);
    }

    public override SelectStatus ChangePosition(bool isAnimated)
    {
        if (IsAnimating()) return this;
        return new SelectedCard(cardTransform, animating);
    }

    //public override bool IsCardSelected => false;

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


    public override SelectStatus UnselectAutomatically() => this;
}
