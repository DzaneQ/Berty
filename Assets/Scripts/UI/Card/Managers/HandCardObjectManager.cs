using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Collection;
using Berty.UI.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
using Berty.Gameplay.Entities;

namespace Berty.UI.Card.Managers
{
    public class HandCardObjectManager : UIObjectManager<HandCardObjectManager>, IHandCardObjectManager
    {
        private GameObject playerTable;
        private GameObject opponentTable;
        
        private HandCardCollection behaviourCollection;
        private Game game;
        private CardPile CardPile => game.CardPile;
        private AlignmentEnum CurrentAlignment => game.CurrentAlignment;

        protected override void Awake()
        {
            base.Awake();
            playerTable = ObjectReadManager.Instance.PlayerTable;
            opponentTable = ObjectReadManager.Instance.OpponentTable;
            behaviourCollection = ObjectReadManager.Instance.HandCardObjectCollection.GetComponent<HandCardCollection>();
            game = EntityLoadManager.Instance.Game;
        }

        public void AddCardObjects()
        {
            AddCardObjectsForTable(CurrentAlignment);
        }

        public void RemoveCardObjects()
        {
            RemoveCardObjectsForTable(CurrentAlignment);
        }

        public Sprite GetSpriteFromHandCardObject(CharacterConfig characterConfig)
        {
            return behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).Sprite;
        }

        public void AddCardObjectFromConfig(CharacterConfig characterConfig)
        {
            Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(characterConfig).transform;
            Transform table = GetTableObjectFromAlignment(CurrentAlignment).transform;
            card.SetParent(table, false);
        }

        private void AddCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            IReadOnlyList<CharacterConfig> ownedCards = CardPile.GetCardsFromAlign(alignment);
            AddCardObjectsFromPileData(table, ownedCards);
        }

        private void RemoveCardObjectsForTable(AlignmentEnum alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            IReadOnlyList<CharacterConfig> ownedCards = CardPile.GetCardsFromAlign(alignment);
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
