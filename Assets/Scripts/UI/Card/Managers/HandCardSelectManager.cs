using Berty.BoardCards.ConfigData;
using Berty.CardTransfer.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Systems;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Card.Managers
{
    public class HandCardSelectManager : UIObjectManager<HandCardSelectManager>
    {
        public SelectionSystem SelectionSystem { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            SelectionSystem = new SelectionSystem();
        }

        public void ChangeSelection(HandCardBehaviour card)
        {
            if (SelectionSystem.IsSelected(card.Character))
            {
                UnselectCard(card);
            }
            else if (SelectionSystem.CanSelectCard())
            {
                SelectionSystem.SelectCard(card.Character);
                card.ShowObjectAsSelected();
            }
        }

        public void UnselectCard(HandCardBehaviour card)
        {
            if (!SelectionSystem.IsSelected(card.Character)) return;
            SelectionSystem.UnselectCard(card.Character);
            card.ShowObjectAsUnselected();
        }
    }
}
