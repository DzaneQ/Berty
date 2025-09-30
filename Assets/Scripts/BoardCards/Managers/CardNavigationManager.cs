using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
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
            card.BoardCard.AdvanceCardSetAngleBy(angle);
        }

        public void MoveCard(BoardCardCore card, BoardField targetField)
        {
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            BoardCard backupCard = card.BoardCard.OccupiedField.BackupCard;
            card.BoardCard.OccupiedField.RemoveAllCards();
            targetField.PlaceExistingCard(card.BoardCard, cardAlign);
            targetField.SetBackupCard(backupCard);
            card.CardNavigation.MoveCardObject(FieldCollectionManager.Instance.GetBehaviourFromEntity(targetField));
        }

        public void SwapCards(BoardCardCore firstCardObject, BoardCardCore secondCardObject)
        {
            // Get variables
            BoardCard firstOccupantCard = firstCardObject.BoardCard;
            BoardCard secondOccupantCard = secondCardObject.BoardCard;
            BoardField firstField = firstOccupantCard.OccupiedField;
            BoardField secondField = secondOccupantCard.OccupiedField;
            BoardCard firstBackupCard = firstField.BackupCard;
            BoardCard secondBackupCard = secondField.BackupCard;
            AlignmentEnum firstFieldAlign = firstField.Align;
            AlignmentEnum secondFieldAlign = secondField.Align;
            FieldBehaviour firstFieldObject = firstCardObject.ParentField;
            FieldBehaviour secondFieldObject = secondCardObject.ParentField;
            // Update entities
            firstField.PlaceExistingCard(secondOccupantCard, secondFieldAlign);
            firstField.SetBackupCard(secondBackupCard);
            secondField.PlaceExistingCard(firstOccupantCard, firstFieldAlign);
            secondField.SetBackupCard(firstBackupCard);
            // Move card objects
            firstCardObject.CardNavigation.MoveCardObject(secondFieldObject);
            secondCardObject.CardNavigation.MoveCardObject(firstFieldObject);
            // Rotate cards
            RotateCard(firstCardObject, 180);
            RotateCard(secondCardObject, 180);
        }
    }
}