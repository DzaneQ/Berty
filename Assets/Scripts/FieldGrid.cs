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
    protected MeshRenderer mesh;

    private const int columns = 3;
    private const int rows = 3;

    private Field[,] fields = new Field[columns, rows];

    public GameObject CardPrefab { get => cardPrefab; }
    public Turn Turn { get => turn; }

    private void Awake()
    {
        turn = GameObject.Find("EventSystem").GetComponent<Turn>();
    }

    private void Start()
    {
        AttachFieldMechanic();
    }

    private void AttachFieldMechanic()
    {
        int index = 0;
        int midColumn = (columns - 1) / 2;
        int midRow = (rows - 1) / 2;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Transform fieldTransform = transform.GetChild(index);
                //Debug.Log(fieldTransform.localPosition.x + ", " + fieldTransform.localPosition.y);
                fields[i, j] = fieldTransform.gameObject.GetComponent<Field>();
                fields[i, j].SetCoordinates(i - midColumn, j - midRow);
                index++;
            }
        }
    }

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


    public void ChangeAlignmentOnFieldGrid()
    {
        foreach (Field field in fields) field.OccupantCard.HandleAlignmentChange();
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
}
