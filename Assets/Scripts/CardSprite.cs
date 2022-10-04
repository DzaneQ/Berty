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
    private Transform[] bars;
    private CardImage imageReference;
    private int[] relCoord = new int[2];
    private Rigidbody cardRB;
    private SpriteRenderer spriteRenderer;
    private Character character;
    private CardState state;
    private CharacterStat cardStatus;

    private Turn Turn => Grid.Turn;
    public FieldGrid Grid => occupiedField.Grid;
    public CharacterStat CardStatus => cardStatus;
    public Character Character
    {
        get => character;
        private set
        {
            character = value;
            spriteRenderer.sprite = imageReference.Sprite;
        }
    }

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
        InitializeBars();
        cardButton = transform.GetComponentsInChildren<CardButton>();
        occupiedField = transform.GetComponentInParent<Field>();
        state = new InactiveState(this);
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

    private void InitializeBars()
    {
        bars = new Transform[4];
        for (int i = 0; i < bars.Length; i++) bars[i] = transform.GetChild(8).transform.GetChild(i+1);
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
        UpdateBars();
        UpdateRelativeCoordinates();
        CallPayment(cardStatus.Power);
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
        cardStatus.hasAttacked = false;
    }

    public bool CanCharacterAttack()
    {
        return cardStatus.Strength > 0 && !cardStatus.hasAttacked;
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

    public void SetField(Field field)
    {
        occupiedField = field;
        transform.SetParent(field.transform, false);
    }

    public void SetActive()
    {
        //Debug.Log($"Set active for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
        if (!cardStatus.isTired) state = state.SetActive;
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
        if (!CanCharacterAttack()) return;
        CallPayment(6 - cardStatus.Dexterity);
        state = new AttackingState(this);
    }

    public bool CanAttackField(Field targetField)
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
        if (CanAttackField(targetField)) targetField.OccupantCard.TakeDamage(cardStatus.Strength, targetField);
    }

    public void AttackWholeRange()
    {
        cardStatus.hasAttacked = true;
        bool successfulAttack = false;
        foreach (int[] distance in Character.AttackRange)
        {
            int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
            Field targetField = GetRelativeField(target[0], target[1]);
            if (targetField == null || !targetField.IsOccupied()) continue;
            if (targetField.OccupantCard.TakeDamage(cardStatus.Strength, occupiedField)) successfulAttack = true;
            Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
        if (successfulAttack) Character.SkillOnSuccessfulAttack(this);
    }

    public bool TakeDamage(int damage, Field source)
    {
        Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
        if (!gameObject.activeSelf) throw new Exception("This card shouldn't be attacked!");
        int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
        int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
        Debug.Log("Character health: " + CardStatus.Health);
        if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(cardStatus.Strength, source);
        if (Character.CanBlock(srcDistance)) return false;
        AdvanceHealth(-damage);
        Debug.Log($"{damage} damage taken for card {name}. Remaining HP: {cardStatus.Health}");
        return true;
    }

    public void AdvanceStrength(int value)
    {
        cardStatus.Strength += value;
        UpdateBar(0);
    }

    public void AdvancePower(int value)
    {
        cardStatus.Power += value;
        if (cardStatus.Power <= 0) SwitchSides();
        UpdateBar(1);
    }

    private void SwitchSides()
    {
        occupiedField.GoToOppositeSide();
        cardStatus.Power = Character.Power;
        if (occupiedField.IsAligned(Turn.CurrentAlignment)) SetActive();
        else SetIdle();
    }

    public void AdvanceDexterity(int value)
    {
        cardStatus.Dexterity += value;
        if (cardStatus.Dexterity <= 0) cardStatus.isTired = true;
        if (cardStatus.Dexterity >= Character.Dexterity) cardStatus.isTired = false;
        UpdateBar(2);
    }

    public void RegenerateDexterity()
    {
        if (!CardStatus.isTired) return;
        AdvanceDexterity(1);
    }

    public void AdvanceHealth(int value)
    {
        cardStatus.Health += value;
        if (IsDead()) DeactivateCard();
        UpdateBar(3);
    }

    private bool IsDead()
    {
        return cardStatus.Health <= 0;
    }

    public bool IsAllied(Field targetField)
    {
        return targetField.IsAligned(occupiedField.Align);
    }    

    private void UpdateBars()
    {
        for (int i = 0; i < bars.Length; i++) UpdateBar(i);
    }

    private void UpdateBar(int index)
    {
        int barValue = 0;
        switch (index)
        {
            case 0:
                barValue = cardStatus.Strength;
                break;
            case 1:
                barValue = cardStatus.Power;
                break;
            case 2:
                barValue = cardStatus.Dexterity;
                break;
            case 3:
                barValue = cardStatus.Health;
                break;
            default:
                throw new IndexOutOfRangeException($"Updating bar of index {index}.");
        }
        float unitScale = 15.9f / 6f;
        float positionX0Unit = 3.04f;
        float positionX6Unit = 11.05f;
        float currentX = (positionX6Unit - positionX0Unit) / 6 * barValue + positionX0Unit;
        bars[index].localPosition = new Vector3(currentX, bars[index].localPosition.y, bars[index].localPosition.z);
        bars[index].localScale = new Vector3(unitScale * barValue, bars[index].localScale.y, bars[index].localScale.z);
    }

    public void ConfirmNewCard()
    {
        Grid.AttackNewStand(occupiedField);
        Character.SkillOnNewCard(this);
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

    public Field GetAdjacentField(float angle)
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
        cardStatus = new CharacterStat(Character);
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