using Berty.BoardCards;
using Berty.Grid.Field;
using Berty.Grid;
using Berty.Gameplay;
using UnityEngine;

namespace Berty.Display
{
    public class CameraMechanics : MonoBehaviour
    {
        float edgeWidth = 10f;
        float rotManSpeed = 90f;
        float rotAutoMultiplier = 0.25f;

        private OutdatedFieldBehaviour[] fields;
        private CardSpriteBehaviour selectedCard;
        Camera cam;
        private Turn turn;

        private OutdatedFieldBehaviour targetField;
        private OutdatedFieldBehaviour lastTarget;

        private Color attackColor = new Color(1f, 0.55f, 0f, 1f);
        private Color blockColor = new Color(0f, 0.35f, 0.8f, 1f);

        private Color highlightAttackColor;
        private Color highlightBlockColor;

        public CardSpriteBehaviour FocusedCard => selectedCard;


        private void Awake()
        {
            cam = GetComponent<Camera>();
            //AssignFields();
        }

        private void Start()
        {
            AdjustHighlightSaturation(0.8f);
        }

        void Update()
        {
            HandleCameraTransform();
            //HandleCardSpriteFocus();
        }

        private void AssignFields()
        {
            FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
            turn = fg.Turn;
            fields = new OutdatedFieldBehaviour[9];
            if (fields.Length != fg.transform.childCount) Debug.LogError("Number of fields is not equal to number of child count!");
            for (int index = 0; index < fields.Length; index++) fields[index] = fg.transform.GetChild(index).GetComponent<OutdatedFieldBehaviour>();
        }

        private void AdjustHighlightSaturation(float saturation)
        {
            highlightAttackColor = new Color(
                Saturate(attackColor.r, saturation), Saturate(attackColor.g, saturation), Saturate(attackColor.b, saturation));
            highlightBlockColor = new Color(
                Saturate(blockColor.r, saturation), Saturate(blockColor.g, saturation), Saturate(blockColor.b, saturation));
            //Debug.Log($"Highlight attack color values: {attackColor.r}, {attackColor.g}, {attackColor.b}");
        }

        private float Saturate(float colorValue, float saturation)
        {
            //Debug.Log("Saturate return value: " + ((255 - colorValue) * (1 - saturation) + colorValue));
            return (1 - colorValue) * (1 - saturation) + colorValue;
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

        public float RightAngleValue()
        {
            float cameraRotation = AngleValue();
            //Debug.Log("Camera rotation: " + cameraRotation);
            return Mathf.Round(cameraRotation / 90) * 90;
        }

        private float AngleValue() => transform.localEulerAngles.z;

        private void HandleCardSpriteFocus()
        {
            if (turn.InteractableDisabled) return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int fieldIndex;
            if (Physics.Raycast(ray, out hit) && IsValidField(hit, out fieldIndex))
            {
                targetField = fields[fieldIndex];
                if (lastTarget == null) lastTarget = targetField;
                if (targetField != lastTarget || selectedCard == null) SetTargets(targetField);
                //Debug.DrawRay(transform.position, hit.point - transform.position, Color.blue);
                lastTarget = targetField;
            }
            else
            {
                if (targetField == null)
                {
                    if (lastTarget != null) Debug.LogError("Last target not null with target renderer being null");
                    return;
                }
                targetField = null;
                lastTarget = null;
                ClearTarget();
            }
        }

        public void SetTargets(OutdatedFieldBehaviour sourceField)
        {
            if (sourceField.OccupantCard == selectedCard) return;
            if (selectedCard != null) ClearTarget();
            selectedCard = sourceField.OccupantCard;
            selectedCard.EnableButtons();
            selectedCard.ShowLookupCard(false);
            bool riposte = false;
            foreach (int[] distance in selectedCard.Character.AttackRange)
            {
                OutdatedFieldBehaviour targetField = selectedCard.GetTargetField(distance);
                if (targetField == null) continue;
                bool block = false;
                if (targetField.IsOccupied())
                {
                    CardSpriteBehaviour targetCard = targetField.OccupantCard;
                    if (!riposte &&
                        targetCard.Character.CanRiposte(targetCard.GetFieldDistance(sourceField))) riposte = true;
                    if (targetCard.Character.CanBlock(targetCard.GetFieldDistance(sourceField))) block = true;
                }
                HighlightTarget(targetField, block);
            }
            if (riposte) HighlightTarget(sourceField);
        }

        private void HighlightTarget(OutdatedFieldBehaviour target, bool blockState = false)
        {
            target.HighlightField(blockState ? highlightBlockColor : highlightAttackColor);
        }

        public void ClearTarget()
        {
            if (selectedCard == null) return;
            foreach (OutdatedFieldBehaviour field in fields)
            {
                //field.FieldOutline.enabled = false;
                field.UnhighlightField();
            }
            turn.CM.HideLookupCard(false);
            selectedCard.DisableButtons();
            selectedCard = null;
        }

        private bool IsValidField(RaycastHit hit, out int index)
        {
            Transform targetObject = hit.transform;
            index = -1;
            for (int i = 0; i < fields.Length; i++)
            {
                if (!IsFieldTargeted(fields[i], targetObject)) continue;
                index = i;
                return fields[i].OccupantCard.gameObject.activeSelf && !fields[i].OccupantCard.IsAnimating();
            }
            return false;
        }

        private bool IsFieldTargeted(OutdatedFieldBehaviour field, Transform targetTransform)
        {
            if (field.transform == targetTransform) return true;
            if (field.OccupantCard.transform == targetTransform) return true;
            if (targetTransform.parent != null && targetTransform.parent.parent != null
                && field.OccupantCard.transform == targetTransform.parent.parent) return true;
            return false;
        }
    }
}