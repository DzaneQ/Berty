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

        public void AdvanceStrength(BoardCardCore card, int value)
        {
            
        }
    }
}