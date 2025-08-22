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
            Card.AdvanceDexterity(value);
            Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(int value, BoardCardCore source)
        {
            if (Card == null) return;
            if (ModifyStatChangeManager.Instance.BeforeHealthChange(core, ref value, source)) return;
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
                core.KillCard();
                return;
            }
            if (Card.Stats.Dexterity <= 0)
            {
                Card.MarkAsTired(); 
            }
            if (Card.Stats.Dexterity >= Card.CharacterConfig.Dexterity)
            {
                Card.MarkAsRested();
            }
            if (Card.Stats.Power <= 0)
            {
                core.SwitchSides();
            }    
        }
    }
}