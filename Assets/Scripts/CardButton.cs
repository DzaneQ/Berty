using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{

    CardSprite card;
    Renderer rend;
    Collider coll;
    [SerializeField] Material dexterityMaterial;

    void Start()
    {
        card = GetComponentInParent<CardSprite>();
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        //Debug.Log("OnMouseDown called: " + name);
        switch (name)
        {
            case "RotateRight":
                RotateCard(-1);
                break;

            case "RotateLeft":
                RotateCard(1);
                break;

            case "Return":
                card.CancelCard();
                break;

            default: break;
               
        }
    }

    private void RotateCard(int direction)
    {
        card.transform.Rotate(0, 0, direction * 90);
    }

    public void EnableButton()
    {
        //Debug.Log("Enable: " + name);
        //if (rend == null) rend = GetComponent<Renderer>();
        rend.enabled = true;
        //if (coll == null) coll = GetComponent<Collider>();
        coll.enabled = true;
        //Debug.Log(rend.enabled);
    }

    public void DisableButton()
    {
        //Debug.Log("Disable: " + name);
        if (rend == null) rend = GetComponent<Renderer>();
        rend.enabled = false;
        if (coll == null) coll = GetComponent<Collider>();
        coll.enabled = false;
    }

    public void ShowDexterityButton()
    {
        rend.material = dexterityMaterial;
        EnableButton();
    }
}
