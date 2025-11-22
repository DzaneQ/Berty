using Berty.BoardCards.Behaviours;
using Berty.Gameplay;
using Berty.Gameplay.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    public class AnimatedRotateCard : BoardCardBehaviour, IRotateCard
    {
        private float durationSeconds;
        private float rotatingAngle;
        private int _coroutineCount;
        private Transform cardSetTransform;

        public int CoroutineCount
        {
            get => _coroutineCount;
            private set
            {
                _coroutineCount = value;
                if (_coroutineCount == 0) Core.HandleAnimationEnd();
                if (_coroutineCount < 0) throw new Exception($"Negative coroutine count for card {name}!");
            }
        }

        protected override void Awake()
        {
            base.Awake();
            rotatingAngle = 0;
            _coroutineCount = 0;
            durationSeconds = EntityLoadManager.Instance.Game.GameConfig.AnimationSeconds;
            cardSetTransform = transform.parent;
        }

        public void ByAngleWithoutAnimation(int angle)
        {
            cardSetTransform.Rotate(Vector3.forward, angle);
        }

        public void ByAngle(int angle)
        {
            StartCoroutine(RotateCardCoroutine(angle));
        }

        private IEnumerator RotateCardCoroutine(int angle)
        {
            EventManager.Instance.RaiseOnHighlightEnd();
            Bars.HideBars();
            yield return StartCoroutine(RotateObject(angle, durationSeconds));
        }

        private IEnumerator RotateObject(float angle, float durationSeconds)
        {
            SetRotation(angle);
            yield return StartCoroutine(AnimateRotate(durationSeconds));
        }

        private void SetRotation(float angle)
        {
            if (rotatingAngle != 0) Debug.LogWarning("Rotating angles value set at " + rotatingAngle);
            rotatingAngle += angle;
            //endingRotation = Quaternion.Euler(0f, 0f, -rotatingAngle) * transform.rotation;
        }

        private IEnumerator AnimateRotate(float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            for (; rotatingAngle != 0;)
            {
                RotateFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            CoroutineCount--;
        }

        private void RotateFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (rotatingAngle == 0) throw new Exception("Rotating angle at 0!");
            else if (frameTime + currentTime >= endTime)
            {
                cardSetTransform.Rotate(Vector3.forward, rotatingAngle);
                rotatingAngle = 0;
            }
            else
            {
                float step = frameTime / (endTime - currentTime);
                float frameAngle = rotatingAngle * step;
                cardSetTransform.Rotate(Vector3.forward, frameAngle);
                rotatingAngle -= frameAngle;
            }
            currentTime += frameTime;
        }
    }
}
