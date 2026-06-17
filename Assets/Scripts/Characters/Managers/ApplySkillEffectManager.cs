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
                case CharacterEnum.BertaGejsza:
                case CharacterEnum.BertaSJW:
                case CharacterEnum.BertaTrojanska:
                case CharacterEnum.BertWho:
                case CharacterEnum.BertZawodowiec:
                case CharacterEnum.CheBert:
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
        private bool ApplyCharacterEffect(BoardCardBehaviour target, BoardCardBehaviour skillOwner, int delta = 0)
        {
            if (DoesPreventEffect(target.BoardCard, skillOwner.BoardCard)) return false;

            switch (skillOwner.BoardCard.GetSkill())
            {
                case CharacterEnum.BertaGejsza:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvanceDexterity(-1, skillOwner);
                    else target.EntityHandler.AdvanceDexterity(-3, skillOwner);
                    break;
                case CharacterEnum.BertaSJW:
                    target.EntityHandler.AdvancePower(-3, skillOwner);
                    break;
                case CharacterEnum.BertaTrojanska:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    else target.EntityHandler.AdvanceStrength(-1, skillOwner);
                    break;
                case CharacterEnum.BertVentura:
                    StatusManager.Instance.SetChargedStatusWithProvider(StatusEnum.Ventura, skillOwner.BoardCard, game.Grid.GetEnemyNeighborCount(skillOwner.BoardCard));
                    break;
                case CharacterEnum.BertWho:
                    target.EntityHandler.AdvancePower(-1, skillOwner);
                    break;
                case CharacterEnum.BertZawodowiec:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    skillOwner.EntityHandler.AdvanceStrength(1, null);
                    break;
                case CharacterEnum.CheBert:
                    if (AreAllied(target, skillOwner) && target.BoardCard.GetRole() == RoleEnum.Special && !target.IsEqualTo(skillOwner))
                        target.EntityHandler.AdvanceStrength(1, skillOwner);
                    break;
                case CharacterEnum.EBerta:
                    if (!AreAllied(target, skillOwner)) return false;
                    ApplyEBertaEffect(target, skillOwner);
                    break;
                case CharacterEnum.KonstablBert:
                    if (target.BoardCard.GetRole() != RoleEnum.Special) return false;
                    target.EntityHandler.AdvanceHealth(-1, skillOwner);
                    break;
                case CharacterEnum.KowbojBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceDexterity(delta, skillOwner);
                    break;
                case CharacterEnum.KuglarzBert:
                case CharacterEnum.SuperfanBert:
                    if (AreAllied(target, skillOwner)) target.EntityHandler.AdvancePower(1, skillOwner);
                    else return false;
                    break;
                case CharacterEnum.MisiekBert:
                    CardNavigationManager.Instance.RotateCard(target, 270);
                    break;
                case CharacterEnum.PapiezBertII:
                    if (AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvancePower(-2, skillOwner);
                    break;
                case CharacterEnum.PrezydentBert:
                    if (!AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceStrength(1, skillOwner);
                    break;
                case CharacterEnum.PrymusBert:
                    target.EntityHandler.AdvancePower(3, skillOwner);
                    break;
                case CharacterEnum.SamurajBert:
                    return ApplySamurajBertEffectAndResistance(skillOwner);
                case CharacterEnum.ShaolinBert:
                    if (AreAllied(target, skillOwner)) return false;
                    target.EntityHandler.AdvanceStrength(-target.BoardCard.Stats.Power / 3, skillOwner);
                    break;
                case CharacterEnum.ZalobnyBert:
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
            if (eBerta.BoardCard.GetSkill() != CharacterEnum.EBerta)
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
            if (samurajBert.BoardCard.GetSkill() != CharacterEnum.SamurajBert)
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
                case CharacterEnum.BertPogromca:
                    return skillCard.GetRole() == RoleEnum.Special;
                case CharacterEnum.BigMadB:
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