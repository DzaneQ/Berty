using Berty.BoardCards.Button;
using Berty.BoardCards.Entities;
using Berty.BoardCards.State;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardStateMachine : BoardCardBehaviour
    {
        private CardState currentState;
        /* CardButton index:
            0 - RotateLeft
            1 - RotateRight
            2 - MoveUp
            3 - MoveRight
            4 - MoveDown
            5 - MoveLeft
         */

        private GameObject buttonSet;

        public CardButton[] Buttons { get; private set; }
        public CardStateEnum StateName => currentState.GetNameEnum();
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            cam = Camera.main;
        }

        private void OnEnable()
        {
            if (Buttons == null || Buttons.Length == 0) InitializeButtons(); // Handle first time activation
            else SetMainState();
        }

        private void OnDisable()
        {
            SetIdle();
        }

        public void SetNewState()
        {
            SetState(new NewCardState(this));
        }

        private void InitializeButtons()
        {
            Transform buttonsParentObject = transform.GetChild(2);
            buttonSet = buttonsParentObject.gameObject;
            Buttons = new CardButton[6];
            for (int index = 0; index < 6; index++) Buttons[index] = buttonsParentObject.GetChild(index + 2).GetComponent<CardButton>();
        }

        private void SetState(CardState state)
        {
            if (currentState != null && currentState.GetNameEnum() == state.GetNameEnum()) return;
            currentState = state;
            currentState.OnStateEnter();
            RefreshButtons();
        }

        public void SetPendingStateFromEnum(CardStateEnum stateEnum, NavigationEnum navigation)
        {
            CardState pendingState = GetStateFromEnum(stateEnum, navigation);
            if (!pendingState.IsForPay()) throw new Exception($"State {stateEnum} is not pending");
            SetState(pendingState); // NOTE: Is it okay to execute OnStateEnter()?
        }

        public void SetMainState()
        {
            if (BoardCard == null || BoardCard.OccupiedField == null)
            {
                SetIdle();
                return;
            }
            AlignmentEnum currentAlign = ManagerLocator.TurnManagerInstance.CurrentAlignment;
            AlignmentEnum cardAlign = BoardCard.Align;
            if (currentAlign == cardAlign && !BoardCard.IsTired) SetActive();
            else if (IsEligibleForTelekineticState()) SetTelekinetic();
            else SetIdle();
        }

        public bool IsEligibleForTelekineticState()
        {
            Status telekinesisArea = game.GetStatusByNameOrNull(StatusEnum.TelekineticArea);
            if (telekinesisArea == null) return false;
            if (telekinesisArea.Provider.IsTired) return false;
            if (ApplySkillEffectManager.Instance.DoesPreventEffect(BoardCard, telekinesisArea.Provider)) return false;
            return telekinesisArea.Provider.Align == ManagerLocator.TurnManagerInstance.CurrentAlignment;
        }

        public void SetActive()
        {
            SetState(new ActiveState(this));
        }

        public void SetIdle()
        {
            SetState(new IdleState(this));
        }

        public void SetTelekinetic()
        {
            SetState(new TelekineticState(this));
        }

        public void SetAttacking()
        {
            SetState(new AttackingState(this));
        }

        public void SetNewTransformStateFromNavigation(NavigationEnum navigatingFrom)
        {
            SetState(new NewTransformState(this, navigatingFrom));
        }

        public void SetEffectable()
        {
            SetState(new EffectableState(this));
        }

        public void HandleLeftClick()
        {
            currentState.HandleLeftClick();
        }

        public void TryShowingButtons()
        {
            if (Navigation.IsCardAnimating()) return;
            if (Bars.AreBarsAnimating()) return;
            if (!IsCursorFocused()) return;
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
            buttonSet.SetActive(true);
        }

        public void HideButtons()
        {
            buttonSet.SetActive(false);
        }

        public void UpdateButtons()
        {
            currentState.UpdateButtons();
        }

        public CardButton GetButton(NavigationEnum navigation) => Buttons[(int)navigation - 1];

        public bool HasState(CardStateEnum stateEnum)
        {
            return currentState.GetNameEnum() == stateEnum;
        }

        public bool IsForPay()
        {
            return currentState.IsForPay();
        }

        public bool IsOnNewMove()
        {
            return currentState.IsOnNewMove();
        }

        public bool IsDexterityBased()
        {
            return currentState.IsDexterityBased();
        }

        public bool IsCursorFocused()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return false;
            if (hit.transform == transform) return true; // Is cursor on card square object?
            if (hit.transform.parent == null) return false;
            return hit.transform.parent.parent == transform; // Is cursor on card's button object?
        }

        public NavigationEnum GetNewTransformNavigation()
        {
            if (!HasState(CardStateEnum.NewTransform)) throw new Exception("Current state is not NewTransform");
            return ((NewTransformState)currentState).Navigation;
        }

        private void RefreshButtons()
        {
            HideButtons();
            TryShowingButtons();
        }

        private CardState GetStateFromEnum(CardStateEnum stateEnum, NavigationEnum navigation)
        {
            if (stateEnum == CardStateEnum.NewTransform && navigation == NavigationEnum.None) throw new Exception("Navigation shouldn't be none for NewTransformState");
            return stateEnum switch
            {
                CardStateEnum.Active => new ActiveState(this),
                CardStateEnum.Idle => new IdleState(this),
                CardStateEnum.Telekinetic => new TelekineticState(this),
                CardStateEnum.Attacking => new AttackingState(this),
                CardStateEnum.NewTransform => new NewTransformState(this, (NavigationEnum)navigation), // Default navigation, will be updated later
                CardStateEnum.Effectable => new EffectableState(this),
                _ => throw new Exception("Unknown state enum"),
            };
        }
    }
}