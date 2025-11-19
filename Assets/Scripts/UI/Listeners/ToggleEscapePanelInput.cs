using Berty.Gameplay.Listeners;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.UI.Listeners
{
    public class ToggleEscapePanelInput : MonoBehaviour
    {
        [SerializeField] private GameObject escapePanel;
        private Collider[] disabledColliders;
        private int[] interactableLayers;

        private void Awake()
        {
            interactableLayers = GetAllInteractableLayers();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleEscapePanel();
                ToggleAllColliderInputs();
            }
        }

        private void ToggleEscapePanel()
        {
            escapePanel.SetActive(!escapePanel.activeSelf);
        }

        private void ToggleAllColliderInputs()
        {
            Collider[] colliders = escapePanel.activeSelf ? FindObjectsOfType<Collider>(false).ToArray() : disabledColliders;
            foreach (Collider coll in colliders)
            {
                if (!interactableLayers.Contains(coll.gameObject.layer)) continue;
                coll.enabled = !escapePanel.activeSelf;
            }
            disabledColliders = escapePanel.activeSelf ? colliders : null; 
        }

        private int[] GetAllInteractableLayers()
        {
            return new int[] {
                LayerMask.NameToLayer("Card"),
                LayerMask.NameToLayer("Field"),
                LayerMask.NameToLayer("Button")
            };
        }
    }
}
