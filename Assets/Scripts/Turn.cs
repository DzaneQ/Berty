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
    {
        get => currentStep;
        set
        {
            currentStep = value;
            AdjustStep();
        }
    }
    public Alignment CurrentAlignment
    {
        get => currentAlign;
        private set
        {
            if (value == Alignment.None) return;
            if (value == currentAlign) throw new Exception("Switching to the same alignment."); 
            fg.DisableAllButtons();
            currentAlign = value;
            cm.SwitchTable(value);
            if (value == Alignment.Player) EnableInteractions();
            if (value == Alignment.Opponent && oc != null) DisableInteractions();
            if (!cm.PullCards(value))
            {
                CheckWinConditions(true);
                return;
            }
            fg.AdjustNewTurn();
            Debug.Log("Currently: " + value);
            cm.DebugPrintCardCollection(value);
            ExecuteAutomaticOpponentTurn();
        }
    }

    public bool InteractableDisabled => interactableDisabled;
    public CardManager CM => cm;

    private void Awake()
    {
        if (Debug.isDebugBuild) Application.targetFrameRate = 20;
        cm = GetComponent<CardManager>();
        oc = GetComponent<OpponentControl>();
        fg = FindObjectOfType<FieldGrid>();
    }

    private void Start()
    {
        endingMessage = GameOverText.transform.GetChild(0).GetComponent<Text>();
        ps = new PricingSystem(cm);
        SetStartingParameters();
        //Debug.Log(SystemInfo.processorType);
        //Debug.Log(SystemInfo.graphicsDeviceName);
    }

    private void SetStartingParameters()
    {
        currentStep = Step.Move;
        CurrentAlignment = Alignment.Player;
    }

    public void EndTurn() // Note: If the character in CardManager.TakeRandomCharacter() not found, the game will end!
    {
        if (CheckWinConditions()) return;
        cm.DeselectCards();
        SwitchAlign();
    }

    public bool IsItMoveTime()
    {
        return CurrentStep == Step.Move;
    }

    public bool IsItPaymentTime()
    {
        return CurrentStep == Step.Payment;
    }

    public void SetMoveTime()
    {
        if (IsItMoveTime()) throw new Exception("Trying to change move step into self.");
        CurrentStep = Step.Move;
    }

    public void SetPayment(int price)
    {
        if (IsItPaymentTime()) throw new Exception("Trying to set while in the payment step.");
        CurrentStep = Step.Payment;
        ps.DemandPayment(price);
    }

    private void AdjustStep()
    {
        ShowEndTurnButton(IsItMoveTime());
        if (IsItPaymentTime()) return;
        cm.DeselectCards();
        fg.ActivateCardButtons();
    }

    public void ExecutePrincessTurn(Alignment decidingAlign)
    {
        Debug.Log("Executing special turn for alignment: " + decidingAlign);
        if (decidingAlign == Alignment.None) throw new Exception("Special turn for no alignment.");
        if (decidingAlign == Alignment.Opponent && oc != null) oc.ExecutePrincessTurn();
        else
        {
            CurrentStep = Step.Special;
            fg.SetTargetableCards(decidingAlign);
        }
    }

    public void ExecuteResurrection(Alignment decidingAlign)
    {
        Debug.Log("Executing special turn for alignment: " + decidingAlign);
        if (decidingAlign == Alignment.None) throw new Exception("Special turn for no alignment.");
        if (!cm.AreThereDeadCards()) return;
        if (decidingAlign == Alignment.Opponent && oc != null) oc.ExecuteResurrection();
        else
        {
            CurrentStep = Step.Special;
            cm.DisplayDeadCards(decidingAlign);
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

    private bool CheckWinConditions(bool forceEnd = false)
    {
        if (fg.AlignedFields(Alignment.Player, true).Count >= cardsToWin)
        {
            Debug.Log("Player got " + fg.AlignedFields(Alignment.Player).Count + " cards!");
            EndTheGame(Alignment.Player);
            return true;
        }
        if (fg.AlignedFields(Alignment.Opponent, true).Count >= cardsToWin)
        {
            Debug.Log("Opponent got " + fg.AlignedFields(Alignment.Opponent).Count + " cards!");
            EndTheGame(Alignment.Opponent);
            return true;
        }
        if (forceEnd)
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

    public void EnableInteractions()
    {
        //fg.UnlockInteractables();
        interactableDisabled = false;
        ShowEndTurnButton(true);
    }

    public void DisableInteractions()
    {
        //fg.LockInteractables();
        cm.HideTables();
        EndTurnButton.SetActive(false);
        interactableDisabled = true;
    }

    public void ExecuteAutomaticOpponentTurn() // TODO: Make it clean and private.
    {
        if (CurrentAlignment == Alignment.Opponent && oc != null) oc.PlayTurn();
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
