using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material player;
    [SerializeField] private Material opponent;
    [SerializeField] private GameObject cardPrefab;

    private Turn turn;
    //protected MeshRenderer mesh;
    private Field[] fields;

    public GameObject CardPrefab { get => cardPrefab; }
    public Turn Turn { get => turn; }

    private void Awake()
    {
        turn = FindObjectOfType<Turn>();
    }

    private void Start()
    {
        GridInitialization init = GetComponent<GridInitialization>();
        init.InitializeFields(out fields);
        //init.InitializeFieldGrid();
        //AttachFieldMechanic();
        Destroy(init);
    }

    //public void AttachFields(Field[] fields)
    //{
    //    if (this.fields == null) this.fields = fields;
    //}

    //public void AttachTurn(Turn turn)
    //{
    //    if (this.turn == null) this.turn = turn;
    //}

    //private void AttachFieldMechanic()
    //{
    //    int index = 0;
    //    int midColumn = (columns - 1) / 2;
    //    int midRow = (rows - 1) / 2;
    //    for (int i = 0; i < columns; i++)
    //    {
    //        for (int j = 0; j < rows; j++)
    //        {
    //            Transform fieldTransform = transform.GetChild(index);
    //            //Debug.Log(fieldTransform.localPosition.x + ", " + fieldTransform.localPosition.y);
    //            fields[i, j] = fieldTransform.gameObject.GetComponent<Field>();
    //            fields[i, j].SetCoordinates(i - midColumn, midRow - j);
    //            index++;
    //        }
    //    }
    //}

    public Material GetMaterial(Alignment alignment)
    {
        switch (alignment)
        {
            case Alignment.Player:
                return player;
            case Alignment.Opponent:
                return opponent;
            default:
                return defaultMaterial;
        }
    }

    public void AttackNewStand(Field targetField)
    {
        int targetPower = targetField.OccupantCard.Character.Power;
        foreach (Field field in fields)
        {
            if (field.IsAligned(Alignment.None)) continue;
            if (field.IsAligned(turn.CurrentAlignment)) continue;
            if (field.OccupantCard.Character.Power > targetPower) field.OccupantCard.TryToAttack(targetField);
        }
    }
    

    public void AdjustCardButtons()
    {
        //Debug.Log("Adjusting cards...");
        foreach (Field field in fields)
        {
            //Debug.Log("Checking field: x=" + field.GetX() + "; y= " + field.GetY());
            if (field.IsAligned(Alignment.None)) continue;
            //Debug.Log("IsPlayerTurn? " + isPlayerTurn);
            //Debug.Log("IsPlayerField? " + field.IsPlayerField());
            if (field.IsAligned(turn.CurrentAlignment))
                field.OccupantCard.SetActive();
            else field.OccupantCard.SetIdle();
        }
    }

    public void AdjustNewTurn()
    {
        foreach (Field field in fields) field.OccupantCard.ResetAttack();
        AdjustCardButtons();
    }

    public void LockInteractables()
    {
        foreach (Field field in fields)
        {
            field.OccupantCard.Lock();
        }
        AdjustCardButtons();
    }

    public void UnlockInteractables()
    {
        foreach (Field field in fields)
        {
            field.OccupantCard.Unlock();
        }
        AdjustCardButtons();
    }

    public void DisableAllButtons()
    {
        foreach (Field field in fields)
        {
            if (!field.IsOccupied()) continue;
            //Debug.Log("Field align: " + field.Align);
            //Debug.Log("Current alignment: " + turn.CurrentAlignment);
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


    public Field GetRelativeField(int x, int y, float angle = 0)
    {
        int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
        int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
        int relativeX = cosinus * x + sinus * y;
        int relativeY = cosinus * y - sinus * x;
        return GetField(relativeX, relativeY);
    }

    public void SwapCards(Field first, Field second)
    {
        CardSprite tempCardSprite = first.OccupantCard;
        first.OccupantCard = second.OccupantCard;
        second.OccupantCard = tempCardSprite;
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
            if (enemyCard.CanAttack(field)) heat += enemyCard.Character.Strength;
        }
        return heat;
    }
}
