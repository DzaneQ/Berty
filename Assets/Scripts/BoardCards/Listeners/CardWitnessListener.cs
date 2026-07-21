using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.BoardCards.Listeners
{
    public class CardWitnessListener : BoardCardBehaviour
    {
        private void OnEnable()
        {
            EventManager.Instance.OnNewCharacter += HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter += HandleMovedCharacter;
            EventManager.Instance.OnCharacterSpecialEffect += HandleCharacterSpecialEffect;
            EventManager.Instance.OnValueChange += HandleValueChange;
            EventManager.Instance.OnSideChanged += HandleSideChanged;
            EventManager.Instance.OnCharacterDeath += HandleCharacterDeath;
            EventManager.Instance.OnFieldFreed += HandleOnFieldFreed;
        }

        private void OnDisable()
        {
            if (!gameObject.scene.isLoaded) return;
            EventManager.Instance.OnNewCharacter -= HandleNewCharacter;
            EventManager.Instance.OnMovedCharacter -= HandleMovedCharacter;
            EventManager.Instance.OnCharacterSpecialEffect -= HandleCharacterSpecialEffect;
            EventManager.Instance.OnValueChange -= HandleValueChange;
            EventManager.Instance.OnSideChanged -= HandleSideChanged;
            EventManager.Instance.OnCharacterDeath -= HandleCharacterDeath;
            EventManager.Instance.OnFieldFreed -= HandleOnFieldFreed;
        }

        private void HandleNewCharacter(object sender, EventArgs args)
        {
            BoardCardBehaviour newCharacter = (BoardCardBehaviour)sender;
            HandleNewCardWitness(this, newCharacter);
        }

        private void HandleMovedCharacter(object sender, EventArgs args)
        {
            BoardCardBehaviour movedCharacter = (BoardCardBehaviour)sender;
            if (movedCharacter.BoardCard == null) return;
            HandleMovedCardWitness(this, movedCharacter);
        }

        private void HandleCharacterDeath(object sender, EventArgs args)
        {
            BoardCardBehaviour dyingCharacter = (BoardCardBehaviour)sender;
            BoardCard.RemoveResistanceToCharacter(dyingCharacter.BoardCard.CharacterConfig);
            HandleDeathWitness(this, dyingCharacter);
        }

        private void HandleCharacterSpecialEffect(object sender, EventArgs args)
        {
            BoardCardBehaviour specialCharacter = (BoardCardBehaviour)sender;
            HandleCustomEffect(this, specialCharacter);
        }

        private void HandleSideChanged(object sender, EventArgs args)
        {
            BoardCardBehaviour convertedCharacter = (BoardCardBehaviour)sender;
            HandleSideChangeWitness(this, convertedCharacter);
        }

        private void HandleValueChange(object sender, ValueChangeEventArgs args)
        {
            BoardCardBehaviour sourceCharacter = (BoardCardBehaviour)sender;
            HandleCustomEffect(this, sourceCharacter, args.Delta);
        }

        private void HandleOnFieldFreed(object sender, EventArgs args)
        {
            FieldBehaviour field = (FieldBehaviour)sender;
            if (!game.Grid.AreNeighboring(field.BoardField, ParentField.BoardField)) return;
            StateMachine.UpdateButtons();
        }

        private void HandleNewCardWitness(BoardCardBehaviour witness, BoardCardBehaviour newCard)
        {
            if (witness.IsEqualTo(newCard)) HandleNewCardSelf(newCard);

            // When new card is the character with skill
            switch (newCard.BoardCard.GetSkill())
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.BertWho:
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, newCard);
                    break;
                case CharacterEnum.CheBert:
                case CharacterEnum.KonstablBert:
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.HandleCharacterSkill(witness, newCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.GetSkill())
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertVentura:
                case CharacterEnum.BertWho:
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(newCard, witness);
                    break;
                case CharacterEnum.CheBert:
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.HandleCharacterSkill(newCard, witness);
                    break;
            }
        }

        private void HandleMovedCardWitness(BoardCardBehaviour witness, BoardCardBehaviour movedCard)
        {
            if (witness.IsEqualTo(movedCard)) HandleMovedCardSelf(movedCard);

            // When moved card is the character with skill
            switch (movedCard.BoardCard.GetSkill())
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.BertWho:
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrezydentBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, movedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.GetSkill())
            {
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.BertVentura:
                case CharacterEnum.BertWho:
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SamurajBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(movedCard, witness);
                    break;
            }
        }

        private void HandleSideChangeWitness(BoardCardBehaviour witness, BoardCardBehaviour convertedCard)
        {
            // When converted card is the character with skill
            switch (convertedCard.BoardCard.GetSkill())
            {
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, convertedCard);
                    break;
                case CharacterEnum.CheBert:
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.HandleCharacterSkill(witness, convertedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.GetSkill())
            {
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.SuperfanBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(convertedCard, witness);
                    break;
                case CharacterEnum.CheBert:
                case CharacterEnum.ShaolinBert:
                    ApplySkillEffectManager.Instance.HandleCharacterSkill(convertedCard, witness);
                    break;
            }
        }

        private void HandleDeathWitness(BoardCardBehaviour witness, BoardCardBehaviour dyingCard)
        {
            if (witness.IsEqualTo(dyingCard)) return;

            // When witness is the character with skill
            switch (witness.BoardCard.GetSkill())
            {
                case CharacterEnum.BertVentura:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, dyingCard.ParentField.BoardField))
                        StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, witness.BoardCard, game.Grid.GetEnemyNeighborCount(BoardCard));
                    break;
                case CharacterEnum.Zombert:
                    if (dyingCard.BoardCard.Align == witness.BoardCard.Align) break;
                    witness.EntityHandler.AdvanceHealth(1, null);
                    break;
            }

            // When dying card is the character with skill
            switch (dyingCard.BoardCard.GetSkill())
            {
                case CharacterEnum.SedziaBertt:
                    if (dyingCard.BoardCard.Align == witness.BoardCard.Align) witness.EntityHandler.AdvanceTempStrength(1, dyingCard); // does not apply to backup card
                    return;
            }
        }

        private void HandleCustomEffect(BoardCardBehaviour witness, BoardCardBehaviour customEffectCard, int delta = 0)
        {
            // When customed effect is triggered by the character with skill
            switch (customEffectCard.BoardCard.GetSkill())
            {
                case CharacterEnum.PapiezBertII:
                    ApplySkillEffectManager.Instance.HandleCharacterSkill(witness, customEffectCard);
                    break;
                case CharacterEnum.KowbojBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, customEffectCard, delta);
                    break;
                case CharacterEnum.ZalobnyBert:
                    ApplySkillEffectManager.Instance.HandleNeighborCharacterSkill(witness, customEffectCard, delta);
                    break;
            }
        }

        private void HandleNewCardSelf(BoardCardBehaviour skillCard)
        {
            switch (skillCard.BoardCard.GetSkill())
            {
                case CharacterEnum.BertVentura:
                    StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, skillCard.BoardCard, game.Grid.GetEnemyNeighborCount(BoardCard));
                    break;
                case CharacterEnum.BertWho:
                    StatusManager.Instance.IncrementChargedStatusWithAlignment(StatusEnum.ExtraCardNextTurn, AlignmentEnum.Player, 2);
                    StatusManager.Instance.IncrementChargedStatusWithAlignment(StatusEnum.ExtraCardNextTurn, AlignmentEnum.Opponent, 2);
                    break;
                case CharacterEnum.CheBert:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.DisableEnemySpecialSkill, skillCard.BoardCard);
                    break;
                case CharacterEnum.GotkaBerta:
                    if (game.CardPile.AreThereAnyDeadCards())
                        StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.RevivalSelect, skillCard.BoardCard);
                    break;  
                case CharacterEnum.RycerzBerti:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.TelekineticArea, skillCard.BoardCard);
                    DecreasePowerForNeighbor(skillCard);
                    break;
                case CharacterEnum.SedziaBertt:
                    StatusManager.Instance.AddUniqueStatusWithProvider(StatusEnum.ForceSpecialRole, skillCard.BoardCard);
                    break;
                case CharacterEnum.SuperfanBert:
                    int hour = DateTime.Now.Hour;
                    if (hour < 5 || 18 <= hour)
                    {
                        skillCard.EntityHandler.AdvanceStrength(1, null);
                        skillCard.EntityHandler.AdvancePower(2, null);
                    }
                    break;
            }
        }

        private void HandleMovedCardSelf(BoardCardBehaviour skillCard)
        {
            switch (skillCard.BoardCard.GetSkill())
            {
                case CharacterEnum.BertVentura:
                    StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, skillCard.BoardCard, game.Grid.GetEnemyNeighborCount(BoardCard));
                    break;
                case CharacterEnum.PrezydentBert:
                    skillCard.EntityHandler.AdvancePower(-1, null);
                    break;
            }
        }

        private void DecreasePowerForNeighbor(BoardCardBehaviour skillCard) // BUG: Not working
        {
            // Get neighbors that do not resist the skill card
            List<BoardCard> neighbors = game.Grid.GetOccupantNeighbors(skillCard.BoardCard)
                .Where(card => !ApplySkillEffectManager.Instance.DoesPreventEffect(card, skillCard.BoardCard)).ToList();
            if (neighbors.Count <= 0) return;
            // Get enemy if possible
            BoardCard target = neighbors.Find(card => card.Align != skillCard.BoardCard.Align);
            if (target == null) target = neighbors.First();
            BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(target).EntityHandler.AdvancePower(-3, skillCard);
        }
    }
}