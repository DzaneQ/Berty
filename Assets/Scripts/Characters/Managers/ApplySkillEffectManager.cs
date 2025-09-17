using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
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

            switch (skillOwner.BoardCard.CharacterConfig.Character)
            {
                // Handle characters which skills are applied only once
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.EBerta:
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.PrymusBert:
                case CharacterEnum.SuperfanBert:
                    if (target.BoardCard.IsResistantTo(skillOwner.BoardCard)) return;
                    if (ApplyCharacterEffect(target, skillOwner, delta))
                        target.BoardCard.AddResistanceToCharacter(skillOwner.BoardCard.CharacterConfig);
                    break;
                default:
                    ApplyCharacterEffect(target, skillOwner, delta);
                    break;
            }          
        }

        // output: Has the effect been applied
        public bool ApplyCharacterEffect(BoardCardCore target, BoardCardCore skillOwner, int delta = 0)
        {
            if (DoesPreventEffect(target, skillOwner)) return false;

            switch (skillOwner.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertaSJW:
                    target.StatChange.AdvancePower(-3, skillOwner);
                    break;
                case CharacterEnum.BertaGejsza:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvanceDexterity(-1, skillOwner);
                    else target.StatChange.AdvanceDexterity(-3, skillOwner);
                    break;
                case CharacterEnum.BertaTrojanska:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvancePower(1, skillOwner);
                    else target.StatChange.AdvanceStrength(-1, skillOwner);
                    break;
                case CharacterEnum.EBerta:
                    if (!AreAllied(target, skillOwner)) return false;
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case CharacterEnum.KonstablBert:
                    if (target.BoardCard.GetRole() != RoleEnum.Special) return false;
                    target.StatChange.AdvanceHealth(-1, skillOwner);
                    break;
                case CharacterEnum.KowbojBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceDexterity(delta, skillOwner);
                    break;
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.SuperfanBert:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvancePower(1, skillOwner);
                    else return false;
                    break;
                case CharacterEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case CharacterEnum.PapiezBertII:
                    if (AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvancePower(-2, skillOwner);
                    break;
                case CharacterEnum.PrezydentBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceStrength(1, skillOwner);
                    break;
                case CharacterEnum.PrymusBert:
                    target.StatChange.AdvancePower(3, skillOwner);
                    break;
                case CharacterEnum.SamurajBert:
                    return ApplySamurajBertEffectAndResistance(skillOwner);
                case CharacterEnum.ShaolinBert:
                    if (AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceStrength(-target.BoardCard.Stats.Power / 3, skillOwner);
                    break;
                case CharacterEnum.ZalobnyBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceHealth(-2 * delta, skillOwner);
                    break;
                default:
                    throw new Exception($"Applying unknown effect for {target.name} from {skillOwner.name}");
            }
            return true;
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
            if (stats[0] == minStat) target.StatChange.AdvanceStrength(1, eBerta);
            if (stats[1] == minStat) target.StatChange.AdvancePower(1, eBerta);
            if (stats[2] == minStat) target.StatChange.AdvanceDexterity(1, eBerta);
            if (stats[3] == minStat) target.StatChange.AdvanceHealth(1, eBerta);
        }

        private bool ApplySamurajBertEffectAndResistance(BoardCardCore samurajBert)
        {
            if (samurajBert.BoardCard.CharacterConfig.Character != CharacterEnum.SamurajBert)
                throw new Exception($"SamurajBert effect is casted by {samurajBert.BoardCard.CharacterConfig.Name}");
            if (samurajBert.BoardCard.IsResistantTo(samurajBert.BoardCard)) return false;
            if (game.Grid.GetAllNeighbors(samurajBert.BoardCard).Count < 3) return false;
            samurajBert.StatChange.AdvanceDexterity(1, samurajBert);
            samurajBert.StatChange.AdvancePower(1, samurajBert);
            samurajBert.BoardCard.AddResistanceToCharacter(samurajBert.BoardCard.CharacterConfig);
            return true;
        }

        private bool DoesPreventEffect(BoardCardCore target, BoardCardCore skillCard)
        {
            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertPogromca:
                    return skillCard.BoardCard.GetRole() == RoleEnum.Special;
                case CharacterEnum.BigMadB:
                    return skillCard.BoardCard.GetRole() == RoleEnum.Support;
                default:
                    return false;
            }
        }

        private bool AreAllied(BoardCardCore firstCard, BoardCardCore secondCard)
        {
            return game.Grid.AreAligned(firstCard.ParentField.BoardField, secondCard.ParentField.BoardField);
        }
    }
}