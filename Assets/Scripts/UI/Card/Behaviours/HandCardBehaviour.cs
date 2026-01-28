using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Animation;
using Berty.UI.Card.Selection;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Card
{
    public class HandCardBehaviour : MonoBehaviour
    {
        private SelectStatus _select;
        private Image _imageRenderer;
        private CharacterConfig _character;

        private Image ImageRenderer
        {
            get
            {
                if (_imageRenderer == null) _imageRenderer = GetComponent<Image>();
                return _imageRenderer;
            }
        }

        public CharacterConfig Character
        {
            get => _character;
            private set
            {
                _character = value;
                ImageRenderer.sprite = Resources.Load<Sprite>("BERTYmirrorY/" + _character.Name);
            }
        }
        public Sprite Sprite
        {
            get => ImageRenderer.sprite;
        }

        private void Awake()
        {
            _select = new UnselectedCard(GetComponent<RectTransform>(), GetComponent<AnimatingCardImage>());
        }

        public void AssignCharacter(CharacterConfig newCharacter)
        {
            Character = newCharacter;
        }

        public void Unselect()
        {
            _select = _select.UnselectAutomatically();
        }

        public bool IsCardSelected()
        {
            return _select.IsCardSelected;
        }

        public void ShowObjectAsSelected()
        {
            if (_select.IsCardSelected) throw new Exception("This card is already selected!");
            ChangeSelection();
        }

        public void ShowObjectAsUnselected()
        {
            if (!_select.IsCardSelected) throw new Exception("This card is already unselected!");
            ChangeSelection();
        }

        public void MakeObjectUnselectedWithoutAnimation()
        {
            if (!_select.IsCardSelected) throw new Exception("This card is already unselected!");
            ChangeSelection(true);
        }

        public void ChangeSelection(bool ignoreAnimation = false)
        {
            _select = _select.ChangePosition(!ignoreAnimation);
        }

        public void ReturnCard()
        {
            //cardManager.AddToTable(this, returnTable);
        }

        public bool IsAnimating()
        {
            return _select.IsAnimating();
        }
    }
}