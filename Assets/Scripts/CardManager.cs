using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int cardsCount = 40;
    private const int imageCount = 12;
    private const int tableCapacity = 6;

    private List<GameObject> drawPileCards = new List<GameObject>();
    private CardImage[] cardImages = new CardImage[imageCount];
    private Rigidbody rb;
    //private CardImage[] heldCards = new CardImage[6];
    //private GameObject[] heldCards = new GameObject[6];
    //private List<GameObject> heldCards = new List<GameObject>();
    //private CardImage[] heldImages = new CardImage[6];
    private List<CardImage> enabledCards = new List<CardImage>();
    private List<CardImage> disabledCards = new List<CardImage>();

    [SerializeField] private GameObject PiledCard;
    [SerializeField] private GameObject DrawPile;
    [SerializeField] private GameObject DiscardPile;
    [SerializeField] private GameObject playerTable;
    [SerializeField] private GameObject opponentTable;
    [SerializeField] private GameObject cardImage;

    //public void AddHeldCard(CardImage card, int index)
    //{
    //    playerCards.Add(card);
    //}

    private void InitializeCardPile()
    {
        for (int i = 0; i < cardsCount; i++)
        {
            AddToPile(DrawPile, i);
        }
    }
    public void AddToPile(GameObject stack, int offset = 0)
    {
        GameObject piledCard = Instantiate(PiledCard, new Vector3(offset * 0.0001f, -offset * 0.0001f, offset * 0.0001f), Quaternion.identity);
        piledCard.transform.SetParent(stack.transform, false);
        rb = piledCard.AddComponent<Rigidbody>();
        rb.detectCollisions = true;
        AddPileCard(piledCard, stack);
    }

    public void AddPileCard(GameObject card, GameObject stack)
    {
        if (stack == DrawPile) drawPileCards.Add(card);
        else if (stack == DiscardPile) drawPileCards.Insert(0, card);
        else throw new Exception("Unknown stack for AddPileCard!");
    }

    public void InitializeCards()
    {
        for (int i = 0; i < cardImages.Length; i++)
        {

            GameObject drawnCard = Instantiate(cardImage, new Vector3(0, 0, 0), Quaternion.identity);
            cardImages[i] = drawnCard.GetComponent<CardImage>();
            //drawnCard.name = "Card " + i.ToString();
            //AddToPile(DrawPile, i);
        }
        playerTable.SetActive(true);
        opponentTable.SetActive(false);
        InitializeCardPile();
    }

    public void PullCards(bool isPlayerTurn)
    {
        Transform table;
        List<CardImage> heldCards;
        int cardsToPull = tableCapacity;
        if (isPlayerTurn)
        {
            table = playerTable.transform;
            //cardsToPull -= enabledCards.Count;
            //heldCards = enabledCards;
        }
        else
        {
            table = opponentTable.transform;
            //cardsToPull -= disabledCards.Count;
            //heldCards = disabledCards;
        }
        cardsToPull -= enabledCards.Count;
        heldCards = enabledCards;
        Debug.Log("Cards to pull: " + cardsToPull);
        foreach (CardImage drawnCard in cardImages)
        {
            if (cardsToPull <= 0) break;
            if (drawnCard.TableAssigned() != null) continue;
            RemoveFromDrawPile();
            drawnCard.transform.SetParent(table, false);
            drawnCard.AssignTable();
            heldCards.Add(drawnCard);
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
        enabledCards.Remove(card);
        //disabledCards.Remove(card);
    }

    public void DiscardCards()
    {
        foreach (CardImage card in SelectedCards())
        {
            card.DeactivateCard();
            RemoveFromTable(card);
            AddToPile(DiscardPile);
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
        foreach (CardImage card in SelectedCards()) card.DeselectCard();
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
