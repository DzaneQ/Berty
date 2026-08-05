using Berty.Audio.Managers;
using Berty.UI.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.UI.Listeners
{
    public class ButtonInput : MonoBehaviour, IPointerUpHandler
    {
        private CornerButton core;

        private void Awake()
        {
            core = GetComponent<CornerButton>();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject == transform.GetChild(0).gameObject) HandleTheButtonClick();
        }

        private void HandleTheButtonClick()
        {
            SoundManager.Instance.ButtonClickSound();
            ButtonActionManager.Instance.HandleCornerButtonClick(core.ButtonType);
        }
    }
}
