using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Gameplay.Managers;
using Berty.Grid.Field;
using Berty.Grid.Field.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    public class AnimatedMoveCard : MonoBehaviour, IMoveCard
    {
        private float durationSeconds;
        private BoardCardCore card;
        private Vector3 targetPosition;
        private int _coroutineCount;
        private Transform cardSetTransform;

        public int CoroutineCount
        {
            get => _coroutineCount;
            private set
            {
                _coroutineCount = value;
                if (_coroutineCount == 0) card.HandleAnimationEnd();
                if (_coroutineCount < 0) throw new Exception($"Negative coroutine count for card {name}!");
            }
        }

        private void Awake()
        {
            _coroutineCount = 0;
            durationSeconds = CoreManager.Instance.Game.GameConfig.AnimationSeconds;
        }

        void Start()
        {
            cardSetTransform = transform.parent;
            card = GetComponent<BoardCardCore>();
        }

        // BUG: After the animation is over, the destination field is not colorized when the card is hovered over.
        public void ToField(FieldBehaviour field)
        {
            StartCoroutine(MoveCardCoroutine(field));
        }

        private IEnumerator MoveCardCoroutine(FieldBehaviour target)
        {
            AlignmentEnum align = card.BoardCard.Align;
            card.CardNavigation.DisableInteraction();
            yield return MoveToField(target, durationSeconds);
            card.CardNavigation.HandleAfterMoveAnimation();
            yield return null;
        }

        private IEnumerator MoveToField(FieldBehaviour target, float duration)
        {
            //Debug.Log($"Card initial coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
            SetTargetField(target);
            //Debug.Log($"Card target coordinates: {targetPosition.x}; {targetPosition.y}; {targetPosition.z}");
            yield return StartCoroutine(AnimateMove(target, duration));
        } 

        private void SetTargetField(FieldBehaviour target)
        {
            //targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            targetPosition = cardSetTransform.position;
            targetPosition.x = target.transform.position.x;
            targetPosition.z = target.transform.position.z;
        }

        private IEnumerator AnimateMove(FieldBehaviour target, float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            //Debug.Log($"Target location for {transform.name}: {targetPosition.x}, {targetPosition.y}, {targetPosition.z}");
            for (; targetPosition != cardSetTransform.position;)
            {
                MoveFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            cardSetTransform.SetParent(target.transform, true);
            CoroutineCount--;
        }

        private void MoveFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime) cardSetTransform.position = targetPosition;
            else
            {
                //Debug.Log($"Current location for {transform.name} before: {transform.position.x}, {transform.position.y}, {transform.position.z}");
                float step = frameTime / (endTime - currentTime) * Vector3.Distance(cardSetTransform.position, targetPosition);
                //Debug.Log($"Step value for {transform.name}: {step}");
                cardSetTransform.position = Vector3.MoveTowards(cardSetTransform.position, targetPosition, step);
                //Debug.Log($"Current location for {transform.name} after: {transform.position.x}, {transform.position.y}, {transform.position.z}");
            }
            currentTime += frameTime;
        }
    }
}
