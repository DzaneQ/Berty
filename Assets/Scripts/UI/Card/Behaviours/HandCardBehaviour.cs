using Berty.BoardCards.ConfigData;
using Berty.Gameplay;
using Berty.UI.Card.Animation;
using Berty.UI.Card.Selection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.UI.Card
{
    public class HandCardBehaviour : MonoBehaviour
    {
        private SelectStatus select;
        private Image imageRenderer;
        private CharacterConfig character;

        public CharacterConfig Character
        {
            get => character;
            private set
            {
                character = value;
                imageRenderer.sprite = Resources.Load<Sprite>("BERTYmirrorY/" + character.Name);
            }
        }
        public Sprite Sprite
        {
            get => imageRenderer.sprite;
        }

        private void Awake()
        {
            imageRenderer = GetComponent<Image>();
            select = new UnselectedCard(GetComponent<RectTransform>(), GetComponent<AnimatingCardImage>());
        }

        public void AssignCharacter(CharacterConfig newCharacter)
        {
            Character = newCharacter;
        }

        public void Unselect()
        {
            select = select.UnselectAutomatically();
        }

        public bool IsCardSelected()
        {
            return select.IsCardSelected;
        }

        public void ShowObjectAsSelected()
        {
            if (select.IsCardSelected) throw new Exception("This card is already selected!");
            ChangeSelection();
        }

        public void ShowObjectAsUnselected()
        {
            if (!select.IsCardSelected) throw new Exception("This card is already unselected!");
            ChangeSelection();
        }

        public void MakeObjectUnselectedWithoutAnimation()
        {
            if (!select.IsCardSelected) throw new Exception("This card is already unselected!");
            ChangeSelection(true);
        }

        public void ChangeSelection(bool ignoreAnimation = false)
        {
            select = select.ChangePosition(!ignoreAnimation);
        }

        public void ReviveCard()
        {
            //cardManager.ReviveCard(this);
            //Turn.SetMoveTime();
        }

        public void ReturnCard()
        {
            //cardManager.AddToTable(this, returnTable);
        }

        public bool IsAnimating()
        {
            return select.IsAnimating();
        }
    }
}