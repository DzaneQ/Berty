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
    private Transform table;

    private void Start()
    {
        card = GetComponent<RectTransform>();
        turn = GameObject.Find("EventSystem").GetComponent<TurnManager>();
        isSelected = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            SwitchCardSelection();
        }
    }

    private void SwitchCardSelection()
    {
        if (isSelected) DeselectCard();
        else if (!turn.IsSelectionNow()) SelectCard();
    }

    private void SelectCard()
    {
        cardPosition = card.position;
        card.position += new Vector3(0f, 25f, 0f);
        isSelected = true;
        if (turn.IsMoveNow()) turn.NextStep();
    }

    public void DeselectCard()
    {
        card.position = cardPosition;
        isSelected = false;
        if (turn.IsSelectionNow()) turn.PreviousStep();
    }

    public void DeactivateCard()
    {
        isSelected = false;
        transform.SetParent(table.parent.parent, false);
    }

    public void DisableCard()
    {
        table = null;
    }

    public void ReturnCard()
    {
        transform.SetParent(table, false);
    }
}
