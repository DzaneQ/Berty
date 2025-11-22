using Berty.Gameplay.Managers;
using Berty.Utility;
using System.Linq;
using UnityEngine;

namespace Berty.UI.Listeners
{
    public class EscapePanelManager : ManagerSingleton<EscapePanelManager>
    {
        private GameObject escapePanel;
        private Collider[] disabledColliders;
        private int[] interactableLayers;

        protected override void Awake()
        {
            base.Awake();
            escapePanel = ObjectReadManager.Instance.EscapeCanvas;
            interactableLayers = GetAllInteractableLayers();
        }

        public void ToggleEscapePanel()
        {
            escapePanel.SetActive(!escapePanel.activeSelf);
            ToggleAllColliderInputs();
        }

        public void DeactivateEscapePanel()
        {
            if (!escapePanel.activeSelf) return;
            ToggleEscapePanel();
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
