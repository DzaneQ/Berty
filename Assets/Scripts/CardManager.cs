using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    private const int cardsCount = 40;
    private const int imageCount = 12;
    private const int tableCapacity = 6;
    private const float offsetFactor = 0.0001f;

    private CardImage[] cardImages = new CardImage[imageCount];
    private List<CardImage> enabledCards = new List<CardImage>();
    private List<CardImage> disabledCards = new List<CardImage>();
    private List<Character> characterPile = new List<Character>();
    private List<Character> discardedCharacters = new List<Character>();
    private readonly System.Random rng = new System.Random();

    public List<CardImage> EnabledCards { get => enabledCards; }


    [SerializeField] private GameObject pileCardTemplate;
    [SerializeField] private GameObject drawPile;
    [SerializeField] private GameObject discardPile;
    [SerializeField] private GameObject playerTable;
    [SerializeField] private GameObject opponentTable;
    [SerializeField] private GameObject cardImageTemplate;
    [SerializeField] private GameObject cardImageCollection;

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
    }

    public void InitializeCards()
    {
        InstantiateCardImages();
        playerTable.SetActive(true);
        opponentTable.SetActive(false);
        LoadCharacters();
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

    private void LoadCharacters()
    {
        CharacterData data = new CharacterData();
        if (Debug.isDebugBuild) characterPile = data.LoadDataForBuild();
        else
        { 
            string path = Application.dataPath + "/Resources/BERTY/";
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles("*.png");
            foreach (FileInfo file in files)
            {
                string fileName = file.ToString();
                fileName = fileName.Substring(fileName.LastIndexOf('\\') + 1);
                fileName = fileName.Substring(0, fileName.IndexOf('.'));
                data.LoadCharacter(characterPile, fileName);
            }
        }
    }

    public void PullCards(Alignment align)
    {
        Transform table;
        if (align == Alignment.Player) table = playerTable.transform;
        else table = opponentTable.transform;
        bool isShuffled = false;
        int cardsToPull = tableCapacity;
        cardsToPull -= enabledCards.Count;
        int cardsInDrawPile = drawPile.transform.childCount;
        //Debug.Log("Cards to pull: " + cardsToPull);
        foreach (CardImage drawnCard in cardImages)
        {
            //Debug.Log("Cards to pull in loop: " + cardsToPull);
            if (cardsToPull <= 0) break;
            if (cardsInDrawPile <= 0) break;
            if (drawnCard.TableAssigned() != null) continue;
            //Debug.Log("Pulling a card...");
            DrawCard(drawnCard, table, cardsCount - cardsInDrawPile);
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

    private void DrawCard(CardImage drawnCard, Transform table, int discardOffset)
    {
        int index = rng.Next(characterPile.Count);
        Character character = characterPile[index];
        RemoveFromDrawPile(discardOffset);
        drawnCard.AssignCharacter(character);
        characterPile.Remove(character);
        AddToTable(drawnCard, table);
    }

    private void ShuffleDiscardPile()
    {
        GameObject card;
        int discardedCardsCount = discardPile.transform.childCount;
        for (int i = 0; i < cardsCount && discardPile.transform.childCount > 0; i++)
        {
            card = discardPile.transform.GetChild(0).gameObject;
            //Debug.Log("Shuffled card: " + (i) + " : " + card.name);
            if (card.activeSelf) PrepareCardInPile(card.transform, drawPile.transform, i);
            else break;
        }
        characterPile = discardedCharacters;
        discardedCharacters = new List<Character>();
    }

    private void RemoveFromDrawPile(int discardOffset)
    {
        GameObject card = drawPile.transform.GetChild(drawPile.transform.childCount - 1).gameObject;
        card.SetActive(false);
        PrepareCardInPile(card.transform, discardPile.transform, discardOffset);
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
            discardedCharacters.Add(card.Character);
            DiscardPileCard();
        }
    }

    public void DiscardPileCard()
    {
        GameObject pileCard;
        Debug.Log("Card in pile discarded");
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
        card.DisableCard();
        SetCardObjectIdle(card.transform, cardImageCollection.transform);
        enabledCards.Remove(card);
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

    public void ShowTable(Alignment alignment)
    {
        if (alignment == Alignment.Player)
        {
            playerTable.SetActive(true);
            opponentTable.SetActive(false);
        }
        else
        {
            playerTable.SetActive(false);
            opponentTable.SetActive(true);
        }

        List<CardImage> temp = enabledCards;
        enabledCards = disabledCards;
        disabledCards = temp;

        //foreach (CardImage card in enabledCards) Debug.Log("Enabled card: " + card.name);
        //foreach (CardImage card in disabledCards) Debug.Log("Disabled card: " + card.name);
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

}
