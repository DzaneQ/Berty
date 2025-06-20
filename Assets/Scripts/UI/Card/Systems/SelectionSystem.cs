using Berty.BoardCards.ConfigData;
using Berty.CardTransfer.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.UI.Card.Systems
{
    public class SelectionSystem
    {
        private List<CharacterConfig> selectedCards = new List<CharacterConfig>();
        private int? cardPrice;

        public void SelectCard(CharacterConfig card)
        {
            if (!selectedCards.Contains(card)) selectedCards.Add(card);
        }

        public void UnselectCard(CharacterConfig card)
        {
            selectedCards.Remove(card);
        }

        public void ClearSelection()
        {
            selectedCards.Clear();
        }

        public int GetSelectedCardsCount()
        {
            return selectedCards.Count;
        }

        public bool IsSelected(CharacterConfig card)
        {
            return selectedCards.Contains(card);
        }

        public void DemandPayment(int demandedPrice)
        {
            cardPrice = demandedPrice;
        }

        public bool IsItPaymentTime()
        {
            return cardPrice != null;
        }

        public void SetAsNotPaymentTime()
        {
            cardPrice = null;
        }

        public bool CanSelectCard()
        {
            return GetSelectedCardsCount() < (cardPrice ?? 1);
        }
    }
}