using Berty.UI.Card.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.ConfigData;

namespace Berty.UI.Card.Managers
{
    public class FieldToHandManager : ManagerSingleton<FieldToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;
        private SelectionAndPaymentSystem selectionSystem;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
            selectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
        }

        public void RetrieveCardOnHold()
        {
            AlignmentEnum align = game.CurrentAlignment;
            CharacterConfig cardOnHold = selectionSystem.GetCardOnHoldOrThrow();
            cardPile.RetrieveCard(cardOnHold, align);
            HandCardObjectManager.Instance.AddCardObjectFromConfigForTable(cardOnHold, align);
        }
    }
}
