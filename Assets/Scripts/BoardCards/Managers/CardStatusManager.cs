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
    public class CardStatusManager : ManagerSingleton<CardStatusManager>
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public void DisableAttack(BoardCardCore card)
        {
            if (card.BoardCard == null) return;
            card.BoardCard.MarkAsHasAttacked();
        }
    }
}