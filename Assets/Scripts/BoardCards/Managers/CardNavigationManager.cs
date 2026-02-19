using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.Utility;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class CardNavigationManager : ManagerSingleton<CardNavigationManager>
    {
        private BoardGrid Grid;

        protected override void Awake()
        {
            base.Awake();
            Grid = EntityLoadManager.Instance.Game.Grid;
        }

        public void RotateCard(BoardCardBehaviour card, int angle)
        {
            card.BoardCard.AdvanceCardSetAngleBy(angle);
            card.Navigation.RotateCardObject(angle);
            card.StateMachine.UpdateButtons();
        }

        // BUG: Moved cards are not highlighted when in attack range
        public void MoveCard(BoardCardBehaviour card, BoardField targetField, bool isOrdered = false)
        {
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            BoardCard backupCard = card.BoardCard.OccupiedField.BackupCard;
            FieldBehaviour targetFieldBehaviour = FieldCollectionManager.Instance.GetBehaviourFromEntity(targetField);

            // Update entity
            card.BoardCard.OccupiedField.RemoveAllCards();
            targetField.PlaceExistingCard(card.BoardCard, cardAlign);
            targetField.SetBackupCard(backupCard);

            // Update object
            targetFieldBehaviour.transform.GetChild(0).SetParent(card.transform.parent.parent, false);
            card.Navigation.MoveCardObject(targetFieldBehaviour);

            if (!isOrdered) card.Navigation.HandleNewMovementSkillEffect();
            card.StateMachine.UpdateButtons();
        }

        public void SwapCards(BoardCardBehaviour firstCardObject, BoardCardBehaviour secondCardObject)
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
            // Rotate cards
            RotateCard(firstCardObject, 180);
            RotateCard(secondCardObject, 180);
            // Update entities
            firstField.PlaceExistingCard(secondOccupantCard, secondFieldAlign);
            firstField.SetBackupCard(secondBackupCard);
            secondField.PlaceExistingCard(firstOccupantCard, firstFieldAlign);
            secondField.SetBackupCard(firstBackupCard);
            // Move card objects
            firstCardObject.Navigation.MoveCardObject(secondFieldObject);
            secondCardObject.Navigation.MoveCardObject(firstFieldObject);
        }
    }
}