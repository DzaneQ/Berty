using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
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
            DiscardSelectedCardsFromHandServerRpc(selectedCardNames, cardFocus, OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DiscardSelectedCardsFromHandServerRpc(CharacterEnum[] selectedCardNames, BoardCardNetworkData cardFocus, ulong sourceClientId)
        {
            if (!IsServer) throw new InvalidOperationException("Discarding cards should be processed in server. server.");
            AlignmentEnum align = Game.CurrentAlignment;
            if (PlayerReadManager.Instance.GetClientIdFromAlignment(align) != sourceClientId) throw new InvalidOperationException($"Align {align} does not belong to the client.");
            IReadOnlyList<CharacterConfig> playerCards = CardPile.GetCardsFromAlign(align);
            IReadOnlyList<CharacterConfig> selectedCards = playerCards.Where(card => selectedCardNames.Contains(card.CharacterName)).ToList();
            CharacterConfig cardFocusInHands = playerCards.FirstOrDefault(card => card.CharacterName == cardFocus.CharacterName);
            if (cardFocusInHands != null) CardPile.LeaveCard(cardFocusInHands, align);
            CardPile.DiscardCards(selectedCards, align);
            FinishPaymentClientRpc(cardFocus, sourceClientId);
            if (!ServerIsHost) InstantiateBoardCardEntity(cardFocus); // NOTE: Experimental
        }

        // TODO: It applies only for new card. Other payment related actions should be adjusted.
        [ClientRpc]
        public void FinishPaymentClientRpc(BoardCardNetworkData cardFocus, ulong sourceClientId)
        {
            if (OwnerClientId == sourceClientId)
            {
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
                newCardBehaviour.StateMachine.HandleStateForNewCard();
            }
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
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
