using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Managers;
using Berty.Utility;
using System;
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

        public void OrderRotateCard(BoardCardCore card, NavigationEnum navigation)
        {
            if (card.IsDexterityBased() && card.BoardCard.IsTired) return;
            SoundManager.Instance.MoveSound(card.SoundSource);
            int angle = navigation switch
            {
                NavigationEnum.RotateLeft => -90,
                NavigationEnum.RotateRight => 90,
                _ => throw new ArgumentException("Invalid NavigationEnum for RotateCard")
            };
            CardNavigationManager.Instance.RotateCard(card, angle);
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

        public void OrderMoveCard(BoardCardCore card, NavigationEnum navigation)
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
            CardNavigationManager.Instance.MoveCard(card, targetField);
            if (card.CardState == CardStateEnum.NewTransform)
            {
                PaymentManager.Instance.CancelPayment();
                return;
            }
            if (!card.IsDexterityBased()) return;
            card.SetNewTransformFromNavigation(navigation);
            PaymentManager.Instance.CallPayment(GetPriceForMoving(card), card);
            ButtonObjectManager.Instance.HideCornerButton();
        }

        public void PrepareToAttack(BoardCardCore card)
        {
            if (!CanOrderAttack(card)) return;
            card.SetAttacking();
            PaymentManager.Instance.CallPayment(6 - card.BoardCard.Stats.Dexterity, card);
        }

        public void ConfirmPayment(BoardCardCore card)
        {
            PaymentManager.Instance.ConfirmPayment();
        }

        public void ApplySpecialEffect(BoardCardCore card)
        {
            Status status = Grid.Game.GetStatusByNameOrThrow(StatusEnum.ClickToApplyEffect);
            BoardCardCore source = BoardCardCollectionManager.Instance.GetCoreFromEntityOrThrow(status.Provider);
            card.StatChange.AdvanceStrength(2, source);
            card.StatChange.AdvanceHealth(1, source);
            StatusManager.Instance.RemoveStatus(status);
        }

        private int GetPriceForMoving(BoardCardCore card)
        {
            if (card.IsEligibleForTelekineticState())
                return Mathf.Min(6 - card.BoardCard.Stats.Dexterity,
                    6 - Grid.Game.GetStatusByNameOrThrow(StatusEnum.TelekineticArea).Provider.Stats.Dexterity);
            return 6 - card.BoardCard.Stats.Dexterity;
        }

        private bool CanOrderAttack(BoardCardCore card)
        {
            if (card.BoardCard.HasAttacked) return false;
            if (card.BoardCard.IsTired) return false;
            // Ventura check
            Status venturaStatus = Grid.Game.GetStatusByNameOrNull(StatusEnum.Ventura);
            if (venturaStatus == null) return true;
            if (ApplySkillEffectManager.Instance.DoesPreventEffect(card.BoardCard, venturaStatus.Provider)) return true;
            if (!Grid.AreNeighboring(card.ParentField.BoardField, venturaStatus.Provider.OccupiedField)) return true;
            if (Grid.AreAligned(card.ParentField.BoardField, venturaStatus.Provider.OccupiedField)) return true;
            if (card.BoardCard.CanAttackCard(venturaStatus.Provider)) return true;
            return false;
        }
    }
}