using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardInitialization : MonoBehaviour
{

    private const int deckSize = 40;
    private const int cardImageCount = 12;
    private const int fgColumns = 3;
    private const int fgRows = 3;

    [SerializeField] private GameObject cardBlankPrefab;
    [SerializeField] private GameObject cardImagePrefab;
    [SerializeField] private GameObject cardSpritePrefab;
    [SerializeField] private GameObject drawPile;
    [SerializeField] private GameObject discardPile;
    [SerializeField] private GameObject cardImageCollection;

    private Turn turn;
    private CardManager cm;
    private FieldGrid fg;


    //void Start()
    //{
    //    LoadVariables();
    //    Initialize();
    //    Destroy(this);
    //}

    public void LoadVariables()
    {
        turn = GetComponent<Turn>();
        cm = GetComponent<CardManager>();
        fg = FindObjectOfType<FieldGrid>();
    }

    //public void Initialize()
    //{
    //    InitializeCardManager();
    //    InitializeFieldGrid();
    //}

    //public void InitializeCardManager()
    //{
    //    //cm.SwitchTable(turn.CurrentAlignment);
    //    InitializeCharacters();
    //    InitializeCards();
    //}

    public void InitializeFieldGrid()
    {
        InitializeFields();
        fg.AttachTurn(turn);
    }

    public List<Character> InitializeCharacters()
    {
        CharacterData data = new CharacterData();
        //List<Character> characterList = data.LoadCharacterData();
        return data.LoadCharacterData();
        //cm.LoadCharacters(characterList);
    }

    public void InitializeCards()
    {
        InitializeCardPile();
        InitializeCardImages();
    }

    private void InitializeFields()
    {
        Field[] fields = new Field[fgColumns * fgRows];
        int index = 0;
        int midColumn = (fgColumns - 1) / 2;
        int midRow = (fgRows - 1) / 2;
        for (int i = 0; i < fgColumns; i++)
        {
            for (int j = 0; j < fgRows; j++)
            {
                Transform fieldTransform = fg.transform.GetChild(index);
                //Debug.Log(fieldTransform.localPosition.x + ", " + fieldTransform.localPosition.y);
                fields[index] = fieldTransform.gameObject.GetComponent<Field>();
                fields[index].SetCoordinates(i - midColumn, midRow - j);
                //InitializeCardSprite(fields[index]);
                index++;
            }
        }
        fg.AttachFields(fields);
    }

    private void InitializeCardPile()
    {
        cm.AttachPile(deckSize, drawPile, discardPile);
        for (int i = 0; i < deckSize; i++) Instantiate(cardBlankPrefab, discardPile.transform);
        cm.ShufflePile();
    }

    private void InitializeCardImages()
    {
        CardImage[] cardCollection = new CardImage[cardImageCount];
        for (int i = 0; i < cardCollection.Length; i++)
        {
            GameObject cardImage = Instantiate(cardImagePrefab, cardImageCollection.transform);
            cardCollection[i] = cardImage.GetComponent<CardImage>();
        }
        cm.CardImages = cardCollection;
    }

    //private void InitializeCardSprite(Field field)
    //{
    //    field.OccupantCard = Instantiate(cardSpritePrefab, transform).GetComponent<CardSprite>();
    //}
}
