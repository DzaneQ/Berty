using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material player;
    [SerializeField] private Material opponent;

    [SerializeField] private Material defaultAttacked;
    //[SerializeField] private Material playerAttacked;
    //[SerializeField] private Material opponentAttacked;

    private Turn turn;
    private Field[] fields;
    private DefaultTransform cardOnBoard;
    private Color defaultColor;
    private GlobalStatus temporaryStatuses;
    private CardSprite backupCard;

    public Turn Turn => turn;
    public Field[] Fields => fields;
    public GlobalStatus CurrentStatus => temporaryStatuses;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
    }

    private void Start()
    {
        GridInitialization init = GetComponent<GridInitialization>();
        temporaryStatuses = new GlobalStatus(this);
        init.InitializeFields(out fields);
        init.InitializeDefaultCardTransform(out cardOnBoard, out defaultColor);
        backupCard = init.InitializeBackupCard();
        Destroy(init);
    }

    public Material GetMaterial(Alignment alignment, bool underAttack)
    {
        switch (alignment)
        {
            case Alignment.Player: return player;
            case Alignment.Opponent: return opponent;
            default: return underAttack ? defaultAttacked : defaultMaterial;
        }
    }

    public void CancelCard()
    {
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            if (field.OccupantCard.CancelDecision()) return;
        }
    }

    public void ResetCardTransform(Transform cardSprite)
    {
        cardSprite.localPosition = cardOnBoard.defaultPosition;
        cardSprite.localRotation = cardOnBoard.defaultRotation;
    }

    public void UnhighlightCard(SpriteRenderer cardRenderer)
    {
        cardRenderer.color = defaultColor;
    }

    internal void StopHighlightingCards()
    {
        throw new NotImplementedException();
    }

    public float GetDefaultAngle()
    {
        return cardOnBoard.defaultRotation.eulerAngles.z;
    }

    public void AddCardIntoQueue(Alignment align)
    {
        temporaryStatuses.RequestCard(align);
    }

    public void SetJudgement()
    {
        temporaryStatuses.SetJudgement();
        if (temporaryStatuses.Revolution == Alignment.None) return;
        foreach (Field field in fields) // not tested
        {
            if (!field.IsOccupied()) continue;
            if (!field.IsAligned(temporaryStatuses.Revolution)) continue;
            if (field.OccupantCard.GetRole() != field.OccupantCard.Character.Role) field.OccupantCard.AdvanceStrength(1);
        }
    }

    public void RemoveJudgement(Alignment align)
    {
        if (temporaryStatuses.Revolution != Alignment.None)
            foreach (Field field in fields) // not tested
            {
                if (!field.IsOccupied()) continue;
                if (!field.IsAligned(temporaryStatuses.Revolution)) continue;
                if (field.OccupantCard.GetRole() != field.OccupantCard.Character.Role) field.OccupantCard.AdvanceStrength(-1);
            }
        temporaryStatuses.RemoveJudgement(align);  
    }

    public void SetRevolution(Alignment align)
    {
        temporaryStatuses.SetRevolution(align);
        CalmJudgement();
    }

    private void CalmJudgement()
    {
        if (temporaryStatuses.Revolution == Alignment.None) throw new Exception("There's no revolution to adjust judgement!");
        if (!temporaryStatuses.IsJudgement) return;
        if (IsJudgementWithRevolution()) return;
        temporaryStatuses.CalmJudgement();
    }

    private bool IsJudgementWithRevolution()
    {
        foreach (Field field in fields) // not tested
        {
            if (!field.IsOccupied()) continue;
            if (field.OccupantCard.Character.Name != "sedzia bertt") continue;
            if (field.IsAligned(temporaryStatuses.Revolution)) return true;
            if (field.IsOpposed(temporaryStatuses.Revolution)) return false;
        }
        throw new Exception("Card sedzia bertt not found!");
    }

    public void RemoveRevolution()
    {
        temporaryStatuses.RemoveRevolution();
    }

    public void SetTelekinesis(Alignment align, int dexterity)
    {
        temporaryStatuses.SetTelekinesis(align);
        temporaryStatuses.SetTelekinesisDexterity(dexterity);
    }

    public void RemoveTelekinesis()
    {
        temporaryStatuses.RemoveTelekinesis();
    }

    public bool IsTelekineticMovement()
    {
        return turn.CurrentAlignment == temporaryStatuses.Telekinesis;
    }

    public void InitiateResurrection()
    {
        turn.ExecuteResurrection();
    }

    public void AttackNewStand(Field targetField)
    {
        int targetPower = targetField.OccupantCard.CardStatus.Power;
        foreach (Field field in fields)
        {
            if (field.IsAligned(Alignment.None)) continue;
            if (field.IsAligned(turn.CurrentAlignment)) continue;
            if (field.OccupantCard.CardStatus.Power > targetPower) field.OccupantCard.TryToAttackTarget(targetField);
        }
    }
    

    public void SetAlignedCardsActive()
    {
        if (!Turn.IsItMoveTime()) return;
        foreach (Field field in fields)
        {
            if (field.IsOpposed(turn.CurrentAlignment) && field.OccupantCard.CanBeTelecinetic()) field.OccupantCard.SetTelecinetic();
            if (!field.IsAligned(turn.CurrentAlignment)) continue;
            field.OccupantCard.SetActive();
        }
    }

    public void SetTargetableCards(Alignment align)
    {
        foreach (Field field in AlignedFields(align)) field.OccupantCard.SetTargetable();
    }

    public void ApplyPrincessBuff(CardSprite card)
    {
        card.AdvanceStrength(2);
        card.AdvanceHealth(1);
    }

    public void AdjustNewTurn() // Can be unstable due to unclear priorities.
    {  
        foreach (Field field in fields)
        {
            if (field.IsAligned(Alignment.None)) continue;
            if (field.IsAligned(turn.CurrentAlignment))
            {
                field.OccupantCard.ResetAttack();
                field.OccupantCard.RegenerateDexterity();
                
            }
            field.OccupantCard.ProgressTemporaryStats();
            if (!field.OccupantCard.CanUseSkill()) continue;
            field.OccupantCard.Character.SkillOnNewTurn(field.OccupantCard);
        }
        temporaryStatuses.AdjustNewTurn(turn.CurrentAlignment);
        SetAlignedCardsActive();
    }

    public void ShowJudgement(Alignment align) // TODO: Fix repeats!
    {
        foreach (Field field in fields)
        {
            if (!field.IsAligned(align)) continue;
            field.OccupantCard.AdvanceTempStrength(1);
        }
    }

    public void SetBackupCard(Field field)
    {
        //backupCard.transform.SetParent(field.transform, false);
        field.PlaceBackupCard(backupCard);
    }


    public bool IsLocked()
    {
        return Turn.InteractableDisabled;
    }

    public void MakeAllStatesIdle(Field exceptionField = null)
    {
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            if (field == exceptionField) continue;
            //if (field.IsAligned(turn.CurrentAlignment))
            field.OccupantCard.SetIdle();
        }
    }

    public Field GetField(int x, int y)
    {
        foreach (Field field in fields)
        {
            if (field.GetX() != x) continue;
            if (field.GetY() != y) continue;
            //Debug.Log("Got x=" + x + "; y=" + y);
            return field;
        }
        //Debug.Log("Got x=" + x + "; y=" + y + "as null!");
        return null;
    }

    public int[] GetRelativeCoordinates(int x, int y, float angle = 0)
    {
        int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
        int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
        int[] relCoord = new int[2];
        relCoord[0] = cosinus * x + sinus * y;
        relCoord[1] = cosinus * y - sinus * x;
        return relCoord;
    }

    public Field GetRelativeField(int x, int y, float angle = 0) // TODO: Merge
    {
        int[] relCoord = GetRelativeCoordinates(x, y, angle);
        return GetField(relCoord[0], relCoord[1]);
    }

    public void SwapCards(Field first, Field second)
    {
        CardSprite tempCard = first.OccupantCard;
        Alignment tempAlign = first.Align;
        SwapBackupCards(first, second);
        first.PlaceCard(second.OccupantCard, second.Align);
        //first.ConvertField(second.Align);
        second.PlaceCard(tempCard, tempAlign);
        //second.ConvertField(tempAlign);
    }

    private void SwapBackupCards(Field first, Field second)
    {
        if (first.AreThereTwoCards()) first.TransferBackupCard(second);
        else if (second.AreThereTwoCards()) second.TransferBackupCard(first);
    }

    public List<Field> AlignedFields(Alignment alignment, bool countBackup = false)
    {
        List<Field> alignedFields = new List<Field>();
        foreach (Field field in fields)
        {
            if (!field.IsAligned(alignment)) continue;
            alignedFields.Add(field);
            if (countBackup && field.AreThereTwoCards()) alignedFields.Add(field);
        }
        return alignedFields;
    }

    private int HighestAmountOfType(Alignment alignment)
    {
        int result = AmountOfType(alignment, Role.Offensive);
        if (result < AmountOfType(alignment, Role.Support)) result = AmountOfType(alignment, Role.Support);
        if (result < AmountOfType(alignment, Role.Agile)) result = AmountOfType(alignment, Role.Agile);
        if (result < AmountOfType(alignment, Role.Special)) result = AmountOfType(alignment, Role.Special);
        return result;
    }

    public Alignment HigherByAmountOfType()
    {
        if (HighestAmountOfType(Alignment.Player) > HighestAmountOfType(Alignment.Opponent)) return Alignment.Player;
        if (HighestAmountOfType(Alignment.Player) < HighestAmountOfType(Alignment.Opponent)) return Alignment.Opponent;
        return Alignment.None;
    }

    private int AmountOfType(Alignment alignment, Role role)
    {
        int result = 0;
        foreach (Field field in AlignedFields(alignment))
        {
            if (field.OccupantCard.Character.Role == role) result++;
            if (role == Role.Offensive && field.AreThereTwoCards()) result++;
        }
        return result;
    }

    public int HeatLevel(Field field, Alignment enemy)
    {
        int heat = 0;
        foreach (Field enemyField in AlignedFields(enemy))
        {
            CardSprite enemyCard = enemyField.OccupantCard;
            if (enemyCard.CanAttackField(field)) heat += enemyCard.GetStrength();
        }
        return heat;
    }

    public List<Character> AllInsideCharacters()
    {
        List<Character> list = new List<Character>();
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            list.Add(field.OccupantCard.Character);
        }
        Debug.Log("AllInsideCharacters count: " + list.Count);
        return list;
    }

    public void DebugForceRemoveCardFromField(CardSprite card)
    {
        if (!Debug.isDebugBuild) return;
        if (card == null) return;
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            if (field.OccupantCard == card)
            {
                field.OccupantCard.DebugForceDeactivateCard();
                return;
            }
        }
    }
}
