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
using Berty.Debugging;
using Berty.BoardCards.ConfigData;

namespace Berty.UI.Card.Managers
{
    public class PileToHandManager : ManagerSingleton<PileToHandManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
        }

        public void PullCardsTo(int capacity)
        {
            AlignmentEnum align = game.CurrentAlignment;
            DebugManager.Instance?.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (cardPile.PullCardsTo(capacity, align)) HandCardObjectManager.Instance.AddCardObjects();
            else TurnManager.Instance.EndTheGame();
        }
    }
}
