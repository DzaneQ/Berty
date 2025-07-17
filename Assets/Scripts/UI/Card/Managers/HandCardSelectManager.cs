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
        private SelectionAndPaymentSystem selectionSystem;

        protected override void Awake()
        {
            base.Awake();
            selectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
        }

        public void ChangeSelection(HandCardBehaviour card)
        {
            if (selectionSystem.IsSelected(card.Character))
            {
                UnselectCard(card);
            }
            else if (selectionSystem.CanSelectCard())
            {
                selectionSystem.SelectCard(card.Character);
                card.ShowObjectAsSelected();
            }
        }

        public void UnselectCard(HandCardBehaviour card)
        {
            if (!selectionSystem.IsSelected(card.Character)) return;
            selectionSystem.UnselectCard(card.Character);
            card.ShowObjectAsUnselected();
        }

        public void ClearSelection()
        {
            selectionSystem.ClearSelection();
        }
    }
}
