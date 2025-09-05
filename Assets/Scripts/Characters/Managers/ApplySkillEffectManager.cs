using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Structs;
using Berty.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class ApplySkillEffectManager : ManagerSingleton<ApplySkillEffectManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void HandleNeighborCharacterSkill(BoardCardCore target, BoardCardCore skillOwner, int delta = 0)
        {
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, skillOwner.ParentField.BoardField)) return;
            Debug.Log("Handling neighbor skill");
            if (IsResistant(target.BoardCard, skillOwner.BoardCard)) return;
            Debug.Log("No resistance for neighbor skill");
            ApplyCharacterEffect(target, skillOwner, delta);
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

        public void ApplyCharacterEffect(BoardCardCore target, BoardCardCore skillOwner, int delta = 0)
        {
            switch (skillOwner.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    target.StatChange.AdvancePower(-3, skillOwner);
                    break;
                case CharacterEnum.EBerta:
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case CharacterEnum.KonstablBert:
                    if (target.BoardCard.GetRole() != RoleEnum.Special) break;
                    target.StatChange.AdvanceHealth(-1, skillOwner);
                    break;
                case CharacterEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case CharacterEnum.PrezydentBert:
                    if (!AreAllied(target, skillOwner)) break;
                    target.StatChange.AdvanceStrength(1, skillOwner);
                    break;
                case CharacterEnum.PrymusBert:
                    target.StatChange.AdvancePower(3, skillOwner);
                    break;
                case CharacterEnum.ShaolinBert:
                    if (AreAllied(target, skillOwner)) break;
                    target.StatChange.AdvanceStrength(-target.BoardCard.Stats.Power / 3, skillOwner);
                    break;
                case CharacterEnum.ZalobnyBert:
                    if (!AreAllied(target, skillOwner)) break;
                    target.StatChange.AdvanceHealth(-2 * delta, skillOwner);
                    break;
                default:
                    throw new Exception($"Applying unknown effect for {target.name} from {skillOwner.name}");
            }
        }    

        private void ApplyEBertaEffect(BoardCardCore target, BoardCardCore eBerta)
        {
            if (eBerta.BoardCard.CharacterConfig.Character != CharacterEnum.EBerta)
                throw new Exception($"eBerta effect is casted by {eBerta.BoardCard.CharacterConfig.Name}");
            if (!AreAllied(target, eBerta)) return;
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

        private bool AreAllied(BoardCardCore firstCard, BoardCardCore secondCard)
        {
            return game.Grid.AreAligned(firstCard.ParentField.BoardField, secondCard.ParentField.BoardField);
        }    
    }
}