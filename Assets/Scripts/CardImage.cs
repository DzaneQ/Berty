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
        select = new SelectedCard(GetComponent<RectTransform>(), this);
    }

    public void AssignCharacter(Character newCharacter)
    {
        Character = newCharacter;
    }

    public void Unselect()
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
        if (turn.IsItMoveTime()) return cardManager.SelectedCard() == null;
        return false;
    }

    public void CardClick()
    {
        if (Input.GetMouseButtonDown(0)) ChangeSelection();
    }

    public void ChangeSelection()
    {
        //Debug.Log("Position changing...");
        if (transform.parent.name.Contains("Dead")) ReviveCard();
        else select = select.ChangePosition(CanSelect());
    }

    public void SetBackupTable()
    {
        select.SetToBackup();
    }

    //public void KillCard()
    //{
    //    select.KillCard();
    //}

    public void ReviveCard()
    {
        cardManager.ReviveCard(this);
        turn.SetMoveTime();
    }

    public void ReturnCard()
    {
        cardManager.AddToTable(this, select.ReturnCard());
    }
}
