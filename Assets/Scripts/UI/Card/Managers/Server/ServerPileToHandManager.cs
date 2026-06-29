using Berty.BoardCards.ConfigData;
using Berty.Debugging.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Network.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Berty.UI.Card.Managers.Server
{
    public class ServerPileToHandManager : ServerManagerSingleton<ServerPileToHandManager>, IPileToHandManager
    {
        private Game game;
        private CardPile CardPile => game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            game = EntityLoadManager.Instance.Game;
        }

        public void PullCards()
        {
            int capacity = game.GameConfig.TableCapacity;
            AlignmentEnum align = ManagerLocator.TurnManagerInstance.CurrentAlignment;
            Debug.Log($"Pulling cards for {align}");
            DebugManager instance = DebugManager.Instance;
            if (instance != null) instance.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (CardPile.PullCardsTo(capacity, align)) AddCardObjectsToAlignment(align); // BUG: Second client receives the first client's batch of cards
            else ManagerLocator.TurnManagerInstance.EndTheGame();
        }

        private CharacterEnum[] GetPlayerCardsAsCharacterNames(AlignmentEnum align)
        {
            List<CharacterConfig> playerCards = align switch
            {
                AlignmentEnum.Player => CardPile.PlayerCards,
                AlignmentEnum.Opponent => CardPile.OpponentCards,
                _ => throw new ArgumentException($"Unknown argument to get cards: {align}"),
            };
            Debug.Log($"Player has {playerCards.Count} cards. First card is: {playerCards[0].CharacterName}. Second card is: {playerCards[1].CharacterName}.");
            return playerCards.ConvertAll(card => card.CharacterName).ToArray();
        }

        private void AddCardObjectsToAlignment(AlignmentEnum align)
        {
            ulong targetClientId = PlayerReadManager.Instance.GetClientIdFromAlignment(align);
            CharacterEnum[] playerCards = GetPlayerCardsAsCharacterNames(align);
            ClientRpcParams sendRpcParam = new()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { targetClientId }
                }
            };
            NetworkCardManager.Instance.AddCardObjectsClientRpc(playerCards, sendRpcParam);
        }
    }
}