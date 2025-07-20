using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Card.Systems;
using Berty.UI.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
            cardPile = CoreManager.Instance.Game.CardPile;
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

        private void AddCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            List<CharacterConfig> ownedCards = cardPile.GetCardsFromAlign(alignment);
            AddCardObjectsFromPileData(table, ownedCards);
        }

        private void RemoveCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            List<CharacterConfig> ownedCards = cardPile.GetCardsFromAlign(alignment);
            RemoveCardObjectsFromTable(table, ownedCards);
        }

        private void AddCardObjectsFromPileData(Transform table, List<CharacterConfig> pileData)
        {
            for (int i = 0; i < pileData.Count; i++)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(pileData[i]).transform;
                if (card.parent == table) continue;
                card.SetParent(table);
            }
        }

        private void RemoveCardObjectsFromTable(Transform table, List<CharacterConfig> pileData)
        {
            List<Transform> ownedCardTransforms = behaviourCollection.GetTransformListFromCharacterConfigs(pileData);
            for (int i = 0; i < table.childCount; i++)
            {
                Transform card = table.GetChild(i);
                if (ownedCardTransforms.Contains(card)) continue;
                card.SetParent(behaviourCollection.transform);
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
