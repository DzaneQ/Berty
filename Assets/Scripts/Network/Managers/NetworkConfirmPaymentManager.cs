using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Berty.Network.Managers
{
    public class NetworkConfirmPaymentManager : RpcManagerSingleton<NetworkConfirmPaymentManager>, IConfirmPaymentManager
    {
        private Game Game { get; set; }
        private CardPile CardPile => Game.CardPile;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        public void ConfirmPayment(BoardCardBehaviour card)
        {
            if (!SelectionManager.Instance.CheckOffer()) return;
            CharacterEnum[] selectedCardNames = SelectionManager.Instance.SelectedCards.Select(card => card.CharacterName).ToArray();
            BoardCardNetworkData cardFocus = new()
            {
                CharacterName = card.BoardCard.CharacterConfig.CharacterName,
                FieldCoords = card.BoardCard.OccupiedField.Coordinates,
                Direction = card.BoardCard.Direction,
                Alignment = card.BoardCard.Align
            };
            ProcessPaymentServerRpc(selectedCardNames, cardFocus);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ProcessPaymentServerRpc(CharacterEnum[] selectedCardNames, BoardCardNetworkData cardFocus, ServerRpcParams rpcParams = default)
        {
            if (!IsServer) throw new InvalidOperationException("Discarding cards should be processed in server.");
            ulong sourceClientId = rpcParams.Receive.SenderClientId;
            AlignmentEnum align = cardFocus.Alignment;
            if (PlayerReadManager.Instance.GetClientIdFromAlignment(align) != sourceClientId) throw new InvalidOperationException($"Align {align} does not belong to the client.");
            bool isSentByHost = ServerIsHost && sourceClientId == NetworkManager.Singleton.LocalClientId;
            IReadOnlyList<CharacterConfig> playerCards = CardPile.GetCardsFromAlign(align);
            IReadOnlyList<CharacterConfig> selectedCards = playerCards.Where(card => selectedCardNames.Contains(card.CharacterName)).ToList();
            CardStateEnum cardFocusState = GetStateOnCardFocus(cardFocus, isSentByHost);
            Debug.Log("Server reads card focus state: " + cardFocusState);
            if (cardFocusState == CardStateEnum.NewCard)
            {
                CharacterConfig cardFocusInHands = playerCards.First(card => card.CharacterName == cardFocus.CharacterName);
                CardPile.LeaveCard(cardFocusInHands, align);
            }
            CardPile.DiscardCards(selectedCards, align);
            FinishPaymentClientRpc(cardFocus, CardPile.GetCharacterNamesFromAlignedTable(align), sourceClientId);
            if (!isSentByHost && cardFocusState == CardStateEnum.NewCard) InstantiateBoardCardEntity(cardFocus); // NOTE: Experimental
        }

        // TODO: It applies only for new card. Other payment related actions should be adjusted.
        [ClientRpc]
        public void FinishPaymentClientRpc(BoardCardNetworkData cardFocus, CharacterEnum[] playerTable, ulong sourceClientId)
        {
            if (NetworkManager.Singleton.LocalClientId == sourceClientId)
            {
                NetworkCardManager.Instance.UpdatePlayerHandCards(playerTable);
                ManagerLocator.HandCardObjectManagerInstance.RemoveCardObjects();
                HandCardSelectManager.Instance.ClearSelection();
                SelectionManager.Instance.SetAsNotPaymentTime();
                ButtonObjectManager.Instance.DisplayEndTurnButton();
            }
            else
            {
                BoardCard newCard = InstantiateBoardCardEntity(cardFocus);
                BoardCardBehaviour newCardBehaviour = FieldCollectionManager.Instance.GetBehaviourFromEntity(newCard.OccupiedField).LoadTheCard();
                if (newCardBehaviour == null) throw new Exception("Failed to load the new card behaviour");
                newCardBehaviour.StateMachine.SetNewState();
            }
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
        }

        private CardStateEnum GetStateOnCardFocus(BoardCardNetworkData cardFocus, bool isSentByHost)
        {
            if (!IsServer) throw new Exception("Getting state on card focus should be processed from server.");
            BoardCard card = Game.Grid.FindCardByCharacterNameOrNull(cardFocus.CharacterName);
            if (card == null) return CardStateEnum.NewCard;
            if (isSentByHost)
            {
                BoardCardBehaviour cardBehaviour = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(card);
                return cardBehaviour.StateMachine.StateName;
            }
            if (card.OccupiedField.Coordinates == cardFocus.FieldCoords && card.Direction == cardFocus.Direction) return CardStateEnum.Attacking;
            return CardStateEnum.NewTransform;
        }

        private BoardCard InstantiateBoardCardEntity(BoardCardNetworkData cardFocus)
        {
            BoardField targetField = Game.Grid.GetFieldFromCoordsOrThrow(cardFocus.FieldCoords.x, cardFocus.FieldCoords.y);
            CharacterConfig character = NetworkCardManager.Instance.GetConfigFromCharacterName(cardFocus.CharacterName);
            BoardCard newCard = targetField.AddNewCard(character, cardFocus.Alignment);
            if (targetField.BackupCard == null) newCard.SetDirection(cardFocus.Direction);
            return newCard;
        }
    }

    public struct BoardCardNetworkData : INetworkSerializable
    {
        public CharacterEnum CharacterName;
        public Vector2Int FieldCoords;
        public DirectionEnum Direction;
        public AlignmentEnum Alignment;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref CharacterName);
            serializer.SerializeValue(ref FieldCoords);
            serializer.SerializeValue(ref Direction);
            serializer.SerializeValue(ref Alignment);
        }
    }
}
