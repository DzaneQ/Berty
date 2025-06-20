using Berty.BoardCards.ConfigData;
using Berty.Characters.Init;
using Berty.Display;
using Berty.UI.Card;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Berty.UI.Card.Init
{
    public class HandCardInitialization : MonoBehaviour
    {
        [SerializeField] private GameObject cardImagePrefab;

        private void Awake()
        {
            if (cardImagePrefab != null) cardImagePrefab = Resources.Load<GameObject>("Prefabs/CardImage");
        }

        public List<CharacterConfig> InitializeAllCharacterCards(GameObject stack, out List<HandCardBehaviour> behaviourCollection)
        {
            CharacterData data = new CharacterData();
            List<CharacterConfig> characters = data.LoadCharacterData();
            behaviourCollection = new List<HandCardBehaviour>();
            for (int i = 0; i < characters.Count; i++)
            {
                GameObject handCardObject = Instantiate(cardImagePrefab, stack.transform);
                HandCardBehaviour handCardBehaviour = handCardObject.GetComponent<HandCardBehaviour>() ?? handCardObject.AddComponent<HandCardBehaviour>();
                handCardBehaviour.AssignCharacter(characters[i]);
                handCardBehaviour.name = characters[i].Name;
                behaviourCollection.Add(handCardBehaviour);
            }
            return characters;
        }
    }
}