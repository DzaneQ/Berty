using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    private Vector3 cardPosition;
    private SelectStatus select;
    private RectTransform card;
    private Turn turn;
    private CardManager cardManager;
    private Transform table;
    private Transform lastTable;
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
        card = GetComponent<RectTransform>();
        imageRenderer = GetComponent<Image>();
    }

    private void Start()
    {
        turn = GameObject.Find("EventSystem").GetComponent<Turn>();
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        select = new UnselectedCard(this);
        //leftClick = SelectCard;
    }

    //public void LoadSprite(Sprite sprite)
    //{
    //    if (imageRenderer == null) Debug.LogWarning("imageRenderer not assigned");
    //    //if (imageRenderer.sprite == null) Debug.LogWarning("imageRenderer.sprite not assigned");
    //    //imageRenderer.sprite = sprite;
    //}

    public void AssignCharacter(Character newCharacter)
    {
        Character = newCharacter;
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
        return select.IsCardSelected();
    }

    public bool CanSelect()
    {
        if (turn.IsSelectionNow()) return false;
        if (turn.IsPaymentNow()) return !turn.CheckOffer();
        return true;
    }

    public void CardClick()
    {
        //Debug.Log("Clicked cardImage.");
        if (Input.GetMouseButtonDown(0)) ChangePosition();
    }

    public void SavePosition()
    {
        cardPosition = card.position;
    }

    public void LoadPosition()
    {
        card.position = cardPosition;
    }

    public void ChangePosition()
    {
        select = select.ChangePosition();
    }

    public void SelectPosition()
    {
        SavePosition();
        card.position += new Vector3(0f, 25f, 0f);
        //select = new SelectedCard(this);
        if (turn.IsMoveNow()) turn.NextStep();
    }

    public void DeselectPosition()
    {
        LoadPosition();
        //select = new UnselectedCard(this);
        if (turn.IsSelectionNow()) turn.PreviousStep();
    }

    public void DisableCard()
    {
        select = new UnselectedCard(this);
        lastTable = table;
        table = null;
    }

    public void ReturnCard()
    {
        cardManager.AddToTable(this, lastTable);
    }
}
