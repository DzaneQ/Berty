using Berty.BoardCards;
using Berty.Grid.Field;
using Berty.Grid;
using Berty.Gameplay;
using UnityEngine;

namespace Berty.Display.View
{
    public class RotateCamera : MonoBehaviour
    {
        float edgeWidth = 10f;
        float rotManSpeed = 90f;
        float rotAutoMultiplier = 0.25f;

        void Update()
        {
            HandleCameraTransform();
        }

        public float RightAngleValue()
        {
            float cameraRotation = AngleValue();
            return Mathf.Round(cameraRotation / 90f) * 90f;
        }

        private void HandleCameraTransform()
        {
            if (Input.mousePosition.x <= edgeWidth) RotateCameraCounterclockwise(rotManSpeed);
            else if (Input.mousePosition.x >= Screen.width - edgeWidth) RotateCameraClockwise(rotManSpeed);
            else RotateAutomatically();
        }

        private void RotateCameraCounterclockwise(float rotSpeed)
        {
            transform.Rotate(Vector3.back, rotSpeed * Time.deltaTime);
        }

        private void RotateCameraClockwise(float rotSpeed)
        {
            transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
        }

        private void RotateAutomatically()
        {
            float currentAngle = AngleValue();
            if (Mathf.Approximately(currentAngle % 90, 0)) return;
            float rightAngle = RightAngleValue();
            Vector3 targetAngles = transform.localEulerAngles;
            if (Mathf.Abs(currentAngle - rightAngle) < rotManSpeed * rotAutoMultiplier) targetAngles.z = rightAngle;
            else targetAngles.z = (currentAngle - rightAngle) * (1 - rotAutoMultiplier) + rightAngle;
            transform.localEulerAngles = targetAngles;
        }

        private float AngleValue() => transform.localEulerAngles.z;
    }
}