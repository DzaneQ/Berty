using Berty.BoardCards.ConfigData;
using Berty.CardTransfer.Entities;
using Berty.Entities;
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

        public void UpdateCardObjects()
        {
            if (playerTable.activeSelf) UpdateCardObjectsForTable(Alignment.Player);
            if (opponentTable.activeSelf) UpdateCardObjectsForTable(Alignment.Opponent);
        }

        private void UpdateCardObjectsForTable(Alignment alignment)
        {
            Transform table = GetTableObjectFromAlignment(alignment).transform;
            List<CharacterConfig> cards = cardPile.GetCardsFromAlign(alignment);
            //List<string> cardNames = cards.Select(characterConfig => characterConfig.Name).ToList();
            for (int i = 0; i < cards.Count; i++)
            {
                Transform card = behaviourCollection.GetBehaviourFromCharacterConfig(cards[i]).transform;
                if (card.parent == table) continue;
                card.SetParent(table);
            }
        }

        private GameObject GetTableObjectFromAlignment(Alignment alignment)
        {
            return alignment switch
            {
                Alignment.Player => playerTable,
                Alignment.Opponent => opponentTable,
                _ => throw new InvalidOperationException("Invalid align to call table object.")
            };
        }
    }
}
