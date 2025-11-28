using Berty.Audio.Managers;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
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
            if (card.gameObject == gameObject) return;
            StateMachine.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            if (StateMachine.HasState(CardStateEnum.Attacking))
            {
                SoundManager.Instance.AttackSound(Sound.Source, Core.BoardCard.CharacterConfig.AttackSound);
                EventManager.Instance.RaiseOnDirectlyAttacked(Core);
                HandleAfterAttackOrder();
                Core.BoardCard.MarkAsHasAttacked();
            }
            else if (StateMachine.HasState(CardStateEnum.NewCard))
            {
                SoundManager.Instance.ConfirmSound(Sound.Source);
                EventManager.Instance.RaiseOnAttackNewStand(Core);
                EventManager.Instance.RaiseOnNewCharacter(Core);
            }
            else if (StateMachine.IsOnNewMove())
            {
                EventManager.Instance.RaiseOnMovedCharacter(Core);
            }
            StateMachine.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (StateMachine.HasState(CardStateEnum.NewCard))
            {
                FieldToHandManager.Instance.RetrievePendingCard();
                SoundManager.Instance.TakeSound(Core.transform);
                Entity.RemoveCard();
                return;
            }
            StateMachine.SetMainState();
        }

        private void HandleAfterAttackOrder()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.KoszmarZBertwood:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ExtraAttackCooldown, Core.BoardCard);
                    break;
            }

            // Handle successful attack
            if (Core.AttackedCards.Count <= 0) return;

            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaSerferka:
                    BoardCardBehaviour swapTarget = Core.AttackedCards.OrderByDescending(attackedCard => Core.BoardCard.GetDistanceTo(attackedCard.BoardCard).x).First();
                    CardNavigationManager.Instance.SwapCards(Core, swapTarget);
                    break;
                case SkillEnum.Bertonator:
                    if (Core.AttackedCards.Count > 1) throw new Exception($"Bertonator is targeting {Core.AttackedCards.Count} cards");
                    BoardCardBehaviour pushedCard = Core.AttackedCards[0];
                    pushedCard.Entity.AdvanceDexterity(-1, Core);
                    PushCardAway(pushedCard, Core);
                    break;
                case SkillEnum.KowbojBert:
                    Core.Entity.AdvanceDexterity(1, Core);
                    EventManager.Instance.RaiseOnValueChange(Core, 1);
                    break;
                case SkillEnum.KuglarzBert:
                    Core.Entity.AdvanceDexterity(-1, null);
                    Core.Entity.AdvanceHealth(1, null);
                    break;
                case SkillEnum.RoninBert:
                    HandleRoninBertEffect(Core.AttackedCards, Core);
                    break;
            }
        }

        private void PushCardAway(BoardCardBehaviour target, BoardCardBehaviour bertonator)
        {
            if (bertonator.BoardCard.GetSkill() != SkillEnum.Bertonator) 
                throw new Exception($"Bertonator effect is casted by {bertonator.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = bertonator.BoardCard.GetDistanceTo(target.BoardCard);
            BoardField targetField = game.Grid.GetFieldDistancedFromCardOrNull(distance.x * 2, distance.y * 2, bertonator.BoardCard);
            if (targetField == null || targetField.IsOccupied()) target.Entity.AdvanceHealth(-1, bertonator);
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
                roninBert.Entity.AdvanceHealth(-1, null);
            // Swap card positions
            CardNavigationManager.Instance.SwapCards(roninBert, swapTarget);
        }
    }
}