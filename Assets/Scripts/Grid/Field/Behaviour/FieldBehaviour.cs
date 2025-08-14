using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.Grid.Field.Entities;
using System;
using Berty.Grid.Managers;
using Berty.BoardCards.Behaviours;

namespace Berty.Grid.Field.Behaviour
{
    public class FieldBehaviour : MonoBehaviour
    {
        private MeshRenderer render;

        public BoardField BoardField { get; private set; }

        public BoardCardCore ChildCard { get; private set; }

        private void Awake()
        {
            render = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            BoardGrid grid = CoreManager.Instance.Game.Grid;
            BoardField = name switch
            {
                "Field NW" => grid.GetFieldFromCoordsOrThrow(-1, 1),
                "Field W" => grid.GetFieldFromCoordsOrThrow(-1, 0),
                "Field SW" => grid.GetFieldFromCoordsOrThrow(-1, -1),
                "Field N" => grid.GetFieldFromCoordsOrThrow(0, 1),
                "Field CT" => grid.GetFieldFromCoordsOrThrow(0, 0),
                "Field S" => grid.GetFieldFromCoordsOrThrow(0, -1),
                "Field NE" => grid.GetFieldFromCoordsOrThrow(1, 1),
                "Field E" => grid.GetFieldFromCoordsOrThrow(1, 0),
                "Field SE" => grid.GetFieldFromCoordsOrThrow(1, -1),
                _ => throw new Exception("Unknown field name to handle."),
            };
            UpdateField();
            //Debug.Log($"{name} got coordinates: ({BoardField.Coordinates.x}, {BoardField.Coordinates.y})");
        }

        public void UpdateField()
        {
            if (transform.childCount == 0) ChildCard = null;
            else if (transform.GetChild(transform.childCount - 1) != ChildCard.transform && )
                ChildCard = transform.GetChild(transform.childCount - 1).GetComponent<BoardCardCore>();
            ColorizeField();
        }

        public void UpdateFieldWithCard(BoardCardCore card)
        {
            if (card == null) throw new Exception("Updating field with null card");
            if (transform.childCount == 0) throw new Exception("Attempted to update field with card when no children");
            if (card.BoardCard.OccupiedField != BoardField) throw new Exception("Board field entities are not matching");
            ChildCard = card;
            ColorizeField();
        }

        private void ColorizeField()
        {
            render.material = ColorizeFieldManager.Instance.GetMaterialFromAlignment(BoardField.Align);
        }

        // NOTE: Use for animation purposes
        public void ColorizeNotOccupied()
        {
            render.material = ColorizeFieldManager.Instance.GetNeutralMaterial();
        }
    }
}
