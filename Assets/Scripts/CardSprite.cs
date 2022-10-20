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

    public Field OccupiedField => occupiedField;
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
            name = character.Name;
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
        return !cardStatus.hasAttacked;
    }

    private void DeactivateCard()
    {
        Debug.Log($"Deactivating card: {name}");
        occupiedField.ConvertField(Alignment.None);
        //character = null;
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
        UpdateRelativeCoordinates();
    }

    public void SetActive()
    {
        Debug.Log($"Set active for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
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
        if (!Turn.CheckOffer()) return;
        cardManager.DiscardCards();
        state.TakePaidAction();
        Turn.UnsetPayment();
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

    public void OrderAttack()
    {
        cardStatus.hasAttacked = true;
        if (CanUseSkill() && Character.SkillSpecialAttack(this)) return;
        bool successfulAttack = false;
        foreach (int[] distance in Character.AttackRange)
        {
            Field targetField = GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            if (targetField.OccupantCard.TakeDamage(cardStatus.Strength, occupiedField)) successfulAttack = true;
            Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
        if (successfulAttack && CanUseSkill()) Character.SkillOnSuccessfulAttack(this);
        if (CanUseSkill()) Character.SkillOnAttack(this);
    }


    public bool TakeDamage(int damage, Field source, bool riposte = false)
    {
        if (CanUseSkill()) damage = Character.SkillDefenceModifier(damage, source.OccupantCard);
        if (source.OccupantCard.CanUseSkill()) damage = source.OccupantCard.Character.SkillAttackModifier(damage, this);
        Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
        if (!gameObject.activeSelf) throw new Exception("This card shouldn't be attacked!");
        if (riposte)
        {
            AdvanceHealth(-damage);
            return false;
        }
        int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
        int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
        Debug.Log("Character health: " + CardStatus.Health);
        if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(cardStatus.Strength, OccupiedField, true);
        if (Character.CanBlock(srcDistance)) return false;
        AdvanceHealth(-damage);
        Debug.Log($"{damage} damage taken for card {name}. Remaining HP: {cardStatus.Health}");
        return true;
    }

    private bool CanUseSkill()
    {
        return true;
    }    

    public void AdvanceStrength(int value, CardSprite spellSource = null)
    {
        if (!Character.CanAffectStrength(this, spellSource)) return;
        cardStatus.Strength += value;
        if (CanUseSkill()) Character.SkillAdjustStrengthChange(value, this);
        UpdateBar(0);
    }

    public void AdvancePower(int value, CardSprite spellSource = null)
    {
        if (!Character.CanAffectPower(this, spellSource)) return;
        cardStatus.Power += value;
        if (CanUseSkill()) Character.SkillAdjustPowerChange(value, this);
        if (cardStatus.Power <= 0) SwitchSides();
        UpdateBar(1);
    }

    private void SwitchSides()
    {
        occupiedField.GoToOppositeSide();
        cardStatus.Power = Character.Power;
        //if (occupiedField.IsAligned(Turn.CurrentAlignment)) SetActive();
        //else SetIdle();
    }

    public void AdvanceDexterity(int value, CardSprite spellSource = null)
    {
        if (spellSource == null) Debug.LogWarning($"No spell source affecting {Character.Name}");
        cardStatus.Dexterity += value;
        if (CanUseSkill()) Character.SkillAdjustDexterityChange(value, this);
        if (cardStatus.Dexterity <= 0) cardStatus.isTired = true;
        if (cardStatus.Dexterity >= Character.Dexterity) cardStatus.isTired = false;
        UpdateBar(2);
    }

    public void RegenerateDexterity()
    {
        if (!CardStatus.isTired) return;
        AdvanceDexterity(1, this);
    }

    public void AdvanceHealth(int value)
    {
        cardStatus.Health += value;
        if (CanUseSkill()) Character.SkillAdjustHealthChange(value, this);
        if (IsDead()) KillCard();    
        UpdateBar(3);
    }

    private bool IsDead()
    {
        return cardStatus.Health <= 0;
    }

    private void KillCard()
    {
        if (!gameObject.activeSelf) return;
        foreach (Field field in Grid.Fields)
            if (field.IsOccupied() && field.OccupantCard.CanUseSkill())
                field.OccupantCard.Character.SkillOnOtherCardDeath(field.OccupantCard, this);
        DeactivateCard();
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

    public Role GetRole()
    {
        return Character.Role;
    }

    public void ConfirmNewCard()
    {
        Grid.AttackNewStand(occupiedField);
        if (CanUseSkill()) Character.SkillOnNewCard(this);
        for (int i = 0; i < 4; i++)
        {
            Field adjacentField = GetAdjacentField(i * 90);
            if (adjacentField == null || !adjacentField.IsOccupied()) continue;
            if (adjacentField.OccupantCard.CanUseSkill()) adjacentField.OccupantCard.Character.SkillOnNeighbor(adjacentField.OccupantCard, this);
        }
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

    public List<CardSprite> GetAdjacentCards()
    {
        List<CardSprite> adjacentCards = new List<CardSprite>();
        for (int i = 0; i < 4; i++)
        {
            Field adj = GetAdjacentField(i * 90);
            if (adj != null && adj.IsOccupied()) adjacentCards.Add(adj.OccupantCard);
        }
        return adjacentCards;
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
        //UpdateRelativeCoordinates();
        state = state.AdjustTransformChange(returnButtonIndex);
    }

    public void ConfirmMove()
    {
        if (CanUseSkill()) Character.SkillOnMove(this);
    }

    public void RotateCard(int angle)
    {
        int returnButtonIndex = (450 - angle) / 180;
        transform.Rotate(0, 0, -angle);
        UpdateRelativeCoordinates();
        state = state.AdjustTransformChange(returnButtonIndex);
    }

    public void SwapWith(Field targetField)
    {
        if (targetField.IsOccupied())
        {
            RotateCard(180);
            targetField.OccupantCard.RotateCard(180);
        }
        Grid.SwapCards(occupiedField, targetField);
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

    public Field GetTargetField(int[] distance)
    {
        if (distance.Length > 2) throw new IndexOutOfRangeException("Wrong distance argument.");
        return GetRelativeField(distance[0] + relCoord[0], distance[1] + relCoord[1]);
    }

    public void UpdateRelativeCoordinates()
    {
        float angle = GetRelativeAngle();
        relCoord = occupiedField.GetRelativeCoordinates(angle);
    }
}