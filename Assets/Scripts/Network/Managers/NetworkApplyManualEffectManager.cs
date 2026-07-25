using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.Utility;
using System;
using Unity.Netcode;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class NetworkApplyManualEffectManager : RpcManagerSingleton<NetworkApplyManualEffectManager>, IApplyManualEffectManager
    {
        private Game Game { get; set; }

        protected override void Awake()
        {
            InitializeSingleton();
            Game = EntityLoadManager.Instance.Game;
        }

        public void ReviveCard(HandCardBehaviour handCardObject)
        {
            throw new NotImplementedException();
        }

        public void EnhanceCard(BoardCardBehaviour boardCardObject)
        {
            EnhanceCardServerRpc(boardCardObject.BoardCard.CharacterConfig.CharacterName);
        }

        [ServerRpc(RequireOwnership = false)]
        public void EnhanceCardServerRpc(CharacterEnum targetCharacter)
        {
            if (Game.Grid.FindCardByCharacterNameOrNull(targetCharacter) == null) throw new Exception("The target card is not on the board to enhance.");
            if (Game.GetStatusByNameOrNull(StatusEnum.ClickToApplyEffect) == null) throw new Exception("There is not status to enhance a card.");

            EnhanceCardClientRpc(targetCharacter);
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
        }
    }
}