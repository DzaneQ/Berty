using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Turn : MonoBehaviour
{
    [SerializeField] private GameObject EndTurnButton;
    [SerializeField] private GameObject GameOverText;

    private FieldGrid grid;
    private CardManager cardManager;

    private Step currentStep;
    private Alignment currentAlign;

    private Step CurrentStep 
    { get => currentStep;
        set 
        {
            currentStep = value;
            AdjustStep();
        }
    }
    public Alignment CurrentAlignment { get => currentAlign; }

    private void Awake()
    {
        cardManager = GetComponent<CardManager>();
    }

    private void Start()
    {
        grid = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cardManager.InitializeCards();
        StartTheGame();
    }

    public void StartTheGame()
    { 
        currentStep = Step.Move;
        currentAlign = Alignment.Player;
        cardManager.PullCards(currentAlign);
    }

    public void EndTurn()
    {
        if (cardManager.SelectedCard() != null)
            cardManager.SelectedCard().DeselectPosition();
        SwitchAlign();
        grid.ChangeAlignmentOnFieldGrid();
        CheckWinConditions();
    }

    public bool IsMoveNow()
    {
        if (CurrentStep == Step.Move) return true;
        else return false;
    }

    public bool IsSelectionNow()
    {
        if (CurrentStep == Step.Selection) return true;
        else return false;
    }

    public bool IsPaymentNow()
    {
        if (CurrentStep == Step.Payment) return true;
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

    public void SetPayment()
    {
        CurrentStep = Step.Payment;
    }

    private void AdvanceStep(int stepCount)
    {
        int currentStepInt = (int)CurrentStep;
        currentStepInt = (currentStepInt + stepCount) % 3;
        CurrentStep = (Step)currentStepInt;
    }

    private void AdjustStep()
    {
        if (IsPaymentNow()) EndTurnButton.SetActive(false);
        else EndTurnButton.SetActive(true);
        if (IsMoveNow())
        {
            cardManager.DeselectCards();
            grid.AdjustCardButtons();
        }
    }



    public int IsPlayerTurn()
    {
        if (currentAlign == Alignment.Player) return 1;
        else return -1;
    }

    private void SwitchAlign()
    {
        if (currentAlign == Alignment.Player) currentAlign = Alignment.Opponent;
        else currentAlign = Alignment.Player;
        cardManager.SwitchTables();
        cardManager.PullCards(currentAlign);
        grid.AdjustCardButtons();
        Debug.Log("Currently: " + currentAlign);
    }

    private void CheckWinConditions()
    {
        if (cardManager.ArePilesEmpty()) EndTheGame();
    }

    private void EndTheGame()
    {
        grid.DisableAllButtons();
        EndTurnButton.SetActive(false);
        GameOverText.SetActive(true);
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
