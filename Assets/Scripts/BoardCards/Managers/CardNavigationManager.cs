using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.Utility;

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
            card.Navigation.RotateCardObject(angle);
            card.BoardCard.AdvanceCardSetAngleBy(angle);
            card.StateMachine.UpdateButtons();
        }

        public void MoveCard(BoardCardBehaviour card, BoardField targetField, bool isOrdered = false)
        {
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            BoardCard backupCard = card.BoardCard.OccupiedField.BackupCard;
            card.BoardCard.OccupiedField.RemoveAllCards();
            targetField.PlaceExistingCard(card.BoardCard, cardAlign);
            targetField.SetBackupCard(backupCard);
            card.Navigation.MoveCardObject(FieldCollectionManager.Instance.GetBehaviourFromEntity(targetField));
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
            FieldBehaviour firstFieldObject = firstCardObject.Core.ParentField;
            FieldBehaviour secondFieldObject = secondCardObject.Core.ParentField;
            // Update entities
            firstField.PlaceExistingCard(secondOccupantCard, secondFieldAlign);
            firstField.SetBackupCard(secondBackupCard);
            secondField.PlaceExistingCard(firstOccupantCard, firstFieldAlign);
            secondField.SetBackupCard(firstBackupCard);
            // Move card objects
            firstCardObject.Navigation.MoveCardObject(secondFieldObject);
            secondCardObject.Navigation.MoveCardObject(firstFieldObject);
            // Rotate cards
            RotateCard(firstCardObject, 180);
            RotateCard(secondCardObject, 180);
        }
    }
}