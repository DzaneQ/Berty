using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class StatChangeManager : ManagerSingleton<StatChangeManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void AdvanceDexterity(BoardCardCore card, int value)
        {
            if (card.BoardCard == null) return;
            card.BoardCard.AdvanceDexterity(value);
            card.Bars.UpdateBar(StatEnum.Dexterity);
        }

        public void AdvanceHealth(BoardCardCore card, int value)
        {
            if (card.BoardCard == null) return;
            card.BoardCard.AdvanceHealth(value);
            card.Bars.UpdateBar(StatEnum.Health);
        }

        public void ProgressTemporaryStats(BoardCardCore card)
        {
            if (card.BoardCard.Stats.AreTempStatZeros()) return;
            card.BoardCard.Stats.ProgressTempStats();
            card.Bars.UpdateBars();
        }
    }
}