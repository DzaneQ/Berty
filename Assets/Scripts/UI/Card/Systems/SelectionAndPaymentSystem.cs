using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Managers;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace Berty.UI.Card.Systems
{
    public class SelectionAndPaymentSystem
    {
        public List<CharacterConfig> SelectedCards { get; }
        public int? cardPrice;
        private CharacterConfig pendingCard;

        public SelectionAndPaymentSystem()
        {
            SelectedCards = new List<CharacterConfig>();
        }

        public void SelectCard(CharacterConfig card)
        {
            if (!SelectedCards.Contains(card)) SelectedCards.Add(card);
        }

        public void UnselectCard(CharacterConfig card)
        {
            SelectedCards.Remove(card);
        }

        public int GetSelectedCardsCount()
        {
            return SelectedCards.Count;
        }

        public CharacterConfig GetSelectedCardOrThrow()
        {
            if (GetSelectedCardsCount() != 1) throw new InvalidOperationException($"Trying to call the only selected card when the count is: {GetSelectedCardsCount()}");
            return SelectedCards[0];
        }

        public bool IsSelected(CharacterConfig card)
        {
            return SelectedCards.Contains(card);
        }

        public CharacterConfig GetTheOnlySelectedCardOrNull()
        {
            if (GetSelectedCardsCount() != 1) return null;
            return SelectedCards[0];
        }

        public void DemandPayment(int demandedPrice)
        {
            if (IsItPaymentTime()) throw new Exception("Demanding payment when there's already demanded payment.");
            cardPrice = demandedPrice;
        }

        public bool IsItPaymentTime()
        {
            return cardPrice != null;
        }

        public void SetAsNotPaymentTime()
        {
            cardPrice = null;
            ClearPendingCard();
        }

        public bool CheckOffer()
        {
            return IsItPaymentTime() && GetSelectedCardsCount() == cardPrice;
        }

        public bool CanSelectCard()
        {
            return GetSelectedCardsCount() < (cardPrice ?? 1);
        }

        public CharacterConfig GetPendingCardOrThrow()
        {
            if (pendingCard == null) throw new ArgumentNullException("Card on hold cannot be null!");
            return pendingCard;
        }

        public void PutSelectedCardAsPending()
        {
            pendingCard = GetSelectedCardOrThrow();
        }

        public void SetCardAsPending(CharacterConfig characterConfig)
        {
            pendingCard = characterConfig;
        }

        public void ClearPendingCard()
        {
            pendingCard = null;
        }
    }
}