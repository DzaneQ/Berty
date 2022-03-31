using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour
{
    //int strength;
    //int will;
    //int dexterity;
    //int health;
    //int startStrength;
    //int startWill;
    //int startDexterity;
    //int startHealth;

    private Field occupiedField;
    private CardManager cardManager;
    private Turn turn;
    private CardButton[] cardButton;
    private CardImage imageReference;
    private FieldGrid grid;
    private int[] relativeCoordinates = new int[2];
    private float defAngle;
    private Rigidbody cardRB;
    private Vector3 defPosition;
    private Quaternion defRotation;
    private CardState state;
    private SpriteRenderer spriteRenderer;
    private CardState State 
    { 
        get => state;
        set
        {
            state = value;
            state.InitiateState(this);
        }
    }

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

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetDefaultTransform();
    }

    //private void OnDisable()
    //{
    //    ApplyPhysics(false);
    //    SetCardToDefaultTransform();
    //}

    private void Start()
    {
        turn = GameObject.Find("EventSystem").GetComponent<Turn>();
        //if (!turn.IsSelectionNow()) throw new Exception("CardSprite created when step is not Selection.");
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        grid = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cardButton = new CardButton[transform.childCount];
        cardButton = transform.GetComponentsInChildren<CardButton>();
        defAngle = transform.localEulerAngles.z;
        occupiedField = transform.GetComponentInParent<Field>();
        State = new InactiveState();
        InitializeRigidbody();
    }

    //private void OnEnable()
    //{
    //    CallPayment();
    //    ImportFromImage();
    //    ApplyPhysics();
    //}

    //private void Start()
    //{
    //    //UpdateRelativeCoordinates();
    //    //for (int i = 0; i < transform.childCount; i++) 
    //    //    cardButton[i] = transform.GetChild(i).gameObject.GetComponent<CardButton>();
    //    //ImportFromImage();
    //    //gameObject.SetActive(false);
    //    //turn.NextStep();
    //}




    //CardSprite(int strStat, int willStat, int dexStat, int hpStat)
    //{
    //    startStrength = strStat;
    //    strength = startStrength;
    //    startWill = willStat;
    //    will = startWill;
    //    startDexterity = dexStat;
    //    dexterity = startDexterity;
    //    startHealth = hpStat;
    //    health = startHealth;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        //Debug.Log(occupiedField.gameObject);
        //occupiedField = collision.gameObject.GetComponent<Field>();
        if (collision.gameObject == occupiedField.gameObject)
        {
            State.HandleFieldCollision();
            UpdateRelativeCoordinates();
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("Clicked: " + name);
        //if (turn.IsPaymentNow()) Debug.Log("Payment time!");
        //if (isNew) Debug.Log("The card's new!");
        //if (!isNew) Debug.Log("It's not new!");
        State.HandleClick();
        //if (turn.IsPaymentNow() && isTransformNew)
        //{
        //    cardManager.DiscardCards();
        //    turn.NextStep();
        //    ShowDexterityButtons();
        //    cardManager.RemoveFromTable(imageReference);
        //    //imageReference.DisableCard();
        //    isTransformNew = false;
        //    grid.AdjustCardButtons();
        //}
    }


    private void InitializeRigidbody()
    {
        cardRB = GetComponent<Rigidbody>();
        cardRB.detectCollisions = true;
        cardRB.isKinematic = true;
    }

    public void ActivateCard()
    {
        if (!turn.IsSelectionNow()) throw new Exception("CardSprite created when step is not Selection.");
        if (State.GetType() != typeof(InactiveState)) throw new Exception("Wrong state to activate.");
        State = new NewCardState();
    }

    public void ApplyPhysics(bool isApplied = true)
    {
        cardRB.isKinematic = !isApplied;
    }

    private void DeactivateCard()
    {
        //cardRB.isKinematic = true;
        State = new InactiveState();
        //SetCardToDefaultTransform();
    }

    private void SetDefaultTransform()
    {
        defPosition = transform.localPosition;
        defRotation = transform.localRotation;
    }

    public void SetCardToDefaultTransform()
    {
        transform.localPosition = defPosition;
        transform.localRotation = defRotation;
    }

    //public void SetOccupiedField(Field value)
    //{
    //    occupiedField = value;
    //    transform.SetParent(value.transform, false);
    //}

    public Field OccupiedField 
    { 
        set
        {
            occupiedField = value;
            transform.SetParent(value.transform, false);
        } 
    }
    public void HandleAlignmentChange()
    {
        State.HandleAlignmentChange();
    }


    public void NextState()
    {
        State = State.GoToNext();
    }

    public void SetActive()
    {
        State = new ActiveState();
    }

    public void SetIdle()
    {
        State = new IdleState();
    }

    public void CallPayment()
    {
        //Debug.Log("Payment called!");
        grid.DisableAllButtons();
        turn.SetPayment();
    }

    public void ConfirmPayment()
    {
        if (!turn.IsPaymentNow()) throw new Exception("Confirming payment when not in payment mode.");
        if (State.GetType() != typeof(NewCardState) && State.GetType() != typeof(NewTransformState)) 
            throw new Exception("Not new card to make a payment.");
        cardManager.DiscardCards();
        //cardManager.RemoveFromTable(imageReference);
        State = new ActiveState();
        turn.NextStep();
        //imageReference.DisableCard();
        grid.AdjustCardButtons();
    }

    public void CancelPayment()
    {
        if (!turn.IsPaymentNow()) throw new Exception("Trying to cancel outside the payment step.");
        turn.NextStep();
    }

    public void CancelCard()
    {
        if (State.GetType() != typeof(NewCardState)) throw new Exception("Not new card to cancel.");
        imageReference.ReturnCard();
        occupiedField.ConvertField(Alignment.None);
        //cardManager.DeselectCards();
        DeactivateCard();
        CancelPayment();
    }

    public void EnableNeutralButton(int index)
    {
        cardButton[index].ChangeButtonToNeutral();
        cardButton[index].EnableButton();
    }

    public void DisableButtons()
    {
        //Debug.Log("Disable buttons!");
        foreach (CardButton button in cardButton) button.DisableButton();
    }

    public void ShowNeutralButtons()
    {
        if (State.GetType() != typeof(NewCardState)) throw new Exception("New card buttons reveal attempt for not new card state!");
        //DisableButtons();
        for (int i = 1; i <= 3; i++) cardButton[i].EnableButton();
    }

    public void ShowDexterityButtons()
    {
        DisableButtons();
        for (int i = 2; i <= 7; i++)
        {
            cardButton[i].ChangeButtonToDexterity();
            cardButton[i].EnableButton();
        }
        UpdateMoveButtons();
    }

    public void UpdateMoveButtons()
    {
        if (State.GetType() != typeof(ActiveState)) throw new Exception("Updating move buttons when not in active state.");
        for (int i = 4; i <= 7; i++)
        {
            int targetX = (int)Math.Round(relativeCoordinates[0] + Math.Sin(i * Math.PI / 2));
            int targetY = (int)Math.Round(relativeCoordinates[1] - Math.Cos(i * Math.PI / 2));
            if (GetRelativeField(targetX, targetY) == null || GetRelativeField(targetX, targetY).IsOccupied())
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
        int returnButtonIndex = ((angle + 180) % 360) / 90 + 4;
        Debug.Log("Return button: " + returnButtonIndex);
        //if (State.GetType() == typeof(ActiveState)) State = new NewTransformState();
        int targetX = (int)Math.Round(relativeCoordinates[0] + Math.Sin(angle * Math.PI / 180));
        int targetY = (int)Math.Round(relativeCoordinates[1] - Math.Cos(angle * Math.PI / 180));
        Field toField = GetRelativeField(targetX, targetY);
        grid.SwapCards(occupiedField, toField);
        //TODO: occupiedField.PlayCard();
        State = State.AdjustTransformChange(returnButtonIndex);
        UpdateRelativeCoordinates();

    }

    public void RotateCard(int angle)
    {
        int returnButtonIndex = (450 - angle) / 180;
        Debug.Log("Return button: " + returnButtonIndex);
        //if (State.GetType() == typeof(ActiveState)) State = new NewTransformState();
        transform.Rotate(0, 0, -angle);
        //Debug.Log("Z rotation:" + transform.localEulerAngles.z);
        if (State.GetType() != typeof(NewCardState)) State = State.AdjustTransformChange(returnButtonIndex);
        UpdateRelativeCoordinates();
    }

    public void ImportFromImage()
    {
        imageReference = cardManager.SelectedCard();
        spriteRenderer.sprite = imageReference.Sprite();
        cardManager.RemoveFromTable(imageReference);
    }

    private Field GetRelativeField(int targetX, int targetY)
    {
        return grid.GetRelativeField(targetX, targetY, transform.localEulerAngles.z - defAngle);
    }

    public void UpdateRelativeCoordinates(bool moveButtons = true)
    {
        float angle = transform.localRotation.eulerAngles.z - defAngle;
        relativeCoordinates = occupiedField.GetRelativeCoordinates(angle);
        //Debug.Log("IsTransformNew? " + isTransformNew);
        if (State.GetType() == typeof(ActiveState) && moveButtons) UpdateMoveButtons();
    }
}
