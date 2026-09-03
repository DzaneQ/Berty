using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Display.View;
using Berty.Gameplay.Managers;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardActivation : BoardCardBehaviour
    {
        private Vector3 defaultCardPosition;
        private bool isDeactivating;

        protected override void Awake()
        {
            base.Awake();
            defaultCardPosition = transform.localPosition;
            isDeactivating = false;
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
        }

        public void HandleNewCardActivated(CharacterConfig characterConfig)
        {
            Sound.PlayNewCardSound();
            EntityHandler.LoadBoardCardEntity(characterConfig, ManagerLocator.TurnManagerInstance.CurrentAlignment);
            DisableTheOtherCardOnTheField();
            AdjustInitRotation();
            StateMachine.SetNewState();
        }

        // The opposite of HandleNewCardActivated + default transform + deactivation
        public void DeactivateCard()
        {
            if (Navigation.IsCardAnimating())
            {
                isDeactivating = true;
                return;
            }
            EnableTheOtherCardOnTheFieldAndFreeSpace();
            EntityHandler.DeactivateBoardCardEntity();
            UpdateHighlightOfDeactivatedCard();
            SetToDefaultLocalTransform();
            gameObject.SetActive(false);
        }

        public void LoadCard(BoardCard savedCard)
        {
            EntityHandler.LoadBoardCardEntityFromData(savedCard);
            Navigation.RotateObjectWithoutAnimation((int)savedCard.Direction);
            StateMachine.SetMainState();
        }

        public bool TryDeactivatingIfFlagged()
        {
            if (!isDeactivating) return false;
            isDeactivating = false;
            DeactivateCard();
            return true;
        }

        private void DisableTheOtherCardOnTheField()
        {
            GameObject firstCard = transform.parent.GetChild(0).gameObject;
            if (firstCard == gameObject) return;
            firstCard.SetActive(false);
        }

        private bool EnableTheOtherCardOnTheFieldAndFreeSpace()
        {
            GameObject firstCard = transform.parent.GetChild(0).gameObject;
            if (firstCard == gameObject) // If this is the only card, handle empty field
            {
                SetDefaultRotationForCardSet();
                EventManager.Instance.RaiseOnFieldFreed(ParentField);
                if (StateMachine.IsCursorFocused()) EventManager.Instance.RaiseOnHighlightEnd();
                return false;
            }
            else // Otherwise, enable the other card
            {
                firstCard.SetActive(true);
                transform.SetParent(null, false);
                return true;
            }
        }

        private void UpdateHighlightOfDeactivatedCard()
        {
            if (ParentField == null) throw new Exception("Parent field should not be null when updating highlight of deactivated card.");
            BoardCardBehaviour otherCard = ParentField.ChildCard;
            if (otherCard == null && StateMachine.IsCursorFocused()) EventManager.Instance.RaiseOnHighlightEnd();
            else if (otherCard != null && (StateMachine.IsCursorFocused() || otherCard.StateMachine.IsCursorFocused())) EventManager.Instance.RaiseOnHighlightStart(otherCard);
            else ParentField.RefreshHighlight(); // TODO: Adjust backup card highlight to the attacker
        }    

        private void AdjustInitRotation()
        {
            if (transform.parent.childCount > 1) return; // Keep the backup card's rotation.
            int rightAngle = (180 - Mathf.RoundToInt(Camera.main.GetComponent<RotateCamera>().RightAngleValue())) % 360;
            Navigation.RotateObjectWithoutAnimation(rightAngle);
            BoardCard.AdvanceCardSetAngleBy(rightAngle);
        }

        private void SetToDefaultLocalTransform()
        {
            transform.localPosition = defaultCardPosition;
            // Rotation is not supposed to be changed during the game
        }

        private void SetDefaultRotationForCardSet()
        {
            transform.parent.localRotation = Quaternion.identity;
        }
    }
}