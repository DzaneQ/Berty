using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
