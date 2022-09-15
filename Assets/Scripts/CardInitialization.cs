using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardInitialization : MonoBehaviour
{

    private const int cardImageCount = 12;

    private int characterCount;
    [SerializeField] private GameObject cardBlankPrefab;
    [SerializeField] private GameObject cardImagePrefab;
    [SerializeField] private GameObject cardSpritePrefab;




    //void Start()
    //{
    //    LoadVariables();
    //    Initialize();
    //    Destroy(this);
    //}




    public void InitializeCharacters(out List<Character> list)
    {
        CharacterData data = new CharacterData();
        list = data.LoadCharacterData();
        characterCount = list.Count;
        //cm.LoadCharacters(characterList);
    }


    public void InitializeCardPile(GameObject pile)
    {
        for (int i = 0; i < characterCount; i++) Instantiate(cardBlankPrefab, pile.transform);
    }

    public void InitializeCardImages(GameObject stack, out CardImage[] cardCollection)
    {
        cardCollection = new CardImage[cardImageCount];
        for (int i = 0; i < cardCollection.Length; i++)
        {
            GameObject cardImage = Instantiate(cardImagePrefab, stack.transform);
            cardImage.name = "Card Image " + (i + 1);
            cardCollection[i] = cardImage.GetComponent<CardImage>();
        }
    }

}
