using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int cardsCount = 40;
    private const int imageCount = 12;
    private const int spriteCount = 9;
    private const int tableCapacity = 6;

    private List<GameObject> drawPileCards = new List<GameObject>();
    private CardImage[] cardImages = new CardImage[imageCount];
    //private CardSprite[] cardSprites = new CardSprite[spriteCount];
    private Rigidbody rb;
    //private CardImage[] heldCards = new CardImage[6];
    //private GameObject[] heldCards = new GameObject[6];
    //private List<GameObject> heldCards = new List<GameObject>();
    //private CardImage[] heldImages = new CardImage[6];
    private List<CardImage> enabledCards = new List<CardImage>();
    private List<CardImage> disabledCards = new List<CardImage>();

    [SerializeField] private GameObject pileCardTemplate;
    [SerializeField] private GameObject drawPile;
    [SerializeField] private GameObject discardPile;
    [SerializeField] private GameObject playerTable;
    [SerializeField] private GameObject opponentTable;
    [SerializeField] private GameObject cardImageTemplate;
    //[SerializeField] private GameObject cardSpriteTemplate;
    [SerializeField] private GameObject cardImageCollection;
    //[SerializeField] private GameObject cardSpriteCollection;

    //public void AddHeldCard(CardImage card, int index)
    //{
    //    playerCards.Add(card);
    //}

    private void InitializeCardPile()
    {
        for (int i = 0; i < cardsCount; i++)
        {
            AddToPile(drawPile, i);
        }
    }
    public void AddToPile(GameObject stack, int offset = 0)
    {
        GameObject piledCard = Instantiate(pileCardTemplate, new Vector3(offset * 0.0001f, -offset * 0.0001f, offset * 0.0001f), Quaternion.identity);
        piledCard.transform.SetParent(stack.transform, false);
        rb = piledCard.AddComponent<Rigidbody>();
        rb.detectCollisions = true;
        AddPileCard(piledCard, stack);
    }

    public void AddPileCard(GameObject card, GameObject stack)
    {
        if (stack == drawPile) drawPileCards.Add(card);
        else if (stack == discardPile) drawPileCards.Insert(0, card);
        else throw new Exception("Unknown stack for AddPileCard!");
    }

    public void InitializeCards()
    {
        InstantiateCardImages();
        //InstantiateCardSprites();

        playerTable.SetActive(true);
        opponentTable.SetActive(false);
        InitializeCardPile();
    }

    private void InstantiateCardImages()
    {
        for (int i = 0; i < cardImages.Length; i++)
        {
            GameObject cardImage = Instantiate(cardImageTemplate, new Vector3(0, 0, 0), Quaternion.identity);
            cardImages[i] = cardImage.GetComponent<CardImage>();
            cardImage.transform.SetParent(cardImageCollection.transform);
        }
    }

    //private void InstantiateCardSprites()
    //{
    //    for (int i = 0; i < cardSprites.Length; i++)
    //    {
    //        GameObject cardSprite = Instantiate(cardSpriteTemplate, new Vector3(0, 0, 0.001f), Quaternion.Euler(0, 180f, 180f));
    //        cardSprites[i] = cardSprite.GetComponent<CardSprite>();
    //        cardSprite.transform.SetParent(cardSpriteCollection.transform);
    //    }
    //}

    public void PullCards(TurnManager.Alignment align)
    {
        Transform table;
        int cardsToPull = tableCapacity;
        if (align == TurnManager.Alignment.Player) table = playerTable.transform;
        else table = opponentTable.transform;
        cardsToPull -= enabledCards.Count;
        //Debug.Log("Cards to pull: " + cardsToPull);
        foreach (CardImage drawnCard in cardImages)
        {
            //Debug.Log("Cards to pull in loop: " + cardsToPull);
            if (cardsToPull <= 0) break;
            if (drawnCard.TableAssigned() != null) continue;
            //Debug.Log("Pulling a card...");
            RemoveFromDrawPile();
            AddToTable(drawnCard, table);
            cardsToPull--;
        }
        //AddHeldCard(drawnCard, index);
    }

    public void SwitchTables()
    {
        playerTable.SetActive(!playerTable.activeSelf);
        opponentTable.SetActive(!opponentTable.activeSelf);

        List<CardImage> temp = enabledCards;
        enabledCards = disabledCards;
        disabledCards = temp;

        //foreach (CardImage card in enabledCards) Debug.Log("Enabled card: " + card.name);
        //foreach (CardImage card in disabledCards) Debug.Log("Disabled card: " + card.name);
    }

    private void RemoveFromDrawPile()
    {
        GameObject card = drawPileCards[drawPileCards.Count - 1];
        drawPileCards.Remove(card);
        Destroy(card);
    }

    public void RemoveFromTable(CardImage card)
    {
        card.DisableCard();
        SetCardObjectIdle(card.transform, cardImageCollection.transform);
        enabledCards.Remove(card);
        //disabledCards.Remove(card);
    }

    public void AddToTable(CardImage card, Transform table)
    {
        card.transform.SetParent(table, false);
        card.AssignTable();
        enabledCards.Add(card);
    }

    private void SetCardObjectIdle(Transform card, Transform stack)
    {
        card.SetParent(stack, false);
    }

    //public GameObject GetCardSprite()
    //{
    //    return cardSpriteCollection.transform.GetChild(0).gameObject;
    //}

    public void DiscardCards()
    {
        foreach (CardImage card in SelectedCards())
        {
            RemoveFromTable(card);
            AddToPile(discardPile);
        }
    }

    //public bool CheckCardSelection(int amount)
    //{
    //    int cardsSelected = 0;
    //    foreach (CardImage card in heldImages)
    //    {
    //        if (card.isSelected)
    //        {
    //            cardsSelected++;
    //            if (cardsSelected >= amount) return true;
    //        }
    //    }
    //    return false;
    //}

    //public int SelectedCount()
    //{
    //    int cardsSelected = 0;
    //    foreach (CardImage card in heldImages)
    //    {
    //        if (card.isSelected) cardsSelected++;
    //    }
    //    return cardsSelected;
    //}

    public void DeselectCards()
    {
        foreach (CardImage card in SelectedCards()) card.DeselectPosition();
    }
        
    public List<CardImage> SelectedCards()
    {
        List<CardImage> selectedCards = new List<CardImage>();
        //Debug.Log("Count before: " + selectedCards.Count);
        foreach (CardImage card in enabledCards) if (card.IsCardSelected()) selectedCards.Add(card);
        //foreach (CardImage card in disabledCards) if (card.IsCardSelected()) selectedCards.Add(card);
        //Debug.Log("Count after: " + selectedCards.Count);
        return selectedCards;
    }

    public CardImage SelectedCard()
    {
        foreach (CardImage card in enabledCards)
            if (card.IsCardSelected()) return card;
        //foreach (CardImage card in disabledCards)
        //    if (card.IsCardSelected()) return card;
        return null;
    }

}
