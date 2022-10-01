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
            if (value == Alignment.Opponent && oc != null) oc.PlayTurn();
        }
    }

    public bool InteractableDisabled => interactableDisabled;

    private void Awake()
    {
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

    public void EndTurn()
    {
        if (CheckWinConditions()) return;
        cm.DeselectCards();
        SwitchAlign();
    }

    public bool IsItPaymentTime()
    {
        return CurrentStep == Step.Payment;
    }

    public void UnsetPayment()
    {
        if (!IsItPaymentTime()) throw new Exception("Trying to unset outside the payment step.");
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
        ShowEndTurnButton(!IsItPaymentTime());
        if (IsItPaymentTime()) return;
        cm.DeselectCards();
        fg.AdjustCardButtons();
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
