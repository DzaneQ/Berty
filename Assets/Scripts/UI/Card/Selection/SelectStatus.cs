using Berty.UI.Card.Animation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.UI.Card.Selection
{
    abstract public class SelectStatus
    {
        protected RectTransform cardTransform;
        protected AnimatingCardImage animating;

        protected SelectStatus(RectTransform cardImageTransform, AnimatingCardImage animation)
        {
            cardTransform = cardImageTransform;
            animating = animation;
        }

        public virtual SelectStatus ChangePosition(bool isAnimated) => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
        public virtual bool IsCardSelected { get => false; }
        //public virtual void SetToBackup() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
        //public virtual Transform ReturnCard() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
        //public virtual SelectStatus KillCard() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
        public virtual SelectStatus UnselectAutomatically() => throw new InvalidOperationException("Card " + cardTransform.gameObject.name + " is faulty!");
        protected bool IsAnimating()
        {
            if (animating == null) return false;
            return animating.CoroutineCount > 0;
        }
    }
}