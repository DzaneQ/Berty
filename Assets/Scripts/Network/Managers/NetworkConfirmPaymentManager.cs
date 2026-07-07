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
            NavigationEnum cardFocusNavigation = NavigationEnum.None;
            if (cardFocusState == CardStateEnum.NewTransform) cardFocusNavigation = GetNavigationOnCardFocusOrThrow(cardFocus, isSentByHost);
            ulong[] otherClientIds = NetworkManager.Singleton.ConnectedClientsIds.Where(clientId => clientId != sourceClientId).ToArray();
            ClientRpcParams sendToSourceRpcParam = new()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { sourceClientId }
                }
            };
            ClientRpcParams sendToOtherRpcParam = new()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = otherClientIds
                }
            };

            // Discard cards from pile
            if (cardFocusState == CardStateEnum.NewCard)
            {
                CharacterConfig cardFocusInHands = playerCards.First(card => card.CharacterName == cardFocus.CharacterName);
                CardPile.LeaveCard(cardFocusInHands, align);
            }
            CardPile.DiscardCards(selectedCards, align);

            FinishPaymentClientRpc(CardPile.GetCharacterNamesFromAlignedTable(align), sendToSourceRpcParam);
            ProcessPaymentForOtherClientRpc(cardFocus, cardFocusState, cardFocusNavigation, sendToOtherRpcParam);
            //if (!isSentByHost && cardFocusState == CardStateEnum.NewCard) InstantiateBoardCardEntity(cardFocus); // NOTE: Experimental
        }

        [ClientRpc]
        private void FinishPaymentClientRpc(CharacterEnum[] playerTable, ClientRpcParams sourceClientRpcParams)
        {
            NetworkCardManager.Instance.UpdatePlayerHandCards(playerTable);
            ManagerLocator.HandCardObjectManagerInstance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
            SelectionManager.Instance.SetAsNotPaymentTime();
            ButtonObjectManager.Instance.DisplayEndTurnButton();
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
        }

        // TODO: It applies only for new card. Other payment related actions should be adjusted.
        [ClientRpc]
        private void ProcessPaymentForOtherClientRpc(BoardCardNetworkData cardFocus, CardStateEnum cardState, NavigationEnum cardNavigation, ClientRpcParams otherClientRpcParams)
        {
            BoardCard card;
            BoardCardBehaviour behaviour;
            if (cardState == CardStateEnum.NewCard)
            {
                card = InstantiateBoardCardEntity(cardFocus);
                behaviour = FieldCollectionManager.Instance.GetBehaviourFromEntityOrNull(card.OccupiedField).LoadTheCard();
                if (behaviour == null) throw new Exception("Failed to load the new card behaviour");
            }
            else
            {
                card = GetBoardCardEntity(cardFocus);
                behaviour = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(card);
            }
            
            behaviour.StateMachine.SetPendingStateFromEnum(cardState, cardNavigation);

            if (cardState == CardStateEnum.NewTransform) NavigateCard(behaviour, cardFocus);

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

        private NavigationEnum GetNavigationOnCardFocusOrThrow(BoardCardNetworkData cardFocus, bool isSentByHost)
        {
            if (!IsServer) throw new Exception("Getting navigation on card focus should be processed from server.");
            BoardCard card = Game.Grid.FindCardByCharacterNameOrThrow(cardFocus.CharacterName);
            if (isSentByHost)
            {
                BoardCardBehaviour cardBehaviour = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(card);
                return cardBehaviour.StateMachine.GetNewTransformNavigation();
            }

            // Return card rotation
            if (card.Direction != cardFocus.Direction)
            {
                if (card.OccupiedField.Coordinates != cardFocus.FieldCoords) throw new Exception("Card should not have different coordinates and rotation.");
                int angleDifference = ((int)cardFocus.Direction - (int)card.Direction) % 360;
                return angleDifference switch
                {
                    90 => NavigationEnum.RotateRight,
                    270 => NavigationEnum.RotateLeft,
                    _ => throw new Exception("The angle difference for rotated card should not be " + angleDifference),
                };
            }

            // Return card movement
            Vector2Int originFieldCoords = card.OccupiedField.Coordinates;
            Vector2Int destinationFieldCoords = cardFocus.FieldCoords;
            Vector2Int fieldDistance = destinationFieldCoords - originFieldCoords;
            Vector2Int relativeDistance = card.OccupiedField.Grid.GetToRelativeCoordinates(fieldDistance.x, fieldDistance.y, card.GetAngle());
            return relativeDistance switch
            {
                { x: 0, y: 1 } => NavigationEnum.MoveUp,
                { x: 1, y: 0 } => NavigationEnum.MoveRight,
                { x: 0, y: -1 } => NavigationEnum.MoveDown,
                { x: -1, y: 0 } => NavigationEnum.MoveLeft, 
                _ => throw new Exception("The relative distance for moved card should not be " + relativeDistance),
            };
        }

        private BoardCard InstantiateBoardCardEntity(BoardCardNetworkData cardFocus)
        {
            BoardField targetField = Game.Grid.GetFieldFromCoordsOrThrow(cardFocus.FieldCoords.x, cardFocus.FieldCoords.y);
            CharacterConfig character = NetworkCardManager.Instance.GetConfigFromCharacterName(cardFocus.CharacterName);
            BoardCard newCard = targetField.AddNewCard(character, cardFocus.Alignment);
            if (targetField.BackupCard == null) newCard.SetDirection(cardFocus.Direction);
            return newCard;
        }

        private BoardCard GetBoardCardEntity(BoardCardNetworkData cardFocus)
        {
            return Game.Grid.FindCardByCharacterNameOrThrow(cardFocus.CharacterName);
        }

        private void NavigateCard(BoardCardBehaviour origin, BoardCardNetworkData destination)
        {
            NavigationEnum navigation = origin.StateMachine.GetNewTransformNavigation();
            switch (navigation)             {
                case NavigationEnum.RotateLeft:
                    CardNavigationManager.Instance.RotateCard(origin, -90);
                    break;
                case NavigationEnum.RotateRight:
                    CardNavigationManager.Instance.RotateCard(origin, 90);
                    break;
                case NavigationEnum.MoveUp:
                case NavigationEnum.MoveRight:
                case NavigationEnum.MoveDown:
                case NavigationEnum.MoveLeft:
                    BoardField targetField = Game.Grid.GetFieldFromCoordsOrThrow(destination.FieldCoords.x, destination.FieldCoords.y);
                    CardNavigationManager.Instance.MoveCard(origin, targetField, true); // NOTE: Assumed that navigation is part of payment process
                    break;
                default:
                    throw new Exception("Invalid navigation for card transformation: " + navigation);
            }
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
