using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public partial class Turn : MonoBehaviour
{
    const int cardsToWin = 5;

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
    public Alignment CurrentAlignment { get => currentAlign; }

    private void Awake()
    {
        cm = GetComponent<CardManager>();
        oc = GetComponent<OpponentControl>(); 
    }

    private void Start()
    {
        endingMessage = GameOverText.transform.GetChild(0).GetComponent<Text>();
        ps = new PricingSystem(cm);
        fg = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        cm.InitializeCards();
        StartTheGame();
        //Debug.Log(SystemInfo.processorType);
        //Debug.Log(SystemInfo.graphicsDeviceName);
    }

    public void StartTheGame()
    {
        currentStep = Step.Move;
        currentAlign = Alignment.Player;
        cm.PullCards(currentAlign);
    }

    public void EndTurn()
    {
        if (CheckWinConditions()) EndTheGame();
        else
        {
            if (cm.SelectedCard() != null)
                cm.SelectedCard().ChangePosition();
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
        if (IsPaymentNow()) EndTurnButton.SetActive(false);
        else ShowEndTurnButton();
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

    public int IsPlayerTurn()
    {
        if (currentAlign == Alignment.Player) return 1;
        else return -1;
    }

    private void SwitchAlign()
    {
        if (currentAlign == Alignment.Player) currentAlign = Alignment.Opponent;
        else currentAlign = Alignment.Player;
        if (currentAlign == Alignment.Player) EnableInteractions();
        cm.ShowTable(currentAlign);
        cm.PullCards(currentAlign);
        fg.AdjustNewTurn();
        Debug.Log("Currently: " + currentAlign);
        if (currentAlign == Alignment.Opponent) oc.PlayTurn();
    }

    private bool CheckWinConditions()
    {
        if (cm.ArePilesEmpty()) return true;
        if (fg.AlignedFields(Alignment.Player).Count >= cardsToWin) return true;
        if (fg.AlignedFields(Alignment.Opponent).Count >= cardsToWin) return true;
        return false;
    }

    private void EndTheGame()
    {
        if (Winner() == Alignment.Player) endingMessage.text = "Wygrana!";
        if (Winner() == Alignment.Opponent) endingMessage.text = "Przegrana!";
        DisableInteractions();
        GameOverText.SetActive(true);
    }

    private Alignment Winner()
    {
        if (fg.AlignedFields(Alignment.Player).Count >= cardsToWin) return Alignment.Player;
        if (fg.AlignedFields(Alignment.Opponent).Count >= cardsToWin) return Alignment.Opponent;
        if (fg.HighestAmountOfType(Alignment.Player) > fg.HighestAmountOfType(Alignment.Opponent)) return Alignment.Player;
        if (fg.HighestAmountOfType(Alignment.Player) < fg.HighestAmountOfType(Alignment.Opponent)) return Alignment.Opponent;
        return Alignment.None;
    }

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

    private void ShowEndTurnButton()
    {
        if (!interactableDisabled) EndTurnButton.SetActive(true);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
