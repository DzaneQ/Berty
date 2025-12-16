using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class PaymentListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnPaymentStart += HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm += HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel += HandlePaymentCancel;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnPaymentStart -= HandlePaymentStart;
            EventManager.Instance.OnPaymentConfirm -= HandlePaymentConfirm;
            EventManager.Instance.OnPaymentCancel -= HandlePaymentCancel;
        }

        private void HandlePaymentStart(object sender, EventArgs args)
        {
            BoardCardBehaviour card = (BoardCardBehaviour)sender;
            if (card.IsEqualTo(this)) return;
            StateMachine.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            if (StateMachine.HasState(CardStateEnum.Attacking))
            {
                SoundManager.Instance.AttackSound(Sound.Source, BoardCard.CharacterConfig.AttackSound);
                List<BoardCardBehaviour> attackedCards = EventManager.Instance.RaiseOnDirectlyAttacked(this);
                HandleAfterAttackOrder(attackedCards);
                BoardCard.MarkAsHasAttacked();
            }
            else if (StateMachine.HasState(CardStateEnum.NewCard))
            {
                SoundManager.Instance.ConfirmSound(Sound.Source);
                EventManager.Instance.RaiseOnAttackNewStand(this);
                EventManager.Instance.RaiseOnNewCharacter(this);
            }
            else if (StateMachine.IsOnNewMove())
            {
                EventManager.Instance.RaiseOnMovedCharacter(this);
            }
            StateMachine.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (StateMachine.HasState(CardStateEnum.NewCard))
            {
                FieldToHandManager.Instance.RetrievePendingCard();
                SoundManager.Instance.TakeSound(transform);
                Activation.DeactivateCard();
                return;
            }
            StateMachine.SetMainState();
        }

        private void HandleAfterAttackOrder(List<BoardCardBehaviour> attackedCards)
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.KoszmarZBertwood:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ExtraAttackCooldown, BoardCard);
                    break;
            }

            // Handle successful attack
            if (attackedCards.Count <= 0) return;

            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BertkaSerferka:
                    BoardCardBehaviour swapTarget = attackedCards.OrderByDescending(attackedCard => BoardCard.GetDistanceTo(attackedCard.BoardCard).x).First();
                    CardNavigationManager.Instance.SwapCards(this, swapTarget);
                    break;
                case SkillEnum.Bertonator:
                    if (attackedCards.Count > 1) throw new Exception($"Bertonator is targeting {attackedCards.Count} cards");
                    BoardCardBehaviour pushedCard = attackedCards[0];
                    pushedCard.EntityHandler.AdvanceDexterity(-1, this);
                    PushCardAway(pushedCard, this);
                    break;
                case SkillEnum.KowbojBert:
                    EntityHandler.AdvanceDexterity(1, this);
                    EventManager.Instance.RaiseOnValueChange(this, 1);
                    break;
                case SkillEnum.KuglarzBert:
                    EntityHandler.AdvanceDexterity(-1, null);
                    EntityHandler.AdvanceHealth(1, null);
                    break;
                case SkillEnum.RoninBert:
                    HandleRoninBertEffect(attackedCards, this);
                    break;
            }
        }

        private void PushCardAway(BoardCardBehaviour target, BoardCardBehaviour bertonator)
        {
            if (bertonator.BoardCard.GetSkill() != SkillEnum.Bertonator) 
                throw new Exception($"Bertonator effect is casted by {bertonator.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = bertonator.BoardCard.GetDistanceTo(target.BoardCard);
            BoardField targetField = game.Grid.GetFieldDistancedFromCardOrNull(distance.x * 2, distance.y * 2, bertonator.BoardCard);
            if (targetField == null || targetField.IsOccupied()) target.EntityHandler.AdvanceHealth(-1, bertonator);
            else CardNavigationManager.Instance.MoveCard(target, targetField);
        }

        private void HandleRoninBertEffect(IReadOnlyList<BoardCardBehaviour> attackedCards, BoardCardBehaviour roninBert)
        {
            if (roninBert.BoardCard.GetSkill() != SkillEnum.RoninBert)
                throw new Exception($"RoninBert effect is casted by {roninBert.BoardCard.CharacterConfig.Name}");
            if (attackedCards.Count > 2)
                throw new Exception($"RoninBert got {attackedCards.Count} cards attacked");
            BoardCardBehaviour swapTarget = attackedCards[0];
            // Get further card as swap target
            if (attackedCards.Count > 1
                && roninBert.BoardCard.GetDistanceTo(attackedCards[1].BoardCard).magnitude > roninBert.BoardCard.GetDistanceTo(swapTarget.BoardCard).magnitude)
                swapTarget = attackedCards[1];
            // Lose health if attacking more powerful card
            if (attackedCards.Any(core => core.BoardCard.Stats.Power > roninBert.BoardCard.Stats.Power))
                roninBert.EntityHandler.AdvanceHealth(-1, null);
            // Swap card positions
            CardNavigationManager.Instance.SwapCards(roninBert, swapTarget);
        }
    }
}