using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using System;
using System.Collections;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    public class AnimatedMoveCard : BoardCardBehaviour, IMoveCard
    {
        private float durationSeconds;
        private Vector3 targetPosition;
        private int _coroutineCount;
        private Transform cardSetTransform;

        public int CoroutineCount
        {
            get => _coroutineCount;
            private set
            {
                _coroutineCount = value;
                if (_coroutineCount == 0) Navigation.HandleAnimationEnd();
                if (_coroutineCount < 0) throw new Exception($"Negative coroutine count for card {name}!");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _coroutineCount = 0;
            durationSeconds = EntityLoadManager.Instance.Game.GameConfig.AnimationSeconds;
        }

        void Start()
        {
            cardSetTransform = transform.parent;
        }

        public void ToField(FieldBehaviour field)
        {
            StartCoroutine(MoveCardCoroutine(field));
        }

        private IEnumerator MoveCardCoroutine(FieldBehaviour target)
        {
            AlignmentEnum align = BoardCard.Align;
            EventManager.Instance.RaiseOnHighlightEnd();
            yield return MoveToField(target, durationSeconds);
            Navigation.HandleAfterMoveAnimation();
            yield return null;
        }

        private IEnumerator MoveToField(FieldBehaviour target, float duration)
        {
            SetTargetField(target);
            yield return StartCoroutine(AnimateMove(target, duration));
        } 

        private void SetTargetField(FieldBehaviour target)
        {
            targetPosition = cardSetTransform.position;
            targetPosition.x = target.transform.position.x;
            targetPosition.z = target.transform.position.z;
        }

        private IEnumerator AnimateMove(FieldBehaviour target, float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            for (; targetPosition != cardSetTransform.position;)
            {
                MoveFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            SetCardParentToField(target);
            CoroutineCount--;
        }

        private void MoveFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime) cardSetTransform.position = targetPosition;
            else
            {
                float step = frameTime / (endTime - currentTime) * Vector3.Distance(cardSetTransform.position, targetPosition);
                cardSetTransform.position = Vector3.MoveTowards(cardSetTransform.position, targetPosition, step);
            }
            currentTime += frameTime;
        }

        private void SetCardParentToField(FieldBehaviour field)
        {
            cardSetTransform.SetParent(field.transform, true);
            field.UpdateField();
        }
    }
}
