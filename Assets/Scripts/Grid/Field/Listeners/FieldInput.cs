using Berty.Gameplay.Listeners;
using Berty.Grid.Field.Behaviour;
using Berty.UI.Card.Managers;
using Berty.Utility;
using UnityEngine;

namespace Berty.Grid.Field.Listeners
{
    public class FieldInput : MonoBehaviour, IColliderInput
    {
        private FieldBehaviour behaviour;

        private void Awake()
        {
            behaviour = GetComponent<FieldBehaviour>();
        }

        private void Start()
        {
            // Enabling toggle from the inspector.
        }

        private void OnMouseOver()
        {
            if (ManagerLocator.TurnManagerInstance.IsItNotMyTurn()) return;
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
            if (SelectionManager.Instance.IsItPaymentTime()) return false;
            if (!HasSelectedOneCard()) return false;
            behaviour.PutTheCard();
            return true;
        }

        private bool IsLeftClicked()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool HasSelectedOneCard()
        {
            return SelectionManager.Instance.GetTheOnlySelectedCardOrNull() != null;
        }

        private bool HasACard()
        {
            return behaviour.ChildCard != null;
        }    
    }
}
