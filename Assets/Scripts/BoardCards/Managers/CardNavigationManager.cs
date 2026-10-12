using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.Utility;

namespace Berty.BoardCards.Managers
{
    public class CardNavigationManager : ManagerSingleton<CardNavigationManager>
    {
        private const int UPSIDE_DOWN_ROTATION = 180;

        public void RotateCard(BoardCardBehaviour card, int angle)
        {
            card.BoardCard.AdvanceCardSetAngleBy(angle);
            card.Navigation.RotateCardObject(angle);
            card.StateMachine.UpdateButtons();
        }

        public void MoveCard(BoardCardBehaviour card, BoardField targetField, bool isOnOrderStandby = false)
        {
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            BoardCard backupCard = card.BoardCard.OccupiedField.BackupCard;
            FieldBehaviour targetFieldBehaviour = FieldCollectionManager.Instance.GetBehaviourFromEntityOrThrow(targetField);
            targetField = targetFieldBehaviour.BoardField; // Overwrite potential copy with an actual entity

            // Update entity
            card.BoardCard.OccupiedField.RemoveAllCards();
            targetField.PlaceExistingCard(card.BoardCard, cardAlign);
            targetField.SetBackupCard(backupCard);

            // Update object
            targetFieldBehaviour.transform.GetChild(Constants.CARD_SET_CHILD_INDEX).SetParent(card.transform.parent.parent, false);
            card.Navigation.MoveCardObject(targetFieldBehaviour);

            if (!isOnOrderStandby) card.Navigation.HandleNewMovementSkillEffect();
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
            RotateCard(firstCardObject, UPSIDE_DOWN_ROTATION);
            RotateCard(secondCardObject, UPSIDE_DOWN_ROTATION);
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