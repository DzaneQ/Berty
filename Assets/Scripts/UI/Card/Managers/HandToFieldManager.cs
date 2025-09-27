using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card;
using Berty.UI.Card.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.ConfigData;
using Berty.Grid.Field.Behaviour;
using Berty.BoardCards.State;
using Berty.BoardCards.Behaviours;

namespace Berty.UI.Card.Managers
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
            SelectionSystem.PutSelectedCardAsPending();
            CardPile.LeaveCard(selectedCard, Game.CurrentAlignment);
            HandCardObjectManager.Instance.RemoveCardObjects();
            HandCardSelectManager.Instance.ClearSelection();
        }

        public BoardCardCore SetCardOnField(FieldBehaviour field)
        {
            // Create empty child transform if field has no children
            if (field.transform.childCount == 0)
            {
                GameObject fieldChild = new("CardSetTransform");
                fieldChild.transform.SetParent(field.transform, false);
            }
            // Put new card on this empty child transform
            BoardCardCore card = Instantiate(boardCardPrefab, field.transform.GetChild(0)).GetComponent<BoardCardCore>();
            field.UpdateField();
            return card;
        }
    }
}
