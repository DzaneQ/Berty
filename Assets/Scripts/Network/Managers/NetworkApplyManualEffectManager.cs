using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Network.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class NetworkApplyManualEffectManager : RpcManagerSingleton<NetworkApplyManualEffectManager>, IApplyManualEffectManager
    {
        private Game Game { get; set; }
        private HandCardCollection _handCardCollection;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        private void Start()
        {
            _handCardCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
        }

        public void ReviveCard(HandCardBehaviour handCardObject)
        {
            foreach (CharacterConfig config in Game.CardPile.DeadCards) Debug.Log("Dead card available: " + config.CharacterName);
            ReviveCardServerRpc(handCardObject.Character.CharacterName);
        }

        public void EnhanceCard(BoardCardBehaviour boardCardObject)
        {
            EnhanceCardServerRpc(boardCardObject.BoardCard.CharacterConfig.CharacterName);
        }

        [ServerRpc(RequireOwnership = false)]
        public void ReviveCardServerRpc(CharacterEnum targetCharacter)
        {
            if (!Game.CardPile.DeadCards.Select(config => config.CharacterName).Contains(targetCharacter)) throw new Exception("The target card is not in the dead pile to revive.");

            ReviveCardClientRpc(targetCharacter);
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnhanceCardServerRpc(CharacterEnum targetCharacter)
        {
            if (Game.Grid.FindCardByCharacterNameOrNull(targetCharacter) == null) throw new Exception("The target card is not on the board to enhance.");
            if (Game.GetStatusByNameOrNull(StatusEnum.ClickToApplyEffect) == null) throw new Exception("There is not status to enhance a card.");

            EnhanceCardClientRpc(targetCharacter);
        }

        [ClientRpc]
        public void ReviveCardClientRpc(CharacterEnum targetCharacter)
        {
            HandCardBehaviour handCardObject = _handCardCollection.GetBehaviourFromCharacterName(targetCharacter);

            Status revival = Game.GetStatusByNameOrNull(StatusEnum.RevivalSelect); // Upon deactivation, the status is null for the other client
            AlignmentEnum? targetAlign = revival?.GetAlign();
            AlignmentEnum clientAlign = PlayerReadManager.Instance.MyAlignment;
            if (clientAlign == targetAlign)
            {
                Game.CardPile.ReviveCard(handCardObject.Character, clientAlign);
                NetworkCardManager.Instance.RetrieveCard(handCardObject.Character);
                ManagerLocator.HandCardObjectManagerInstance.AddCardObjects();
            }
            else
            {
                Game.CardPile.ReviveCard(handCardObject.Character, AlignmentEnum.None);
                if (revival == null)
                {
                    BoardCardBehaviour gotkaBerta = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(Game.Grid.FindCardByCharacterNameOrThrow(CharacterEnum.GotkaBerta));
                    gotkaBerta.Activation.DeactivateCard();
                }
            }

            if (revival != null) StatusManager.Instance.RemoveStatus(revival);
            ManagerLocator.CheckpointManagerInstance.RequestCheckpoint();
        }

        [ClientRpc]
        public void EnhanceCardClientRpc(CharacterEnum targetCharacter)
        {
            BoardCard target = Game.Grid.FindCardByCharacterNameOrThrow(targetCharacter);
            BoardCardBehaviour targetBehaviour = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(target);

            Status enhancement = Game.GetStatusByNameOrThrow(StatusEnum.ClickToApplyEffect);
            BoardCardBehaviour source = BoardCardCollectionManager.Instance.GetActiveBehaviourFromEntityOrThrow(enhancement.Provider);
            targetBehaviour.EntityHandler.AdvanceStrength(2, source);
            targetBehaviour.EntityHandler.AdvanceHealth(1, source);
            StatusManager.Instance.RemoveStatus(enhancement);
            ManagerLocator.CheckpointManagerInstance.RequestCheckpoint();
        }
    }
}