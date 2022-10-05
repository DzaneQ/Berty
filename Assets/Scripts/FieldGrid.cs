using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material player;
    [SerializeField] private Material opponent;

    private Turn turn;
    //protected MeshRenderer mesh;
    private Field[] fields;
    private DefaultTransform cardOnBoard;

    public Turn Turn => turn;

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
    }

    private void Start()
    {
        GridInitialization init = GetComponent<GridInitialization>();
        init.InitializeFields(out fields);
        init.InitializeDefaultCardTransform(out cardOnBoard);
        Destroy(init);
    }

    public Material GetMaterial(Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Player: return player;
            case Alignment.Opponent: return opponent;
            default: return defaultMaterial;
        }
    }

    public void ResetCardTransform(Transform cardSprite)
    {
        cardSprite.localPosition = cardOnBoard.defaultPosition;
        cardSprite.localRotation = cardOnBoard.defaultRotation;
    }

    public float GetDefaultAngle()
    {
        return cardOnBoard.defaultRotation.eulerAngles.z;
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
    

    public void AdjustCardButtons()
    {
        foreach (Field field in fields)
        {
            if (field.IsAligned(Alignment.None)) continue;
            if (field.IsAligned(turn.CurrentAlignment))
                field.OccupantCard.SetActive();
            else field.OccupantCard.SetIdle();
        }
    }

    public void AdjustNewTurn()
    {
        foreach (Field field in fields)
        {
            //if (field.IsAligned(Alignment.None)) continue;
            //field.OccupantCard.Character.CastGlobalEvent(field.OccupantCard);
            if (!field.IsAligned(turn.CurrentAlignment)) continue;
            field.OccupantCard.ResetAttack();
            field.OccupantCard.RegenerateDexterity();
        }
        AdjustCardButtons();
    }

    public bool IsLocked()
    {
        return Turn.InteractableDisabled;
    }

    public void DisableAllButtons()
    {
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            if (field.IsAligned(turn.CurrentAlignment))
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
        first.TakeCard(second.OccupantCard);
        first.ConvertField(second.Align);
        second.TakeCard(tempCard);
        second.ConvertField(tempAlign);
    }

    public List<Field> AlignedFields(Alignment alignment)
    {
        List<Field> alignedFields = new List<Field>();
        foreach (Field field in fields)
        {
            if (field.IsAligned(alignment)) alignedFields.Add(field);
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
        }
        return result;
    }

    public int HeatLevel(Field field, Alignment enemy)
    {
        int heat = 0;
        foreach (Field enemyField in AlignedFields(enemy))
        {
            CardSprite enemyCard = enemyField.OccupantCard;
            if (enemyCard.CanAttackField(field)) heat += enemyCard.CardStatus.Strength;
        }
        return heat;
    }
}
