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

    private TurnManager turn;
    protected MeshRenderer mesh;

    private const int columns = 3;
    private const int rows = 3;

    private Field[,] fields = new Field[columns, rows];

    private void Start()
    {
        turn = GameObject.Find("EventSystem").GetComponent<TurnManager>();
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
                //fields[i, j].AttachGrid(this);
                fields[i, j].SetCoordinates(i - midColumn, j - midRow);
                index++;
            }
        }
    }

    public Material GetMaterial(int isPlayerColor = 0)
    {
        switch (isPlayerColor)
        {
            case 1:
                return player;
            case -1:
                return opponent;
            default:
                return defaultMaterial;
        }
    }

    public TurnManager Turn()
    {
        return turn;
    }

    public GameObject GetCardSprite()
    {
        return cardPrefab;
    }

    public void AdjustCards()
    {
        Debug.Log("Adjusting cards...");
        foreach (Field field in fields)
        {
            Debug.Log("Checking field: x=" + field.GetX() + "; y= " + field.GetY());
            if (!field.IsOccupied()) continue;
            //Debug.Log("IsPlayerTurn? " + isPlayerTurn);
            //Debug.Log("IsPlayerField? " + field.IsPlayerField());
            if (field.IsPlayerField() == turn.IsPlayerTurn())
                field.GetOccupantCard().ShowDexterityButtons();
            else field.GetOccupantCard().DisableButtons();
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
        CardSprite tempCardSprite = first.GetOccupantCard();
        first.SetOccupantCard(second.GetOccupantCard());
        second.SetOccupantCard(tempCardSprite);
        first.GetOccupantCard().SetOccupiedField(first);
        second.GetOccupantCard().SetOccupiedField(second);
    }

    //public void RemoveAlignment(Field targetField)
    //{
    //    targetField = (Field)targetField;
    //    targetField.UpdateField();
    //}
}
