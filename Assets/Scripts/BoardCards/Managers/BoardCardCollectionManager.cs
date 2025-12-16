using Berty.BoardCards.Behaviours;
using Berty.BoardCards.Entities;
using Berty.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class BoardCardCollectionManager : ManagerSingleton<BoardCardCollectionManager>
    {
        private List<BoardCardBehaviour> boardCardCoreCollection;

        protected override void Awake()
        {
            base.Awake();
            InitializeCollection();
        }

        private void InitializeCollection()
        {
            if (boardCardCoreCollection != null) throw new Exception("Card collection is already initialized");
            boardCardCoreCollection = new List<BoardCardBehaviour>();
        }

        public BoardCardBehaviour GetActiveBehaviourFromEntityOrThrow(BoardCard boardCard)
        {
            BoardCardBehaviour card = boardCardCoreCollection.Find((BoardCardBehaviour bhvr) => bhvr.BoardCard == boardCard);
            if (card == null) throw new Exception($"Trying to get behaviour from entity {boardCard.CharacterConfig.Name} that is null");
            if (!card.isActiveAndEnabled) throw new Exception($"Trying to get behaviour {card.name} from entity {boardCard.CharacterConfig.Name} that is not active");
            return card;
        }

        public void AddCardToCollection(BoardCardBehaviour card)
        {
            if (boardCardCoreCollection.Contains(card)) throw new Exception($"Card {card.name} already exists in the collection");
            boardCardCoreCollection.Add(card);
        }

        public void RemoveCardFromCollection(BoardCardBehaviour card)
        {
            if (!boardCardCoreCollection.Contains(card)) throw new Exception($"Card {card.name} does not exist in the collection");
            boardCardCoreCollection.Remove(card);
        }
    }
}