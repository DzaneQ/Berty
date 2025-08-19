using Berty.Grid.Field;
using Berty.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Animation
{
    public class AnimatingCardSprite : MonoBehaviour
    {
        private float rotatingAngle;
        private Vector3 targetPosition;
        private int coroutineCount;
        private AudioSource soundSrc;

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

        public void AttachSound()
        {
            soundSrc = GetComponent<AudioSource>();
        }

        #region CardRotation
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
            //if (sound != null) sound.MoveSound(soundSrc);
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
        #endregion

        #region CardMovement
        public IEnumerator MoveToField(OutdatedFieldBehaviour target, float duration)
        {
            //Debug.Log($"Card initial coordinates: {transform.position.x}; {transform.position.y}; {transform.position.z}");
            SetTargetField(target);
            //Debug.Log($"Card target coordinates: {targetPosition.x}; {targetPosition.y}; {targetPosition.z}");
            yield return StartCoroutine(AnimateMove(duration));
        }

        private void SetTargetField(OutdatedFieldBehaviour target)
        {
            //targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
            targetPosition = transform.position;
            targetPosition.x = target.transform.position.x;
            targetPosition.z = target.transform.position.z;
        }

        private IEnumerator AnimateMove(float durationSeconds)
        {
            CoroutineCount++;
            //if (sound != null) sound.MoveSound(soundSrc);
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
        #endregion

        #region BarChange
        public IEnumerator MoveAndScaleBar(NewBar target, float duration)
        {
            yield return StartCoroutine(AnimateBarChange(target, duration));
        }

        private IEnumerator AnimateBarChange(NewBar target, float durationSeconds)
        {
            CoroutineCount++;
            float currentTime = 0;
            for (; target.barLocation != target.bar.localPosition;)
            {
                BarChangeFrame(target, ref currentTime, durationSeconds);
                yield return null;
            }
            CoroutineCount--;
        }

        private void BarChangeFrame(NewBar target, ref float currentTime, float endTime)
        {
            float frameTime = Time.deltaTime;
            if (currentTime + frameTime >= endTime)
            {
                target.bar.localPosition = target.barLocation;
                target.rend.size = target.rendSize;
            }
            else
            {
                float stepPiece = frameTime / (endTime - currentTime);
                float distanceStep = stepPiece * Vector3.Distance(target.bar.localPosition, target.barLocation);
                float sizeStep = stepPiece * Vector2.Distance(target.rend.size, target.rendSize);
                target.bar.localPosition = Vector3.MoveTowards(target.bar.localPosition, target.barLocation, distanceStep);
                target.rend.size = Vector2.MoveTowards(target.rend.size, target.rendSize, sizeStep);
            }
            currentTime += frameTime;
        }
        #endregion

        #region Sound
        //public void PutCardSound()
        //{
        //    if (sound == null) return;
        //    sound.PutSound(soundSrc);
        //}

        //public void TakeCardSound()
        //{
        //    if (sound == null) return;
        //    sound.TakeSound(transform);
        //}

        //public void AttackingSound(CharacterConfig character)
        //{
        //    if (sound == null) return;
        //    sound.AttackSound(soundSrc, character.AttackSound);
        //}

        //public void ConfirmSound()
        //{
        //    if (sound == null) return;
        //    sound.ConfirmSound(soundSrc);
        //}
        #endregion
    }
}