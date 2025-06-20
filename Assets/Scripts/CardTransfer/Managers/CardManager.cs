using Berty.CardTransfer.Entities;
using Berty.Entities;
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

namespace Berty.CardTransfer.Managers
{
    public class CardManager : ManagerSingleton<CardManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;
        public SelectionSystem SelectionSystem { get; private set; }

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
            SelectionSystem = new SelectionSystem();
        }

        public void PullCardsTo(int capacity)
        {
            Alignment align = game.CurrentAlignment;

            if (cardPile.PullCardsTo(capacity, align)) HandCardObjectManager.Instance.UpdateCardObjects();
            else TurnManager.Instance.EndTheGame();
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
