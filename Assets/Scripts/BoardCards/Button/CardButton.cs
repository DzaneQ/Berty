using Berty.BoardCards.Behaviours;
using Berty.Display.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Button
{
    abstract public class CardButton : MonoBehaviour
    {

        protected BoardCardCore card;
        private Renderer rend;
        private Collider coll;
        [SerializeField] private Material dexterityMaterial;
        private Material neutralMaterial;
        private bool isActivatedByState;
        private Camera cam;

        public bool IsActivated => isActivatedByState;

        private void Awake()
        {
            card = GetComponentInParent<BoardCardCore>();
            rend = GetComponent<Renderer>();
            coll = GetComponent<Collider>();
            neutralMaterial = rend.material;
            isActivatedByState = false;
            cam = Camera.main;
        }

        public void OnMouseExit()
        {
            if (!IsCursorFocusedOnCard()) return; // Stop running method if cursor is on the card
            DisplayManager.Instance.HideLookupCard();
            EventManager.Instance.RaiseOnHighlightEnd();
        }

        private bool IsCursorFocusedOnCard()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit)) return false;
            return hit.transform == transform.parent.parent;
        }

        // BUG: After animating, not all buttons are enabled on focus until this method is called again.
        // TODO: Enabling buttons should process by enabling their parent object.
        public void EnableButton()
        {                                                      // Depending on...
            if (!isActivatedByState) return;                  // ...the card's state
            if (!CanNavigate()) return;                      // ...the card's positioning
            if (card.Navigation.IsCardAnimating()) return;  // ...whether the card is animating
            if (!IsCursorFocusedOnCard()) return;          // ...whether the cursor is on the card
            rend.enabled = true;
            coll.enabled = true;
        }

        public void DisableButton()
        {
            rend.enabled = false;
            coll.enabled = false;
        }

        public void ActivateDexterityButton()
        {
            ChangeButtonToDexterity();
            isActivatedByState = true;
            EnableButton();
        }

        public void ActivateNeutralButton()
        {
            ChangeButtonToNeutral();
            isActivatedByState = true;
            EnableButton();
        }

        public void DeactivateButton()
        {
            isActivatedByState = false;
            DisableButton();
        }

        private void ChangeButtonToDexterity()
        {
            rend.material = dexterityMaterial;
        }

        private void ChangeButtonToNeutral()
        {
            rend.material = neutralMaterial;
        }

        abstract protected bool CanNavigate();

        public virtual bool IsMoveButton() => false;

        abstract public NavigationEnum GetName();
    }
}