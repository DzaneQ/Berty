using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Display.View;
using Berty.Gameplay.Managers;
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
            EntityHandler.LoadBoardCardEntity(characterConfig, game.CurrentAlignment);
            DisableTheOtherCardOnTheField();
            AdjustInitRotation();
            StateMachine.HandleStateForNewCard();
        }

        // The opposite of HandleNewCardActivated + default transform + deactivation
        public void DeactivateCard()
        {
            if (Navigation.IsCardAnimating())
            {
                isDeactivating = true;
                return;
            }
            bool backupCardActivated = EnableTheOtherCardOnTheFieldAndFreeSpace();
            EntityHandler.DeactivateBoardCardEntity();
            SetToDefaultLocalTransform();
            //if (backupCardActivated) ParentField.ChildCard.StateMachine.SetMainState();
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
                return false;
            }
            else // Otherwise, enable the other card
            {
                firstCard.SetActive(true);
                transform.SetParent(null, false);
                return true;
            }
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