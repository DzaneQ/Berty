using Berty.BoardCards.Bar;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.State;
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

        public void AdvanceStrength(int value)
        {
            if (Card == null) return;
            Card.AdvanceStrength(value);
            Bars.UpdateBar(StatEnum.Strength);
        }

        public void AdvancePower(int value)
        {
            if (Card == null) return;
            Card.AdvancePower(value);
            Bars.UpdateBar(StatEnum.Power);
        }

        public void AdvanceDexterity(int value)
        {
            if (Card == null) return;
            Card.AdvanceDexterity(value);
            Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(int value)
        {
            if (Card == null) return;
            Card.AdvanceHealth(value);
            Bars.UpdateBar(StatEnum.Health);
        }

        public void ProgressTemporaryStats()
        {
            if (Card.Stats.AreTempStatZeros()) return;
            Card.Stats.ProgressTempStats();
            Bars.UpdateBars();
        }
    }
}