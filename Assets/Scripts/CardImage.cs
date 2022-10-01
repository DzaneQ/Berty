using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    private SelectStatus select;
    private Turn turn;
    private CardManager cardManager;
    private Image imageRenderer;
    private Character character;
    public Character Character 
    { 
        get => character;
        private set
        {
            character = value;
            imageRenderer.sprite = Resources.Load<Sprite>("BERTY/" + character.Name);
        }
    }
    public Sprite Sprite
    {
        get => imageRenderer.sprite;
    }

    //private delegate void ClickHandler();
    //ClickHandler leftClick;

    private void Awake()
    {
        imageRenderer = GetComponent<Image>();
        turn = FindObjectOfType<Turn>();
        cardManager = FindObjectOfType<CardManager>();
        select = new SelectedCard(GetComponent<RectTransform>());
    }

    public void AssignCharacter(Character newCharacter)
    {
        Character = newCharacter;
    }

    public void AssignTable()
    {
        select = select.SetUnselected();
    }

    public bool IsCardSelected()
    {
        return select.IsCardSelected;
    }

    public bool CanSelect()
    {
        if (turn.IsItPaymentTime()) return !turn.CheckOffer();
        return cardManager.SelectedCard() == null;
    }

    public void CardClick()
    {
        if (Input.GetMouseButtonDown(0)) ChangePosition();
    }

    public void ChangePosition()
    {
        select = select.ChangePosition(CanSelect());
    }

    public void SetBackupTable()
    {
        select.SetToBackup();
    }

    public void ReturnCard()
    {
        //Transform lastTable = select.ReturnCard();
        cardManager.AddToTable(this, select.ReturnCard());
    }
}
