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

    public void InitializeCharacters(out List<Character> list)
    {
        CharacterData data = new CharacterData();
        list = data.LoadCharacterData();
        characterCount = list.Count;
        foreach (Character isSupport in list) if (isSupport.Role == Role.Support) Debug.Log(isSupport.Name);
        //cm.LoadCharacters(characterList);
    }


    public void InitializeCardPile(GameObject pile)
    {
        for (int i = 0; i < characterCount; i++) Instantiate(cardBlankPrefab, pile.transform);
    }

    public void InitializeCardImages(GameObject stack, out List<CardImage> cardCollection)
    {
        cardCollection = new List<CardImage>();
        for (int i = 0; i < cardImageCount; i++)
        {
            GameObject cardImage = Instantiate(cardImagePrefab, stack.transform);
            cardImage.name = "Card Image " + (i + 1);
            cardCollection.Add(cardImage.GetComponent<CardImage>());
        }
    }

}
