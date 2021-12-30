using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldGrid : MonoBehaviour
{
    private const int columns = 3;
    private const int rows = 3;

    private Field[,] field = new Field[columns, rows];

    private void Start()
    {
        AttachFieldMechanic();
    }

    private void AttachFieldMechanic()
    {
        int index = 0;
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                Transform fieldTransform = transform.GetChild(index);
                //Debug.Log(fieldTransform.localPosition.x + ", " + fieldTransform.localPosition.y);
                field[i, j] = fieldTransform.gameObject.GetComponent<Field>();
                field[i, j].AttachGrid(this);
                index++;
            }
        }
    }

    public Field GetField(int x, int y)
    {
        if (Math.Min(x, y) < 0 || x > columns || y > rows) return null;
        else return field[x, y];
    }
}
