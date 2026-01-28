using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
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
            BoardGrid grid = EntityLoadManager.Instance.Game.Grid;
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
            if (!LoadTheCard()) UpdateField();
            //Debug.Log($"{name} got coordinates: ({BoardField.Coordinates.x}, {BoardField.Coordinates.y})");
        }

        public void UpdateField()
        {
            if (BoardField.OccupantCard == null) ChildCard = null;
            else if (ChildCard == null || ChildCard.BoardCard != BoardField.OccupantCard)
                ChildCard = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(BoardField.OccupantCard);  
            ColorizeField();
        }

        public void UpdateFieldWith(BoardCard card)
        {
            if (ChildCard == null || ChildCard.BoardCard != card)
            {
                ChildCard = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(card);
            }
            ColorizeField();
        }

        public void PutTheCard()
        {
            CharacterConfig selectedCardConfig = HandToFieldManager.Instance.RemoveSelectedCardFromHand();
            HandToFieldManager.Instance.ActivateCardOnField(this, selectedCardConfig);
            PaymentManager.Instance.CallPayment(selectedCardConfig.Power, ChildCard);
        }
        
        // BUG: When occupant card dies showing the backup card, it throws an error on turn change
        private bool LoadTheCard() // TODO: Handle backup card
        {
            if (BoardField.OccupantCard == null) return false;
            ChildCard = transform.GetChild(0).GetChild(0).GetComponent<BoardCardBehaviour>();
            ChildCard.gameObject.SetActive(true);
            if (BoardField.BackupCard == null) ChildCard.Activation.LoadCard(BoardField.OccupantCard);
            else
            {
                // If there's backup card, load it first then hide
                ChildCard.Activation.LoadCard(BoardField.BackupCard);
                ChildCard.gameObject.SetActive(false);
                // Then load the occupant card
                ChildCard = ObjectReadManager.Instance.BackupCard.GetComponent<BoardCardBehaviour>();
                ChildCard.transform.SetParent(transform.GetChild(0), false);
                ChildCard.gameObject.SetActive(true);
                ChildCard.Activation.LoadCard(BoardField.OccupantCard);
            }
            return true;
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
            ChildCard.Sprite.HighlightAs(Highlight);
        }
    }
}
