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
                card.RotateCard(-1);
                break;

            case "RotateLeft":
                card.RotateCard(1);
                break;

            case "MoveUp":
                card.MoveCard(0);
                break;

            case "MoveRight":
                card.MoveCard(90);
                break;

            case "MoveDown":
                card.MoveCard(180);
                break;

            case "MoveLeft":
                card.MoveCard(270);
                break;

            case "Return":
                card.CancelCard();
                break;

            default: break;
               
        }
    }

    public void EnableButton()
    {
        //Debug.Log("Enable: " + name);
        //if (rend == null) rend = GetComponent<Renderer>();
        rend.enabled = true;
        //if (coll == null) coll = GetComponent<Collider>();
        coll.enabled = true;
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
