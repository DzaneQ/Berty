using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSprite : MonoBehaviour
{
    private Field occupiedField;
    private CardManager cardManager;
    private Turn turn;
    private CardButton[] cardButton;
    private Transform healthBar;
    private CardImage imageReference;
    private FieldGrid grid;
    private int[] relCoord = new int[2];
    private float defAngle;
    private Rigidbody cardRB;
    private Vector3 defPosition;
    private Quaternion defRotation;
    private CardState state;
    private SpriteRenderer spriteRenderer;
    private Character character;
    private bool hasAttacked;
    private bool isLocked;
    public Turn Turn { get => turn; }
    private CardState State 
    { 
        get => state;
        set
        {
            state = value;
            state.InitiateState(this);
        }
    }
    public Character Character
    {
        get => character;
        private set
        {
            character = value;
            spriteRenderer.sprite = imageReference.Sprite;
        }
    }
    public bool HasAttacked { get => hasAttacked; }
    public bool IsLocked { get => isLocked;}

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

    private void Start()
    {
        turn = GameObject.Find("EventSystem").GetComponent<Turn>();
        cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
        healthBar = transform.GetChild(8).transform.GetChild(1); 
        grid = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cardButton = new CardButton[transform.childCount];
        cardButton = transform.GetComponentsInChildren<CardButton>();
        defAngle = transform.localEulerAngles.z;
        occupiedField = transform.GetComponentInParent<Field>();
        State = new InactiveState();
        Unlock();
        ResetAttack();
        InitializeRigidbody();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.gameObject);
        //Debug.Log(occupiedField.gameObject);
        if (collision.gameObject == occupiedField.gameObject)
        {
            State.HandleFieldCollision();
        }
    }

    public void OnMouseDown()
    {
        //Debug.Log("Clicked: " + name);
        if (!isLocked) State.HandleClick();
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
        if (State.GetType() != typeof(InactiveState)) throw new Exception("Wrong state to activate: " + State.GetType());
        State = new NewCardState();
    }

    public void ApplyPhysics(bool isApplied = true)
    {
        //Debug.Log("Is card kinematic before? " + cardRB.isKinematic);
        //Debug.Log("Psychics set: " + !isApplied);
        cardRB.isKinematic = !isApplied;
        //Debug.Log("Is card kinematic after? " + cardRB.isKinematic);
    }

    public void ResetAttack()
    {
        hasAttacked = false;
    }

    public void Lock()
    {
        isLocked = true;
    }

    public void Unlock()
    {
        isLocked = false;
    }

    private void DeactivateCard()
    {
        occupiedField.ConvertField(Alignment.None);
        State = new InactiveState();
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

    public Field OccupiedField 
    {
        set
        {
            occupiedField = value;
            transform.SetParent(value.transform, false);
        } 
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

    public void CallPayment(int price)
    {
        //Debug.Log("Payment called!");
        grid.DisableAllButtons();
        turn.SetPayment(price);
    }

    public void ConfirmPayment(bool isAttacking = false)
    {
        if (!turn.IsPaymentNow()) throw new Exception("Confirming payment when not in payment mode.");
        if (!State.IsForPaymentConfirmation()) throw new Exception("Wrong card state for payment..");
        if (turn.CheckOffer())
        {
            //Debug.Log("Price is good!");
            cardManager.DiscardCards();
            State.TakePaidAction();
            turn.NextStep();
            grid.AdjustCardButtons();
        }
    }

    public void CancelPayment()
    {
        if (!turn.IsPaymentNow()) throw new Exception("Trying to cancel outside the payment step.");
        turn.NextStep();
    }

    public void CancelDecision()
    {
        State.Cancel();
    }

    public void CancelCard()
    {
        imageReference.ReturnCard();
        DeactivateCard();
    }

    public void PrepareToAttack()
    {
        if (!hasAttacked)
        {
            CallPayment(6 - Character.Dexterity);
            State = new AttackingState();
        }
    }

    public bool CanAttack(Field targetField)
    {
        if (targetField == null) return false;
        foreach (int[] distance in Character.AttackRange)
        {
            //Debug.Log("Relative coordinates: " + relCoord[0] + "," + relCoord[1]);
            int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
            //Debug.Log("Relative target: " + target[0] + "," + target[1]);
            if (targetField == GetRelativeField(target[0], target[1])) return true;
        }
        return false;
    }

    public void TryToAttack(Field targetField)
    {
        Debug.Log("Seeing attack on field - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        if (CanAttack(targetField)) targetField.OccupantCard.TakeDamage(Character.Strength, targetField);
        //foreach (int[] distance in Character.AttackRange)
        //{
        //    int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
        //    if (targetField == GetRelativeField(target[0], target[1])) 
        //        targetField.OccupantCard.TakeDamage(Character.Strength, targetField);
        //}
    }

    public void Attack()
    {
        hasAttacked = true;
        Debug.Log("Checking attacking status for character: " + Character.Name);
        foreach (int[] distance in Character.AttackRange)
        {
            Debug.Log("Checking distance: " + distance[0] + "," + distance[1]);
            int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
            Field targetField = GetRelativeField(target[0], target[1]);
            if (targetField != null) targetField.OccupantCard.TakeDamage(Character.Strength, occupiedField);
            if (targetField != null) Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
        }
    }

    public void TakeDamage(int damage, Field source)
    {
        Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
        if (gameObject.activeSelf)
        {
            int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
            int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
            //Debug.Log("Source distance: " + srcDistance[0] + "," + srcDistance[1]);
            Debug.Log("Character health: " + Character.Health);
            if (!Character.CanBlock(srcDistance)) Character.TakeDamage(damage);
            if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(Character.Strength, source);
            Debug.Log("Damage taken: " + damage + "; Remaining HP: " + Character.Health);
            if (Character.IsDead()) DeactivateCard();
            else UpdateHealthBar(Character.Health);
        }
    }

    public void UpdateHealthBar(int health)
    {
        float unitScale = 2.67f;
        float positionX0Unit = -6.5f;
        float positionX6Unit = 1.4f;
        float currentX = (positionX6Unit - positionX0Unit) / 6 * health + positionX0Unit;
        healthBar.localPosition = new Vector3(currentX, healthBar.localPosition.y, healthBar.localPosition.z);
        healthBar.localScale = new Vector3(unitScale * health, healthBar.localScale.y, healthBar.localScale.z);
    }

    public void DefendNewStand()
    {
        grid.AttackNewStand(occupiedField);
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
        for (int i = 1; i <= 3; i++)
        {
            cardButton[i].ChangeButtonToNeutral();
            cardButton[i].EnableButton();
        }
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

    private int[] GetTargetCoordinates(float angle)
    {
        int targetX = (int)Math.Round(relCoord[0] + Math.Sin(angle * Math.PI / 180));
        int targetY = (int)Math.Round(relCoord[1] + Math.Cos(angle * Math.PI / 180));
        //Debug.Log("Target X: " + targetX + "; Target Y: " + targetY + "; angle: " + angle);
        int[] coordinates = { targetX, targetY };
        return coordinates;
    }

    public void UpdateMoveButtons()
    {
        if (State.GetType() != typeof(ActiveState)) throw new Exception("Updating move buttons when not in active state.");
        for (int i = 4; i <= 7; i++)
        {
            int[] target = GetTargetCoordinates(i * 90);
            if (GetRelativeField(target[0], target[1]) == null || GetRelativeField(target[0], target[1]).IsOccupied())
            {
                cardButton[i].DisableButton();
                //Debug.Log("X: " + target[0] + "; Y:" + target[1] + " can't be accessed!");
                //Debug.Log("Disabled button: " + i);
            }
            else
            {
                cardButton[i].EnableButton();
                //Debug.Log("X: " + target[0] + "; Y:" + target[1] + " exists!");
                //Debug.Log("Enabled button: " + i);
            }
        }
    }

    public void MoveCard(int angle)
    {
        int returnButtonIndex = ((angle + 180) % 360) / 90 + 4;
        //Debug.Log("Return button: " + returnButtonIndex);
        int[] target = GetTargetCoordinates(angle);
        Field toField = GetRelativeField(target[0], target[1]);
        grid.SwapCards(occupiedField, toField);
        State = State.AdjustTransformChange(returnButtonIndex);
        UpdateRelativeCoordinates();

    }

    public void RotateCard(int angle)
    {
        int returnButtonIndex = (450 - angle) / 180;
        //Debug.Log("Return button: " + returnButtonIndex);
        transform.Rotate(0, 0, -angle);
        //Debug.Log("Z rotation:" + transform.localEulerAngles.z);
        if (State.GetType() != typeof(NewCardState)) State = State.AdjustTransformChange(returnButtonIndex);
        UpdateRelativeCoordinates();
    }

    public void ImportFromCardImage()
    {
        imageReference = cardManager.SelectedCard();
        Character = imageReference.Character;
        cardManager.RemoveFromTable(imageReference);
    }

    private float GetRelativeAngle()
    {
        return defAngle - transform.localRotation.eulerAngles.z;
    }

    private Field GetRelativeField(int targetX, int targetY)
    {
        //Debug.Log("Target X: " + targetX + "; Target Y: " + targetY + "; defAngle: " + defAngle);
        //Debug.Log("localEulerAngle (z): " + transform.localEulerAngles.z);
        return grid.GetRelativeField(targetX, targetY, GetRelativeAngle());
    }

    public void UpdateRelativeCoordinates(bool moveButtons = true)
    {
        float angle = GetRelativeAngle();
        relCoord = occupiedField.GetRelativeCoordinates(angle);
        //Debug.Log("IsTransformNew? " + isTransformNew);
        if (State.GetType() == typeof(ActiveState) && moveButtons) UpdateMoveButtons();
    }
}
