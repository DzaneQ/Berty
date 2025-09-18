using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class ModifyStatChangeManager : ManagerSingleton<ModifyStatChangeManager>
    {
        private BoardGrid grid;

        protected override void Awake()
        {
            grid = CoreManager.Instance.Game.Grid;
        }

        // NOTE: Ensure that BigMadB, PogromcaBert (and other prevention skill cards) have logic applied when new stat modifier is made

        // output: If true, prevent stat change
        public bool BeforeStrengthChange(BoardCardCore target, ref int value, BoardCardCore source)
        {
            bool shouldPreventStatChange = false;

            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaIdolka:
                    if (target != source) return true;
                    break;
            }

            return shouldPreventStatChange;
        }

        public bool BeforePowerChange(BoardCardCore target, ref int value, BoardCardCore source)
        {
            bool shouldPreventStatChange = false;

            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaIdolka:
                    if (!grid.AreAligned(target.BoardCard.OccupiedField, source.BoardCard.OccupiedField) && target.BoardCard.Stats.Power <= 3) return true;
                    break;
            }

            return shouldPreventStatChange;
        }

        public bool BeforeHealthChange(BoardCardCore target, ref int value, BoardCardCore source, bool isBasicAttack = false)
        {
            bool shouldPreventStatChange = false;

            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special && !isBasicAttack) return true;
                    break;
                case SkillEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support && !isBasicAttack) return true;
                    break;
                case SkillEnum.PrymusBert:
                    if (value < 0) value++;
                    break;
                case SkillEnum.ZalobnyBert:
                    if (value < 0 && source.BoardCard.GetRole() == RoleEnum.Offensive) return true;
                    break;
            }

            switch (source.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaSerferka:
                    if (isBasicAttack) return PreventDependingOnBertkaSerferkaPosition(target, source);
                    break;
                case SkillEnum.Bertolaj:
                    if (isBasicAttack && target.BoardCard.Stats.Power > 3) return true;
                    break;
            }

            return shouldPreventStatChange;
        }

        // NOTE: After<stat>Change is executed during stat change animation

        public void AfterPowerChange(BoardCardCore target, int value, BoardCardCore source)
        {
            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertkaIdolka:
                    target.StatChange.SetStrength(target.BoardCard.Stats.Power, target);
                    break;
                case SkillEnum.Bertolaj:
                    if (target.BoardCard.Stats.Power <= 0) 
                        StatusManager.Instance.IncrementChargedStatusWithAlignment(StatusEnum.ExtraCardNextTurn, source.BoardCard.Align, 1);
                    break;
            }
        }

        public void AfterHealthChange(BoardCardCore target, int value, BoardCardCore source)
        {
            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special) return;
                    break;
                case SkillEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support) return;
                    break;
                case SkillEnum.KrzyzowiecBert:
                    if (value < 0) target.StatChange.AdvanceStrength(-value, null);
                    break;
                case SkillEnum.Tankbert:
                    if (value < 0)
                    {
                        target.StatChange.AdvanceStrength(value, null);
                        target.StatChange.AdvanceDexterity(-value, null);
                    }
                    break;
                case SkillEnum.ZalobnyBert:
                    if (value < 0) EventManager.Instance.RaiseOnValueChange(target, value);
                    break;
            }

            if (source == null) return;

            switch (source.BoardCard.GetSkill())
            {
                case SkillEnum.Zombert:
                    if (target.BoardCard.Align == source.BoardCard.Align) break;
                    if (value < 0) target.StatChange.AdvancePower(-1, source);
                    break;
            }
        }

        public int GetModifiedStrengthForAttack(BoardCardCore target, BoardCardCore source)
        {
            int strength = source.BoardCard.Stats.Strength;

            switch (target.BoardCard.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special) return strength;
                    break;
                case SkillEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support) return strength;
                    break;
            }    

            switch (source.BoardCard.GetSkill())
            {
                case SkillEnum.BertPogromca:
                    if (target.BoardCard.GetRole() == RoleEnum.Special) strength = strength + 2;
                    break;
                case SkillEnum.KonstablBert:
                    RoleEnum targetRole = target.BoardCard.GetRole();
                    RoleEnum[] vulnerableRoles = { RoleEnum.Special, RoleEnum.Support };
                    if (vulnerableRoles.Contains(targetRole))
                        strength++;
                    break;
                case SkillEnum.StaryBert:
                    if (target.BoardCard.Stats.Health < target.BoardCard.CharacterConfig.Health)
                        strength = strength + 2;
                    break;
            }

            return strength;
        }

        private bool PreventDependingOnBertkaSerferkaPosition(BoardCardCore target, BoardCardCore bertkaSerferka)
        {
            if (bertkaSerferka.BoardCard.GetSkill() != SkillEnum.BertkaSerferka)
                throw new Exception($"BertkaSerferka effect is casted by {bertkaSerferka.BoardCard.CharacterConfig.Name}");
            Vector2Int distance = bertkaSerferka.BoardCard.GetDistanceTo(target.BoardCard);
            if (distance.y < 2) return false; // Allow riposte attack
            switch (distance.x)
            {
                case -1:
                    return bertkaSerferka.BoardCard.Stats.Strength < target.BoardCard.Stats.Strength;
                case 0:
                    return bertkaSerferka.BoardCard.Stats.Power < target.BoardCard.Stats.Power;
                case 1:
                    return bertkaSerferka.BoardCard.Stats.Dexterity < target.BoardCard.Stats.Dexterity;
                default:
                    throw new Exception($"Undefined BertkaSerferka's distance to target: {distance.x}, {distance.y}");
            }
        }    
    }
}