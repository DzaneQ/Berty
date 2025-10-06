using Berty.UI.Card.Managers;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card.Systems;
using UnityEngine;
using Berty.BoardCards.Behaviours;

namespace Berty.Grid.Field.Listeners
{
    public class FieldInput : MonoBehaviour
    {
        private FieldBehaviour behaviour;
        private SelectionAndPaymentSystem selectionSystem;

        void Awake()
        {
            behaviour = GetComponent<FieldBehaviour>();
            selectionSystem = CoreManager.Instance.SelectionAndPaymentSystem;
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }

        private void OnMouseOver()
        {
            if (TryHandlingCardClick()) return; // When there's a card, handle card click instead.
            TryPuttingCardOnField();
        }

        private bool TryHandlingCardClick()
        {
            if (!HasACard()) return false;
            behaviour.ChildCard.SendMessage("OnMouseOver");
            return true;
        }

        private bool TryPuttingCardOnField()
        {
            if (!IsLeftClicked()) return false;
            if (selectionSystem.IsItPaymentTime()) return false;
            if (!HasSelectedOneCard()) return false;
            PutTheCard();
            return true;
        }

        private bool IsLeftClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool HasSelectedOneCard()
        {
            return selectionSystem.GetTheOnlySelectedCardOrNull() != null;
        }

        private void PutTheCard()
        {
            HandToFieldManager.Instance.RemoveSelectedCardFromHand();
            BoardCardCore newCard = HandToFieldManager.Instance.PutCardOnField(behaviour);
            PaymentManager.Instance.CallPayment(behaviour.BoardField.OccupantCard.Stats.Power, newCard);
        }

        private bool HasACard()
        {
            //Debug.Log("Checking for card...");
            return behaviour.ChildCard != null;
        }    
    }
}
