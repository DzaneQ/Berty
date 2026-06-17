using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using System.Collections.Generic;
using UnityEngine;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers.Client;
using Berty.UI.Managers;
using Berty.Network.Managers;

namespace Berty.UI.Card.Managers.Client
{
    public class ClientHandCardObjectManager : ClientUIObjectManager<ClientHandCardObjectManager>, IHandCardObjectManager
    {
        private GameObject myTable;

        private HandCardCollection behaviourCollection;

        protected override void Awake()
        {
            base.Awake();
            myTable = ObjectReadManager.Instance.PlayerTable;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
            if (behaviourCollection == null) Debug.LogWarning("Failed to get HandCardCollection.");
            else Debug.Log("Attached HandCardCollection.");
        }

        public void AddCardObjects()
        {
            IReadOnlyList<CharacterConfig> ownedCards = NetworkCardPileManager.Instance.MyTable;
            AddCardObjectsFromPileData(myTable.transform, ownedCards);
        }

        public void RemoveCardObjects()
        {
            IReadOnlyList<CharacterConfig> ownedCards = NetworkCardPileManager.Instance.MyTable;
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
