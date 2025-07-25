﻿using Berty.BoardCards.Animation;
using Berty.BoardCards.Button;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Listeners;
using Berty.BoardCards.State;
using Berty.Enums;
using Berty.Grid.Field.Behaviour;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardMovableObject : MonoBehaviour
    {
        /* CardButton index:
            0 - RotateLeft
            1 - RotateRight
            2 - MoveUp
            3 - MoveRight
            4 - MoveDown
            5 - MoveLeft
         */
        public CardButton[] Buttons { get; private set; }
        public BoardCard BoardCard => core.BoardCard;
        private BoardCardCore core;
        private BoardCardInput input;
        private IMoveCard moveCard;
        private IRotateCard rotateCard;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            moveCard = GetComponent<IMoveCard>();
            rotateCard = GetComponent<IRotateCard>();
            input = GetComponent<BoardCardInput>();
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            Transform parent = transform.GetChild(0);
            Buttons = new CardButton[6];
            for (int index = 0; index < 6; index++) Buttons[index] = parent.GetChild(index + 2).GetComponent<CardButton>();
        }

        public void ActivateButtonsBasedOnState()
        {
            switch (core.CardState)
            {
                case CardStateEnum.Idle:
                    DeactivateAllButtons();
                    break;
                case CardStateEnum.Active:
                    ActivateAllDexterityButtons();
                    break;
                case CardStateEnum.NewCard:
                    ActivateRotateNeutralButtons();
                    break;
                case CardStateEnum.NewTransform:
                    // Handled from core
                    break;
                case CardStateEnum.Telekinetic:
                    ActivateMoveDexterityButtons();
                    break;
                default:
                    throw new Exception("Undefined state");
            };
        }

        private void ActivateAllDexterityButtons()
        {
            foreach (CardButton button in Buttons) button.ActivateDexterityButton();
        }

        private void ActivateMoveDexterityButtons()
        {
            foreach (CardButton button in Buttons)
            {
                if (button.IsMoveButton()) button.ActivateDexterityButton();
                else button.DeactivateButton();
            }
        }

        private void ActivateRotateNeutralButtons()
        {
            foreach (CardButton button in Buttons)
            {
                if (button.IsMoveButton()) button.DeactivateButton();
                else button.ActivateNeutralButton();
            }
        }

        public void ActivateNeutralButton(NavigationEnum navigation)
        {
            foreach (CardButton button in Buttons)
            {
                if (button.GetName() == navigation) button.ActivateNeutralButton();
                else button.DeactivateButton();
            }
        }

        private void DeactivateAllButtons()
        {
            foreach (CardButton button in Buttons) button.DeactivateButton();
        }

        public void MoveCardObject(FieldBehaviour field)
        {
            core.ParentField.ColorizeNotOccupied();
            moveCard.ToField(field);
            core.SetFieldBehaviour(field);
        }

        public void RotateCardObject(int angle)
        {
            rotateCard.ByAngle(angle);
        }

        public void DisableInteraction()
        {
            input.enabled = false;
        }

        public void EnableInteraction()
        {
            input.enabled = true;
        }

        public bool IsInteractableEnabled()
        {
            return input.isActiveAndEnabled;
        }

        public bool IsCardAnimating()
        {
            return rotateCard.CoroutineCount > 0 || moveCard.CoroutineCount > 0;
        }
    }
}