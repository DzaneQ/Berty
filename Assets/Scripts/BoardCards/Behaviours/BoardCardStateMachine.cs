using Berty.BoardCards.Button;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Listeners;
using Berty.BoardCards.State;
using Berty.Characters.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
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
        public CardButton[] Buttons { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            InitializeButtons();
        }

        private void Start()
        {
            SetState(new NewCardState(this));
        }

        private void InitializeButtons()
        {
            Transform parent = transform.GetChild(0);
            Buttons = new CardButton[6];
            for (int index = 0; index < 6; index++) Buttons[index] = parent.GetChild(index + 2).GetComponent<CardButton>();
        }

        private void SetState(CardState state)
        {
            if (currentState != null && currentState.GetNameEnum() == state.GetNameEnum()) return;
            currentState = state;
            currentState.OnStateEnter();
        }

        public void SetMainState()
        {
            if (Core.BoardCard == null || Core.BoardCard.OccupiedField == null)
            {
                SetIdle();
                return;
            }
            AlignmentEnum currentAlign = game.CurrentAlignment;
            AlignmentEnum cardAlign = Core.BoardCard.Align;
            if (currentAlign == cardAlign && !Core.BoardCard.IsTired) SetActive();
            else if (IsEligibleForTelekineticState()) SetTelekinetic();
            else SetIdle();
        }

        public bool IsEligibleForTelekineticState()
        {
            Status telekinesisArea = game.GetStatusByNameOrNull(StatusEnum.TelekineticArea);
            if (telekinesisArea == null) return false;
            if (telekinesisArea.Provider.IsTired) return false;
            if (ApplySkillEffectManager.Instance.DoesPreventEffect(Core.BoardCard, telekinesisArea.Provider)) return false;
            return telekinesisArea.Provider.Align == game.CurrentAlignment;
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
            Core.ClearAttackedCardsCache();
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

        public void EnableButtons()
        {
            foreach (CardButton button in Buttons) button.EnableButton();
        }

        public void DisableButtons()
        {
            foreach (CardButton button in Buttons) button.DisableButton();
        }

        public void ActivateNeutralButton(NavigationEnum navigation)
        {
            foreach (CardButton button in Buttons)
            {
                if (button.GetName() == navigation) button.ActivateNeutralButton();
                else button.DeactivateButton();
            }
        }

        public CardButton GetButton(NavigationEnum navigation) => Buttons[(int)navigation];

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
    }
}