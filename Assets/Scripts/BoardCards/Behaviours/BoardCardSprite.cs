using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Grid.Managers;
using Berty.Utility;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardSprite : BoardCardBehaviour
    {
        private const int CHARACTER_SPRITE_INDEX = 0;

        private SpriteRenderer characterSprite;
        private Color defaultColor;
        public Sprite LookupSprite => characterSprite.sprite;

        protected override void Awake()
        {
            base.Awake();
            characterSprite = transform.GetChild(CHARACTER_SPRITE_INDEX).GetComponent<SpriteRenderer>();
            defaultColor = characterSprite.color;
        }

        public void UpdateObjectFromCharacterConfig()
        {
            CharacterConfig character = BoardCard.CharacterConfig;
            characterSprite.sprite = ManagerLocator.HandCardObjectManagerInstance.GetSpriteFromHandCardObject(character);
            gameObject.name = character.Name;
        }

        public void HighlightAs(HighlightEnum highlight)
        {
            characterSprite.color = highlight switch
            {
                HighlightEnum.UnderAttack or HighlightEnum.UnderBlock => ColorizeObjectManager.Instance.GetColorForCard(highlight),
                _ => defaultColor
            };
        }
    }
}