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
            Card.AdvanceStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void AdvancePower(int value, BoardCardCore source)
        {
            if (Card == null) return;
            Card.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
        }

        public void SetPower(int value)
        {
            if (Card == null) return;
            Card.SetPower(value);
            Bars.UpdateBar(StatEnum.Power);
        }

        public void AdvanceDexterity(int value, BoardCardCore source)
        {
            if (Card == null) return;
            Debug.Log($"Advancing dexterity for {Card.CharacterConfig.Name} of value {value}");
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
            switch (core.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.AstronautaBert:
                    core.KillCard();
                    break;
                default:
                    core.SwitchSides();
                    break;
            }
        }

        private void HandleZeroDexterity()
        {
            switch (core.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertWick:
                    core.KillCard();
                    break;
                default:
                    Card.MarkAsTired();
                    break;
            }
        }

        private void HandleZeroHealth()
        {
            switch (core.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertWick:
                    AdvanceHealth(2, null);
                    AdvancePower(1, null);
                    AdvanceStrength(1, null);
                    AdvanceDexterity(-1, null);
                    break;
                default:
                    core.KillCard();
                    break;
            }
        }
    }
}