using Berty.Audio.Managers;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.Managers;
using Berty.Display.View;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.Settings;
using Berty.UI.Card.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardCore : BoardCardBehaviour
    {
        private List<BoardCardBehaviour> _attackedCards = new();
        public IReadOnlyList<BoardCardBehaviour> AttackedCards => _attackedCards;
        private Camera cam;

        protected override void Awake()
        {
            base.Awake();
            BoardCardCollectionManager.Instance.AddCardToCollection(this);
            cam = Camera.main;
        }

        private void Start()
        {
            DisableBackupCard();
            AdjustInitRotation();
            ParentField.UpdateField();
        }

        public void ClearAttackedCardsCache()
        {
            _attackedCards.Clear();
        }

        // WARNING: The code logic applies successful attack on riposte and attack new stand as it's basic attack.
        //    But it's only used when ordered attack is applied so it doesn't matter... until there's a new code that does.
        public void MarkSuccessfulAttack(BoardCardBehaviour card)
        {
            _attackedCards.Add(card);
        }

        public bool IsCursorFocused()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return false;
            if (hit.transform == transform) return true; // Is cursor on card square object?
            if (hit.transform.parent == null) return false;
            return hit.transform.parent.parent == transform; // Is cursor on card's button object?
        }
        
        public void HandleAnimationEnd()
        {
            if (BoardCard == null) return;
            if (Navigation.IsCardAnimating()) return;
            Bars.ShowBars();
            CheckpointManager.Instance.HandleIfRequested();
            if (Bars.AreBarsAnimating()) return;
            StateMachine.TryShowingButtons();      
        }
 
        public void EnableBackupCard()
        {
            transform.parent.GetChild(0).gameObject.SetActive(true);
        }

        private void DisableBackupCard()
        {
            GameObject backupCard = transform.parent.GetChild(0).gameObject;
            if (backupCard == gameObject) return;
            backupCard.SetActive(false);
        }
        
        private void AdjustInitRotation()
        {
            if (transform.parent.childCount > 1) return; // Keep the backup card's rotation.
            int rightAngle = (180 - Mathf.RoundToInt(Camera.main.GetComponent<RotateCamera>().RightAngleValue())) % 360;
            Navigation.RotateObjectWithoutAnimation(rightAngle);
            BoardCard.AdvanceCardSetAngleBy(rightAngle);
        }
    }
}