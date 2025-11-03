using Berty.BoardCards.Entities;
using Berty.Characters.Managers;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardStatChange : BoardCardBehaviour
    {
        private BoardCard Card => Core.BoardCard;

        public void AdvanceStrength(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(Core, ref value, source)) return;
            Card.AdvanceStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetStrength(int value, BoardCardCore source)
        {
            if (Card == null) return;
            int delta = value - Card.Stats.Strength;
            AdvanceStrength(delta, source);
        }

        public void AdvanceTempStrength(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(Core, ref value, source)) return;
            Card.AdvanceTempStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void SetTempStrength(int value, BoardCardCore source)
        {
            if (Card == null) return;
            int delta = value - Card.Stats.TempStrength;
            AdvanceTempStrength(delta, source);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void AdvancePower(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(Core, ref value, source)) return;
            Card.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(Core, value, source);
        }

        public void SetPower(int value, BoardCardCore source)
        {
            if (Card == null) return;
            int delta = value - Card.Stats.Power;
            AdvancePower(delta, source);
        }

        public void AdvanceTempPower(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforePowerChange(Core, ref value, source)) return;
            Card.AdvanceTempPower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(Core, value, source);
        }

        public void AdvanceDexterity(int value, BoardCardCore source)
        {
            if (Card == null) return;
            Card.AdvanceDexterity(value);
            Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(int value, BoardCardCore source, bool isBasicAttack = false)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforeHealthChange(Core, ref value, source, isBasicAttack)) return;
            if (isBasicAttack) source.MarkSuccessfulAttack(Core);
            Card.AdvanceHealth(value);
            Bars.UpdateBar(StatEnum.Health);
            ModifyStatChangeManager.Instance.AfterHealthChange(Core, value, source);
        }

        public void ProgressTemporaryStats()
        {
            if (Card.Stats.AreTempStatZeros()) return;
            Card.Stats.ProgressTempStats();
            Bars.UpdateBars();
        }

        public void HandleAfterAnimationStatChange()
        {
            if (Card.Stats.Health <= 0)
            {
                HandleZeroHealth();
            }
            if (Card == null) return;
            if (Card.Stats.Dexterity <= 0)
            {
                HandleZeroDexterity(); 
            }
            if (Card == null) return;
            if (Card.Stats.Dexterity >= Card.CharacterConfig.Dexterity)
            {
                Card.MarkAsRested();
            }
            if (Card == null) return;
            if (Card.Stats.Power <= 0)
            {
                HandleZeroPower();
            }    
        }

        private void HandleZeroPower()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.AstronautaBert:
                    Core.KillCard();
                    break;
                case SkillEnum.KsiezniczkaBerta:
                    SetPower(Core.BoardCard.CharacterConfig.Power, Core);
                    break;
                default:
                    Core.SwitchSides();
                    break;
            }
        }

        private void HandleZeroDexterity()
        {
            switch (Core.BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    Core.KillCard();
                    break;
                default:
                    Card.MarkAsTired();
                    break;
            }
        }

        private void HandleZeroHealth()
        {
            switch (Core.BoardCard.GetSkill())
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