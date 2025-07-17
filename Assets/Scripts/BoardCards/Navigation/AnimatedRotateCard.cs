using Berty.BoardCards.Behaviours;
using Berty.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Navigation
{
    public class AnimatedRotateCard : MonoBehaviour, IRotateCard
    {
        private BoardCardMovableObject card;
        private float rotatingAngle;
        private int coroutineCount;

        public int CoroutineCount
        {
            get => coroutineCount;
            private set
            {
                coroutineCount = value;
                if (coroutineCount < 0) throw new Exception($"Negative coroutine count for card {name}!");
            }
        }

        private void Awake()
        {
            rotatingAngle = 0;
            coroutineCount = 0;
        }

        void Start()
        {
            card = GetComponent<BoardCardMovableObject>();
        }

        public void Execute(int angle)
        {
            StartCoroutine(RotateCardCoroutine(angle));
        }

        private IEnumerator RotateCardCoroutine(int angle)
        {
            card.DisableInteraction();
            //HideBars();
            yield return StartCoroutine(RotateObject(-angle, 1f));
            card.EnableInteraction();
            yield return null;
        }

        public IEnumerator RotateObject(float angle, float durationSeconds)
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
                transform.Rotate(Vector3.forward, rotatingAngle);
                rotatingAngle = 0;
            }
            else
            {
                float step = frameTime / (endTime - currentTime);
                float frameAngle = rotatingAngle * step;
                transform.Rotate(Vector3.forward, frameAngle);
                rotatingAngle -= frameAngle;
            }
            currentTime += frameTime;
        }
    }
}
