using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Entities;
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
            CharacterEnum cardFocusName = card.BoardCard.CharacterConfig.CharacterName;
            DiscardSelectedCardsFromHandServerRpc(selectedCardNames, cardFocusName, OwnerClientId);
        }

        [ServerRpc(RequireOwnership = false)]
        public void DiscardSelectedCardsFromHandServerRpc(CharacterEnum[] selectedCardNames, CharacterEnum cardFocusName, ulong sourceClientId)
        {
            if (!IsServer) throw new InvalidOperationException("Discarding cards should be processed in server. server.");
            AlignmentEnum align = Game.CurrentAlignment;
            if (PlayerReadManager.Instance.GetClientIdFromAlignment(align) != sourceClientId) throw new InvalidOperationException($"Align {align} does not belong to the client.");
            IReadOnlyList<CharacterConfig> playerCards = CardPile.GetCardsFromAlign(align);
            IReadOnlyList<CharacterConfig> selectedCards = playerCards.Where(card => selectedCardNames.Contains(card.CharacterName)).ToList();
            CharacterConfig cardFocus = playerCards.FirstOrDefault(card => card.CharacterName == cardFocusName);
            if (cardFocus != null) CardPile.LeaveCard(cardFocus, align);
            CardPile.DiscardCards(selectedCards, align);
            FinishPaymentClientRpc(sourceClientId);
        }

        [ClientRpc]
        public void FinishPaymentClientRpc(ulong sourceClientId)
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
                // TODO: Add card on the field
            }
            EventManager.Instance.RaiseOnPaymentConfirm();
            CheckpointManager.Instance.RequestCheckpoint();
        }
    }
}
