using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInitialization : MonoBehaviour
{
    private const int columns = 3;
    private const int rows = 3;

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
                //Debug.Log(fieldTransform.localPosition.x + ", " + fieldTransform.localPosition.y);
                fields[index] = fieldTransform.gameObject.GetComponent<Field>();
                fields[index].SetCoordinates(i - midColumn, midRow - j);
                //InitializeCardSprite(fields[index]);
                index++;
            }
        }
    }

    //private void InitializeCardSprite(Field field)
    //{
    //    field.OccupantCard = Instantiate(cardSpritePrefab, transform).GetComponent<CardSprite>();
    //}
}
