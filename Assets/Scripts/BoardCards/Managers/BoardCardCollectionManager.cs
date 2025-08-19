using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Field.Entities;
using Berty.UI.Card.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class BoardCardCollectionManager : ManagerSingleton<BoardCardCollectionManager>
    {
        private List<BoardCardCore> boardCardCoreCollection;

        protected override void Awake()
        {
            base.Awake();
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            if (boardCardCoreCollection != null) throw new Exception("Card collection is already initialized");
            boardCardCoreCollection = new List<BoardCardCore>();
        }

        public BoardCardCore GetCoreFromEntityOrThrow(BoardCard boardCard)
        {
            BoardCardCore core = boardCardCoreCollection.Find((BoardCardCore core) => core.BoardCard == boardCard);
            if (core == null) throw new Exception($"Trying to get core from entity {boardCard.CharacterConfig.Name} that is null");
            return core;
        }

        public void AddCardToCollection(BoardCardCore card)
        {
            if (boardCardCoreCollection.Contains(card)) throw new Exception($"Card {card.name} already exists in the collection");
            boardCardCoreCollection.Add(card);
        }

        public void RemoveCardFromCollection(BoardCardCore card)
        {
            if (!boardCardCoreCollection.Contains(card)) throw new Exception($"Card {card.name} does not exist in the collection");
            boardCardCoreCollection.Remove(card);
        }
    }
}