using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookupCard : MonoBehaviour
{
    private Image render;

    private void Start()
    {
        render = GetComponent<Image>();
    }

    public void ShowLookupCard(Sprite sprite)
    {
        render.sprite = sprite;
        render.enabled = true;
        //gameObject.SetActive(true);
    }

    public void HideLookupCard()
    {
        render.enabled = false;
        //gameObject.SetActive(false);
    }
}
