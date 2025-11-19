using Berty.BoardCards.Behaviours;
using Berty.Display.Managers;
using Berty.Enums;
using Berty.Gameplay.Listeners;
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
        private Camera cam;

        private void Awake()
        {
            card = GetComponentInParent<BoardCardCore>();
            rend = GetComponent<Renderer>();
            coll = GetComponent<Collider>();
            neutralMaterial = rend.material;
            cam = Camera.main;
        }

        public void OnMouseExit()
        {
            if (IsCursorFocusedOnCard()) return; // Stop running method if cursor is on the card
            DisplayManager.Instance.HideLookupCard();
            EventManager.Instance.RaiseOnHighlightEnd();
        }

        private bool IsCursorFocusedOnCard()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit)) return false;
            return hit.transform == transform.parent.parent;
        }

        public void ActivateButton()
        {
            rend.enabled = true;
            coll.enabled = true;
        }

        public void DeactivateButton()
        {
            rend.enabled = false;
            coll.enabled = false;
        }

        public void TryActivatingDexterityButton()
        {
            if (!CanNavigate()) return;
            ChangeButtonToDexterity();
            ActivateButton();
        }

        public void TryActivatingNeutralButton()
        {
            if (!CanNavigate()) return;
            ChangeButtonToNeutral();
            ActivateButton();
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