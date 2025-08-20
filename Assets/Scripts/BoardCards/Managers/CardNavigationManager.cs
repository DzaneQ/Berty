using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Collection;
using Berty.Grid.Entities;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class CardNavigationManager : ManagerSingleton<CardNavigationManager>
    {
        private BoardGrid Grid;

        protected override void Awake()
        {
            base.Awake();
            Grid = CoreManager.Instance.Game.Grid;
        }

        public void RotateCard(BoardCardCore card, int angle)
        {
            card.CardNavigation.RotateCardObject(angle);
            card.BoardCard.AdvanceAngleBy(angle);
        }

        public void MoveCard(BoardCardCore card, BoardField targetField)
        {
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            card.BoardCard.OccupiedField.RemoveCard();
            targetField.PlaceCard(card.BoardCard, cardAlign);
            card.CardNavigation.MoveCardObject(FieldCollectionManager.Instance.GetBehaviourFromEntity(targetField));
        }
    }
}