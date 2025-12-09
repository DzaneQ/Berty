using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using System.Linq;

namespace Berty.Characters.Managers
{
    public class ApplySkillEffectManager : ManagerSingleton<ApplySkillEffectManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = EntityLoadManager.Instance.Game;
        }

        public void HandleNeighborCharacterSkill(BoardCardBehaviour target, BoardCardBehaviour skillOwner, int delta = 0)
        {
            if (!game.Grid.AreNeighboring(target.ParentField.BoardField, skillOwner.ParentField.BoardField)) return;
            HandleCharacterSkill(target, skillOwner, delta);         
        }

        public void HandleCharacterSkill(BoardCardBehaviour target, BoardCardBehaviour skillOwner, int delta = 0)
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
        private bool ApplyCharacterEffect(BoardCardBehaviour target, BoardCardBehaviour skillOwner, int delta = 0)
        {
            if (DoesPreventEffect(target.BoardCard, skillOwner.BoardCard)) return false;

            switch (skillOwner.BoardCard.GetSkill())
            {
                case SkillEnum.BertaGejsza:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvanceDexterity(-1, skillOwner);
                    else target.EntityHandler.AdvanceDexterity(-3, skillOwner);
                    break;
                case SkillEnum.BertaSJW:
                    target.EntityHandler.AdvancePower(-3, skillOwner);
                    break;
                case SkillEnum.BertaTrojanska:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    else target.EntityHandler.AdvanceStrength(-1, skillOwner);
                    break;
                case SkillEnum.BertVentura:
                    StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, skillOwner.BoardCard, game.Grid.GetEnemyNeighborCount(skillOwner.BoardCard));
                    break;
                case SkillEnum.BertWho:
                    target.EntityHandler.AdvancePower(-1, skillOwner);
                    break;
                case SkillEnum.BertZawodowiec:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    skillOwner.EntityHandler.AdvanceStrength(1, null);
                    break;
                case SkillEnum.CheBert:
                    if (AreAllied(target, skillOwner) && target.BoardCard.GetRole() == RoleEnum.Special && !target.IsEqualTo(skillOwner))
                        target.EntityHandler.AdvanceStrength(1, skillOwner);
                    break;
                case SkillEnum.EBerta:
                    if (!AreAllied(target, skillOwner)) return false;
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case SkillEnum.KonstablBert:
                    if (target.BoardCard.GetRole() != RoleEnum.Special) return false;
                    target.EntityHandler.AdvanceHealth(-1, skillOwner);
                    break;
                case SkillEnum.KowbojBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceDexterity(delta, skillOwner);
                    break;
                case SkillEnum.KuglarzBert:
                case SkillEnum.SuperfanBert:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    else return false;
                    break;
                case SkillEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case SkillEnum.PapiezBertII:
                    if (AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvancePower(-2, skillOwner);
                    break;
                case SkillEnum.PrezydentBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceStrength(1, skillOwner);
                    break;
                case SkillEnum.PrymusBert:
                    target.EntityHandler.AdvancePower(3, skillOwner);
                    break;
                case SkillEnum.SamurajBert:
                    return ApplySamurajBertEffectAndResistance(skillOwner);
                case SkillEnum.ShaolinBert:
                    if (AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceStrength(-target.BoardCard.Stats.Power / 3, skillOwner);
                    break;
                case SkillEnum.ZalobnyBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceHealth(-2 * delta, skillOwner);
                    break;
                default:
                    throw new Exception($"Applying unknown effect for {target.name} from {skillOwner.name}");
                }
            return true;
        }

        private void ApplyEBertaEffect(BoardCardBehaviour target, BoardCardBehaviour eBerta)
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
            if (stats[0] == minStat) target.EntityHandler.AdvanceStrength(1, eBerta);
            if (stats[1] == minStat) target.EntityHandler.AdvancePower(1, eBerta);
            if (stats[2] == minStat) target.EntityHandler.AdvanceDexterity(1, eBerta);
            if (stats[3] == minStat) target.EntityHandler.AdvanceHealth(1, eBerta);
        }

        private bool ApplySamurajBertEffectAndResistance(BoardCardBehaviour samurajBert)
        {
            if (samurajBert.BoardCard.GetSkill() != SkillEnum.SamurajBert)
                throw new Exception($"SamurajBert effect is casted by {samurajBert.BoardCard.CharacterConfig.Name}");
            if (samurajBert.BoardCard.IsResistantTo(samurajBert.BoardCard)) return false;
            if (game.Grid.GetAllNeighbors(samurajBert.BoardCard).Count < 3) return false;
            samurajBert.EntityHandler.AdvanceDexterity(1, samurajBert);
            samurajBert.EntityHandler.AdvancePower(1, samurajBert);
            samurajBert.BoardCard.AddResistanceToCharacter(samurajBert.BoardCard.CharacterConfig);
            return true;
        }

        public bool DoesPreventEffect(BoardCard target, BoardCard skillCard)
        {
            switch (target.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    return skillCard.GetRole() == RoleEnum.Special;
                case SkillEnum.BigMadB:
                    return skillCard.GetRole() == RoleEnum.Support;
                default:
                    return false;
            }
        }

        private bool AreAllied(BoardCardBehaviour firstCard, BoardCardBehaviour secondCard)
        {
            return game.Grid.AreAligned(firstCard.ParentField.BoardField, secondCard.ParentField.BoardField);
        }
    }
}