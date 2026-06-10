using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using System.Collections.Generic;
using UnityEngine;
using Berty.Gameplay.Entities;
using Berty.Utility;
using Berty.Gameplay.Managers.Client;
using Berty.UI.Managers;

namespace Berty.UI.Card.Managers.Client
{
    public class ClientHandCardObjectManager : ClientUIObjectManager<ClientHandCardObjectManager>, IHandCardObjectManager
    {
        private GameObject myTable;

        private HandCardCollection behaviourCollection;
        private Game game;
        private CardPile CardPile => game.CardPile;

        protected override void Awake()
        {
            base.Awake();
            myTable = ObjectReadManager.Instance.PlayerTable;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
            game = EntityLoadManager.Instance.Game; // TODO: Change so it's not locally obtained
            if (behaviourCollection == null) Debug.LogWarning("Failed to get HandCardCollection.");
        }

        public void AddCardObjects()
        {
            IReadOnlyList<CharacterConfig> ownedCards = CardPile.GetCardsFromAlign(PlayerReadManager.Instance.MyAlignment);
            AddCardObjectsFromPileData(myTable.transform, ownedCards);
        }

        public void RemoveCardObjects()
        {
            IReadOnlyList<CharacterConfig> ownedCards = CardPile.GetCardsFromAlign(PlayerReadManager.Instance.MyAlignment);
            RemoveCardObjectsFromTable(myTable.transform, ownedCards);
        }

        public Sprite GetSpriteFromHandCardObject(CharacterConfig characterConfig)
        {
            return behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).Sprite;
        }

        public void AddCardObjectFromConfig(CharacterConfig characterConfig)
        {
            Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).transform;
            card.SetParent(myTable.transform, false);
        }

        private void AddCardObjectsFromPileData(Transform table, IReadOnlyList<CharacterConfig> pileData)
        {
            if (behaviourCollection == null) Debug.LogWarning("Trying to add card objects without collection!");
            for (int i = 0; i < pileData.Count; i++)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(pileData[i]).transform;
                if (card.parent == table) continue;
                card.SetParent(table, false);
            }
        }

        private void RemoveCardObjectsFromTable(Transform table, IReadOnlyList<CharacterConfig> pileData)
        {
            List<Transform> ownedCardTransforms = behaviourCollection.GetTransformListFromCharacterConfigs(pileData);
            int tableCount = table.childCount;
            for (int i = tableCount - 1; 0 <= i; i--)
            {
                Transform card = table.GetChild(i);
                if (ownedCardTransforms.Contains(card)) continue;
                card.SetParent(behaviourCollection.transform, false);
            }
        }
    }
}
