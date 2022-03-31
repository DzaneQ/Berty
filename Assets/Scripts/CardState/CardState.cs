using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardState
{
    protected CardSprite card;
    public abstract void InitiateState(CardSprite cardSprite);
    public abstract void HandleFieldCollision();
    public abstract void HandleClick();
    public abstract CardState HandleAlignmentChange();
    public abstract CardState AdjustTransformChange(int buttonIndex);
    public abstract CardState GoToNext();
}
