using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private List<Character> resistChar;

    public Field OccupiedField => occupiedField;
    public CardManager CardManager => cardManager;
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
            cardStatus = new CharacterStat(Character);
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
        cardButton = transform.GetComponentsInChildren<CardButton>();
        occupiedField = transform.GetComponentInParent<Field>();
        state = new InactiveState(this);
        InitializeBars();
        InitializeRigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == occupiedField.gameObject) state.HandleFieldCollision();
    }

    public void OnMouseOver()
    {
        if (IsLeftClicked()) state.HandleClick();
        else if (IsRightClicked()) state.HandleSideClick();
    }

    private bool IsLeftClicked()
    {
        if (!Grid.IsLocked() && Input.GetMouseButtonDown(0)) return true;
        else return false;
    }

    private bool IsRightClicked()
    {
        //Debug.Log($"Card " + name + " is checked whether right clicked.");
        //if (Input.GetMouseButtonDown(1)) Debug.Log(($"Card {0} is right clicked.", name));
        if (!Grid.IsLocked() && Input.GetMouseButtonDown(1)) return true;
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

    private void ClearCardResistance()
    {
        resistChar = new List<Character>();
    }

    public void TryToActivateCard() // TODO: rework (not to state to this)
    {
        if (IsCardSelected()) state = state.ActivateCard();
    }

    public void ActivateCard() // TODO: rework
    {
        gameObject.SetActive(true);
        ImportFromCardImage();
        UpdateBars();
        UpdateRelativeCoordinates();
        CallPayment(cardStatus.Power);
        occupiedField.ConvertField(Turn.CurrentAlignment, false);
        ApplyPhysics();
    }

    public void UpdateCard(Character character)
    {
        Character = character;
        //cardStatus = new CharacterStat(Character);
        spriteRenderer.sprite = Resources.Load<Sprite>("BERTY/" + character.Name);
        UpdateBars();
        ConfirmNewCard(); // experimental - not tested
        //ImportFromCardImage();
    }

    public bool IsCardSelected()
    {
        if (!Turn.IsItMoveTime()) return false;
        return cardManager.SelectedCard() != null;
    }

    public void ApplyPhysics(bool isApplied = true)
    {
        cardRB.isKinematic = !isApplied;
    }

    public void ProgressTemporaryStats()
    {
        if (cardStatus.CurrentTempStatBonus.All(x => x == 0)) return;
        //Debug.Log("Clearing stats for card: " + name);
        //cardStatus.CurrentTempStatBonus = new int[4];
        //Array.Clear(cardStatus.CurrentTempStatBonus, 0, 4);
        cardStatus.CurrentTempStatBonus = (int[]) cardStatus.NextTempStatBonus.Clone();
        Array.Clear(cardStatus.NextTempStatBonus, 0, 4);
        //cardStatus.FutureTempStatBonus = new int[4];
        UpdateBars();
    }

    public void ResetAttack()
    {
        cardStatus.hasAttacked = false;
    }

    public void BlockAttack()
    {
        cardStatus.hasAttacked = true;
    }

    public bool CanCharacterAttack()
    {
        return !cardStatus.hasAttacked;
    }

    public void DeactivateCard()
    {
        Debug.Log($"Deactivating card: {name}");
        occupiedField.AdjustCardRemoval();
        //character = null;
        state = state.DeactivateCard();
        //cardManager.RemoveFromField(imageReference);
    }

    public void SetCardToDefaultTransform()
    {
        if (occupiedField != null) Grid.ResetCardTransform(transform);
    }

    public void SetField(Field field)
    {
        occupiedField = field;
        transform.SetParent(field.transform, false);
        UpdateRelativeCoordinates();
    }

    public void SetActive()
    {
        //Debug.Log($"Set active for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
        if (!cardStatus.isTired) state = state.SetActive;
        else state = state.SetIdle;
    }

    public void SetIdle()
    {
        //Debug.Log($"Set idle for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
        state = state.SetIdle;
    }

    public void SetTelecinetic()
    {
        state = state.SetTelecinetic;
    }

    public void SetTargetable()
    {
        state = state.SetTargetable;
    }

public void CallPayment(int price)
    {
        Grid.DisableAllButtons(occupiedField);
        Turn.SetPayment(price);
    }

    public void ConfirmPayment()
    {
        if (!Turn.IsItPaymentTime()) throw new Exception("Confirming payment when not in payment mode.");
        if (!state.IsForPaymentConfirmation()) throw new Exception("Wrong card state for payment..");
        if (!Turn.CheckOffer()) return;
        cardManager.DiscardCards();
        state = state.TakePaidAction();
        if (Turn.IsItPaymentTime()) Turn.SetMoveTime();
    }

    public void CancelPayment()
    {
        Turn.SetMoveTime();
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
        //Debug.Log("Seeing attack on field - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        if (CanAttackField(targetField)) targetField.OccupantCard.TakeDamage(GetStrength(), targetField);
    }

    public void OrderAttack()
    {
        Debug.Log("Start ordering attack");
        cardStatus.hasAttacked = true;
        if (VenturaCheck()) return; // TODO: Swap place after adjusting opponent script.
        if (CanUseSkill() && Character.SkillSpecialAttack(this)) return;
        bool successfulAttack = false;
        Debug.Log("Check ranges");
        foreach (int[] distance in Character.AttackRange)
        {
            Field targetField = GetTargetField(distance);
            if (targetField == null || !targetField.IsOccupied()) continue;
            if (targetField.OccupantCard.TakeDamage(GetStrength(), occupiedField)) successfulAttack = true;
            Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
        Debug.Log("Ranges checked");
        if (successfulAttack && CanUseSkill()) Character.SkillOnSuccessfulAttack(this);
        if (CanUseSkill()) Character.SkillOnAttack(this);
        Debug.Log("End ordering attack");
    }


    public bool TakeDamage(int damage, Field source, bool riposte = false)
    {
        if (CanUseSkill()) damage = Character.SkillDefenceModifier(damage, source.OccupantCard);
        if (source.OccupantCard.CanUseSkill()) damage = source.OccupantCard.Character.SkillAttackModifier(damage, this);
        Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
        //if (!gameObject.activeSelf) throw new Exception("This card shouldn't be attacked!");
        if (!gameObject.activeSelf) return false;
        if (riposte)
        {
            AdvanceHealth(-damage);
            return false;
        }
        int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
        int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
        Debug.Log("Character health: " + CardStatus.Health);
        if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(GetStrength(), OccupiedField, true);
        if (Character.CanBlock(srcDistance)) return false;
        AdvanceHealth(-damage);
        Debug.Log($"{damage} damage taken for card {name}. Remaining HP: {cardStatus.Health}");
        return true;
    }

    public bool CanUseSkill()
    {
        if (OccupiedField.IsOpposed(Grid.CurrentStatus.Revolution) && GetRole() == Role.Special) return false;
        return true;
    }

    private bool VenturaCheck()
    {
        //Debug.Log("Ventura check!");
        foreach (CardSprite card in GetAdjacentCards())
        {
            if (card.Character.GetType() != typeof(BertVentura) || IsAllied(card.OccupiedField)) continue;
            foreach (int[] range in Character.AttackRange)
            {
                Field targetField = GetTargetField(range);
                if (targetField == null || !targetField.IsOccupied()) continue;
                if (targetField.OccupantCard.Character.GetType() == typeof(BertVentura)) return false;
            }
            //Debug.Log("Ventura not targeted. Block!");
            return true;
        }
        //Debug.Log("No Ventura in enemy cards.");
        return false;
    }

    public void AdvanceTempStrength(int value)
    {
        if (!Character.CanAffectStrength(this, null)) return;
        Debug.Log("Next tempStr before: " + cardStatus.NextTempStatBonus.GetValue(0));
        cardStatus.TempStrength += value;
        Debug.Log("Next tempStr after: " + cardStatus.NextTempStatBonus.GetValue(0));
        UpdateBar(0);
    }

    public void AdvanceStrength(int value, CardSprite spellSource = null)
    {
        if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
        if (!Character.CanAffectStrength(this, spellSource)) return;
        cardStatus.Strength += value;
        if (CanUseSkill()) Character.SkillAdjustStrengthChange(value, this);
        UpdateBar(0);
    }

    public void AdvanceTempPower(int value)
    {
        if (!Character.CanAffectPower(this, null)) return;
        cardStatus.TempPower += value;
        UpdateBar(1);
    }

    public void AdvancePower(int value, CardSprite spellSource = null)
    {
        if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
        if (!Character.CanAffectPower(this, spellSource)) return;
        cardStatus.Power += value;
        if (CanUseSkill()) Character.SkillAdjustPowerChange(value, this, spellSource);
        if (cardStatus.Power <= 0) SwitchSides();
        UpdateBar(1);
    }

    private void SwitchSides()
    {
        occupiedField.GoToOppositeSide();
        ResetPower();
        if (occupiedField.IsAligned(Grid.CurrentStatus.JudgementRevenge)) AdvanceTempStrength(1);
        if (Grid.CurrentStatus.Revolution == Alignment.None) return;
        if (GetRole() != Role.Special) return;
        if (Character.Name == "che bert") return;
        if (OccupiedField.IsAligned(Grid.CurrentStatus.Revolution)) AdvanceStrength(1);
        else AdvanceStrength(-1);
        //if (occupiedField.IsAligned(Turn.CurrentAlignment)) SetActive();
        //else SetIdle();
    }

    public void ResetPower()
    {
        cardStatus.Power = Character.Power;
    }

    public void AdvanceDexterity(int value, CardSprite spellSource = null)
    {
        if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
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

    public void AdvanceHealth(int value, CardSprite spellSource = null)
    {
        if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
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
        Character.SkillOnDeath(this);
        foreach (Field field in Grid.Fields)
            if (field.IsOccupied() && field.OccupantCard.CanUseSkill())
                field.OccupantCard.Character.SkillOnOtherCardDeath(field.OccupantCard, this);
        DeactivateCard();
        cardManager.KillCard(imageReference);
    }    

    public void AddResistance(Character character)
    {
        Debug.Log($"Adding resistance of {character} to card: {name}");
        if (character == null) throw new Exception("Trying to resist null.");
        if (character == Character) throw new Exception("Trying to resist self.");
        if (resistChar.Contains(character)) return;
        resistChar.Add(character);
    }

    public bool IsAllied(Field targetField)
    {
        return targetField.IsAligned(occupiedField.Align);
    }    

    public void UpdateBars()
    {
        for (int i = 0; i < bars.Length; i++) UpdateBar(i);
    }

    private void UpdateBar(int index)
    {
        var barValue = index switch
        {
            0 => GetStrength(),
            1 => cardStatus.Power,
            2 => cardStatus.Dexterity,
            3 => cardStatus.Health,
            _ => throw new IndexOutOfRangeException($"Updating bar of index {index}."),
        };
        float unitScale = 15.9f / 6f;
        float positionX0Unit = 3.04f;
        float positionX6Unit = 11.05f;
        float currentX = (positionX6Unit - positionX0Unit) / 6 * barValue + positionX0Unit;
        bars[index].localPosition = new Vector3(currentX, bars[index].localPosition.y, bars[index].localPosition.z);
        bars[index].localScale = new Vector3(unitScale * barValue, bars[index].localScale.y, bars[index].localScale.z);
    }

    public Role GetRole()
    {
        if (Grid.CurrentStatus.IsJudgement) return Role.Special;
        return Character.Role;
    }

    public int GetStrength()
    {
        //if (state.IsJudgementRevenge() && cardStatus.Strength <= 5) return cardStatus.Strength + 1;
        return cardStatus.Strength;
    }

    public void ConfirmNewCard()
    {
        //Debug.Log("Start confirming new card.");
        ClearCardResistance();
        if (occupiedField.IsAligned(Grid.CurrentStatus.Revolution) && GetRole() == Role.Special) AdvanceStrength(1);
        if (occupiedField.IsAligned(Grid.CurrentStatus.JudgementRevenge)) AdvanceTempStrength(1);
        if (CanUseSkill()) Character.SkillOnNewCard(this);
        TakeNeighborsEffect();
        Grid.AttackNewStand(occupiedField);
        //Debug.Log("Finishing confirming new card.");
    }

    private void TakeNeighborsEffect()
    {
        for (int i = 0; i < 4; i++)
        {
            Field adjacentField = GetAdjacentField(i * 90);
            if (adjacentField == null || !adjacentField.IsOccupied()) continue;
            if (adjacentField.OccupantCard.CanUseSkill())
                adjacentField.OccupantCard.Character.SkillOnNeighbor(adjacentField.OccupantCard, this);
        }
    }

    public void EnableNeutralButton(int index)
    {
        DisableButtons();
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

    public void ShowDexterityButtons(bool onlyMove = false)
    {
        DisableButtons();
        int firstIndex = onlyMove ? 4 : 2;
        for (int i = firstIndex; i <= 7; i++)
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
        Debug.Log("Confirming move for " + Character.Name);
        if (CanUseSkill()) Character.SkillOnMove(this);
        TakeNeighborsEffect();
    }

    public void RotateCard(int angle)
    {
        int returnButtonIndex = (450 - angle) / 180;
        transform.Rotate(0, 0, -angle);
        UpdateRelativeCoordinates();
        occupiedField.SynchronizeRotation();
        state = state.AdjustTransformChange(returnButtonIndex);
    }

    public void SwapWith(Field targetField)
    {
        Field sourceField = null;
        if (targetField.IsOccupied())
        {
            RotateCard(180);
            targetField.OccupantCard.RotateCard(180);
            sourceField = occupiedField;
        }
        Grid.SwapCards(occupiedField, targetField);
        ConfirmMove(); // Note: experimental!
        if (sourceField != null) sourceField.OccupantCard.ConfirmMove();
    }

    public void ImportFromCardImage()
    {
        imageReference = cardManager.SelectedCard();
        Character = imageReference.Character;
        //cardStatus = new CharacterStat(Character);
        cardManager.RemoveFromTable(imageReference);
        //cardManager.AddToField(imageReference);
    }

    public void ReturnCharacter()
    {
        cardManager.ReturnCharacter(imageReference);
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

    public void DebugForceDeactivateCard()
    {
        if (!Debug.isDebugBuild) return;
        Debug.Log($"Force deactivating card: {name}");
        occupiedField.AdjustCardRemoval();
        state = new InactiveState(this);
    }
    
    public CardImage DebugGetReference()
    {
        if (!Debug.isDebugBuild) return null;
        return imageReference;
    }

    public void DebugForceActivateCard(CardImage image, int angle)
    {
        gameObject.SetActive(true);
        imageReference = image;
        Character = imageReference.Character;
        UpdateBars();
        transform.Rotate(0, 0, -angle);
        UpdateRelativeCoordinates();
        occupiedField.SynchronizeRotation();
        ApplyPhysics();
        if (occupiedField.IsAligned(Turn.CurrentAlignment)) state = new ActiveState(this);
        else if (occupiedField.IsOpposed(Turn.CurrentAlignment)) state = new IdleState(this);
        else Debug.LogError("Wrongly activated card!");
    }
}