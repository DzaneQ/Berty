using Berty.CardTransfer.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card.Managers;
using Berty.UI.Card.Systems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (Input.GetMouseButtonDown(0)) HandleLeftClick();
            else if (Input.GetMouseButtonDown(1)) HandleRightClick();
        }

        private void HandleLeftClick()
        {
            if (IsItPaymentTime()) ConfirmPayment();
            else if (PuttingIntent()) PutTheCard();
            else if (AttackingIntent()) PrepareAnAttack();
        }

        private void HandleRightClick()
        {
            // TODO: Handle card's right click
        }

        private bool PuttingIntent()
        {
            return selectionSystem.GetTheOnlySelectedCardOrNull() != null;
        }

        private bool AttackingIntent()
        {
            return selectionSystem.GetSelectedCardsCount() == 0;
        }

        private bool IsItPaymentTime()
        { 
            return selectionSystem.IsItPaymentTime();
        }

        private void ConfirmPayment()
        {
            //if (CardManager.Instance.SelectionSystem.CheckOffer()) ; // Remove selected cards and update state.
        }

        private void PutTheCard()
        {
            HandToFieldManager.Instance.RemoveSelectedCardFromHand();
            HandToFieldManager.Instance.SetCardOnHoldOnField(behaviour);
            PaymentManager.Instance.CallPayment(behaviour.BoardField.OccupantCard.Stats.Power);
        }

        private void PrepareAnAttack()
        {
            throw new System.NotImplementedException();
        }
    }
}
