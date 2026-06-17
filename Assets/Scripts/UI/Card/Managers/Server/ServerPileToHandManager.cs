using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using Berty.Debugging.Managers;
using Berty.Gameplay.Managers.Client;
using Berty.Network.Managers;
using Berty.BoardCards.ConfigData;
using System;
using System.Collections.Generic;

namespace Berty.UI.Card.Managers.Client
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
            AlignmentEnum align = game.CurrentAlignment; // BUG: Alignment is not correctly changed on turn end, causing cards not to be pulled on the other client
            DebugManager instance = DebugManager.Instance;
            if (instance != null) instance.TakeCardIfInPile(align);
            Status extraCardStatus = game.GetStatusByNameAndAlignmentOrNull(StatusEnum.ExtraCardNextTurn, align);
            if (extraCardStatus != null)
            {
                capacity += extraCardStatus.Charges;
                StatusManager.Instance.RemoveStatus(extraCardStatus);
            }
            if (CardPile.PullCardsTo(capacity, align)) NetworkCardPileManager.Instance.AddCardObjectsClientRpc(GetPlayerCardsAsCharacterNames(align));
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

            return playerCards.ConvertAll(card => card.CharacterName).ToArray();
        }
    }
}