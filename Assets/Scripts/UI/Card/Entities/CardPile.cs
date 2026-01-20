using Berty.BoardCards.ConfigData;
using Berty.Characters.Init;
using Berty.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public IReadOnlyList<CharacterConfig> PileCards => pileCards;
        public IReadOnlyList<CharacterConfig> DiscardedCards => discardedCards;
        public IReadOnlyList<CharacterConfig> DeadCards => deadCards;

        public CardPile()
        {
            CharacterData data = new();
            pileCards = data.LoadCharacterData();
            discardedCards = new List<CharacterConfig>();
            deadCards = new List<CharacterConfig>();
            PlayerCards = new List<CharacterConfig>();
            OpponentCards = new List<CharacterConfig>();
        }

        public CardPile(CardPileSaveData data, IReadOnlyList<CharacterConfig> allCharacters)
        {
            pileCards = GetCharactersFromNames(data.PileCardNames, allCharacters);
            discardedCards = GetCharactersFromNames(data.DiscardedCardNames, allCharacters);
            deadCards = GetCharactersFromNames(data.DeadCardNames, allCharacters);
            bottomCard = allCharacters.FirstOrDefault(character => character.Name == data.BottomCardName);
            PlayerCards = GetCharactersFromNames(data.PlayerCardNames, allCharacters);
            OpponentCards = GetCharactersFromNames(data.OpponentCardNames, allCharacters);
        }

        public CardPileSaveData SaveEntity()
        {
            return new()
            {
                PileCardNames = GetNamesFromCharacters(pileCards),
                DiscardedCardNames = GetNamesFromCharacters(discardedCards),
                DeadCardNames = GetNamesFromCharacters(deadCards),
                BottomCardName = bottomCard != null ? bottomCard.Name : "",
                PlayerCardNames = GetNamesFromCharacters(PlayerCards),
                OpponentCardNames = GetNamesFromCharacters(OpponentCards),
            };
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

        public void DiscardCards(IReadOnlyList<CharacterConfig> cardsToDiscard, AlignmentEnum align)
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

        public bool AreThereAnyDeadCards()
        {
            return deadCards.Count > 0;
        }

        public void PutCardToTheBottomPile(CharacterConfig card)
        {
            if (bottomCard != null) throw new Exception("There is already a designated card at the bottom of the pile");
            bottomCard = card;
        }

        public void ReviveCard(CharacterConfig card, AlignmentEnum align)
        {
            if (!deadCards.Contains(card)) throw new Exception($"Card {card.Name} is not dead");
            List<CharacterConfig> table = GetCardsFromAlign(align);
            deadCards.Remove(card);
            table.Add(card);
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

        public List<CharacterConfig> GetAllCharactersOutsideField()
        {
            return pileCards.Union(discardedCards).Union(deadCards).Append(bottomCard).Union(PlayerCards).Union(OpponentCards).ToList();
        }

        private string[] GetNamesFromCharacters(List<CharacterConfig> configList)
        {
            return configList.Select(character => character.Name).ToArray();
        }

        private List<CharacterConfig> GetCharactersFromNames(string[] characterNames, IReadOnlyList<CharacterConfig> allCharacters)
        {
            return allCharacters.Where(character => characterNames.Contains(character.Name)).ToList();
        }
    }

    [Serializable]
    public struct CardPileSaveData
    {
        public string[] PileCardNames;
        public string[] DiscardedCardNames;
        public string[] DeadCardNames;
        public string BottomCardName;
        public string[] PlayerCardNames;
        public string[] OpponentCardNames;
    }
}