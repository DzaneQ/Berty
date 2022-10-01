using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardState
{
    protected CardSprite card;
    protected CardState(CardSprite sprite)
    {
        card = sprite;
    }
    
    public virtual CardState ActivateCard() => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual CardState DeactivateCard() => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual void HandleFieldCollision() => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public abstract void HandleClick();
    public virtual void TakePaidAction() => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual CardState AdjustTransformChange(int buttonIndex) => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual void Cancel() => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual bool IsForPaymentConfirmation() => false;
    public virtual CardState SetActive => throw new InvalidOperationException($"Invalid method for: {GetType()}");
    public virtual CardState SetIdle => throw new InvalidOperationException($"Invalid method for: {GetType()}");
}
