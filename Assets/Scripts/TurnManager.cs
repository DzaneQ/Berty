using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private GameObject EndTurnButton;

    private FieldGrid fieldGrid;
    private CardManager cardManager;
    private bool isStarting = true;

    private enum Step
    {
        Move,
        Selection,
        Payment
    }

    private enum Alignment
    {
        Player,
        Opponent
    }

    Step currentStep;
    Alignment currentAlign;

    private void Start()
    {
        fieldGrid = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cardManager = GetComponent<CardManager>();
        cardManager.InitializeCards();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && isStarting)
        {
            StartTheGame();
        }
    }

    public void StartTheGame()
    { 
        currentStep = Step.Move;
        currentAlign = Alignment.Player;
        cardManager.PullCards(IsPlayerTurn());
        isStarting = false;
    }



    public bool IsMoveNow()
    {
        if (currentStep == Step.Move) return true;
        else return false;
    }

    public bool IsSelectionNow()
    {
        if (currentStep == Step.Selection) return true;
        else return false;
    }

    public bool IsPaymentNow()
    {
        if (currentStep == Step.Payment) return true;
        else return false;
    }

    public void NextStep()
    {
        AdvanceStep(1);
    }

    public void PreviousStep()
    {
        AdvanceStep(-1);
    }

    private void AdvanceStep(int stepCount)
    {
        int currentStepInt = (int)currentStep;
        currentStepInt = (currentStepInt + stepCount) % 3;
        currentStep = (Step)currentStepInt;
        AdjustStep();
    }

    private void AdjustStep()
    {
        if (IsPaymentNow()) EndTurnButton.SetActive(false);
        else EndTurnButton.SetActive(true);
        if (!IsSelectionNow())
        {
            cardManager.DeselectCards();
        }
    }

    public bool IsPlayerTurn()
    {
        if (currentAlign == Alignment.Player) return true;
        else return false;
    }

    public void EndTurn()
    {
        if (cardManager.SelectedCard() != null)
            cardManager.SelectedCard().DeselectCard();
        SwitchAlign();
    }

    public void SwitchAlign()
    {
        if (currentAlign == Alignment.Player) currentAlign = Alignment.Opponent;
        else currentAlign = Alignment.Player;
        cardManager.SwitchTables();
        cardManager.PullCards(IsPlayerTurn());
        Debug.Log("Currently: " + currentAlign);
    }





    // For testing purposes
    //private void TestDiscards()
    //{
    //    for (int i = 0; i < 2; i++)
    //    {
    //        GameObject discardedCard = Instantiate(CardPile, new Vector3(0, 0, 0), Quaternion.identity);
    //        discardedCard.transform.SetParent(DiscardPile.transform, false);
    //        rb = discardedCard.AddComponent<Rigidbody>();
    //        rb.detectCollisions = true;
    //    }
    //}
}
