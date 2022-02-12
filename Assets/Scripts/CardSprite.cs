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
    private FieldGrid grid;
    private int[] relativeCoordinates = new int[2];
    private float defAngle;
    private Rigidbody cardRB;
    private Vector3 defPosition;
    private Quaternion defRotation;

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
        //if (!turn.IsSelectionNow()) throw new Exception("CardSprite created when step is not Selection.");
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        grid = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cardButton = new CardButton[transform.childCount];
        cardButton = transform.GetComponentsInChildren<CardButton>();
        defAngle = transform.localEulerAngles.z;
        occupiedField = transform.GetComponentInParent<Field>();
        InitializeRigidbody();
        SetDefaultTransform();
        //UpdateRelativeCoordinates();
        //for (int i = 0; i < transform.childCount; i++) 
        //    cardButton[i] = transform.GetChild(i).gameObject.GetComponent<CardButton>();
        //ImportFromImage();
        isNew = true;
        gameObject.SetActive(false);
        //turn.NextStep();
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
        //occupiedField = collision.gameObject.GetComponent<Field>();
        UpdateRelativeCoordinates();
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

    private void InitializeRigidbody()
    {
        cardRB = gameObject.GetComponent<Rigidbody>();
        cardRB.detectCollisions = true;
        cardRB.isKinematic = true;
    }

    public void ActivateCard()
    {
        //ShowNeutralButtons();
        gameObject.SetActive(true);
        if (!turn.IsSelectionNow()) throw new Exception("CardSprite created when step is not Selection.");
        ImportFromImage();
        cardRB.isKinematic = false;
        turn.NextStep();
    }

    public void UnlockButtons()
    {
        if (isNew) ShowNeutralButtons();
        else ShowDexterityButtons();
    }

    private void DeactivateCard()
    {
        cardRB.isKinematic = false;
        gameObject.SetActive(false);
        SetCardToDefaultTransform();
    }

    private void SetDefaultTransform()
    {
        defPosition = transform.localPosition;
        defRotation = transform.localRotation;
    }

    private void SetCardToDefaultTransform()
    {
        transform.localPosition = defPosition;
        transform.localRotation = defRotation;
    }

    public void SetOccupiedField(Field value)
    {
        occupiedField = value;
        transform.SetParent(value.transform, false);
    }

    public void CancelCard()
    {
        imageReference.ReturnCard();
        occupiedField.BecomeNeutral();
        //cardManager.DeselectCards();
        turn.NextStep();
        DeactivateCard();
    }

    public void DisableButtons()
    {
        //Debug.Log("Disable buttons!");
        foreach (CardButton button in cardButton) button.DisableButton();
    }

    private void ShowNeutralButtons()
    {
        DisableButtons();
        for (int i = 1; i <= 3; i++) cardButton[i].EnableButton();
    }

    public void ShowDexterityButtons()
    {
        DisableButtons();
        for (int i = 2; i <= 7; i++)
        {
            cardButton[i].ShowDexterityButton();
        }
        UpdateMoveButtons();
    }

    public void UpdateMoveButtons()
    {
        for (int i = 4; i <= 7; i++)
        {
            int targetX = (int)Math.Round(relativeCoordinates[0] + Math.Sin(i * Math.PI / 2));
            int targetY = (int)Math.Round(relativeCoordinates[1] - Math.Cos(i * Math.PI / 2));
            if (grid.GetField(targetX, targetY) == null || grid.GetField(targetX, targetY).IsOccupied())
            {
                cardButton[i].DisableButton();
                //Debug.Log("X: " + targetX + "; Y:" + targetY + " can't be accessed!");
                //Debug.Log("Disabled button: " + i);
            }
            else
            {
                cardButton[i].EnableButton();
                //Debug.Log("X: " + targetX + "; Y:" + targetY + " exists!");
                //Debug.Log("Enabled button: " + i);
            }
        }
    }

    public void MoveCard(int angle)
    {
        int targetX = (int)Math.Round(relativeCoordinates[0] + Math.Sin(angle * Math.PI / 180));
        int targetY = (int)Math.Round(relativeCoordinates[1] - Math.Cos(angle * Math.PI / 180));
        Field toField = grid.GetRelativeField(targetX, targetY, transform.localEulerAngles.z - defAngle);
        grid.SwapCards(occupiedField, toField);
        //TODO: occupiedField.PlayCard();
        UpdateRelativeCoordinates();
    }

    public void RotateCard(int direction)
    {
        transform.Rotate(0, 0, direction * 90);
        //Debug.Log("Z rotation:" + transform.localEulerAngles.z);
        UpdateRelativeCoordinates();
    }

    private void ImportFromImage()
    {
        imageReference = cardManager.SelectedCard();
        GetComponent<SpriteRenderer>().sprite = imageReference.gameObject.GetComponent<Image>().sprite;
        cardManager.RemoveFromTable(imageReference);
    }

    public void UpdateRelativeCoordinates()
    {
        float angle = transform.localRotation.eulerAngles.z - defAngle;
        relativeCoordinates = occupiedField.GetRelativeCoordinates(angle);
        if (!isNew) UpdateMoveButtons();
    }
}
