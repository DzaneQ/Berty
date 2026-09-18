using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.Menu.Listeners
{
    public class SliderSoundListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private AudioSource soundChangeSource;

        private void Awake()
        {
            soundChangeSource = GetComponent<AudioSource>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            soundChangeSource.Play();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            soundChangeSource.Stop();
        }
    }
}
