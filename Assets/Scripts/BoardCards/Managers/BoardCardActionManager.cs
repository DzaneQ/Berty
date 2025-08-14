using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Collection;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class BoardCardActionManager : ManagerSingleton<BoardCardActionManager>
    {
        private BoardGrid Grid;
        private FieldCollection fieldCollection;

        protected override void Awake()
        {
            base.Awake();
            Grid = CoreManager.Instance.Game.Grid;
            fieldCollection = ObjectReadManager.Instance.FieldBoard.GetComponent<FieldCollection>();
        }

        public void RotateCard(BoardCardCore card, NavigationEnum navigation)
        {
            int angle = navigation switch
            {
                NavigationEnum.RotateLeft => -90,
                NavigationEnum.RotateRight => 90,
                _ => throw new ArgumentException("Invalid NavigationEnum for RotateCard")
            };
            card.CardNavigation.RotateCardObject(angle);
            card.BoardCard.AdvanceAngleBy(angle);
            Debug.Log($"{card.name} has angle {card.BoardCard.Direction} with coordinates: ({card.BoardCard.RelativeCoordinates.x},{card.BoardCard.RelativeCoordinates.y})");
            if (card.CardState == CardStateEnum.NewTransform)
            {
                PaymentManager.Instance.CancelPayment();
                return;
            }
            bool paidAction = card.IsDexterityBased();
            card.SetNewTransformFromNavigation(navigation);
            if (paidAction) PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
        }

        // TODO: Handle two cards moving.
        public void MoveCard(BoardCardCore card, NavigationEnum navigation)
        {
            Vector2Int distance = navigation switch
            {
                NavigationEnum.MoveUp => new Vector2Int(0, 1),
                NavigationEnum.MoveRight => new Vector2Int(1, 0),
                NavigationEnum.MoveDown => new Vector2Int(0, -1),
                NavigationEnum.MoveLeft => new Vector2Int(-1, 0),
                _ => throw new ArgumentException("Invalid NavigationEnum for MoveCard")
            };
            BoardField targetField = Grid.GetFieldDistancedFromCardOrThrow(distance.x, distance.y, card.BoardCard);
            card.CardNavigation.MoveCardObject(fieldCollection.GetBehaviourFromEntity(targetField));
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            card.BoardCard.OccupiedField.RemoveCard();
            targetField.PlaceCard(card.BoardCard, cardAlign);
            if (card.CardState == CardStateEnum.NewTransform)
            {
                PaymentManager.Instance.CancelPayment();
                return;
            }
            bool paidAction = card.IsDexterityBased();
            card.SetNewTransformFromNavigation(navigation);
            if (paidAction) PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
        }

        public void PrepareToAttack(BoardCardCore card)
        {
            card.SetAttacking();
            PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
        }

        public void ConfirmPayment(BoardCardCore card)
        {
            PaymentManager.Instance.ConfirmPayment();
        }
    }
}