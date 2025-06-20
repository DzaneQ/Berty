using Berty.BoardCards.ConfigData;
using Berty.Characters.Init;
using Berty.Display;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Berty.Gameplay
{
    public class CardInitialization : MonoBehaviour
    {

        [SerializeField] private GameObject cardBlankPrefab;
        [SerializeField] private GameObject cardImagePrefab;
        [SerializeField] private GameObject lookupCard;


        public void InitializeCardPile(GameObject pile, int amount)
        {
            for (int i = 0; i < amount; i++) Instantiate(cardBlankPrefab, pile.transform);
        }

        public void InitializeAllCharacterCards(GameObject stack, out List<HandCardBehaviour> cardCollection)
        {
            CharacterData data = new CharacterData();
            List<CharacterConfig> characters = data.LoadCharacterData();
            cardCollection = new List<HandCardBehaviour>();
            for (int i = 0; i < characters.Count; i++)
            {
                GameObject cardImage = Instantiate(cardImagePrefab, stack.transform);
                cardCollection.Add(cardImage.GetComponent<HandCardBehaviour>());
                cardCollection[i].AssignCharacter(characters[i]);
                cardImage.name = cardCollection[i].Character.Name;
            }
        }

        public LookupCard AttachLookupCard()
        {
            //lookupCard.SetActive(false);
            return lookupCard.GetComponent<LookupCard>();
        }
    }
}