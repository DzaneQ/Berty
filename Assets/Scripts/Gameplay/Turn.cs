using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Berty.Display;
using Berty.Audio;
using Berty.AutomaticPlayer;
using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;
using Berty.Grid;

namespace Berty.Gameplay
{
    public partial class Turn : MonoBehaviour
    {
        const int cardsToWin = 6;

        [SerializeField] private GameObject TheButton;
        [SerializeField] private GameObject GameOverText;

        private Text theButtonText;
        private Text endingMessage;
        private FieldGrid fg;
        private OutdatedCardManager cm;
        private OutdatedPricingSystem ps;
        private OpponentControl oc;
        private CameraMechanics ct;
        private StepEnum currentStep;
        private AlignmentEnum currentAlign;
        private bool interactableDisabled = false;

        private StepEnum CurrentStep
        {
            get => currentStep;
            set
            {
                currentStep = value;
                AdjustStep();
            }
        }
        public AlignmentEnum CurrentAlignment
        {
            get => currentAlign;
            private set
            {
                if (value == AlignmentEnum.None) return;
                if (value == currentAlign) throw new Exception("Switching to the same alignment.");
                fg.MakeAllStatesIdle();
                currentAlign = value;
                cm.SwitchTable(value);
                if (value == AlignmentEnum.Player) EnableInteractions();
                if (value == AlignmentEnum.Opponent && oc != null) DisableInteractions();
                if (!cm.PullCards(value))
                {
                    CheckWinConditions(true);
                    return;
                }
                fg.AdjustNewTurn();
                //Debug.Log("Currently: " + value);
                //cm.DebugPrintCardCollection(value);
                ExecuteAutomaticOpponentTurn();
            }
        }

        public bool InteractableDisabled => interactableDisabled;
        public FieldGrid FG => fg;
        public OutdatedCardManager CM => cm;
        public string TheButtonText
        {
            get => theButtonText.text;
            private set => theButtonText.text = value;
        }

        #region Setup
        private void Awake()
        {
            if (Debug.isDebugBuild) Application.targetFrameRate = 10;
            cm = GetComponent<OutdatedCardManager>();
            OpponentControl tempOC = GetComponent<OpponentControl>();
            if (tempOC != null && tempOC.enabled) oc = tempOC;
            fg = FindAnyObjectByType<FieldGrid>();
            ct = FindAnyObjectByType<CameraMechanics>();
        }

        private void Start()
        {
            endingMessage = GameOverText.transform.GetChild(0).GetComponent<Text>();
            theButtonText = TheButton.transform.GetChild(0).GetComponent<Text>();
            ps = new OutdatedPricingSystem(cm);
            SetStartingParameters();
            //DebugInit();
            //Debug.Log(SystemInfo.processorType);
            //Debug.Log(SystemInfo.graphicsDeviceName);
        }

        private void SetStartingParameters()
        {
            currentStep = StepEnum.Move;
            CurrentAlignment = AlignmentEnum.Player;
        }
        #endregion

        #region TurnsAndSteps
        public void EndTurn()
        {
            if (CheckWinConditions()) return;
            cm.DeselectCards();
            SwitchAlign();
        }

        public bool IsItMoveTime()
        {
            return CurrentStep == StepEnum.Move;
        }

        public bool IsItPaymentTime()
        {
            return CurrentStep == StepEnum.Payment;
        }

        public bool IsItSpecialTime()
        {
            return CurrentStep == StepEnum.Special;
        }

        public void SetMoveTime()
        {
            //Debug.Log("Setting move step.");
            if (IsItMoveTime()) throw new Exception("Trying to change move step into self.");
            CurrentStep = StepEnum.Move;
        }

        public void SetPayment(int price)
        {
            if (IsItPaymentTime()) throw new Exception("Trying to set while in the payment step.");
            CurrentStep = StepEnum.Payment;
            ps.DemandPayment(price);
        }

        private void AdjustStep()
        {
            ShowEndTurnButton(IsItMoveTime());
            if (!IsItPaymentTime()) cm.DeselectCards();
            if (IsItMoveTime()) fg.UpdateCardStates();
        }

        public bool CheckOffer() => ps.CheckOffer();

        private void SwitchAlign()
        {
            CurrentAlignment = CurrentAlignment == AlignmentEnum.Player ? AlignmentEnum.Opponent : AlignmentEnum.Player;
        }

        private void ShowEndTurnButton(bool value = true)
        {
            if (!interactableDisabled)
            {
                TheButtonText = "Koniec tury";
                TheButton.SetActive(value);
            }
            else TheButton.SetActive(false);
        }

        public void ShowCancelButton()
        {
            if (interactableDisabled) return;
            TheButtonText = "Cofnij";
            TheButton.SetActive(true);
        }
        #endregion

        #region CameraControl
        public float GetCameraRightAngle() => ct.RightAngleValue();

        public CardSpriteBehaviour GetFocusedCard() => ct.FocusedCard;
        #endregion

        #region CharacterSkillBased
        public void ExecutePrincessTurn(AlignmentEnum decidingAlign)
        {
            Debug.Log("Executing special turn for alignment: " + decidingAlign);
            if (decidingAlign == AlignmentEnum.None) throw new Exception("Special turn for no alignment.");
            StartCoroutine(WaitForPrincessTurn(decidingAlign));
        }

        private IEnumerator WaitForPrincessTurn(AlignmentEnum decidingAlign)
        {
            DisableInteractions();
            yield return new WaitForSeconds(0.2f);
            if (decidingAlign == AlignmentEnum.Opponent && oc != null) oc.ExecutePrincessTurn();
            else
            {
                CurrentStep = StepEnum.Special;
                EnableInteractions();
                fg.SetTargetableCards(decidingAlign);
            }
        }

        public void ExecuteResurrection()
        {
            Debug.Log("Executing resurrection!");
            if (!cm.AreThereDeadCards()) return;
            StartCoroutine(WaitForResurrection());
        }

        private IEnumerator WaitForResurrection()
        {
            DisableInteractions();
            yield return new WaitForSeconds(0.2f);
            if (currentAlign == AlignmentEnum.Opponent && oc != null) oc.ExecuteResurrection();
            else
            {
                CurrentStep = StepEnum.Special;
                EnableInteractions();
                cm.DisplayDeadCards();
            }
        }
        #endregion

        #region AutomaticOpponentBased
        public void EnableInteractions()
        {
            interactableDisabled = false;
            ShowEndTurnButton(IsItMoveTime());
        }

        public void DisableInteractions(bool hideTables = true)
        {
            if (hideTables) cm.HideTables();
            TheButton.SetActive(false);
            interactableDisabled = true;
        }

        public void ExecuteAutomaticOpponentTurn()
        {
            if (CurrentAlignment == AlignmentEnum.Opponent && oc != null) oc.PlayTurn();
        }

        public void SelectField(OutdatedFieldBehaviour target)
        {
            target.OccupantCard.ShowLookupCard(true);
            //ct.SetTargets(target);
        }

        public void UnselectField()
        {
            cm.HideLookupCard(true);
            ct.ClearTarget();
        }
        #endregion

        #region GameOver
        private bool CheckWinConditions(bool forceEnd = false)
        {
            if (fg.AlignedFields(AlignmentEnum.Player, true).Count >= cardsToWin)
            {
                Debug.Log("Player got " + fg.AlignedFields(AlignmentEnum.Player).Count + " cards!");
                EndTheGame(AlignmentEnum.Player);
                return true;
            }
            if (fg.AlignedFields(AlignmentEnum.Opponent, true).Count >= cardsToWin)
            {
                Debug.Log("Opponent got " + fg.AlignedFields(AlignmentEnum.Opponent).Count + " cards!");
                EndTheGame(AlignmentEnum.Opponent);
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

        private void EndTheGame(AlignmentEnum winner)
        {
            if (winner == AlignmentEnum.Player) endingMessage.text = "Wygrana!";
            if (winner == AlignmentEnum.Opponent) endingMessage.text = "Przegrana!";
            DisableInteractions();
            GameOverText.SetActive(true);
        }

        public void ReturnToMenu() // Note: Executed after clicking GameOver object.
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
        #endregion

        /*#region Debug
        private void DebugInit()
        {
            if (!Debug.isDebugBuild) Destroy(GetComponent<DevTools>());
            else
            {
                DevTools tool = GetComponent<DevTools>();
                tool.Initialize(this, cm, fg);
                if (oc != null) oc.DebugInit(tool);
            }
        }
        #endregion*/
    }
}