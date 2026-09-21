using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

namespace Berty.Network.Managers
{
    public class NetworkDrawFromPileManager : RpcManagerSingleton<NetworkDrawFromPileManager>, IDrawFromPileManager
    {
        private CardPile cardPile;
        private BoardGrid grid;
        private BoardCardNetworkData? _kidDataBuffer;


        public override void OnInitializeScene()
        {
            Game game = EntityLoadManager.Instance.Game;
            if (IsServer)
            {
                cardPile = game.CardPile;
                _kidDataBuffer = null;
            }
            grid = game.Grid;
        }

        public void PutRandomKidOrDeactivate(BoardCardBehaviour card, DirectionEnum direction, AlignmentEnum align)
        {
            PutRandomKidOrDeactivateServerRpc();
        }

        [Rpc(SendTo.Server)]
        public void PutRandomKidOrDeactivateServerRpc(RpcParams rpcParams = default)
        {
            ulong sourceClientId = rpcParams.Receive.SenderClientId;
            ClientRpcParams sendToSourceRpcParam = new()
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = new ulong[] { sourceClientId }
                }
            };

            if (_kidDataBuffer == null)
            {
                SetRandomizedKidToBuffer();
                SendRandomKidToClient(sendToSourceRpcParam);
            }
            else
            {
                SendRandomKidToClient(sendToSourceRpcParam);
                _kidDataBuffer = null;
            }
        }

        private void SetRandomizedKidToBuffer()
        {
            if (!IsServer)
            {
                Debug.LogWarning("Trying to randomize kid outside server.");
                return;
            }

            BoardCard krolPopuBert = grid.FindCardByCharacterNameOrThrow(CharacterEnum.KrolPopuBert);
            CharacterConfig randomKid = cardPile.GetRandomKidFromPile();
            CharacterEnum randomKidName = randomKid != null ? randomKid.CharacterName : CharacterEnum.None;

            _kidDataBuffer = new()
            {
                CharacterName = randomKidName,
                FieldCoords = krolPopuBert.OccupiedField.Coordinates,
                Direction = krolPopuBert.Direction,
                Alignment = krolPopuBert.Align
            };
        }

        private void SendRandomKidToClient(ClientRpcParams rpcParams)
        {
            if (!IsServer)
            {
                Debug.LogWarning("Trying send a random kid from outside server.");
                return;
            }

            if (_kidDataBuffer == null) throw new InvalidOperationException("Trying to send response with a kid that is null");
            BoardCardNetworkData kidData = (BoardCardNetworkData)_kidDataBuffer;
            if (kidData.CharacterName != CharacterEnum.None) PutKidClientRpc(kidData, rpcParams);
            else DeactivateCardClientRpc(kidData.FieldCoords, rpcParams);
        }

        [ClientRpc]
        public void PutKidClientRpc(BoardCardNetworkData kidData, ClientRpcParams rpcParams)
        {
            BoardField field = grid.GetFieldFromCoordsOrThrow(kidData.FieldCoords);
            BoardCardBehaviour card = FieldCollectionManager.Instance.GetBehaviourFromEntityOrThrow(field).ChildCard;
            CharacterConfig kid = NetworkCardManager.Instance.GetConfigFromCharacterName(kidData.CharacterName);

            if (card == null) throw new Exception($"Field at {field.Coordinates} should have a child card not deactivated yet");

            card.BoardCard.DeactivateCard();

            // Activate new kid card
            card.EntityHandler.LoadBoardCardEntity(kid, kidData.Alignment);
            card.BoardCard.SetDirection(kidData.Direction);
            card.Bars.UpdateBars();
            EventManager.Instance.RaiseOnNewCharacter(card);
            ManagerLocator.CheckpointManagerInstance.HandleIfRequested();
        }

        [ClientRpc]
        public void DeactivateCardClientRpc(Vector2Int fieldCoords, ClientRpcParams rpcParams)
        {
            BoardField field = grid.GetFieldFromCoordsOrThrow(fieldCoords);
            BoardCardBehaviour card = FieldCollectionManager.Instance.GetBehaviourFromEntityOrThrow(field).ChildCard;

            if (card == null) throw new Exception($"Field at {field.Coordinates} should have a child card to deactivate");

            card.Activation.DeactivateCard();
            ManagerLocator.CheckpointManagerInstance.HandleIfRequested();
        }
    }
}
