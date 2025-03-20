using Berty.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.UI.Card.Animation
{
    public class AnimatingCardImage : MonoBehaviour
    {
        private Vector3 targetPosition;
        private Vector3 targetRotation;
        private int coroutineCount;
        private SoundSystem sound;

        public int CoroutineCount
        {
            get => coroutineCount;
            private set
            {
                coroutineCount = value;
                if (coroutineCount < 0) throw new Exception("Negative coroutine count!");
            }
        }

        void Start()
        {
            AttachSound();
        }

        public void AttachSound()
        {
            sound = FindObjectOfType<SoundSystem>().GetComponent<SoundSystem>();
        }

        public void MoveCard(Vector3 pos, Vector3 rot, bool selecting, float durationSeconds)
        {
            SetTargetTransform(pos, rot);
            StartCoroutine(AnimateMove(selecting, durationSeconds));
        }

        private void SetTargetTransform(Vector3 pos, Vector3 rot)
        {
            targetPosition = pos;
            targetRotation = rot;
        }

        private IEnumerator AnimateMove(bool selecting, float durationSeconds)
        {
            CoroutineCount++;
            //Debug.Log("Is sound null when animating move? " + (sound == null));
            //Debug.Log("Targeted rotation: " + targetRotation);
            if (sound != null) sound.SelectSound(selecting);
            float currentTime = 0;
            for (; transform.eulerAngles != targetRotation;)
            {
                MoveFrame(ref currentTime, durationSeconds);
                yield return null;
            }
            //Debug.Log("Result rotation: " + transform.eulerAngles);
            CoroutineCount--;
        }

        private void MoveFrame(ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime)
            {
                transform.position = targetPosition;
                transform.eulerAngles = targetRotation;
            }
            else
            {
                //Debug.Log($"Current location for {transform.name} before: {transform.position.x}, {transform.position.y}, {transform.position.z}");
                float step = frameTime / (endTime - currentTime) * Vector3.Distance(transform.position, targetPosition);
                //Debug.Log($"Step value for {transform.name}: {step}");
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, targetRotation, step);
                //Debug.Log($"Current location for {transform.name} after: {transform.position.x}, {transform.position.y}, {transform.position.z}");
            }
            currentTime += frameTime;
        }
    }
}