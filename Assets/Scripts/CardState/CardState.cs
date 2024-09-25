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
        //Debug.Log($"Setting state {GetType()} for {card.name}");
    }
    
    public virtual CardState ActivateCard() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState DeactivateCard() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    //public virtual void HandleFieldCollision() => card.ApplyPhysics(false);
    public abstract void HandleClick();
    public virtual void HandleSideClick() { }
    //public virtual void TakePaidAction() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState TakePaidAction() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState AdjustTransformChange(int buttonIndex) => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual void Cancel() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual bool IsForPaymentConfirmation() => false;
    //public virtual bool IsJudgementRevenge() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState SetActive => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState SetIdle => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState SetTelecinetic => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual CardState SetTargetable => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
    public virtual void EnableButtons() => throw new InvalidOperationException($"Invalid method for: {GetType()} in {card.name}");
}
