using Berty.BoardCards;
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
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, attacker.ParentField.BoardField))
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
                case CharacterEnum.MisiekBert:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, newCard.ParentField.BoardField))
                        ApplyMisiekBertEffect(witness, newCard);
                    break;
                case CharacterEnum.EBerta:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, newCard.ParentField.BoardField)
                        && !witness.BoardCard.IsResistantTo(newCard.BoardCard))
                        ApplyEBertaEffect(witness, newCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.EBerta:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, newCard.ParentField.BoardField)
                        && !newCard.BoardCard.IsResistantTo(witness.BoardCard))
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
                case CharacterEnum.MisiekBert:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, movedCard.ParentField.BoardField))
                        ApplyMisiekBertEffect(witness, movedCard);
                    break;
                case CharacterEnum.EBerta:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, movedCard.ParentField.BoardField)
                        && !witness.BoardCard.IsResistantTo(movedCard.BoardCard))
                        ApplyEBertaEffect(witness, movedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.EBerta:
                    if (game.Grid.AreNeighboring(witness.ParentField.BoardField, movedCard.ParentField.BoardField)
                        && !movedCard.BoardCard.IsResistantTo(witness.BoardCard))
                        ApplyEBertaEffect(movedCard, witness);
                    break;
            }
        }

        private void ApplyMisiekBertEffect(BoardCardCore card, BoardCardCore misiekBert)
        {
            if (misiekBert.BoardCard.CharacterConfig.Character != CharacterEnum.MisiekBert)
                throw new Exception($"Misiek bert effect is casted by {misiekBert.BoardCard.CharacterConfig.Name}");
            CardNavigationManager.Instance.RotateCard(card, 270);
        }

        private void ApplyEBertaEffect(BoardCardCore target, BoardCardCore eBerta)
        {
            if (eBerta.BoardCard.CharacterConfig.Character != CharacterEnum.EBerta)
                throw new Exception($"eBerta effect is casted by {eBerta.BoardCard.CharacterConfig.Name}");
            int[] stats = { 
                target.BoardCard.Stats.Strength, 
                target.BoardCard.Stats.Power, 
                target.BoardCard.Stats.Dexterity, 
                target.BoardCard.Stats.Health
            };
            int minStat = stats.Min();
            if (stats[0] == minStat) StatChangeManager.Instance.AdvanceStrength(target, 1);
            if (stats[1] == minStat) StatChangeManager.Instance.AdvancePower(target, 1);
            if (stats[2] == minStat) StatChangeManager.Instance.AdvanceDexterity(target, 1);
            if (stats[3] == minStat) StatChangeManager.Instance.AdvanceHealth(target, 1);
            target.BoardCard.AddResistanceToCharacter(eBerta.BoardCard.CharacterConfig);
        }
    }
}