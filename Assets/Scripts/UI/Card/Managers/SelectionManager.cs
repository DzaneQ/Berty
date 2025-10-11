using Berty.BoardCards.ConfigData;
using Berty.Utility;
using System;
using System.Collections.Generic;

namespace Berty.UI.Card.Managers
{
    public class SelectionManager : ManagerSingleton<SelectionManager>
    {
        private List<CharacterConfig> selectedCards;
        private int? cardPrice;
        private CharacterConfig pendingCard;
        public IReadOnlyList<CharacterConfig> SelectedCards => selectedCards;

        protected override void Awake()
        {
            base.Awake();
            selectedCards = new List<CharacterConfig>();
        }

        public void SelectCard(CharacterConfig card)
        {
            if (!selectedCards.Contains(card)) selectedCards.Add(card);
        }

        public void UnselectCard(CharacterConfig card)
        {
            selectedCards.Remove(card);
        }

        public int GetSelectedCardsCount()
        {
            return selectedCards.Count;
        }

        public CharacterConfig GetSelectedCardOrThrow()
        {
            if (GetSelectedCardsCount() != 1) throw new InvalidOperationException($"Trying to call the only selected card when the count is: {GetSelectedCardsCount()}");
            return selectedCards[0];
        }

        public bool IsSelected(CharacterConfig card)
        {
            return selectedCards.Contains(card);
        }

        public CharacterConfig GetTheOnlySelectedCardOrNull()
        {
            if (GetSelectedCardsCount() != 1) return null;
            return selectedCards[0];
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