using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    private SelectStatus select;
    private CardManager cardManager;
    private Image imageRenderer;
    private Character character;
    private Transform returnTable;

    private Turn Turn => cardManager.Turn;
    public Character Character 
    { 
        get => character;
        private set
        {
            character = value;
            imageRenderer.sprite = Resources.Load<Sprite>("BERTYmirrorY/" + character.Name);
        }
    }
    public Sprite Sprite
    {
        get => imageRenderer.sprite;
    }

    private void Awake()
    {
        imageRenderer = GetComponent<Image>();
        cardManager = FindObjectOfType<CardManager>();
        select = new UnselectedCard(GetComponent<RectTransform>(), GetComponent<AnimatingCardImage>());
    }

    public void AssignCharacter(Character newCharacter)
    {
        Character = newCharacter;
    }

    public void Unselect()
    {
        select = select.UnselectAutomatically();
    }

    public bool IsCardSelected()
    {
        return select.IsCardSelected;
    }

    public void CardClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (transform.parent.name.Contains("Dead")) ReviveCard();
        else ChangeSelection();
    }

    public void CardFocusOn()
    {
        cardManager.ShowLookupCard(imageRenderer.sprite);
    }

    public void CardFocusOff()
    {
        cardManager.HideLookupCard();
    }

    public void ChangeSelection(bool ignoreAnimation = false)
    {
        if (CanSelect()) select = select.ChangePosition(!ignoreAnimation);
    }

    public bool CanSelect()
    {
        if (IsCardSelected()) return true;
        if (Turn.IsItPaymentTime()) return !Turn.CheckOffer();
        if (Turn.IsItMoveTime()) return cardManager.SelectedCard() == null;
        return false;
    }

    public void SetBackupTable()
    {
        returnTable = transform.parent;
    }

    public void ReviveCard()
    {
        cardManager.ReviveCard(this);
        Turn.SetMoveTime();
    }

    public void ReturnCard()
    {
        cardManager.AddToTable(this, returnTable);
    }
}