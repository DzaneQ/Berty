using UnityEngine;
using UnityEngine.InputSystem;

namespace Berty.UI.Listeners
{
    public class ToggleEscapePanelInput : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                EscapePanelManager.Instance.ToggleEscapePanel();
            }
        }
    }
}
