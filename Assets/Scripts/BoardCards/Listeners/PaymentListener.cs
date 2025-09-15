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
                if (core.AttackedCards.Count > 0) HandleSuccessfulAttack();
                CardStatusManager.Instance.DisableAttack(core);
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
                FieldToHandManager.Instance.RetrieveCardOnHold();
                SoundManager.Instance.TakeSound(core.transform);
                core.DestroyCard();
                return;
            }
            core.SetMainState();
        }

        private void HandleSuccessfulAttack()
        {
            Debug.Log($"Executing successful attack for {core}");
            switch (core.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.Bertonator:
                    if (core.AttackedCards.Count > 1) throw new Exception($"Bertonator is targeting {core.AttackedCards.Count} cards");
                    BoardCardCore pushedCard = core.AttackedCards[0];
                    pushedCard.StatChange.AdvanceDexterity(-1, core);
                    PushCardAway(pushedCard, core);
                    break;
                case CharacterEnum.KowbojBert:
                    core.StatChange.AdvanceDexterity(1, core);
                    EventManager.Instance.RaiseOnValueChange(core, 1);
                    break;
                case CharacterEnum.KuglarzBert:
                    core.StatChange.AdvanceDexterity(-1, null);
                    core.StatChange.AdvanceHealth(1, null);
                    break;
            }
        }

        private void PushCardAway(BoardCardCore target, BoardCardCore bertonator)
        {
            if (bertonator.BoardCard.CharacterConfig.Character != CharacterEnum.Bertonator) 
                throw new Exception($"Bertonator effect is casted by {bertonator.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = bertonator.BoardCard.GetDistanceTo(target.BoardCard);
            BoardField targetField = game.Grid.GetFieldDistancedFromCardOrNull(distance.x * 2, distance.y * 2, bertonator.BoardCard);
            if (targetField == null || targetField.IsOccupied()) target.StatChange.AdvanceHealth(-1, bertonator);
            else CardNavigationManager.Instance.MoveCard(target, targetField);
        }
    }
}