using Berty.BoardCards.Bar;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.State;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using static UnityEngine.CullingGroup;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardStatChange : MonoBehaviour
    {
        private BoardCardCore core;
        private BoardCard Card => core.BoardCard;
        private BoardCardBarsObjects Bars => core.Bars;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            
        }

        public void AdvanceStrength(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(core, ref value, source)) return;
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
            if (ModifyStatChangeManager.Instance.BeforeStrengthChange(core, ref value, source)) return;
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
            if (ModifyStatChangeManager.Instance.BeforePowerChange(core, ref value, source)) return;
            Card.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(core, value, source);
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
            if (ModifyStatChangeManager.Instance.BeforePowerChange(core, ref value, source)) return;
            Card.AdvanceTempPower(value);
            Bars.UpdateBar(StatEnum.Power);
            ModifyStatChangeManager.Instance.AfterPowerChange(core, value, source);
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
            if (ModifyStatChangeManager.Instance.BeforeHealthChange(core, ref value, source, isBasicAttack)) return;
            if (isBasicAttack) source.MarkSuccessfulAttack(core);
            Card.AdvanceHealth(value);
            Bars.UpdateBar(StatEnum.Health);
            ModifyStatChangeManager.Instance.AfterHealthChange(core, value, source);
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
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.AstronautaBert:
                    core.KillCard();
                    break;
                case SkillEnum.KsiezniczkaBerta:
                    SetPower(core.BoardCard.CharacterConfig.Power, core);
                    break;
                default:
                    core.SwitchSides();
                    break;
            }
        }

        private void HandleZeroDexterity()
        {
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    core.KillCard();
                    break;
                default:
                    Card.MarkAsTired();
                    break;
            }
        }

        private void HandleZeroHealth()
        {
            switch (core.BoardCard.GetSkill())
            {
                case SkillEnum.BertWick:
                    AdvanceHealth(2, null);
                    AdvancePower(1, null);
                    AdvanceStrength(1, null);
                    AdvanceDexterity(-1, null);
                    break;
                case SkillEnum.KrolPopuBert:
                    core.UpdateCardWithRandomKid();
                    break;
                default:
                    core.KillCard();
                    break;
            }
        }
    }
}