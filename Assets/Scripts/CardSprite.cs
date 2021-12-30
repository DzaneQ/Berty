using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour
{
    int strength;
    int will;
    int dexterity;
    int health;
    int startStrength;
    int startWill;
    int startDexterity;
    int startHealth;
    
    private Field occupiedField;
    private CardManager cardManager;
    private TurnManager turn;
    private CardButton[] cardButton;
    private bool isNew;
    private CardImage imageReference;

    /* CardSprite children in Hierarchy tab must be in the following order:
        0 - Confirm
        1 - Return
        2 - RotateLeft
        3 - RotateRight
        4 - MoveUp
        5 - MoveRight
        6 - MoveDown
        7 - MoveLeft
        8 - Attack
     */
    
    private void Start()
    {
        turn = GameObject.Find("EventSystem").GetComponent<TurnManager>();
        if (!turn.IsSelectionNow()) throw new Exception("CardSprite created when step is not Selection.");
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        cardButton = new CardButton[transform.childCount];
        cardButton = transform.GetComponentsInChildren<CardButton>();
        //for (int i = 0; i < transform.childCount; i++) 
        //    cardButton[i] = transform.GetChild(i).gameObject.GetComponent<CardButton>();
        ShowNeutralButtons();
        ImportFromImage();
        isNew = true;
        turn.NextStep();
    }


    CardSprite(int strStat, int willStat, int dexStat, int hpStat)
    {
        startStrength = strStat;
        strength = startStrength;
        startWill = willStat;
        will = startWill;
        startDexterity = dexStat;
        dexterity = startDexterity;
        startHealth = hpStat;
        health = startHealth;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Field>() != null)
            occupiedField = collision.gameObject.GetComponent<Field>();
    }

    private void OnMouseDown()
    {
        //Debug.Log("Clicked: " + name);
        //if (turn.IsPaymentNow()) Debug.Log("Payment time!");
        //if (isNew) Debug.Log("The card's new!");
        //if (!isNew) Debug.Log("It's not new!");

        if (turn.IsPaymentNow() && isNew)
        {
            cardManager.DiscardCards();
            turn.NextStep();
            ShowDexterityButtons();
            cardManager.RemoveFromTable(imageReference);
            //imageReference.DisableCard();
            isNew = false;
        }
    }

    public void CancelCard()
    {
        imageReference.ReturnCard();
        occupiedField.BecomeNeutral();
        //cardManager.DeselectCards();
        turn.NextStep();
        Destroy(gameObject);
    }

    private void DisableButtons()
    {
        //Debug.Log("Disable buttons!");
        foreach (CardButton button in cardButton) button.DisableButton();
    }

    private void ShowNeutralButtons()
    {
        DisableButtons();
        for (int i = 1; i <= 3; i++) cardButton[i].EnableButton();
    }

    private void ShowDexterityButtons()
    {
        DisableButtons();
        for (int i = 2; i <= 7; i++)
        {
            cardButton[i].ShowDexterityButton();
        }
    }

    private void ImportFromImage()
    {
        imageReference = cardManager.SelectedCard();
        GetComponent<SpriteRenderer>().sprite = imageReference.gameObject.GetComponent<Image>().sprite;
        imageReference.DeactivateCard();
    }
}
