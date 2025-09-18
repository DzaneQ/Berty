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
    public class PaymentListener : MonoBehaviour
    {
        private BoardCardCore core;
        private Game game;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            game = CoreManager.Instance.Game;
        }

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
            if (sender.Equals(core)) return;
            core.SetIdle();
        }

        private void HandlePaymentConfirm()
        {
            if (core.CardState == CardStateEnum.Attacking)
            {
                SoundManager.Instance.AttackSound(core.SoundSource, core.BoardCard.CharacterConfig.AttackSound);
                EventManager.Instance.RaiseOnDirectlyAttacked(core);
                HandleAfterAttackOrder();
                core.BoardCard.MarkAsHasAttacked();
            }
            else if (core.CardState == CardStateEnum.NewCard)
            {
                SoundManager.Instance.ConfirmSound(core.SoundSource);
                EventManager.Instance.RaiseOnNewCharacter(core);
                EventManager.Instance.RaiseOnAttackNewStand(core);
            }
            else if (core.IsOnNewMove())
            {
                EventManager.Instance.RaiseOnMovedCharacter(core);
            }
            core.SetMainState();
        }

        private void HandlePaymentCancel()
        {
            if (core.CardState == CardStateEnum.NewCard)
            {
                FieldToHandManager.Instance.RetrievePendingCard();
                SoundManager.Instance.TakeSound(core.transform);
                core.RemoveCard();
                return;
            }
            core.SetMainState();
        }

        private void HandleAfterAttackOrder()
        {
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.KoszmarZBertwood:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ExtraAttackCooldown, core.BoardCard);
                    break;
            }

            // Handle successful attack
            if (core.AttackedCards.Count <= 0) return;

            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaSerferka:
                    BoardCardCore swapTarget = core.AttackedCards.OrderByDescending(attackedCard => core.BoardCard.GetDistanceTo(attackedCard.BoardCard).x).First();
                    CardNavigationManager.Instance.SwapCards(core, swapTarget);
                    break;
                case SkillEnum.Bertonator:
                    if (core.AttackedCards.Count > 1) throw new Exception($"Bertonator is targeting {core.AttackedCards.Count} cards");
                    BoardCardCore pushedCard = core.AttackedCards[0];
                    pushedCard.StatChange.AdvanceDexterity(-1, core);
                    PushCardAway(pushedCard, core);
                    break;
                case SkillEnum.KowbojBert:
                    core.StatChange.AdvanceDexterity(1, core);
                    EventManager.Instance.RaiseOnValueChange(core, 1);
                    break;
                case SkillEnum.KuglarzBert:
                    core.StatChange.AdvanceDexterity(-1, null);
                    core.StatChange.AdvanceHealth(1, null);
                    break;
                case SkillEnum.RoninBert:
                    HandleRoninBertEffect(core.AttackedCards, core);
                    break;
            }
        }

        private void PushCardAway(BoardCardCore target, BoardCardCore bertonator)
        {
            if (bertonator.BoardCard.GetSkill() != SkillEnum.Bertonator) 
                throw new Exception($"Bertonator effect is casted by {bertonator.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = bertonator.BoardCard.GetDistanceTo(target.BoardCard);
            BoardField targetField = game.Grid.GetFieldDistancedFromCardOrNull(distance.x * 2, distance.y * 2, bertonator.BoardCard);
            if (targetField == null || targetField.IsOccupied()) target.StatChange.AdvanceHealth(-1, bertonator);
            else CardNavigationManager.Instance.MoveCard(target, targetField);
        }

        private void HandleRoninBertEffect(IReadOnlyList<BoardCardCore> attackedCards, BoardCardCore roninBert)
        {
            if (roninBert.BoardCard.GetSkill() != SkillEnum.RoninBert)
                throw new Exception($"RoninBert effect is casted by {roninBert.BoardCard.CharacterConfig.Name}");
            if (attackedCards.Count > 2)
                throw new Exception($"RoninBert got {attackedCards.Count} cards attacked");
            BoardCardCore swapTarget = attackedCards[0];
            // Get further card as swap target
            if (attackedCards.Count > 1
                && roninBert.BoardCard.GetDistanceTo(attackedCards[1].BoardCard).magnitude > roninBert.BoardCard.GetDistanceTo(swapTarget.BoardCard).magnitude)
                swapTarget = attackedCards[1];
            // Lose health if attacking more powerful card
            if (attackedCards.Any(core => core.BoardCard.Stats.Power > roninBert.BoardCard.Stats.Power))
                roninBert.StatChange.AdvanceHealth(-1, null);
            // Swap card positions
            CardNavigationManager.Instance.SwapCards(roninBert, swapTarget);
        }
    }
}