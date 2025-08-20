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
    public class BoardCardActionManager : ManagerSingleton<BoardCardActionManager>
    {
        private BoardGrid Grid;

        protected override void Awake()
        {
            base.Awake();
            Grid = CoreManager.Instance.Game.Grid;
        }

        public void RotateCard(BoardCardCore card, NavigationEnum navigation)
        {
            if (card.IsDexterityBased() && card.BoardCard.IsTired) return;
            SoundManager.Instance.MoveSound(card.SoundSource);
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
            if (!card.IsDexterityBased()) return;
            card.SetNewTransformFromNavigation(navigation);
            PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
            ButtonObjectManager.Instance.HideCornerButton();
        }

        // TODO: Handle two cards moving.
        public void MoveCard(BoardCardCore card, NavigationEnum navigation)
        {
            if (card.IsDexterityBased() && card.BoardCard.IsTired) return;
            SoundManager.Instance.MoveSound(card.SoundSource);
            Vector2Int distance = navigation switch
            {
                NavigationEnum.MoveUp => new Vector2Int(0, 1),
                NavigationEnum.MoveRight => new Vector2Int(1, 0),
                NavigationEnum.MoveDown => new Vector2Int(0, -1),
                NavigationEnum.MoveLeft => new Vector2Int(-1, 0),
                _ => throw new ArgumentException("Invalid NavigationEnum for MoveCard")
            };
            BoardField targetField = Grid.GetFieldDistancedFromCardOrThrow(distance.x, distance.y, card.BoardCard);
            AlignmentEnum cardAlign = card.BoardCard.OccupiedField.Align;
            card.BoardCard.OccupiedField.RemoveCard();
            targetField.PlaceCard(card.BoardCard, cardAlign);
            card.CardNavigation.MoveCardObject(FieldCollectionManager.Instance.GetBehaviourFromEntity(targetField));
            if (card.CardState == CardStateEnum.NewTransform)
            {
                PaymentManager.Instance.CancelPayment();
                return;
            }
            if (!card.IsDexterityBased()) return;
            card.SetNewTransformFromNavigation(navigation);
            PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
            ButtonObjectManager.Instance.HideCornerButton();
        }

        public void PrepareToAttack(BoardCardCore card)
        {
            if (card.BoardCard.HasAttacked || card.BoardCard.IsTired) return;
            card.SetAttacking();
            PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
        }

        public void ConfirmPayment(BoardCardCore card)
        {
            PaymentManager.Instance.ConfirmPayment();
        }
    }
}