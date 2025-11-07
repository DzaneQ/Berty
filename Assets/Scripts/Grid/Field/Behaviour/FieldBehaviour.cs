using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.Grid.Field.Behaviour
{
    public class FieldBehaviour : MonoBehaviour
    {
        private MeshRenderer render;
        private HighlightEnum highlight;

        public BoardField BoardField { get; private set; }
        public BoardCardBehaviour ChildCard { get; private set; }
        public HighlightEnum Highlight { get => highlight;
            private set
            {
                highlight = value;
                ColorizeField();
                HighlightCard();
            }
        }

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
            if (BoardField.OccupantCard == null) ChildCard = null;
            else ChildCard = BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(BoardField.OccupantCard);  
            ColorizeField();
        }

        public void PutTheCard()
        {
            HandToFieldManager.Instance.RemoveSelectedCardFromHand();
            BoardCardBehaviour newCard = HandToFieldManager.Instance.PutCardOnField(this);
            PaymentManager.Instance.CallPayment(BoardField.OccupantCard.Stats.Power, newCard);
        }

        private void ColorizeField()
        {
            render.material = ColorizeObjectManager.Instance.GetMaterialFromAlignment(BoardField.Align, Highlight);
        }

        public void HighlightAsUnderAttack()
        {
            Highlight = HighlightEnum.UnderAttack;
        }

        public void HighlightAsUnderBlock()
        {
            Highlight = HighlightEnum.UnderBlock;
        }

        public void Unhighlight()
        {
            Highlight = HighlightEnum.None;
        }

        private void HighlightCard()
        {
            if (ChildCard == null) return;
            ChildCard.Core.HighlightAs(Highlight);
        }
    }
}
