using Berty.CardTransfer.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.ConfigData;
using Berty.Grid.Field.Behaviour;

namespace Berty.CardTransfer.Managers
{
    public class HandToFieldManager : ManagerSingleton<HandToFieldManager>
    {
        private Game game { get; set; }
        private CardPile cardPile => game.CardPile;
        private GameObject boardCardPrefab;

        protected override void Awake()
        {
            InitializeSingleton();
            game = CoreManager.Instance.Game;
            boardCardPrefab = Resources.Load<GameObject>("Prefabs/CardSquare");
        }

        public void RemoveSelectedCardFromHand()
        {
            CharacterConfig selectedCard = HandCardSelectManager.Instance.SelectionSystem.GetSelectedCardOrThrow();
            HandCardSelectManager.Instance.SelectionSystem.PutSelectedCardOnHold();
            cardPile.LeaveCard(selectedCard, game.CurrentAlignment);
            HandCardObjectManager.Instance.RemoveCardObjects();
        }

        public void SetCardOnHoldOnField(FieldBehaviour field)
        {
            field.BoardField.AddCard(HandCardSelectManager.Instance.SelectionSystem.GetCardOnHoldOrThrow());
            Instantiate(boardCardPrefab, field.transform);
        }
    }
}
