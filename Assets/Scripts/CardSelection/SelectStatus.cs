using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class SelectStatus
{
    protected RectTransform cardTransform;
    protected CardImage card;

    protected SelectStatus(RectTransform cardImageTransform, CardImage cardImage) 
    {
        cardTransform = cardImageTransform;
        card = cardImage;
    }

    public virtual SelectStatus ChangePosition(bool canSelect) => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
    public virtual bool IsCardSelected { get => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!"); }
    public virtual void SetToBackup() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
    public virtual Transform ReturnCard() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
    //public virtual SelectStatus KillCard() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
    public virtual SelectStatus SetUnselected() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
}
