using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour
{
    private Field occupiedField;
    private CardManager cardManager;
    private CardButton[] cardButton;
    private Transform healthBar;
    private CardImage imageReference;
    private int[] relCoord = new int[2];
    private Rigidbody cardRB;
    private SpriteRenderer spriteRenderer;
    private Character character;
    private CardState state;
    private bool hasAttacked;

    private Turn Turn => Grid.Turn;
    public FieldGrid Grid => occupiedField.Grid;
    public Character Character
    {
        get => character;
        private set
        {
            character = value;
            spriteRenderer.sprite = imageReference.Sprite;
        }
    }
    public bool HasAttacked => hasAttacked;

    /* CardSprite children in Hierarchy tab must be in the following order:
        0 - Confirm
        1 - Return
        2 - RotateLeft
        3 - RotateRight
        4 - MoveUp
        5 - MoveRight
        6 - MoveDown
        7 - MoveLeft
     */

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        healthBar = transform.GetChild(8).transform.GetChild(1); 
        cardButton = transform.GetComponentsInChildren<CardButton>();
        occupiedField = transform.GetComponentInParent<Field>();
        state = new InactiveState(this);
        ResetAttack();
        InitializeRigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == occupiedField.gameObject) state.HandleFieldCollision();
    }

    public void OnMouseDown()
    {
        if (IsLeftClicked()) state.HandleClick();
    }

    private bool IsLeftClicked()
    {
        if (!Grid.IsLocked() && Input.GetMouseButtonDown(0)) return true;
        else return false;
    }

    private void InitializeRigidbody()
    {
        cardRB = GetComponent<Rigidbody>();
        cardRB.detectCollisions = true;
        cardRB.isKinematic = true;
    }

    public void TryToActivateCard()
    {
        if (IsCardSelected()) state = state.ActivateCard();
    }

    public void ActivateCard()
    {
        gameObject.SetActive(true);
        ImportFromCardImage();
        UpdateHealthBar(Character.Health);
        UpdateRelativeCoordinates();
        CallPayment(Character.Power);
        occupiedField.ConvertField(Turn.CurrentAlignment, false);
        ApplyPhysics();
    }

    public bool IsCardSelected()
    {
        if (Turn.IsItPaymentTime()) return false;
        return cardManager.SelectedCard() != null;
    }

    public void ApplyPhysics(bool isApplied = true)
    {
        cardRB.isKinematic = !isApplied;
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }

    private void DeactivateCard()
    {
        Debug.Log("Deactivating card...");
        occupiedField.ConvertField(Alignment.None);
        state = state.DeactivateCard();
    }

    public void SetCardToDefaultTransform()
    {
        Grid.ResetCardTransform(transform);
    }

    public Field OccupiedField 
    {
        set
        {
            occupiedField = value;
            transform.SetParent(value.transform, false);
        } 
    }

    public void SetActive()
    {
        //Debug.Log($"Set active for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
        state = state.SetActive;
    }

    public void SetIdle()
    {
        //Debug.Log($"Set idle for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
        state = state.SetIdle;
    }

    public void CallPayment(int price)
    {
        Grid.DisableAllButtons();
        Turn.SetPayment(price);
    }

    public void ConfirmPayment()
    {
        if (!Turn.IsItPaymentTime()) throw new Exception("Confirming payment when not in payment mode.");
        if (!state.IsForPaymentConfirmation()) throw new Exception("Wrong card state for payment..");
        if (Turn.CheckOffer())
        {
            cardManager.DiscardCards();
            state.TakePaidAction();
            Turn.UnsetPayment();
        }
    }

    public void CancelPayment()
    {
        Turn.UnsetPayment();
    }

    public void CancelDecision()
    {
        state.Cancel();
    }

    public void CancelCard()
    {
        imageReference.ReturnCard();
        DeactivateCard();
    }

    public void PrepareToAttack()
    {
        if (hasAttacked) return;
        CallPayment(6 - Character.Dexterity);
        state = new AttackingState(this);
    }

    public bool CanAttack(Field targetField)
    {
        if (targetField == null) return false;
        foreach (int[] distance in Character.AttackRange)
        {
            int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
            if (targetField == GetRelativeField(target[0], target[1])) return true;
        }
        return false;
    }

    public void TryToAttackTarget(Field targetField)
    {
        Debug.Log("Seeing attack on field - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        if (CanAttack(targetField)) targetField.OccupantCard.TakeDamage(Character.Strength, targetField);
    }

    public void AttackWholeRange()
    {
        hasAttacked = true;
        foreach (int[] distance in Character.AttackRange)
        {
            int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
            Field targetField = GetRelativeField(target[0], target[1]);
            if (targetField != null) targetField.OccupantCard.TakeDamage(Character.Strength, occupiedField);
            if (targetField != null) Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
    }

    public void TakeDamage(int damage, Field source)
    {
        Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
        if (!gameObject.activeSelf) return;
        int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
        int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
        Debug.Log("Character health: " + Character.Health);
        if (!Character.CanBlock(srcDistance)) Character.TakeDamage(damage);
        if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(Character.Strength, source);
        Debug.Log("Damage taken: " + damage + "; Remaining HP: " + Character.Health);
        if (Character.IsDead()) DeactivateCard();
        else UpdateHealthBar(Character.Health);
    }

    public void UpdateHealthBar(int health)
    {
        float unitScale = 2.67f;
        float positionX0Unit = -6.5f;
        float positionX6Unit = 1.4f;
        float currentX = (positionX6Unit - positionX0Unit) / 6 * health + positionX0Unit;
        healthBar.localPosition = new Vector3(currentX, healthBar.localPosition.y, healthBar.localPosition.z);
        healthBar.localScale = new Vector3(unitScale * health, healthBar.localScale.y, healthBar.localScale.z);
    }

    public void DefendNewStand()
    {
        Grid.AttackNewStand(occupiedField);
    }

    public void EnableNeutralButton(int index)
    {
        cardButton[index].ChangeButtonToNeutral();
        cardButton[index].EnableButton();
    }

    public void DisableButtons()
    {
        foreach (CardButton button in cardButton) button.DisableButton();
    }

    public void ShowNeutralButtons()
    {
        if (state.GetType() != typeof(NewCardState)) throw new Exception("New card buttons reveal attempt for not new card state!");
        for (int i = 1; i <= 3; i++)
        {
            cardButton[i].ChangeButtonToNeutral();
            cardButton[i].EnableButton();
        }
    }

    public void ShowDexterityButtons()
    {
        DisableButtons();
        for (int i = 2; i <= 7; i++)
        {
            cardButton[i].ChangeButtonToDexterity();
            cardButton[i].EnableButton();
        }
        UpdateMoveButtons();
    }

    private Field GetAdjacentField(float angle)
    {
        int targetX = (int)Math.Round(relCoord[0] + Math.Sin(angle * Math.PI / 180));
        int targetY = (int)Math.Round(relCoord[1] + Math.Cos(angle * Math.PI / 180));
        int[] target = { targetX, targetY };
        return GetRelativeField(target[0], target[1]);
    }

    public void UpdateMoveButtons()
    {
        for (int i = 4; i <= 7; i++)
        {
            Field targetField = GetAdjacentField(i * 90);
            if (targetField == null || targetField.IsOccupied()) cardButton[i].DisableButton();
            else cardButton[i].EnableButton();
        }
    }

    public void MoveCard(int angle)
    {
        int returnButtonIndex = ((angle / 90 + 2) % 4) + 4;
        Field toField = GetAdjacentField(angle);
        Grid.SwapCards(occupiedField, toField);
        UpdateRelativeCoordinates();
        state = state.AdjustTransformChange(returnButtonIndex);
    }

    public void RotateCard(int angle)
    {
        int returnButtonIndex = (450 - angle) / 180;
        transform.Rotate(0, 0, -angle);
        UpdateRelativeCoordinates();
        state = state.AdjustTransformChange(returnButtonIndex);
    }

    public void ImportFromCardImage()
    {
        imageReference = cardManager.SelectedCard();
        Character = imageReference.Character;
        cardManager.RemoveFromTable(imageReference);
    }

    private float GetRelativeAngle()
    {
        return Grid.GetDefaultAngle() - transform.localRotation.eulerAngles.z;
    }

    private Field GetRelativeField(int targetX, int targetY)
    {
        return Grid.GetRelativeField(targetX, targetY, GetRelativeAngle());
    }

    public void UpdateRelativeCoordinates()
    {
        float angle = GetRelativeAngle();
        relCoord = occupiedField.GetRelativeCoordinates(angle);
    }
}