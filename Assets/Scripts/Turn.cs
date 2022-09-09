using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class Turn : MonoBehaviour
{
    const int cardsToWin = 6;

    [SerializeField] private GameObject EndTurnButton;
    [SerializeField] private GameObject GameOverText;

    private Text endingMessage;
    private FieldGrid fg;
    private CardManager cm;
    private PricingSystem ps;
    private OpponentControl oc;
    private Step currentStep;
    private Alignment currentAlign;
    private bool interactableDisabled = false;

    private Step CurrentStep
    { get => currentStep;
        set
        {
            currentStep = value;
            AdjustStep();
        }
    }
    public Alignment CurrentAlignment
    { get => currentAlign; 
        private set
        {
            currentAlign = value;
            if (value == Alignment.Player) EnableInteractions();
            cm.SwitchTable(value);
            cm.PullCards(value);
            fg.AdjustNewTurn();
            Debug.Log("Currently: " + value);
            if (value == Alignment.Opponent) oc.PlayTurn();
        }
    }

    private void Awake()
    {
        cm = GetComponent<CardManager>();
        oc = GetComponent<OpponentControl>(); 
    }

    private void Start()
    {
        endingMessage = GameOverText.transform.GetChild(0).GetComponent<Text>();
        ps = new PricingSystem(cm);
        //fg = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        fg = FindObjectOfType<FieldGrid>();
        SetStartingParameters();
        //Debug.Log(SystemInfo.processorType);
        //Debug.Log(SystemInfo.graphicsDeviceName);
    }

    private void SetStartingParameters()
    {
        currentStep = Step.Move;
        CurrentAlignment = Alignment.Player;
    }

    public void EndTurn()
    {
        if (!CheckWinConditions()) 
        {
            cm.DeselectCards();
            SwitchAlign();
        }
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

    public void SetPayment(int price)
    {
        CurrentStep = Step.Payment;
        ps.DemandPayment(price);
    }

    private void AdvanceStep(int stepCount)
    {
        int currentStepInt = (int)CurrentStep;
        currentStepInt = (currentStepInt + stepCount) % 3;
        CurrentStep = (Step)currentStepInt;
    }

    private void AdjustStep()
    {
        //if (IsPaymentNow()) EndTurnButton.SetActive(false);
        //else ShowEndTurnButton();
        ShowEndTurnButton(!IsPaymentNow());
        if (IsMoveNow())
        {
            cm.DeselectCards();
            fg.AdjustCardButtons();
        }
    }

    public bool CheckOffer()
    {
        return ps.CheckOffer();
    }

    private void SwitchAlign()
    {
        CurrentAlignment = CurrentAlignment == Alignment.Player ? Alignment.Opponent : Alignment.Player;
    }

    private bool CheckWinConditions()
    {
        if (fg.AlignedFields(Alignment.Player).Count >= cardsToWin)
        {
            Debug.Log("Player got " + fg.AlignedFields(Alignment.Player).Count + " cards!");
            EndTheGame(Alignment.Player);
            return true;
        }
        if (fg.AlignedFields(Alignment.Opponent).Count >= cardsToWin)
        {
            Debug.Log("Opponent got " + fg.AlignedFields(Alignment.Opponent).Count + " cards!");
            EndTheGame(Alignment.Opponent);
            return true;
        }
        if (cm.ArePilesEmpty())
        {
            Debug.Log("Piles are empty!");
            EndTheGame(fg.HigherByAmountOfType());
            return true;
        }
        return false;
    }

    private void EndTheGame(Alignment winner)
    {
        if (winner == Alignment.Player) endingMessage.text = "Wygrana!";
        if (winner == Alignment.Opponent) endingMessage.text = "Przegrana!";
        DisableInteractions();
        GameOverText.SetActive(true);
    }

    //private Alignment Winner()
    //{
    //    if (fg.AlignedFields(Alignment.Player).Count >= cardsToWin) return Alignment.Player;
    //    if (fg.AlignedFields(Alignment.Opponent).Count >= cardsToWin) return Alignment.Opponent;
    //    if (fg.HighestAmountOfType(Alignment.Player) > fg.HighestAmountOfType(Alignment.Opponent)) return Alignment.Player;
    //    if (fg.HighestAmountOfType(Alignment.Player) < fg.HighestAmountOfType(Alignment.Opponent)) return Alignment.Opponent;
    //    return Alignment.None;
    //}

    public void EnableInteractions()
    {
        fg.UnlockInteractables();
        interactableDisabled = false;
        ShowEndTurnButton();
    }

    public void DisableInteractions()
    {
        fg.LockInteractables();
        cm.HideTables();
        EndTurnButton.SetActive(false);
        interactableDisabled = true;
    }

    private void ShowEndTurnButton(bool value = true)
    {
        if (!interactableDisabled) EndTurnButton.SetActive(value);
        else EndTurnButton.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
