using Berty.Characters.Data;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Berty.Entities
{
    public class CardPile
    {
        private List<CharacterConfig> pileCards;
        private List<CharacterConfig> discardedCards;
        private List<CharacterConfig> deadCards;
        private CharacterConfig bottomCard;

        private List<CharacterConfig> playerTable;
        private List<CharacterConfig> opponentTable;

        public CardPile()
        {
            pileCards = new List<CharacterConfig>();
            discardedCards = new List<CharacterConfig>();
            deadCards = new List<CharacterConfig>();
            playerTable = new List<CharacterConfig>();
            opponentTable = new List<CharacterConfig>();
        }

        public bool PullCardsTo(int tableCapacity, Alignment align)
        {
            List<CharacterConfig> table = GetTableFromAlign(align);
            int cardsToPull = tableCapacity - table.Count;
            for (int i = cardsToPull; i > 0; i--) if (!PullCard(table)) return false;
            if (pileCards.Count == 0) Reshuffle();
            return true;
        }

        private bool PullCard(List<CharacterConfig> targetTable)
        {
            if (pileCards.Count == 0 && !Reshuffle()) return false;
            int index = Random.Range(0, pileCards.Count);
            CharacterConfig takenCard = pileCards[index];
            pileCards.Remove(takenCard);
            targetTable.Add(takenCard);
            return true;
        }

        private bool PullCard(Alignment align)
        {
            List<CharacterConfig> table = GetTableFromAlign(align);
            return PullCard(table);
        }

        private bool Reshuffle()
        {
            if (pileCards.Count > 0) throw new InvalidOperationException("There are still cards in pile.");
            if (bottomCard != null)
            {
                pileCards.Add(bottomCard);
                bottomCard = null;
                return true;
            }
            if (discardedCards.Count > 0)
            {
                pileCards.AddRange(discardedCards);
                discardedCards.Clear();
                return true;
            }
            return false;
        }

        private List<CharacterConfig> GetTableFromAlign(Alignment align)
        {
            return align switch
            {
                Alignment.Player => playerTable,
                Alignment.Opponent => opponentTable,
                _ => throw new ArgumentException("Attempting to get table from invalid align."),
            };
        }

        private void DiscardCards(List<CharacterConfig> cardsToDiscard, Alignment align)
        {
            List<CharacterConfig> table = GetTableFromAlign(align);
            int initialTableCount = table.Count;
            table.RemoveAll(card => cardsToDiscard.Contains(card));
            discardedCards.AddRange(cardsToDiscard);
            if (table.Count != initialTableCount - cardsToDiscard.Count) throw new Exception("Unexpected amount of remaining cards.");
        }

        private void KillCard(CharacterConfig card)
        {
            deadCards.Add(card);
        }

        private void ReviveCard(CharacterConfig card)
        {
            deadCards.Remove(card);
        }

        private void LeaveCard(CharacterConfig card, Alignment align)
        {
            List<CharacterConfig> table = GetTableFromAlign(align);
            if (!table.Contains(card)) throw new Exception($"Attempting to get {card.Name} from wrong table");
            table.Remove(card);
        }
    }
}