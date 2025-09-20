using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.ConfigData;
using Berty.Grid.Field.Behaviour;
using Berty.BoardCards.State;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Managers;

namespace Berty.UI.Card.Managers
{
    public class HandCardActionManager : ManagerSingleton<HandCardActionManager>
    {
        private Game Game { get; set; }
        private CardPile CardPile => Game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = CoreManager.Instance.Game;
        }

        public void ReviveCard(HandCardBehaviour cardObject)
        {
            Status revival = Game.GetStatusByNameOrThrow(StatusEnum.RevivalSelect);
            CardPile.ReviveCard(cardObject.Character, revival.GetAlign());
            HandCardObjectManager.Instance.AddCardObjects();
            StatusManager.Instance.RemoveStatus(revival);
        }
    }
}
