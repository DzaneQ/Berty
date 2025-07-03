using Berty.BoardCards.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using System;
using UnityEngine;

namespace Berty.BoardCards.Behvaiours
{
    public class BoardCardBehaviour : MonoBehaviour
    {
        private SpriteRenderer characterSprite;

        public BoardCard BoardCard { get; private set; }

        private void Awake()
        {
            characterSprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            BoardCard = new BoardCard(HandCardSelectManager.Instance.SelectionSystem.GetCardOnHoldOrThrow());
            UpdateCharacterSprite();
        }

        private void UpdateCharacterSprite()
        {
            characterSprite.sprite = HandCardObjectManager.Instance.GetSpriteFromHandCardObject(BoardCard.CharacterConfig);
        }
    }
}