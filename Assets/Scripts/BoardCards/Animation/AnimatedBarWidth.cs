using Berty.BoardCards.Bar;
using Berty.BoardCards.Behaviours;
using Berty.Gameplay;
using Berty.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Berty.BoardCards.Animation
{
    public class AnimatedBarWidth : MonoBehaviour, IBarWidth
    {
        private const float durationSeconds = 1f;

        private BoardCardCore card;
        private int _coroutineCount;
        private SpriteRenderer rend;
        private Vector3 targetLocation;
        private Vector2 targetSize;

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
            rend = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            card = GetComponentInParent<BoardCardCore>();
        }

        public void SetVectorsWithoutAnimation(Vector3 targetLocation, Vector2 targetSize)
        {
            transform.localPosition = targetLocation;
            rend.size = targetSize;
        }

        public void AdvanceToVectors(Vector3 targetLocation, Vector2 targetSize)
        {
            this.targetLocation = targetLocation;
            this.targetSize = targetSize;
            card.CardNavigation.DisableInteraction();
            StartCoroutine(MoveAndScaleBar(durationSeconds));
        }

        private IEnumerator MoveAndScaleBar(float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            for (; targetLocation != transform.localPosition;)
            {
                BarChangeFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            CoroutineCount--;
        }

        private void BarChangeFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime)
            {
                transform.localPosition = targetLocation;
                rend.size = targetSize;
            }
            else
            {
                float stepPiece = frameTime / (endTime - currentTime);
                float distanceStep = stepPiece * Vector3.Distance(transform.localPosition, targetLocation);
                float sizeStep = stepPiece * Vector2.Distance(rend.size, targetSize);
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetLocation, distanceStep);
                rend.size = Vector2.MoveTowards(rend.size, targetSize, sizeStep);
            }
            currentTime += frameTime;
        }
    }
}
