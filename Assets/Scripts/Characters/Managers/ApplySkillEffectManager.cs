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
            HandleCharacterSkill(target, skillOwner, delta);         
        }

        public void HandleCharacterSkill(BoardCardCore target, BoardCardCore skillOwner, int delta = 0)
        {
            switch (skillOwner.BoardCard.GetSkill())
            {
                // Handle characters which skills are applied only once
                case SkillEnum.BertaGejsza:
                case SkillEnum.BertaSJW:
                case SkillEnum.BertaTrojanska:
                case SkillEnum.BertWho:
                case SkillEnum.BertZawodowiec:
                case SkillEnum.CheBert:
                case SkillEnum.EBerta:
                case SkillEnum.KuglarzBert:
                case SkillEnum.PrymusBert:
                case SkillEnum.SuperfanBert:
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
        private bool ApplyCharacterEffect(BoardCardCore target, BoardCardCore skillOwner, int delta = 0)
        {
            if (DoesPreventEffect(target, skillOwner)) return false;

            switch (skillOwner.BoardCard.GetSkill())
            {
                case SkillEnum.BertaGejsza:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvanceDexterity(-1, skillOwner);
                    else target.StatChange.AdvanceDexterity(-3, skillOwner);
                    break;
                case SkillEnum.BertaSJW:
                    target.StatChange.AdvancePower(-3, skillOwner);
                    break;
                case SkillEnum.BertaTrojanska:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvancePower(1, skillOwner);
                    else target.StatChange.AdvanceStrength(-1, skillOwner);
                    break;
                case SkillEnum.BertWho:
                    target.StatChange.AdvancePower(-1, skillOwner);
                    break;
                case SkillEnum.BertZawodowiec:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvancePower(1, skillOwner);
                    skillOwner.StatChange.AdvanceStrength(1, null);
                    break;
                case SkillEnum.CheBert:
                    if (!AreAllied(target, skillOwner)) StatusManager.Instance.RemoveStatusFromProvider(target.BoardCard);
                    else if (target.BoardCard.GetRole() == RoleEnum.Special && target != skillOwner) target.StatChange.AdvanceStrength(1, skillOwner);
                    break;
                case SkillEnum.EBerta:
                    if (!AreAllied(target, skillOwner)) return false;
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case SkillEnum.KonstablBert:
                    if (target.BoardCard.GetRole() != RoleEnum.Special) return false;
                    target.StatChange.AdvanceHealth(-1, skillOwner);
                    break;
                case SkillEnum.KowbojBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceDexterity(delta, skillOwner);
                    break;
                case SkillEnum.KuglarzBert:
                case SkillEnum.SuperfanBert:
                    if (AreAllied(target, skillOwner)) target.StatChange.AdvancePower(1, skillOwner);
                    else return false;
                    break;
                case SkillEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case SkillEnum.PapiezBertII:
                    if (AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvancePower(-2, skillOwner);
                    break;
                case SkillEnum.PrezydentBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceStrength(1, skillOwner);
                    break;
                case SkillEnum.PrymusBert:
                    target.StatChange.AdvancePower(3, skillOwner);
                    break;
                case SkillEnum.SamurajBert:
                    return ApplySamurajBertEffectAndResistance(skillOwner);
                case SkillEnum.ShaolinBert:
                    if (AreAllied(target, skillOwner)) return false;
                    target.StatChange.AdvanceStrength(-target.BoardCard.Stats.Power / 3, skillOwner);
                    break;
                case SkillEnum.ZalobnyBert:
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
            if (eBerta.BoardCard.GetSkill() != SkillEnum.EBerta)
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
            if (samurajBert.BoardCard.GetSkill() != SkillEnum.SamurajBert)
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
            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    return skillCard.BoardCard.GetRole() == RoleEnum.Special;
                case SkillEnum.BigMadB:
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