using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Network.Managers
{
    public class NetworkCardManager : RpcManagerSingleton<NetworkCardManager>
    {
        private List<CharacterConfig> myHandCards;
        private IReadOnlyList<CharacterConfig> allCards;
        public IReadOnlyList<CharacterConfig> MyHandCards => myHandCards;

        private void Start()
        {
            CardPile pile = EntityLoadManager.Instance.Game.CardPile;
            allCards = pile.GetAllCharactersOutsideField(); // NOTE: Expected behavior that no cards are on the field, otherwise there might be missing cards
        }

        public CharacterConfig GetConfigFromCharacterName(CharacterEnum name)
        {
            return allCards.First(card => card.CharacterName == name);
        }

        public void RetrieveCard(CharacterConfig card)
        {
            if (myHandCards.Contains(card)) throw new Exception($"Card {card.Name} is already in the hand.");
            myHandCards.Add(card);
        }

        public void RemoveCardFromMyHandOrThrow(CharacterConfig cardToRemove)
        {
            cardToRemove = myHandCards.First(card => card.CharacterName == cardToRemove.CharacterName); // NOTE: These are not the same config, it needs overwritten
            if (!myHandCards.Remove(cardToRemove)) throw new InvalidOperationException("Cannot remove card from hand: " + cardToRemove.Name);
        }

        private void UpdatePlayerHandCards(CharacterEnum[] cardNames)
        {
            myHandCards = cardNames.Select(name => allCards.First(card => card.CharacterName == name)).Where(card => card != null).ToList();
        }

        [ClientRpc]
        public void AddCardObjectsClientRpc(CharacterEnum[] cardNames, ClientRpcParams rpcParams)
        {
            // NOTE: This rpc is executed before TurnManagerInstance updates the alignment so it'll throw an error
            //if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) throw new InvalidOperationException($"Client attempted to add card objects when it is not their turn.");
            UpdatePlayerHandCards(cardNames);
            ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
        }
    }
}
