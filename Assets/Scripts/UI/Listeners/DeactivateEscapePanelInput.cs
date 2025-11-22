using UnityEngine;

namespace Berty.UI.Listeners
{
    public class DeactivateEscapePanelInput : MonoBehaviour
    {
        public void OnClick()
        {
            EscapePanelManager.Instance.DeactivateEscapePanel();
        }
    }
}
