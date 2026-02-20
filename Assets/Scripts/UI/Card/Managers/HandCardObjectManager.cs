using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.UI.Card.Managers
{
    public class HandCardObjectManager : UIObjectManager<HandCardObjectManager>
    {
        [SerializeField] private GameObject playerTable;
        [SerializeField] private GameObject opponentTable;
        
        private HandCardCollection behaviourCollection;
        private CardPile cardPile;

        protected override void Awake()
        {
            base.Awake();
            playerTable = ObjectReadManager.Instance.PlayerTable;
            opponentTable = ObjectReadManager.Instance.OpponentTable;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
            cardPile = EntityLoadManager.Instance.Game.CardPile;
        }

        public void SwitchTables()
        {
            playerTable.SetActive(!playerTable.activeSelf);
            opponentTable.SetActive(!playerTable.activeSelf);
        }

        public void AddCardObjects()
        {
            if (playerTable.activeSelf) AddCardObjectsForTable(AlignmentEnum.Player);
            if (opponentTable.activeSelf) AddCardObjectsForTable(AlignmentEnum.Opponent);
        }

        public void RemoveCardObjects()
        {
            if (playerTable.activeSelf) RemoveCardObjectsForTable(AlignmentEnum.Player);
            if (opponentTable.activeSelf) RemoveCardObjectsForTable(AlignmentEnum.Opponent);
        }

        public Sprite GetSpriteFromHandCardObject(CharacterConfig characterConfig)
        {
            return behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).Sprite;
        }

        public void AddCardObjectFromConfigForTable(CharacterConfig characterConfig, AlignmentEnum alignment)
        {
            Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).transform;
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            card.SetParent(table);
        }

        private void AddCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            IReadOnlyList<CharacterConfig> ownedCards = cardPile.GetCardsFromAlign(alignment);
            AddCardObjectsFromPileData(table, ownedCards);
        }

        private void RemoveCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            IReadOnlyList<CharacterConfig> ownedCards = cardPile.GetCardsFromAlign(alignment);
            RemoveCardObjectsFromTable(table, ownedCards);
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

        private GameObject GetTableObjectFromAlignment(AlignmentEnum alignment)
        {
            return alignment switch
            {
                AlignmentEnum.Player => playerTable,
                AlignmentEnum.Opponent => opponentTable,
                _ => throw new InvalidOperationException("Invalid align to call table object.")
            };
        }
    }
}
