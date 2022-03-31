using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int cardsCount = 20;
    private const int imageCount = 12;
    private const int tableCapacity = 6;
    private const float offsetFactor = 0.0001f;

    //private List<GameObject> drawPileCards = new List<GameObject>();
    private CardImage[] cardImages = new CardImage[imageCount];
    private List<Sprite> cardSpriteTexture = new List<Sprite>();
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
            InitializeCard(i);
        }
    }

    public void InitializeCard(int offset = 0)
    {
        float offsetUnit = offset * offsetFactor;
        GameObject piledCard = Instantiate(pileCardTemplate, new Vector3(offsetUnit, offsetUnit, offsetUnit), Quaternion.identity);
        //piledCard.name = "pileCardNO" + offset;
        piledCard.transform.SetParent(drawPile.transform, false);
        rb = piledCard.AddComponent<Rigidbody>();
        rb.detectCollisions = true;
        //AddPileCard(piledCard, stack);
    }

    //public void AddPileCard(GameObject card, GameObject stack)
    //{
    //    if (stack == drawPile) drawPileCards.Add(card);
    //    else if (stack == discardPile) drawPileCards.Insert(0, card);
    //    else throw new Exception("Unknown stack for AddPileCard!");
    //}

    //private void PutCardInDiscardPile()
    //{
    //    GameObject piledCard = 
    //}

    private void LoadSpriteMesh()
    {
        string prefixPath = Application.dataPath + "/GameCards/Berty/";
        //Debug.Log(prefixPath);
        cardSpriteTexture.Add(Resources.Load<Sprite>(prefixPath + "misiek bert"));
    }

    public void InitializeCards()
    {
        InstantiateCardImages();
        //InstantiateCardSprites();
        LoadSpriteMesh();
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

    public void PullCards(Alignment align)
    {
        Transform table;
        if (align == Alignment.Player) table = playerTable.transform;
        else table = opponentTable.transform;
        bool isShuffled = false;
        int cardsToPull = tableCapacity;
        cardsToPull -= enabledCards.Count;
        int cardsInDrawPile = drawPile.transform.childCount;
        //int cardsInDiscardPile = discardPile.transform.childCount;
        //Debug.Log("Cards to pull: " + cardsToPull);
        foreach (CardImage drawnCard in cardImages)
        {
            //Debug.Log("Cards to pull in loop: " + cardsToPull);
            if (cardsToPull <= 0) break;
            if (cardsInDrawPile <= 0) break;
            if (drawnCard.TableAssigned() != null) continue;
            //Debug.Log("Pulling a card...");
            DrawCard(drawnCard, table, cardsCount - cardsInDrawPile);
            if (cardsToPull == 3) drawnCard.LoadSprite(cardSpriteTexture[0]); // TEST!
            cardsToPull--;
            cardsInDrawPile--;
            if (isShuffled) ClearCards();
            if (cardsInDrawPile <= 0 && !isShuffled)
            {
                cardsInDrawPile = discardPile.transform.childCount;
                ShuffleDiscardPile();
                isShuffled = true;
            }
        }
        //AddHeldCard(drawnCard, index);
    }

    private void ClearCards()
    {
        GameObject card;
        for (int i = discardPile.transform.childCount - 1; i >= tableCapacity; i--)
        {
            card = discardPile.transform.GetChild(i).gameObject;
            Destroy(card);
        }
    }

    private void DrawCard(CardImage drawnCard, Transform table, int offset)
    {
        RemoveFromDrawPile(offset);
        AddToTable(drawnCard, table);
    }

    private void ShuffleDiscardPile()
    {
        //int discardedCardsCount = discardPile.transform.childCount;
        //RemoveFromDrawPile(discardedCardsCount);
        //CreatePile(discardedCardsCount);
        GameObject card;
        int discardedCardsCount = discardPile.transform.childCount;
        //Debug.Log("Instead of shuffling cards: " + discardPile.transform.childCount);
        //Debug.Log("Shuffled cards: " + cardAmount);
        for (int i = 0; i < cardsCount && discardPile.transform.childCount > 0; i++)
        {
            card = discardPile.transform.GetChild(0).gameObject;
            //Debug.Log("Shuffled card: " + (i) + " : " + card.name);
            if (card.activeSelf) PrepareCardInPile(card.transform, drawPile.transform, i);
            else break;
        }
    }

    private void RemoveFromDrawPile(int offset)
    {
        //GameObject card = drawPileCards[drawPileCards.Count - 1];
        //drawPileCards.Remove(card);
        GameObject card = drawPile.transform.GetChild(drawPile.transform.childCount - 1).gameObject;
        card.SetActive(false);
        PrepareCardInPile(card.transform, discardPile.transform, offset);
        //Destroy(card);
    }

    private void PrepareCardInPile(Transform cardTransform, Transform stack, int offset)
    {
        float offsetUnit = offset * offsetFactor;
        cardTransform.SetParent(stack, false);
        cardTransform.localPosition = new Vector3(offsetUnit, offsetUnit, offsetUnit);
    }

    public void DiscardCards()
    {
        foreach (CardImage card in SelectedCards())
        {
            RemoveFromTable(card);
            DiscardCard();
        }
    }

    public void DiscardCard()
    {
        GameObject card;
        for (int i = 0; i < discardPile.transform.childCount; i++)
        {
            card = discardPile.transform.GetChild(i).gameObject;
            if (!card.activeSelf)
            {
                card.SetActive(true);
                break;
            }
        }
    }

    //private void RemoveFromDrawPile(int cardCount)
    //{
    //    for (int i = 0; i < cardCount; i++)
    //        RemoveFromDrawPile();
    //}

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

    public bool ArePilesEmpty()
    {
        if (drawPile.transform.childCount == 0) return true;
        return false;
    }

    private void SetCardObjectIdle(Transform card, Transform stack)
    {
        card.SetParent(stack, false);
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
