using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.Grid.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Analytics.IAnalytic;

namespace Berty.Network.Managers
{
    public class NetworkDrawFromPileManager : RpcManagerSingleton<NetworkDrawFromPileManager>, IDrawFromPileManager
    {
        private CardPile cardPile;
        private BoardGrid grid;


        public override void OnInitializeScene()
        {
            Game game = EntityLoadManager.Instance.Game;
            if (IsServer)
            {
                cardPile = game.CardPile;
            }
            if (IsClient)
            {
                grid = game.Grid;
            }
        }

        public void PutRandomKidOrDeactivate(BoardCardBehaviour card, DirectionEnum direction, AlignmentEnum align)
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            Vector2Int fieldCoordinates = card.BoardCard.OccupiedField.Coordinates;
            BoardCardNetworkData data = new()
            {
                CharacterName = CharacterEnum.None,
                FieldCoords = card.BoardCard.OccupiedField.Coordinates,
                Direction = direction,
                Alignment = align
            };
            DrawRandomKidServerRpc(data);
        }

        [Rpc(SendTo.Server)]
        public void DrawRandomKidServerRpc(BoardCardNetworkData oldCardData)
        {
            Debug.Log("Server called to draw random kid.");
            CharacterConfig kid = cardPile.GetRandomKidFromPile();
            Debug.Log("Random kid has been drawn.");
            if (kid != null)
            {
                oldCardData.CharacterName = kid.CharacterName;
                PutKidClientRpc(oldCardData);
            }
            else DeactivateCardClientRpc(oldCardData.FieldCoords);
        }

        [ClientRpc]
        public void PutKidClientRpc(BoardCardNetworkData kidData)
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
        public void DeactivateCardClientRpc(Vector2Int fieldCoords)
        {
            BoardField field = grid.GetFieldFromCoordsOrThrow(fieldCoords);
            BoardCardBehaviour card = FieldCollectionManager.Instance.GetBehaviourFromEntityOrThrow(field).ChildCard;

            if (card == null) throw new Exception($"Field at {field.Coordinates} should have a child card to deactivate");

            card.Activation.DeactivateCard();
            ManagerLocator.CheckpointManagerInstance.HandleIfRequested();
        }
    }
}
