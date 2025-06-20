using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Field.Listeners
{
    public class FieldInput : MonoBehaviour
    {
        private OutdatedFieldBehaviour behaviour;

        void Awake()
        {
            behaviour = GetComponent<OutdatedFieldBehaviour>();
        }

        void Start()
        {
            // Enabling toggle from the inspector.
        }

        private void OnMouseOver()
        {
            if (IsLeftClicked()) behaviour.OccupantCard.State.HandleClick();
            else if (IsRightClicked()) behaviour.OccupantCard.State.HandleSideClick();
        }

        private bool IsLeftClicked()
        {
            //Debug.Log($"Card {name} was left clicked. Is it locked? {IsLocked()}");
            return Input.GetMouseButtonDown(0);
        }

        private bool IsRightClicked()
        {
            return Input.GetMouseButtonDown(1);
        }
    }
}
