using Berty.Audio.Managers;
using Berty.UI.Managers.Client;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.UI.Listeners.Client
{
    public class ClientButtonInput : MonoBehaviour, IPointerUpHandler
    {
        private CornerButton core;

        void Awake()
        {
            core = GetComponent<CornerButton>();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (Input.GetMouseButtonUp(0) && eventData.pointerCurrentRaycast.gameObject == transform.GetChild(0).gameObject) HandleTheButtonClick();
        }

        private void HandleTheButtonClick()
        {
            SoundManager.Instance.ButtonClickSound();
            ClientButtonActionManager.Instance.HandleCornerButtonClick(core.ButtonType);
        }
    }
}
