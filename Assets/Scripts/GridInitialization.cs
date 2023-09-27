using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitialization : MonoBehaviour
{
    private const int columns = 3;
    private const int rows = 3;

    [SerializeField] private GameObject cardPrefab;

    public void InitializeFields(out Field[] fields)
    {
        fields = new Field[columns * rows];
        int index = 0;
        int midColumn = (columns - 1) / 2;
        int midRow = (rows - 1) / 2;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Transform fieldTransform = transform.GetChild(index);
                fields[index] = fieldTransform.gameObject.GetComponent<Field>();
                fields[index].SetCoordinates(i - midColumn, midRow - j);
                fields[index].InstantiateCardSprite(cardPrefab);
                index++;
            }
        }
    }

    internal void InitializeDefaultCardTransform(out DefaultTransform cardOnBoard)
    {
        cardOnBoard = new DefaultTransform(cardPrefab.transform);
    }

    internal CardSprite InitializeBackupCard()
    {
        return Instantiate(cardPrefab, transform).GetComponent<CardSprite>();
    }

    //private void InitializeCardSprite(Field field)
    //{
    //    field.OccupantCard = Instantiate(cardSpritePrefab, transform).GetComponent<CardSprite>();
    //}
}
