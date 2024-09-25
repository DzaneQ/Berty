using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanics : MonoBehaviour
{
    float edgeWidth = 10f;
    float rotManSpeed = 90f;
    float rotAutoMultiplier = 0.25f;

    private Field[] fields;
    private CardSprite selectedCard;
    Camera cam;

    private Field targetField;
    private Field lastTarget;

    private Color attackColor = new Color(1f, 0.55f, 0f, 1f);
    private Color blockColor = new Color(0f, 0.35f, 0.8f, 1f);

    private Color highlightAttackColor;
    private Color highlightBlockColor;

    public CardSprite FocusedCard => selectedCard;


    private void Awake()
    {
        cam = GetComponent<Camera>();
        AssignFields();
    }

    private void Start()
    {
        AdjustHighlightSaturation(0.8f);
    }

    void Update()
    {
        HandleCameraTransform();
        HandleCardSpriteFocus();
    }

    private void AssignFields()
    {
        FieldGrid fg = (FieldGrid)FindFirstObjectByType(typeof(FieldGrid));
        fields = new Field[9];
        if (fields.Length != fg.transform.childCount) Debug.LogError("Number of fields is not equal to number of child count!");
        for (int index = 0; index < fields.Length; index++) fields[index] = fg.transform.GetChild(index).GetComponent<Field>();
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
        return ((1 - colorValue) * (1 - saturation) + colorValue);
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
        if (Mathf.Abs(currentAngle - rightAngle) < rotManSpeed * rotAutoMultiplier)
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, rightAngle);
        else
        {
            float targetAngle = (currentAngle - rightAngle) * (1 - rotAutoMultiplier) + rightAngle;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, targetAngle);
        }
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
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int fieldIndex;
        if (Physics.Raycast(ray, out hit) && IsValidField(hit, out fieldIndex))
        {
            targetField = fields[fieldIndex];
            if (lastTarget == null) lastTarget = targetField;
            if (targetField != lastTarget || selectedCard == null) SetTargets(fieldIndex);
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

    private void SetTargets(int sourceIndex)
    {
        if (fields[sourceIndex].OccupantCard == selectedCard) return;
        if (selectedCard != null) ClearTarget();
        selectedCard = fields[sourceIndex].OccupantCard;
        selectedCard.EnableButtons();
        bool riposte = false;
        foreach (int[] distance in selectedCard.Character.AttackRange)
        {
            Field targetField = selectedCard.GetTargetField(distance);
            if (targetField == null) continue;
            bool block = false;
            if (targetField.IsOccupied())
            {
                CardSprite targetCard = targetField.OccupantCard;
                if (!riposte &&
                    targetCard.Character.CanRiposte(targetCard.GetFieldDistance(fields[sourceIndex]))) riposte = true;
                if (targetCard.Character.CanBlock(targetCard.GetFieldDistance(fields[sourceIndex]))) block = true;
            }
            int targetIndex = Array.IndexOf(fields, targetField);
            if (targetIndex < 0) throw new Exception("Target index not found! It's " + targetIndex);
            HighlightTarget(fields[targetIndex], block);
        }
        if (riposte) HighlightTarget(fields[sourceIndex]);
    }

    private void HighlightTarget(Field target, bool blockState = false)
    {
        //target.FieldOutline.OutlineColor = blockState ? blockColor : attackColor;
        //target.FieldOutline.enabled = true;
        target.HighlightField(blockState ? highlightBlockColor : highlightAttackColor);
    }

    private void ClearTarget()
    {
        foreach (Field field in fields)
        {
            //field.FieldOutline.enabled = false;
            field.UnhighlightField();
        }
        selectedCard.DisableButtons();
        selectedCard = null;
    }

    private bool IsValidField(RaycastHit hit, out int index)
    {
        Transform targetObject = hit.transform;
        index = -1;
        for (int i = 0; i < fields.Length; i++)
        {
            if (IsFieldTargeted(fields[i], targetObject))
            {
                if (fields[i].OccupantCard.gameObject.activeSelf)
                {
                    index = i;
                    return true;
                }
                else return false;
            }
        }
        return false;
    }

    private bool IsFieldTargeted(Field field, Transform targetTransform)
    {
        if (field.transform == targetTransform) return true;
        if (field.OccupantCard.transform == targetTransform) return true;
        if (targetTransform.parent != null && targetTransform.parent.parent != null
            && field.OccupantCard.transform == targetTransform.parent.parent) return true;
        return false;
    }
}
