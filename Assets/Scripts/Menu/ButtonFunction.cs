using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Berty.Menu
{
    public class ButtonFunction : MonoBehaviour, IPointerEnterHandler
    {
        private AudioSource src;
        [SerializeField] private AudioClip hoverSound;
        [SerializeField] private AudioClip clickSound;

        public AudioClip ClickSound => clickSound;

        void Start()
        {
            src = transform.parent.GetComponent<AudioSource>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            src.clip = hoverSound;
            src.Play();
        }

        /*public void OnPointerClick(PointerEventData eventData)
        {
            src.clip = clickSound;
            src.Play();
        }*/
    }
}