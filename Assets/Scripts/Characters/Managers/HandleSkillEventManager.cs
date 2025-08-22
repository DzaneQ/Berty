using Berty.BoardCards;
using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
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
    public class HandleSkillEventManager : ManagerSingleton<HandleSkillEventManager>
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
                    HandleNeighborCharacterSkill(witness, attacker);
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
                case CharacterEnum.EBerta:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrymusBert:
                    HandleNeighborCharacterSkill(witness, newCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                case CharacterEnum.EBerta:
                case CharacterEnum.PrymusBert:
                    HandleNeighborCharacterSkill(newCard, witness);
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
                case CharacterEnum.EBerta:
                case CharacterEnum.MisiekBert:
                case CharacterEnum.PrymusBert:
                    HandleNeighborCharacterSkill(witness, movedCard);
                    break;
            }

            // When witness is the character with skill
            switch (witness.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                case CharacterEnum.EBerta:
                case CharacterEnum.PrymusBert:
                    HandleNeighborCharacterSkill(movedCard, witness);
                    break;
            }
        }

        private void HandleNeighborCharacterSkill(BoardCardCore target, BoardCardCore skillOwner)
        {
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, skillOwner.ParentField.BoardField)) return;
            Debug.Log("Handling neighbor skill");
            if (IsResistant(target.BoardCard, skillOwner.BoardCard)) return;
            Debug.Log("No resistance for neighbor skill");
            ApplyCharacterEffect(target, skillOwner);
        }

        // Handles characters which skills are applied only once
        private bool IsResistant(BoardCard target, BoardCard skillOwner)
        {
            switch (skillOwner.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                case CharacterEnum.EBerta:
                case CharacterEnum.PrymusBert:
                    if (target.IsResistantTo(skillOwner)) return true;
                    else
                    {
                        target.AddResistanceToCharacter(skillOwner.CharacterConfig);
                        return false;
                    }
                default:
                    return false;
            }
        }

        private void ApplyCharacterEffect(BoardCardCore target, BoardCardCore skillOwner)
        {
            switch (skillOwner.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    target.StatChange.AdvancePower(-3, skillOwner);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case CharacterEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case CharacterEnum.PrymusBert:
                    target.StatChange.AdvancePower(3, skillOwner);
                    break;
                default:
                    throw new Exception($"Applying unknown effect for {target.name} from {skillOwner.name}");
            }
        }    

        private void ApplyEBertaEffect(BoardCardCore target, BoardCardCore eBerta)
        {
            if (eBerta.BoardCard.CharacterConfig.Character != CharacterEnum.EBerta)
                throw new Exception($"eBerta effect is casted by {eBerta.BoardCard.CharacterConfig.Name}");
            if (!game.Grid.AreAligned(target.ParentField.BoardField, eBerta.ParentField.BoardField)) return;
            int[] stats = { 
                target.BoardCard.Stats.Strength, 
                target.BoardCard.Stats.Power, 
                target.BoardCard.Stats.Dexterity, 
                target.BoardCard.Stats.Health
            };
            int minStat = stats.Min();
            if (stats[0] == minStat) target.StatChange.AdvanceStrength(1, eBerta);
            if (stats[1] == minStat) target.StatChange.AdvancePower(1, eBerta);
            if (stats[2] == minStat) target.StatChange.AdvanceDexterity(1, eBerta);
            if (stats[3] == minStat) target.StatChange.AdvanceHealth(1, eBerta);
        }  
    }
}