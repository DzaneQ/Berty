using Berty.BoardCards.Entities;
using Berty.Characters.Managers;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardStatChange : BoardCardBehaviour
    {
        public void AdvanceStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(Core, ref value, source)) return;
            BoardCard.AdvanceStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.Strength;
            AdvanceStrength(delta, source);
        }

        public void AdvanceTempStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(Core, ref value, source)) return;
            BoardCard.AdvanceTempStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetTempStrength(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.TempStrength;
            AdvanceTempStrength(delta, source);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void AdvancePower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(Core, ref value, source)) return;
            BoardCard.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(Core, value, source);
        }

        public void SetPower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            int delta = value - BoardCard.Stats.Power;
            AdvancePower(delta, source);
        }

        public void AdvanceTempPower(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(Core, ref value, source)) return;
            BoardCard.AdvanceTempPower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(Core, value, source);
        }

        public void AdvanceDexterity(int value, BoardCardBehaviour source)
        {
            if (BoardCard == null) return;
            BoardCard.AdvanceDexterity(value);
            Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(int value, BoardCardBehaviour source, bool isBasicAttack = false)
        {
            if (BoardCard == null) return;
            if (ModifyStatChangeManager.Instance.BeforeHealthChange(Core, ref value, source, isBasicAttack)) return;
            if (isBasicAttack) source.Core.MarkSuccessfulAttack(Core);
            BoardCard.AdvanceHealth(value);
            Bars.UpdateBar(StatEnum.Health);
            ModifyStatChangeManager.Instance.AfterHealthChange(Core, value, source);
        }

        public void ProgressTemporaryStats()
        {
            if (BoardCard.Stats.AreTempStatZeros()) return;
            BoardCard.Stats.ProgressTempStats();
            Bars.UpdateBars();
        }

        public void HandleAfterAnimationStatChange()
        {
            if (BoardCard.Stats.Health <= 0)
            {
                HandleZeroHealth();
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Dexterity <= 0)
            {
                HandleZeroDexterity(); 
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Dexterity >= BoardCard.CharacterConfig.Dexterity)
            {
                BoardCard.MarkAsRested();
            }
            if (BoardCard == null) return;
            if (BoardCard.Stats.Power <= 0)
            {
                HandleZeroPower();
            }    
        }

        private void HandleZeroPower()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.AstronautaBert:
                    Core.KillCard();
                    break;
                case SkillEnum.KsiezniczkaBerta:
                    SetPower(BoardCard.CharacterConfig.Power, Core);
                    break;
                default:
                    Core.SwitchSides();
                    break;
            }
        }

        private void HandleZeroDexterity()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    Core.KillCard();
                    break;
                default:
                    BoardCard.MarkAsTired();
                    break;
            }
        }

        private void HandleZeroHealth()
        {
            switch (BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    AdvanceHealth(2, null);
                    AdvancePower(1, null);
                    AdvanceStrength(1, null);
                    AdvanceDexterity(-1, null);
                    break;
                case SkillEnum.KrolPopuBert:
                    Core.UpdateCardWithRandomKid();
                    break;
                default:
                    Core.KillCard();
                    break;
            }
        }
    }
}