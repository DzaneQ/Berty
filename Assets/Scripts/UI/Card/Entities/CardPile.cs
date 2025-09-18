using Berty.BoardCards.ConfigData;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Berty.UI.Card.Entities
{
    public class CardPile
    {
        private List<CharacterConfig> pileCards;
        private List<CharacterConfig> discardedCards;
        private List<CharacterConfig> deadCards;
        private CharacterConfig bottomCard;

        public List<CharacterConfig> PlayerCards { get; private set; }
        public List<CharacterConfig> OpponentCards { get; private set; }

        public void InitializeCardPile(List<CharacterConfig> characterPile)
        {
            if (pileCards != null
                || discardedCards != null
                || deadCards != null
                || bottomCard != null
                || PlayerCards != null
                || OpponentCards != null)
                throw new Exception("Card pile is already initialized");
            pileCards = characterPile;
            discardedCards = new List<CharacterConfig>();
            deadCards = new List<CharacterConfig>();
            PlayerCards = new List<CharacterConfig>();
            OpponentCards = new List<CharacterConfig>();
        }

        public bool PullCardsTo(int tableCapacity, AlignmentEnum align)
        {
            List<CharacterConfig> table = GetCardsFromAlign(align);
            int cardsToPull = tableCapacity - table.Count;
            for (int i = cardsToPull; i > 0; i--) if (!PullCard(table)) return false;
            if (pileCards.Count == 0) Reshuffle();
            return true;
        }

        public void PullCardIfInPile(SkillEnum character, AlignmentEnum align)
        {
            CharacterConfig takenCard = pileCards.Find(x => x.Skill == character);
            if (takenCard == null) return;
            List<CharacterConfig> targetTable = GetCardsFromAlign(align);
            pileCards.Remove(takenCard);
            targetTable.Add(takenCard);
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

        private bool PullCard(AlignmentEnum align)
        {
            List<CharacterConfig> table = GetCardsFromAlign(align);
            return PullCard(table);
        }

        public void RetrieveCard(CharacterConfig card, AlignmentEnum align)
        {
            List<CharacterConfig> table = GetCardsFromAlign(align);
            table.Add(card);
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

        public List<CharacterConfig> GetCardsFromAlign(AlignmentEnum align)
        {
            return align switch
            {
                AlignmentEnum.Player => PlayerCards,
                AlignmentEnum.Opponent => OpponentCards,
                _ => throw new ArgumentException("Attempting to get cards from invalid align."),
            };
        }

        public void DiscardCards(List<CharacterConfig> cardsToDiscard, AlignmentEnum align)
        {
            List<CharacterConfig> table = GetCardsFromAlign(align);
            int initialTableCount = table.Count;
            table.RemoveAll(card => cardsToDiscard.Contains(card));
            discardedCards.AddRange(cardsToDiscard);
            if (table.Count != initialTableCount - cardsToDiscard.Count) throw new Exception(
                $"Unexpected amount of remaining cards. Count: {table.Count}, initial: {initialTableCount}, cardsToDiscard: {cardsToDiscard.Count}");
        }

        public void MarkCardAsDead(CharacterConfig card)
        {
            deadCards.Add(card);
        }

        public void PutCardToTheBottomPile(CharacterConfig card)
        {
            if (bottomCard != null) throw new Exception("There is already a designated card at the bottom of the pile");
            bottomCard = card;
        }

        private void ReviveCard(CharacterConfig card)
        {
            deadCards.Remove(card);
        }

        public void LeaveCard(CharacterConfig card, AlignmentEnum align)
        {
            List<CharacterConfig> table = GetCardsFromAlign(align);
            if (!table.Contains(card)) throw new Exception($"Attempting to get {card.Name} from wrong table");
            table.Remove(card);
        }

        public CharacterConfig GetRandomKidFromPile()
        {
            List<CharacterConfig> kids = pileCards.Where(card => card.Gender == GenderEnum.Kid).ToList();
            if (kids.Count == 0) kids = discardedCards.Where(card => card.Gender == GenderEnum.Kid).ToList();
            if (kids.Count == 0) return null;
            int index = Random.Range(0, kids.Count);
            discardedCards.Remove(kids[index]);
            pileCards.Remove(kids[index]);
            return kids[index];
        }
    }
}