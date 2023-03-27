using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int tableCapacity = 6;
    private const float offsetFactor = 0.0001f;

    private List<CardImage> unassignedCards;
    private List<CardImage> enabledCards = new List<CardImage>();
    private List<CardImage> disabledCards = new List<CardImage>();
    private List<Character> characterPile;
    private List<Character> discardedCharacters;
    private List<Character> charactersBelow = new List<Character>();
    private readonly System.Random rng = new System.Random();


    public List<CardImage> EnabledCards => enabledCards;

    [SerializeField] private GameObject drawPile;
    [SerializeField] private GameObject discardPile;
    [SerializeField] private GameObject playerTable;
    [SerializeField] private GameObject opponentTable;
    [SerializeField] private GameObject cardImageCollection;

    private void Start()
    {
        CardInitialization init = GetComponent<CardInitialization>();
        init.InitializeCharacters(out discardedCharacters);
        init.InitializeCardPile(discardPile);
        ShufflePile();
        init.InitializeCardImages(cardImageCollection, out unassignedCards);
        Destroy(init);
    }

    public void ShufflePile()
    {
        ShuffleDiscardPile();
    }

    public bool PullCards(Alignment align)
    {
        Transform table = align == Alignment.Player ? playerTable.transform : opponentTable.transform;
        int cardsToPull = tableCapacity - enabledCards.Count;
        for (int i = cardsToPull; i > 0; i--)
        {
            if (!PullCard(table)) return false;
        }
        return true;
    }

    public bool PullCard(Alignment align)
    {
        Transform table = align == Alignment.Player ? playerTable.transform : opponentTable.transform;
        return PullCard(table);
    }

    private bool PullCard(Transform table)
    {
        Character character = TakeFromPile();
        if (character != null) AddCardToTable(table, character);
        else return false;
        return true;
    }

    private Character TakeFromPile()
    {
        if (drawPile.transform.childCount == 0 && !ShuffleDiscardPile()) return null;
        if (characterPile.Count > 0) return TakeRandomCharacter();
        return TakeRemainingCharacter();
    }

    private Character TakeRandomCharacter()
    {
        Character character = characterPile[rng.Next(characterPile.Count)];
        if (drawPile.transform.childCount == 40) character = characterPile.Find(x => x is KoszmarZBertwood); // Testing: Force a card!
        //if (drawPile.transform.childCount == 39) character = characterPile.Find(x => x is BertaSJW); // Testing: Force another card!
        //if (drawPile.transform.childCount == 34) character = characterPile.Find(x => x is PapiezBertII); // Testing: Force opposing card!
        characterPile.Remove(character);
        RemoveFromDrawPile();
        return character;
    }

    private Character TakeRemainingCharacter()
    {
        Character character = charactersBelow[0];
        charactersBelow.Remove(character);
        RemoveFromDrawPile();
        return character;
    }

    private void AddCardToTable(Transform table, Character character)
    {
        Debug.Log("Amount of unassigned cards: " + unassignedCards.Count);
        Debug.Log("Size of character pile: " + characterPile.Count);
        CardImage cardSubject = unassignedCards[0];
        cardSubject.AssignCharacter(character);
        AddToTable(cardSubject, table);
    }

    private bool ShuffleDiscardPile()
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
        characterPile = discardedCharacters;
        discardedCharacters = new List<Character>();
        Debug.Log("Character pile count: " + characterPile.Count);
        return characterPile.Count != 0;
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
            discardedCharacters.Add(card.Character);
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

    public void RemoveFromTable(CardImage card)
    {
        card.SetBackupTable();
        SetCardObjectIdle(card.transform, cardImageCollection.transform);

        enabledCards.Remove(card);
        unassignedCards.Add(card);
    }

    public void AddToTable(CardImage card, Transform table)
    {
        card.transform.SetParent(table, false);
        card.AssignTable();
        unassignedCards.Remove(card);
        enabledCards.Add(card);
    }

    private void SetCardObjectIdle(Transform card, Transform stack)
    {
        card.SetParent(stack, false);
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
        foreach (CardImage card in SelectedCards()) card.ChangePosition();
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

    public void ReturnCharacter(Character character)
    {
        GameObject pileCard = discardPile.transform.GetChild(discardPile.transform.childCount - 1).gameObject;
        if (pileCard.activeSelf) throw new Exception("No inactive cards in discard pile!");
        charactersBelow.Add(character);
        PrepareCardInPile(pileCard.transform, drawPile.transform);
        pileCard.SetActive(true);
    }

}
