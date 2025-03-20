using Berty.CardSprite;
using Berty.Field;
using Berty.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Field.Grid
{
    public class GridInitialization : MonoBehaviour
    {
        private const int columns = 3;
        private const int rows = 3;

        [SerializeField] private GameObject cardPrefab;

        public void InitializeFields(out FieldBehaviour[] fields)
        {
            fields = new FieldBehaviour[columns * rows];
            int index = 0;
            int midColumn = (columns - 1) / 2;
            int midRow = (rows - 1) / 2;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    Transform fieldTransform = transform.GetChild(index);
                    fields[index] = fieldTransform.gameObject.GetComponent<FieldBehaviour>();
                    fields[index].SetCoordinates(i - midColumn, midRow - j);
                    fields[index].InstantiateCardSprite(cardPrefab);
                    index++;
                }
            }
        }

        internal void InitializeDefaultCardTransform(out DefaultTransform cardOnBoard, out Color defaultColor)
        {
            cardOnBoard = new DefaultTransform(cardPrefab.transform);
            defaultColor = cardPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        }

        internal CardSpriteBehaviour InitializeBackupCard()
        {
            return Instantiate(cardPrefab, transform).GetComponent<CardSpriteBehaviour>();
        }

        //private void InitializeCardSprite(Field field)
        //{
        //    field.OccupantCard = Instantiate(cardSpritePrefab, transform).GetComponent<CardSprite>();
        //}
    }
}