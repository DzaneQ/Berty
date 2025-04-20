using Berty.Characters.Data;
using Berty.Display;
using Berty.Enums;
using Berty.UI.Card;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.Gameplay
{
    public class CardManager : MonoBehaviour
    {
        private const int tableCapacity = 6;
        private const float offsetFactor = 0.0001f;

        private Turn turn;
        private List<CardImage> pileCards = new List<CardImage>();
        private List<CardImage> discardedCards;
        private List<CardImage> enabledCards = new List<CardImage>();
        private List<CardImage> disabledCards = new List<CardImage>();
        private List<CardImage> deadCards = new List<CardImage>();
        private CardImage cardBelow;
        private readonly System.Random rng = new System.Random();
        private GridLayoutGroup deadScreenGrid;
        private LookupCard luCard;

        public Turn Turn => turn;
        public List<CardImage> EnabledCards => enabledCards;
        public List<CardImage> DisabledCards => disabledCards;

        [SerializeField] private GameObject drawPile;
        [SerializeField] private GameObject discardPile;
        [SerializeField] private GameObject playerTable;
        [SerializeField] private GameObject opponentTable;
        [SerializeField] private GameObject cardImageCollection;
        [SerializeField] private GameObject deadScreen;

        private void Start()
        {
            turn = GetComponent<Turn>();
            CardInitialization init = GetComponent<CardInitialization>();
            init.InitializeAllCharacterCards(cardImageCollection, out discardedCards);
            init.InitializeCardPile(discardPile, discardedCards.Count);
            luCard = init.AttachLookupCard();
            ShufflePile();
            Destroy(init);
            deadScreenGrid = deadScreen.GetComponent<GridLayoutGroup>();
        }

        #region PullingCard
        public bool PullCards(Alignment align)
        {
            Transform table = align == Alignment.Player ? playerTable.transform : opponentTable.transform;
            int cardsToPull = tableCapacity - enabledCards.Count;
            for (int i = cardsToPull; i > 0; i--) if (!PullCard(table)) return false;
            return true;
        }

        public bool PullCard(Alignment align)
        {
            Transform table = align == Alignment.Player ? playerTable.transform : opponentTable.transform;
            return PullCard(table);
        }

        private bool PullCard(Transform table)
        {
            CardImage card = TakeFromPile();
            if (card != null) AddToTable(card, table);
            else return false;
            return true;
        }

        private CardImage TakeFromPile()
        {
            if (drawPile.transform.childCount == 0 && !ShufflePile()) return null;
            if (pileCards.Count > 0) return TakeRandomCharacter();
            return TakeRemainingCharacter();
        }

        private CardImage TakeRandomCharacter()
        {
            CardImage character = pileCards[rng.Next(pileCards.Count)];
            pileCards.Remove(character);
            RemoveFromDrawPile();
            return character;
        }

        private CardImage TakeRemainingCharacter()
        {
            CardImage character = cardBelow;
            cardBelow = null;
            RemoveFromDrawPile();
            return character;
        }
        #endregion

        #region ShuffleCard
        private bool ShufflePile()
        {
            GameObject card;
            //Debug.Log("Discarded cards to shuffle: " + discardedCardsCount);
            for (int i = discardPile.transform.childCount - 1; i >= 0; i--)
            {
                card = discardPile.transform.GetChild(i).gameObject;
                //Debug.Log("Shuffled card: " + (i) + " : " + card.name);
                if (!card.activeSelf) continue;
                PrepareCardInPile(card.transform, drawPile.transform);
            }
            return ShuffleDiscardedCharacters();
        }

        private bool ShuffleDiscardedCharacters()
        {
            pileCards = discardedCards;
            discardedCards = new List<CardImage>();
            //Debug.Log("Character pile count: " + pileCards.Count);
            return pileCards.Count != 0;
        }
        #endregion

        #region DrawPile
        private void RemoveFromDrawPile()
        {
            GameObject card = drawPile.transform.GetChild(drawPile.transform.childCount - 1).gameObject;
            card.SetActive(false);
            PrepareCardInPile(card.transform, discardPile.transform);
        }

        private void PrepareCardInPile(Transform cardTransform, Transform stack)
        {
            //Debug.Log("Shuffling to: " + stack.name);
            float offsetUnit = stack.childCount * offsetFactor;
            cardTransform.SetParent(stack, false);
            cardTransform.localPosition = new Vector3(offsetUnit, offsetUnit, offsetUnit);
        }
        #endregion

        #region DiscardCard
        public void DiscardCards()
        {
            foreach (CardImage card in SelectedCards())
            {
                RemoveFromTable(card);
                discardedCards.Add(card);
                DiscardPileCard();
            }
        }

        private void DiscardPileCard()
        {
            GameObject pileCard;
            //Debug.Log("Card in pile discarded");
            for (int i = 0; i < discardPile.transform.childCount; i++)
            {
                pileCard = discardPile.transform.GetChild(i).gameObject;
                if (!pileCard.activeSelf)
                {
                    pileCard.SetActive(true);
                    break;
                }
            }
        }
        #endregion

        #region Table
        public void RemoveFromTable(CardImage card, bool disabledCard = false)
        {
            card.SetBackupTable();
            card.transform.SetParent(cardImageCollection.transform, false);
            card.Unselect();
            //Debug.Log($"{card.name}: Table removal for enabled ones: {disabledCard}");
            if (!disabledCard) enabledCards.Remove(card);
            else disabledCards.Remove(card);
            //if (pileCards.Count + discardedCards.Count != cardImageCollection.transform.childCount)
            //{
            //    Debug.Log($"Pile cards: {pileCards.Count}; Discarded cards: {discardedCards.Count}; Collection: {cardImageCollection.transform.childCount}");
            //}
        }

        public void AddToTable(CardImage card, Transform table)
        {
            card.transform.SetParent(table, false);
            enabledCards.Add(card);
        }

        public void SwitchTable(Alignment alignment)
        {
            //Debug.Log("Switching table!");
            ShowTable(alignment);
            SwapTable();
        }

        private void ShowTable(Alignment alignment)
        {
            playerTable.SetActive(alignment == Alignment.Player);
            opponentTable.SetActive(alignment == Alignment.Opponent);
        }

        private void SwapTable()
        {
            List<CardImage> temp = enabledCards;
            enabledCards = disabledCards;
            disabledCards = temp;
        }

        public void HideTables()
        {
            playerTable.SetActive(false);
            opponentTable.SetActive(false);
        }
        #endregion

        #region CardSelection
        public void DeselectCards()
        {
            foreach (CardImage card in SelectedCards()) card.Unselect();
        }

        public List<CardImage> SelectedCards()
        {
            List<CardImage> selectedCards = new List<CardImage>();
            //Debug.Log("Count before: " + selectedCards.Count);
            foreach (CardImage card in enabledCards) if (card.IsCardSelected()) selectedCards.Add(card);
            //Debug.Log("Count after: " + selectedCards.Count);
            return selectedCards;
        }

        public CardImage SelectedCard()
        {
            foreach (CardImage card in enabledCards)
                if (card.IsCardSelected()) return card;
            return null;
        }
        #endregion

        public void KillCard(CardImage card)
        {
            deadCards.Add(card);
            card.transform.SetParent(deadScreen.transform, false);
        }

        #region CharacterSkillBased
        public void ReturnCharacter(CardImage card)
        {
            if (cardBelow != null) throw new Exception("There's a set card below!");
            GameObject pileCard = discardPile.transform.GetChild(discardPile.transform.childCount - 1).gameObject;
            if (pileCard.activeSelf) throw new Exception("No inactive cards in discard pile!");
            cardBelow = card;
            PrepareCardInPile(pileCard.transform, drawPile.transform);
            pileCard.SetActive(true);
        }

        public List<CharacterConfig> AllOutsideCharacters()
        {
            List<CharacterConfig> list = new List<CharacterConfig>();
            foreach (CardImage image in enabledCards) list.Add(image.Character);
            foreach (CardImage image in disabledCards) list.Add(image.Character);
            foreach (CardImage image in pileCards) list.Add(image.Character);
            if (cardBelow != null) list.Add(cardBelow.Character);
            foreach (CardImage image in discardedCards) list.Add(image.Character);
            //Debug.Log("AllOutsideCharacters count: " + list.Count);
            return list;
        }

        public List<CardImage> AllOutsideCards()
        {
            List<CardImage> list = new List<CardImage>();
            foreach (CardImage image in enabledCards) list.Add(image);
            foreach (CardImage image in disabledCards) list.Add(image);
            foreach (CardImage image in pileCards) list.Add(image);
            if (cardBelow != null) list.Add(cardBelow);
            foreach (CardImage image in discardedCards) list.Add(image);
            //Debug.Log("AllOutsideCharacters count: " + list.Count);
            return list;
        }

        public void RemoveCharacter(CharacterConfig character)
        {
            int index = disabledCards.FindIndex(x => x.Character.GetType() == character.GetType());
            if (index >= 0)
            {
                RemoveFromTable(disabledCards[index], true);
                return;
            }
            index = enabledCards.FindIndex(x => x.Character.GetType() == character.GetType());
            if (index >= 0)
            {
                RemoveFromTable(enabledCards[index]);
                return;
            }
            index = pileCards.FindIndex(x => x.Character.GetType() == character.GetType());
            if (index >= 0)
            {
                RemoveFromDrawPile();
                pileCards.Remove(pileCards[index]);
                return;
            }
            index = discardedCards.FindIndex(x => x.Character.GetType() == character.GetType());
            if (index >= 0)
            {
                GameObject card = discardPile.transform.GetChild(index).gameObject;
                if (!card.activeSelf) throw new Exception("Blank card not existing!");
                card.SetActive(false);
                discardedCards.Remove(discardedCards[index]);
                return;
            }
            throw new Exception("Unable to remove the following character: " + character.Name);
        }

        public bool AreThereDeadCards()
        {
            return deadCards.Count > 0;
        }

        public void DisplayDeadCards()
        {
            AdjustDeadScreenView();
            deadScreen.SetActive(true);
        }

        public void AdjustDeadScreenView()
        {
            int cardCount = deadCards.Count;
            switch (cardCount)
            {
                case 0:
                    throw new Exception("The dead screen has no cards!");
                case int n when n > 0 && n <= 7:
                    deadScreenGrid.constraintCount = 1;
                    break;
                case int n when n > 8 && n <= 14:
                    deadScreenGrid.constraintCount = 2;
                    break;
                default:
                    deadScreenGrid.constraintCount = 3;
                    break;
            }
            deadScreenGrid.constraintCount = 1;
        }

        public CardImage FirstDeadCardForOpponent()
        {
            return deadCards[0];
        }

        public void ReviveCard(CardImage card)
        {
            deadCards.Remove(card);
            if (Turn.CurrentAlignment == Alignment.Player) AddToTable(card, playerTable.transform);
            else AddToTable(card, opponentTable.transform);
            deadScreen.SetActive(false);
        }
        #endregion

        #region LookupCard
        public void ShowLookupCard(Sprite sprite, bool ignoreLock = false)
        {
            if (!Turn.InteractableDisabled || ignoreLock) luCard.ShowLookupCard(sprite);
        }

        public void HideLookupCard(bool ignoreLock = false)
        {
            if (!Turn.InteractableDisabled || ignoreLock) luCard.HideLookupCard();
        }
        #endregion

        #region Debug
        /*public void DebugPrintCardCollection(Alignment newTurn)
        {
            if (newTurn == Alignment.Player && enabledCards.Count != playerTable.transform.childCount)
            {
                Debug.LogWarning($"enabledCards: {enabledCards.Count}; playerTable: {playerTable.transform.childCount}");
                foreach (CardImage card in enabledCards)
                {
                    Debug.Log("enabledCards: " + card);
                }
            }
            if (newTurn == Alignment.Opponent && enabledCards.Count != opponentTable.transform.childCount)
            {
                Debug.LogWarning($"enabledCards: {enabledCards.Count}; opponentTable: {opponentTable.transform.childCount}");
                foreach (CardImage card in enabledCards)
                {
                    Debug.Log("enabledCards: " + card);
                }
            }
            if (deadCards.Count != deadScreen.transform.childCount)
            {
                Debug.LogWarning($"deadCards: {enabledCards.Count}; deadScreen: {deadScreen.transform.childCount}");
                foreach (CardImage card in deadCards)
                {
                    Debug.Log("deadCards: " + card);
                }
            }
        }*/

        public void DebugForceRemoveCardFromLists(CardImage card)
        {
            if (!Debug.isDebugBuild) return;
            if (card == null) return;
            if (pileCards.Contains(card))
            {
                pileCards.Remove(card);
                RemoveFromDrawPile();
            }
            if (discardedCards.Contains(card))
            {
                discardedCards.Remove(card);
                for (int i = discardPile.transform.childCount - 1; i >= 0; i--)
                {
                    GameObject pileCard = discardPile.transform.GetChild(i).gameObject;
                    if (!pileCard.activeSelf) continue;
                    pileCard.SetActive(false);
                    break;
                }

            }
            enabledCards.Remove(card);
            deadCards.Remove(card);
            if (cardBelow == card) cardBelow = null;
            card.Unselect();
        }

        public void DebugAssignCardToList(CardImage card, List<CardImage> list)
        {
            if (!Debug.isDebugBuild) return;
            list.Insert(0, card);
        }

        public void DebugDiscardPileCard(CardImage image)
        {
            if (!Debug.isDebugBuild) return;
            DiscardPileCard();
            discardedCards.Add(image);
        }

        public void DebugAddPileCard(CardImage image)
        {
            //Debug.Log("Shuffling to: " + stack.name);
            float offsetUnit = drawPile.transform.childCount * offsetFactor;
            Transform card;
            for (int i = discardPile.transform.childCount - 1; i >= 0; i--)
            {
                card = discardPile.transform.GetChild(i);
                if (card.gameObject.activeSelf) continue;
                card.SetParent(drawPile.transform, false);
                card.localPosition = new Vector3(offsetUnit, offsetUnit, offsetUnit);
                pileCards.Add(image);
                return;
            }
            Debug.LogWarning("Something gone wrong with adding pile card!");
        }
        #endregion
    }
}