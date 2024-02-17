using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour // BUG: Discarded cards go to pile cards.
{
    private const int tableCapacity = 6;
    private const float offsetFactor = 0.0001f;

    private List<CardImage> pileCards = new List<CardImage>();
    private List<CardImage> discardedCards;
    private List<CardImage> enabledCards = new List<CardImage>();
    private List<CardImage> disabledCards = new List<CardImage>();
    //private List<CardImage> fieldCards = new List<CardImage>();
    private List<CardImage> deadCards = new List<CardImage>();
    //private List<Character> characterPile;
    //private List<Character> discardedCharacters;
    //private List<Character> charactersBelow = new List<Character>();
    private CardImage cardBelow;
    private readonly System.Random rng = new System.Random();
    private Alignment tempAlign = Alignment.None;
    GridLayoutGroup deadScreenGrid;


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
        CardInitialization init = GetComponent<CardInitialization>();
        //init.InitializeCharacters(out discardedCharacters);
        init.InitializeAllCharacterCards(cardImageCollection, out discardedCards);
        init.InitializeCardPile(discardPile, discardedCards.Count);
        ShufflePile();
        //init.InitializeCardImages(cardImageCollection, out discardedCards);
        Destroy(init);
        deadScreenGrid = deadScreen.GetComponent<GridLayoutGroup>();
    }

    public bool PullCards(Alignment align)
    {
        //Transform table = align == Alignment.Player ? playerTable.transform : opponentTable.transform;
        int cardsToPull = tableCapacity - enabledCards.Count;
        //Debug.Log("Cards to pull: " + cardsToPull);
        //foreach (CardImage card in enabledCards)
        //{
        //    Debug.Log("Enabled card: " + card.name);
        //}
        for (int i = cardsToPull; i > 0; i--) if (!PullCard(align)) return false;
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
        if (drawPile.transform.childCount == 40) character = pileCards.Find(x => x.Character is GotkaBerta); // Testing: Force a card!
        //if (drawPile.transform.childCount == 39) chosenCard = pileCards.Find(x => x.Character is SedziaBertt); // Testing: Force another card!
        //if (drawPile.transform.childCount == 34) chosenCard = pileCards.Find(x => x.Character is KsiezniczkaBerta); // Testing: Force opposing card!
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


    //private void AddCardToTable(Transform table, CardImage character) // IMPORTANT: Change this code!
    //{
    //    //Debug.Log("Amount of unassigned cards: " + unassignedCards.Count);
    //    //Debug.Log("Size of character pile: " + characterPile.Count);
    //    CardImage cardSubject = character;
    //    cardSubject.AssignCharacter(character);
    //    AddToTable(cardSubject, table);
    //}

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
        Debug.Log("Character pile count: " + pileCards.Count);
        return pileCards.Count != 0;
    }

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

    public void DiscardCards()
    {
        foreach (CardImage card in SelectedCards())
        {
            RemoveFromTable(card);
            discardedCards.Add(card);
            DiscardPileCard();
        }
    }

    public void DiscardPileCard()
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

    /*public void PlaceFromTable(CardImage card)
    {
        card.SetBackupTable();
        SetCardObjectIdle(card.transform);
        Debug.Log($"{card.name}: Table placement");
        enabledCards.Remove(card);
        if (pileCards.Count + discardedCards.Count != cardImageCollection.transform.childCount)
        {
            Debug.Log($"Pile cards: {pileCards.Count}; Discarded cards: {discardedCards.Count}; Collection: {cardImageCollection.transform.childCount}");
        }
    }*/

    public void RemoveFromTable(CardImage card, bool disabledCard = false)
    {
        card.SetBackupTable();
        SetCardObjectIdle(card.transform);
        Debug.Log($"{card.name}: Table removal for enabled ones: {disabledCard}");
        if (!disabledCard) enabledCards.Remove(card);
        else disabledCards.Remove(card);
        if (pileCards.Count + discardedCards.Count != cardImageCollection.transform.childCount)
        {
            Debug.Log($"Pile cards: {pileCards.Count}; Discarded cards: {discardedCards.Count}; Collection: {cardImageCollection.transform.childCount}");
        }
    }

    public void AddToTable(CardImage card, Transform table)
    {
        card.transform.SetParent(table, false);
        card.Unselect();
        Debug.Log($"{card.name}: Table addition.");
        //if (pileCards.Contains(card)) pileCards.Remove(card);
        //else if (cardBelow == card) cardBelow = null;
        //else throw new Exception("Card to add doesn't exist anywhere.");
        enabledCards.Add(card);
        if (enabledCards.Count != playerTable.transform.childCount && enabledCards.Count != opponentTable.transform.childCount)
            Debug.LogWarning("Enabled cards not equal to any table!");
    }

    //public void AddToField(CardImage card)
    //{
    //    fieldCards.Add(card);
    //}

    //public void RemoveFromField(CardImage card)
    //{
    //    fieldCards.Remove(card);
    //}

    private void SetCardObjectIdle(Transform card)
    {
        card.SetParent(cardImageCollection.transform, false);
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

    public void DeselectCards()
    {
        foreach (CardImage card in SelectedCards()) card.ChangeSelection();
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

    public void KillCard(CardImage card)
    {
        deadCards.Add(card);
        card.transform.SetParent(deadScreen.transform, false);
        //card.KillCard();
    }

    public void ReturnCharacter(CardImage card)
    {
        if (cardBelow != null) throw new Exception("There's a set card below!");
        GameObject pileCard = discardPile.transform.GetChild(discardPile.transform.childCount - 1).gameObject;
        if (pileCard.activeSelf) throw new Exception("No inactive cards in discard pile!");
        cardBelow = card;
        PrepareCardInPile(pileCard.transform, drawPile.transform);
        pileCard.SetActive(true);
    }

    public List<Character> AllOutsideCharacters()
    {
        List<Character> list = new List<Character>();
        foreach (CardImage image in enabledCards) list.Add(image.Character);
        foreach (CardImage image in disabledCards) list.Add(image.Character);
        foreach (CardImage image in pileCards) list.Add(image.Character);
        if (cardBelow != null) list.Add(cardBelow.Character);
        foreach (CardImage image in discardedCards) list.Add(image.Character);
        //list.AddRange(characterPile);
        //list.AddRange(discardedCharacters);
        //list.AddRange(charactersBelow); // experimental
        //foreach (Character character in charactersBelow) Debug.Log("charactersBelow list contains character: " + character.Name);
        //foreach (Character character in list) Debug.Log("AllCharacters list contains character: " + character.Name);
        //Debug.Log("AllOutsideCharacters count: " + list.Count);
        return list;
    }

    public void RemoveCharacter(Character character)
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
        index = pileCards.FindIndex(x => x.GetType() == character.GetType());
        if (index >= 0)
        {
            RemoveFromDrawPile();
            pileCards.Remove(pileCards[index]);
            return;
        }
        index = discardedCards.FindIndex(x => x.GetType() == character.GetType());
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

    public void DisplayDeadCards(Alignment decidingAlign)
    {
        tempAlign = decidingAlign;
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
        tempAlign = Alignment.Opponent;
        return deadCards[0];
    }

    private GameObject GetIntendedTable()
    {
        switch (tempAlign)
        {
            case Alignment.Player:
                return playerTable;
            case Alignment.Opponent:
                return opponentTable;
            default:
                throw new Exception("Trying to pick unknown align.");
        }
    }

    public void ReviveCard(CardImage card)
    {
        deadCards.Remove(card);
        AddToTable(card, GetIntendedTable().transform);
        deadScreen.SetActive(false);
        tempAlign = Alignment.None;      
    }

    public void DebugPrintCardCollection(Alignment newTurn)
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
        /*if (pileCards.Count + discardedCards.Count != cardImageCollection.transform.childCount)
        {
            Debug.LogWarning($"pileCards: {pileCards.Count}; discardedCards: {discardedCards.Count}; 
        cardImageCollection: {cardImageCollection.transform.childCount}");
            foreach (CardImage card in pileCards)
            {
                Debug.Log("pileCards: " + card);
            }
            foreach (CardImage card in discardedCards)
            {
                Debug.Log("discardedCards: " + card);
            }
        }*/
    }
}
