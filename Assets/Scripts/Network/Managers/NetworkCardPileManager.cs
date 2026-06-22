using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Managers;
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
        public IReadOnlyList<CharacterConfig> MyHandCards => myHandCards;

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
        public void AddCardObjectsClientRpc(CharacterEnum[] cardNames, ClientRpcParams rpcParams)
        {
            Debug.Log($"Attempted to add card objects when turn is on {NetworkTurnManager.Instance.CurrentAlignment}.");
            // NOTE: The code below causes the other client to throw on the second turn. Race condition?
            //if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) throw new InvalidOperationException($"Client attempted to add card objects when it is not their turn.");
            Debug.Log($"Adding card objects for {cardNames.Length} cards. First card is: {cardNames[0]}. Second card is: {cardNames[1]}.");
            UpdatePlayerHandCards(cardNames);
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
        }
    }
}
