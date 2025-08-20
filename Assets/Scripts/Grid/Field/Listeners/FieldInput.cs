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
            if (HasACard()) HandleCardClick();
            else if (Input.GetMouseButtonDown(0)) HandleLeftClick();
        }

        private void HandleCardClick()
        {
            behaviour.ChildCard.SendMessage("OnMouseOver");
        }

        private void HandleLeftClick()
        {
            if (PuttingIntent()) PutTheCard();
        }

        private bool PuttingIntent()
        {
            return selectionSystem.GetTheOnlySelectedCardOrNull() != null;
        }

        private void PutTheCard()
        {
            HandToFieldManager.Instance.RemoveSelectedCardFromHand();
            BoardCardCore newCard = HandToFieldManager.Instance.SetCardOnHoldOnField(behaviour);
            PaymentManager.Instance.CallPayment(behaviour.BoardField.OccupantCard.Stats.Power, newCard);
        }

        private bool HasACard()
        {
            //Debug.Log("Checking for card...");
            return behaviour.ChildCard != null;
        }    
    }
}
