using Berty.BoardCards.Behaviours;
using Berty.Enums;
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
        private bool isActivated;

        private void Awake()
        {
            card = GetComponentInParent<BoardCardCore>();
            rend = GetComponent<Renderer>();
            coll = GetComponent<Collider>();
            neutralMaterial = rend.material;
            isActivated = false;
        }

        public void EnableButton()
        {
            if (!isActivated) return;
            Debug.Log($"Enable attempt: {name} on card: {card.name}");
            if (!card.CardNavigation.IsInteractableEnabled()) return;
            if (!CanNavigate()) return;
            //if (card != card.Grid.Turn.GetFocusedCard()) return;
            Debug.Log("Enable: " + name + " on card: " + card.name);
            rend.enabled = true;
            coll.enabled = true;
        }

        public void DisableButton()
        {
            //Debug.Log("Disable: " + name + " on card: " + card.name);
            rend.enabled = false;
            coll.enabled = false;
        }

        public void ActivateDexterityButton()
        {
            ChangeButtonToDexterity();
            isActivated = true;
            EnableButton(); // TODO: Change to focus only
        }

        public void ActivateNeutralButton()
        {
            ChangeButtonToNeutral();
            isActivated = true;
            EnableButton(); // TODO: Change to focus only
        }

        public void DeactivateButton()
        {
            isActivated = false;
            DisableButton();
        }

        private void ChangeButtonToDexterity()
        {
            //Debug.Log("Changed to dexterity: " + name);
            rend.material = dexterityMaterial;
        }

        private void ChangeButtonToNeutral()
        {
            //Debug.Log("Changed to neutral: " + name);
            rend.material = neutralMaterial;
        }

        abstract protected bool CanNavigate();

        public virtual bool IsMoveButton() => false;

        abstract public NavigationEnum GetName();
    }
}