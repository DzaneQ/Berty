using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.Grid.Field.Entities;
using System;
using Berty.Grid.Managers;

namespace Berty.Grid.Field.Behaviour
{
    public class FieldBehaviour : MonoBehaviour
    {
        private MeshRenderer render;

        public BoardField BoardField { get; private set; }

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
            ColorizeField();
        }

        public void ColorizeField()
        {
            render.material = ColorizeFieldManager.Instance.GetMaterialFromAlignment(BoardField.Align);
        }
    }
}
