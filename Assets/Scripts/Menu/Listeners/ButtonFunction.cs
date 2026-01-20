using Berty.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.Menu.Listeners
{
    public class ButtonFunction : MonoBehaviour, IPointerEnterHandler
    {
        [SerializeField] private AudioSource src;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;

        public AudioClip ClickSound => clickSound;

        void Start()
        {
            src = transform.parent.parent.GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            src.clip = hoverSound;
            src.Play();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            src.clip = clickSound;
            src.Play();
        }
    }
}