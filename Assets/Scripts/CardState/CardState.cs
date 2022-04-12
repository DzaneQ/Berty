using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardState
{
    protected CardSprite card;
    public abstract void InitiateState(CardSprite cardSprite);
    public virtual void HandleFieldCollision() => throw new System.InvalidOperationException();
    public abstract void HandleClick();
    public virtual void TakePaidAction() => throw new System.InvalidOperationException();
    public virtual CardState AdjustTransformChange(int buttonIndex) => throw new System.InvalidOperationException();
    public abstract CardState GoToNext();
    public virtual void Cancel() => throw new System.InvalidOperationException();
    public virtual bool IsForPaymentConfirmation() => false;

}
