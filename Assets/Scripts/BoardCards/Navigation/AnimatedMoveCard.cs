using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Grid.Field;
using Berty.Grid.Field.Behaviour;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Navigation
{
    public class AnimatedMoveCard : MonoBehaviour, IMoveCard
    {
        private BoardCardCore card;
        private Vector3 targetPosition;
        private int coroutineCount;

        public int CoroutineCount
        {
            get => coroutineCount;
            private set
            {
                coroutineCount = value;
                if (coroutineCount == 0) card.HandleAnimationEnd();
                if (coroutineCount < 0) throw new Exception($"Negative coroutine count for card {name}!");
            }
        }

        private void Awake()
        {
            coroutineCount = 0;
        }

        void Start()
        {
            card = GetComponent<BoardCardCore>();
        }

        public void ToField(FieldBehaviour field)
        {
            //StartCoroutine(RotateCardCoroutine(field));
        }

        public IEnumerator MoveToField(FieldBehaviour target, float duration)
        {
            //Debug.Log($"Card initial coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
            SetTargetField(target);
            //Debug.Log($"Card target coordinates: {targetPosition.x}; {targetPosition.y}; {targetPosition.z}");
            yield return StartCoroutine(AnimateMove(duration));
        }

        private void SetTargetField(FieldBehaviour target)
        {
            //targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            targetPosition = transform.position;
            targetPosition.x = target.transform.position.x;
            targetPosition.z = target.transform.position.z;
        }

        private IEnumerator AnimateMove(float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            //Debug.Log($"Target location for {transform.name}: {targetPosition.x}, {targetPosition.y}, {targetPosition.z}");
            for (; targetPosition != transform.position;)
            {
                MoveFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            CoroutineCount--;
        }

        private void MoveFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime) transform.position = targetPosition;
            else
            {
                //Debug.Log($"Current location for {transform.name} before: {transform.position.x}, {transform.position.y}, {transform.position.z}");
                float step = frameTime / (endTime - currentTime) * Vector3.Distance(transform.position, targetPosition);
                //Debug.Log($"Step value for {transform.name}: {step}");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                //Debug.Log($"Current location for {transform.name} after: {transform.position.x}, {transform.position.y}, {transform.position.z}");
            }
            currentTime += frameTime;
        }
    }
}
