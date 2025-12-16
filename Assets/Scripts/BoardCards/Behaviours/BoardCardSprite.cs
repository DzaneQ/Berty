using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Grid.Field.Behaviour;
using Berty.Grid.Managers;
using Berty.Settings;
using Berty.UI.Card.Managers;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardSprite : BoardCardBehaviour
    {
        private SpriteRenderer characterSprite;
        private Color defaultColor;
        public Sprite LookupSprite => characterSprite.sprite;

        protected override void Awake()
        {
            base.Awake();
            characterSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
            defaultColor = characterSprite.color;
        }

        public void UpdateObjectFromCharacterConfig()
        {
            CharacterConfig character = BoardCard.CharacterConfig;
            characterSprite.sprite = HandCardObjectManager.Instance.GetSpriteFromHandCardObject(character);
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