using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkCardPileManager : SharedManagerSingleton<NetworkCardPileManager>
    {
        private List<CharacterConfig> myHandCards;
        private IReadOnlyList<CharacterConfig> allCards;
        public IReadOnlyList<CharacterConfig> MyTable => myHandCards;

        private void Start()
        {
            CardPile pile = EntityLoadManager.Instance.Game.CardPile;
            allCards = pile.GetAllCharactersOutsideField(); // NOTE: Expected behavior that no cards are on the field, otherwise there might be missing cards
        }

        private void UpdatePlayerHandCards(CharacterEnum[] cardNames)
        {
            myHandCards = cardNames.Select(name => allCards.First(card => card.CharacterName == name)).Where(card => card != null).ToList();
        }

        [ClientRpc]
        public void AddCardObjectsClientRpc(CharacterEnum[] cardNames)
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            UpdatePlayerHandCards(cardNames);
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
        }
    }
}
