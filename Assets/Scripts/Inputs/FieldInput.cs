using Berty.Field;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Inputs
{
    public class FieldInput : MonoBehaviour
    {
        private FieldBehaviour behaviour;

        void Awake()
        {
            behaviour = GetComponent<FieldBehaviour>();
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
            if (!behaviour.OccupantCard.IsLocked() && !behaviour.OccupantCard.IsAnimating() && Input.GetMouseButtonDown(0)) return true;
            else return false;
        }

        private bool IsRightClicked()
        {
            if (!behaviour.OccupantCard.IsLocked() && !behaviour.OccupantCard.IsAnimating() && Input.GetMouseButtonDown(1)) return true;
            else return false;
        }
    }
}
