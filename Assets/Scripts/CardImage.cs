using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    private Vector3 cardPosition;
    private bool isSelected;
    private RectTransform card;
    private TurnManager turn;
    private CardManager cardManager;
    private Transform table;
    private Transform lastTable;

    //private delegate void ClickHandler();
    //ClickHandler leftClick;

    private void Start()
    {
        card = GetComponent<RectTransform>();
        turn = GameObject.Find("EventSystem").GetComponent<TurnManager>();
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        isSelected = false;
        //leftClick = SelectCard;
    }

    public void AssignTable()
    {
        table = transform.parent;
    }

    public Transform TableAssigned()
    {
        return table;
    }

    public bool IsCardSelected()
    {
        if (isSelected) return true;
        return false;
    }

    public void CardClick()
    {
        Debug.Log("Clicked.");
        if (Input.GetMouseButtonDown(0))
        {
            //leftClick();
            SwitchCardSelection();
        }
    }

    private void SwitchCardSelection()
    {
        if (isSelected) DeselectPosition();
        else if (!turn.IsSelectionNow()) SelectPosition();
    }

    private void SelectPosition()
    {
        cardPosition = card.position;
        card.position += new Vector3(0f, 25f, 0f);
        isSelected = true;
        if (turn.IsMoveNow()) turn.NextStep();
        //leftClick = DeselectCard;
    }

    public void DeselectPosition()
    {
        card.position = cardPosition;
        DeselectCard();
        if (turn.IsSelectionNow()) turn.PreviousStep();
    }

    //public void DeactivateCard()
    //{
    //    cardManager.RemoveFromTable(this);
    //    //transform.SetParent(table.parent.parent, false);
    //}

    private void SelectCard()
    {
        isSelected = true;
        //leftClick = DeselectPosition;
    }

    private void DeselectCard()
    {
        isSelected = false;
        //leftClick = SelectPosition;
    }

    public void DisableCard()
    {
        DeselectCard();
        lastTable = table;
        table = null;
    }

    public void ReturnCard()
    {
        cardManager.AddToTable(this, lastTable);
    }
}
