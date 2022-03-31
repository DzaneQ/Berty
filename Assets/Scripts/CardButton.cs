using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardButton : MonoBehaviour
{

    private CardSprite card;
    private Renderer rend;
    private Collider coll;
    [SerializeField] private Material dexterityMaterial;
    private Material neutralMaterial;

    private void Awake()
    {
        card = GetComponentInParent<CardSprite>();
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
        neutralMaterial = rend.material;
    }

    private void OnMouseDown()
    {
        //Debug.Log("OnMouseDown called: " + name);
        switch (name)
        {
            case "RotateRight":
                card.RotateCard(90);
                break;

            case "RotateLeft":
                card.RotateCard(-90);
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
        rend.enabled = true;
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

    public void ChangeButtonToDexterity()
    {
        //Debug.Log("Changed to dexterity: " + name);
        rend.material = dexterityMaterial;
    }

    public void ChangeButtonToNeutral()
    {
        //Debug.Log("Changed to neutral: " + name);
        rend.material = neutralMaterial;
    }

}
