using Berty.BoardCards;
using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Structs;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class HandleCharacterSkillEventManager : ManagerSingleton<HandleCharacterSkillEventManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void HandleDirectAttackWitness(BoardCardCore witness, BoardCardCore attacker)
        {
            if (witness == attacker) return;
            switch (attacker.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.MisiekBert:
                    ApplyMisiekBertEffect(witness, attacker);
                    break;
            }
        }

        public void HandleNewCardWitness(BoardCardCore witness, BoardCardCore newCard)
        {
            if (witness == newCard) return;
            // When new card is the character with skill
            switch (newCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    ApplyBertaSJWEffect(witness, newCard);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(witness, newCard);
                    break;
                case CharacterEnum.MisiekBert:
                    ApplyMisiekBertEffect(witness, newCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    ApplyBertaSJWEffect(newCard, witness);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(newCard, witness);
                    break;
            }
        }

        public void HandleMovedCardWitness(BoardCardCore witness, BoardCardCore movedCard)
        {
            if (witness == movedCard) return;
            // When moved card is the character with skill
            switch (movedCard.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    ApplyBertaSJWEffect(witness, movedCard);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(witness, movedCard);
                    break;
                case CharacterEnum.MisiekBert:
                    ApplyMisiekBertEffect(witness, movedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    ApplyBertaSJWEffect(movedCard, witness);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(movedCard, witness);
                    break;
            }
        }

        private void ApplyMisiekBertEffect(BoardCardCore target, BoardCardCore misiekBert)
        {
            if (misiekBert.BoardCard.CharacterConfig.Character != CharacterEnum.MisiekBert)
                throw new Exception($"Misiek bert effect is casted by {misiekBert.BoardCard.CharacterConfig.Name}");
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, misiekBert.ParentField.BoardField)) return;
            CardNavigationManager.Instance.RotateCard(target, 270);
        }

        private void ApplyEBertaEffect(BoardCardCore target, BoardCardCore eBerta)
        {
            if (eBerta.BoardCard.CharacterConfig.Character != CharacterEnum.EBerta)
                throw new Exception($"eBerta effect is casted by {eBerta.BoardCard.CharacterConfig.Name}");
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, eBerta.ParentField.BoardField)) return;
            if (!game.Grid.AreAligned(target.ParentField.BoardField, eBerta.ParentField.BoardField)) return;
            if (target.BoardCard.IsResistantTo(eBerta.BoardCard)) return;
            int[] stats = { 
                target.BoardCard.Stats.Strength, 
                target.BoardCard.Stats.Power, 
                target.BoardCard.Stats.Dexterity, 
                target.BoardCard.Stats.Health
            };
            int minStat = stats.Min();
            if (stats[0] == minStat) target.StatChange.AdvanceStrength(1);
            if (stats[1] == minStat) target.StatChange.AdvancePower(1);
            if (stats[2] == minStat) target.StatChange.AdvanceDexterity(1);
            if (stats[3] == minStat) target.StatChange.AdvanceHealth(1);
            target.BoardCard.AddResistanceToCharacter(eBerta.BoardCard.CharacterConfig);
        }

        private void ApplyBertaSJWEffect(BoardCardCore target, BoardCardCore bertaSJW)
        {
            if (bertaSJW.BoardCard.CharacterConfig.Character != CharacterEnum.BertaSJW)
                throw new Exception($"BertaSJW effect is casted by {bertaSJW.BoardCard.CharacterConfig.Name}");
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, bertaSJW.ParentField.BoardField)) return;
            if (target.BoardCard.IsResistantTo(bertaSJW.BoardCard)) return;
            target.StatChange.AdvancePower(-3);
            target.BoardCard.AddResistanceToCharacter(bertaSJW.BoardCard.CharacterConfig);
        }
    }
}