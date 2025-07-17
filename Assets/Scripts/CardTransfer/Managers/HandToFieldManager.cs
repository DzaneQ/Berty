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
using Berty.BoardCards.State;

namespace Berty.CardTransfer.Managers
{
    public class HandToFieldManager : ManagerSingleton<HandToFieldManager>
    {
        private Game Game { get; set; }
        private SelectionAndPaymentSystem SelectionSystem { get; set; }
        private CardPile CardPile => Game.CardPile;
        private GameObject boardCardPrefab;

        protected override void Awake()
        {
            InitializeSingleton();
            Game = CoreManager.Instance.Game;
            SelectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
            boardCardPrefab = Resources.Load<GameObject>("Prefabs/CardSquare");
        }

        public void RemoveSelectedCardFromHand()
        {
            CharacterConfig selectedCard = SelectionSystem.GetSelectedCardOrThrow();
            SelectionSystem.PutSelectedCardOnHold();
            CardPile.LeaveCard(selectedCard, Game.CurrentAlignment);
            HandCardObjectManager.Instance.RemoveCardObjects();
        }

        public void SetCardOnHoldOnField(FieldBehaviour field)
        {
            field.BoardField.AddCard(SelectionSystem.GetCardOnHoldOrThrow(), Game.CurrentAlignment);
            Instantiate(boardCardPrefab, field.transform);
            field.ColorizeField();
        }
    }
}
