using Berty.BoardCards.Animation;
using Berty.BoardCards.Button;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Listeners;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

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
        private bool queuedMovementEffect;

        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            moveCard = GetComponent<IMoveCard>();
            rotateCard = GetComponent<IRotateCard>();
            input = GetComponent<BoardCardInput>();
            queuedMovementEffect = false;
            InitializeNavigation();
        }

        private void InitializeNavigation()
        {
            Transform parent = transform.GetChild(0);
            Buttons = new CardButton[6];
            for (int index = 0; index < 6; index++) Buttons[index] = parent.GetChild(index + 2).GetComponent<CardButton>();
        }

        public void EnableButtons()
        {
            foreach (CardButton button in Buttons) button.EnableButton();
        }

        public void DisableButtons()
        {
            foreach (CardButton button in Buttons) button.DisableButton();
        }

        public void ActivateButtonsBasedOnState()
        {
            switch (core.CardState)
            {
                case CardStateEnum.Idle:
                case CardStateEnum.Attacking:
                case CardStateEnum.Effectable:
                    DeactivateAllButtons();
                    break;
                case CardStateEnum.Active:
                    ActivateAllDexterityButtons();
                    break;
                case CardStateEnum.NewCard:
                    ActivateRotateNeutralButtonsIfTheOnlyCardOnField();
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

        private void ActivateRotateNeutralButtonsIfTheOnlyCardOnField()
        {
            foreach (CardButton button in Buttons)
            {
                if (button.IsMoveButton() || transform.parent.childCount > 1) button.DeactivateButton();
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
            moveCard.ToField(field);
            core.ParentField.UpdateField();
            core.SetFieldBehaviour(field);
            if (core.CardState == CardStateEnum.Active || core.CardState == CardStateEnum.Telekinetic) return; // Don't run before NewTransform state is set
            HandleNewMovement();
        }

        public void RotateCardObject(int angle)
        {
            rotateCard.ByAngle(angle);
        }

        public void RotateObjectWithoutAnimation(int angle)
        {
            rotateCard.ByAngleWithoutAnimation(angle);
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

        public bool IsAnyMoveButtonActivated()
        {
            foreach (CardButton button in Buttons)
            {
                if (!button.IsMoveButton()) continue;
                if (button.IsActivated) return true;
            }
            return false;
        }

        public void HandleNewMovement()
        {
            queuedMovementEffect = true;
            if (!IsCardAnimating()) HandleAfterMoveAnimation();
        }

        public void HandleAfterMoveAnimation()
        {
            if (!queuedMovementEffect) return;
            EventManager.Instance.RaiseOnMovedCharacter(core);
            queuedMovementEffect = false;
        }
    }
}