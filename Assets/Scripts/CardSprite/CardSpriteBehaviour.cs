using Berty.CardSprite.Animation;
using Berty.CardSprite.Bar;
using Berty.CardSprite.Button;
using Berty.CardSprite.State;
using Berty.Characters.Data;
using Berty.Enums;
using Berty.Field;
using Berty.Field.Grid;
using Berty.Gameplay;
using Berty.Structs;
using Berty.UI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Berty.CardSprite
{
    public class CardSpriteBehaviour : MonoBehaviour
    {
        private FieldBehaviour occupiedField;
        private CardManager cardManager;
        private CardButton[] cardButton;
        private CardBar[] cardBar;
        //private Transform[] bars;
        private CardImage imageReference;
        private int[] relCoord = new int[2];
        private Rigidbody cardRB;
        private SpriteRenderer spriteRenderer;
        private Character character;
        private CardState state;
        private CharacterStat cardStatus;
        private List<Character> resistChar;
        private AnimatingCardSprite animating;
        //private bool animatingProcess = false;

        public FieldBehaviour OccupiedField => occupiedField;
        public CardManager CardManager => cardManager;
        private Turn Turn => Grid.Turn;
        public FieldGrid Grid => occupiedField.Grid;
        public CardState ResultState => state;
        public CharacterStat CardStatus => cardStatus;
        public AnimatingCardSprite Animate => animating;
        public Character Character
        {
            get => character;
            private set
            {
                character = value;
                spriteRenderer.sprite = imageReference.Sprite;
                name = character.Name;
                cardStatus = new CharacterStat(Character);
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
         */

        #region SetupAndEvents
        private void Awake()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            cardManager = GameObject.Find("EventSystem").GetComponent<CardManager>();
            cardButton = transform.GetChild(0).GetComponentsInChildren<CardButton>();
            cardBar = transform.GetChild(1).GetComponentsInChildren<CardBar>();
            occupiedField = transform.GetComponentInParent<FieldBehaviour>();
            state = new InactiveState(this);
            animating = GetComponent<AnimatingCardSprite>();
            if (animating != null) animating.AttachSound();
            InitializeRigidbody();
        }

        private void InitializeRigidbody()
        {
            cardRB = GetComponent<Rigidbody>();
            cardRB.detectCollisions = true;
            cardRB.isKinematic = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == occupiedField.gameObject) ApplyPhysics(false);  //state.HandleFieldCollision();
            if (occupiedField.AreThereTwoCards())
            {
                occupiedField.AttachCards();
            }
        }

        public void OnMouseOver()
        {
            //Debug.Log("OnMouseOver event trigger on: " + name);
            if (IsLeftClicked()) state.HandleClick();
            else if (IsRightClicked()) state.HandleSideClick();
        }

        private bool IsLeftClicked()
        {
            //Debug.Log($"Card {name} was left clicked. Is it locked? {IsLocked()}");
            if (!IsLocked() && !IsAnimating() && Input.GetMouseButtonDown(0)) return true;
            else return false;
        }

        private bool IsRightClicked()
        {
            if (!IsLocked() && !IsAnimating() && Input.GetMouseButtonDown(1)) return true;
            else return false;
        }

        public void ShowLookupCard(bool ignoreLock) => cardManager.ShowLookupCard(spriteRenderer.sprite, ignoreLock);

        public void HighlightCard(Color color)
        {
            spriteRenderer.color = color;
        }

        public void UnhighlightCard()
        {
            if (occupiedField != null) Grid.UnhighlightCard(spriteRenderer);
        }
        #endregion

        #region ActivatationProcess
        public void LoadSelectedCard()
        {
            if (IsCardSelected()) state = state.ActivateCard();
        }

        public bool IsCardSelected()
        {
            if (!Turn.IsItMoveTime()) return false;
            return cardManager.SelectedCard() != null;
        }

        public void ActivateNewCard()
        {
            AdjustRotationToCamera();
            gameObject.SetActive(true);
            ImportFromSelectedImage();
            UpdateBars(false);
            //UpdateRelativeCoordinates();
            CallPayment(cardStatus.Power);
            occupiedField.ConvertField(Turn.CurrentAlignment);
            if (animating != null) animating.PutCardSound();
            ApplyPhysics();
        }

        private void AdjustRotationToCamera()
        {
            if (!occupiedField.AreThereTwoCards())
            {
                float rightAngle = 180f - Turn.GetCameraRightAngle();
                //Debug.Log("Right angle: " + rightAngle);
                RotateCard(Mathf.RoundToInt(rightAngle), false);
            }
            else occupiedField.SynchronizeRotation();
        }

        public void ImportFromSelectedImage()
        {
            imageReference = cardManager.SelectedCard();
            Character = imageReference.Character;
            //cardStatus = new CharacterStat(Character);
            cardManager.RemoveFromTable(imageReference);
            //cardManager.AddToField(imageReference);
        }

        public void ApplyPhysics(bool isApplied = true)
        {
            cardRB.isKinematic = !isApplied;
        }

        private void ClearCardResistance()
        {
            resistChar = new List<Character>();
        }

        public void ConfirmNewCard()
        {
            if (animating != null) animating.ConfirmSound();
            ClearCardResistance();
            if (occupiedField.IsAligned(Grid.CurrentStatus.Revolution) && GetRole() == Role.Special) AdvanceStrength(1);
            if (occupiedField.IsAligned(Grid.CurrentStatus.JudgementRevenge)) AdvanceTempStrength(1);
            Grid.AttackNewStand(occupiedField);
            if (IsDead()) return;
            if (CanUseSkill()) Character.SkillOnNewCard(this);
            TakeNeighborsEffect();
        }

        public void CancelCard()
        {
            imageReference.ReturnCard();
            DeactivateCard();
        }
        #endregion

        #region DeactivationProcess
        public void DeactivateCard()
        {
            //Debug.Log($"Deactivating card: {name}");
            occupiedField.AdjustCardRemoval();
            if (animating != null) animating.TakeCardSound();
            state = state.DeactivateCard();
            //cardManager.RemoveFromField(imageReference);
        }

        public void SetCardToDefaultTransform()
        {
            if (occupiedField != null) Grid.ResetCardTransform(transform);
        }
        #endregion

        #region Payment
        public void CallPayment(int price)
        {
            Grid.MakeAllStatesIdle(occupiedField);
            Turn.SetPayment(price);
        }

        public void ConfirmPayment(bool ignoreSound = false)
        {
            if (!Turn.IsItPaymentTime()) throw new Exception("Confirming payment when not in payment mode.");
            if (!state.IsForPaymentConfirmation()) throw new Exception("Wrong card state for payment..");
            if (!Turn.CheckOffer()) return;
            cardManager.DiscardCards();
            state = state.TakePaidAction(ignoreSound ? null : animating);
            if (Turn.IsItPaymentTime()) Turn.SetMoveTime();
        }

        public void CancelPayment()
        {
            Turn.SetMoveTime();
        }

        public bool CancelDecision()
        {
            return state.Cancel();
        }
        #endregion

        #region AttackPhase
        public void ResetAttack()
        {
            cardStatus.HasAttacked = false;
        }

        public void ExhaustAttack()
        {
            if (cardStatus.HasAttacked) throw new Exception("Exhausting when already attacked!");
            cardStatus.HasAttacked = true;
        }

        public bool CanCharacterAttack()
        {
            if (VenturaCheck()) return false;
            return !cardStatus.HasAttacked;
        }

        public void PrepareToAttack()
        {
            if (!CanCharacterAttack()) return;
            CallPayment(6 - cardStatus.Dexterity);
            state = new AttackingState(this);
        }

        public bool CanAttackField(FieldBehaviour targetField)
        {
            if (targetField == null) return false;
            foreach (int[] distance in Character.AttackRange)
            {
                int[] target = { relCoord[0] + distance[0], relCoord[1] + distance[1] };
                if (targetField == GetRelativeField(target[0], target[1])) return true;
            }
            return false;
        }

        public void TryToAttackTarget(FieldBehaviour targetField)
        {
            if (CanAttackField(targetField)) targetField.OccupantCard.TakeDamage(GetStrength(), targetField);
        }

        public void OrderAttack()
        {
            //Debug.Log("Start ordering attack");
            ExhaustAttack();
            if (CanUseSkill() && Character.SkillSpecialAttack(this)) return;
            bool successfulAttack = false;
            //Debug.Log("Check ranges");
            foreach (int[] distance in Character.AttackRange)
            {
                FieldBehaviour targetField = GetTargetField(distance);
                if (targetField == null || !targetField.IsOccupied()) continue;
                if (targetField.OccupantCard.TakeDamage(GetStrength(), occupiedField)) successfulAttack = true;
                Debug.Log("Attack - X: " + targetField.GetX() + "; Y: " + targetField.GetY());
            }
            //Debug.Log("Ranges checked");
            if (successfulAttack && CanUseSkill()) Character.SkillOnSuccessfulAttack(this);
            if (CanUseSkill()) Character.SkillOnAttack(this);
            //Debug.Log("End ordering attack");
        }


        public bool TakeDamage(int damage, FieldBehaviour source, bool riposte = false)
        {
            if (CanUseSkill()) damage = Character.SkillDefenceModifier(damage, source.OccupantCard);
            if (source.OccupantCard.CanUseSkill()) damage = source.OccupantCard.Character.SkillAttackModifier(damage, this);
            //Debug.Log("Damage on field - X: " + occupiedField.GetX() + "; Y: " + occupiedField.GetY());
            if (!gameObject.activeSelf) return false;
            if (riposte)
            {
                AdvanceHealth(-damage);
                return false;
            }
            //int[] srcRel = source.GetRelativeCoordinates(GetRelativeAngle());
            //int[] srcDistance = { srcRel[0] - relCoord[0], srcRel[1] - relCoord[1] };
            int[] srcDistance = GetFieldDistance(source);
            //Debug.Log("Character health: " + CardStatus.Health);
            if (Character.CanRiposte(srcDistance)) source.OccupantCard.TakeDamage(GetStrength(), OccupiedField, true);
            if (Character.CanBlock(srcDistance)) return false;
            AdvanceHealth(-damage);
            //Debug.Log($"{damage} damage taken for card {name}. Remaining HP: {cardStatus.Health}");
            return true;
        }
        #endregion

        #region StatAdvancement
        public void AdvanceTempStrength(int value, CardSpriteBehaviour spellSource = null)
        {
            if (!Character.CanAffectStrength(this, spellSource)) return;
            if (Character.GlobalSkillResistance() && spellSource == null) return;
            cardStatus.TempStrength += value;
            StartCoroutine(AdvanceStrengthCoroutine(value, spellSource));
        }

        public void AdvanceStrength(int value, CardSpriteBehaviour spellSource = null)
        {
            if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
            if (Character.GlobalSkillResistance() && spellSource == null) return;
            if (!Character.CanAffectStrength(this, spellSource)) return;
            cardStatus.Strength += value;
            StartCoroutine(AdvanceStrengthCoroutine(value, spellSource));
        }

        private IEnumerator AdvanceStrengthCoroutine(int value, CardSpriteBehaviour spellSource)
        {
            yield return StartCoroutine(UpdateBar(0, true, true));
            NewStrengthAdjustment(value);
        }

        public void NewStrengthAdjustment(int valueChange)
        {
            if (CanUseSkill()) Character.SkillAdjustStrengthChange(valueChange, this);
        }

        public void AdvanceTempPower(int value, CardSpriteBehaviour spellSource = null)
        {
            if (!Character.CanAffectPower(this, spellSource)) return;
            if (Character.GlobalSkillResistance() && spellSource == null) return;
            cardStatus.TempPower += value;
            StartCoroutine(AdvancePowerCoroutine(value, spellSource));
        }

        public void AdvancePower(int value, CardSpriteBehaviour spellSource = null)
        {
            if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
            if (!Character.CanAffectPower(this, spellSource)) return;
            if (Character.GlobalSkillResistance() && spellSource == null) return;
            cardStatus.Power += value;
            StartCoroutine(AdvancePowerCoroutine(value, spellSource));
        }

        private IEnumerator AdvancePowerCoroutine(int value, CardSpriteBehaviour spellSource)
        {
            yield return StartCoroutine(UpdateBar(1, true, true));
            NewPowerAdjustment(value, spellSource);
        }

        public void NewPowerAdjustment(int valueChange, CardSpriteBehaviour spellSource)
        {
            if (CanUseSkill()) Character.SkillAdjustPowerChange(valueChange, this, spellSource);
            if (HasLostWill()) SwitchSides();
        }

        public bool HasLostWill()
        {
            return cardStatus.Power <= 0;
        }

        private void SwitchSides()
        {
            occupiedField.GoToOppositeSide();
            ResetPower();
            UpdateAlignedCardState();
            AdjustGlobalStatusesToNewSide();
        }

        private void AdjustGlobalStatusesToNewSide()
        {
            if (Character.GlobalSkillResistance()) return;
            if (occupiedField.IsAligned(Grid.CurrentStatus.JudgementRevenge)) AdvanceTempStrength(1);
            if (Grid.CurrentStatus.Revolution == Alignment.None) return;
            if (GetRole() != Role.Special) return;
            if (Character.Name == "che bert") return;
            if (OccupiedField.IsAligned(Grid.CurrentStatus.Revolution)) AdvanceStrength(1);
            else AdvanceStrength(-1);
        }

        private void UpdateAlignedCardState()
        {
            if (!occupiedField.IsOccupied()) throw new Exception("Updating state of non-occupied card!");
            if (occupiedField.IsAligned(Turn.CurrentAlignment)) SetActive();
            else
            {
                if (CanBeTelecinetic()) SetTelecinetic();
                else SetIdle();
            }
        }

        public void ResetPower()
        {
            cardStatus.Power = Character.Power;
            UpdateBar(1, true);
        }

        public void AdvanceDexterity(int value, CardSpriteBehaviour spellSource = null)
        {
            if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
            if (spellSource == null) Debug.LogWarning($"No spell source affecting {Character.Name}");
            cardStatus.Dexterity += value;
            StartCoroutine(AdvanceDexterityCoroutine(value));
        }

        private IEnumerator AdvanceDexterityCoroutine(int value)
        {
            yield return StartCoroutine(UpdateBar(2, true, true));
            NewDexterityAdjustment(value);
        }

        public void NewDexterityAdjustment(int valueChange)
        {
            if (CanUseSkill()) Character.SkillAdjustDexterityChange(valueChange, this);
            if (cardStatus.Dexterity <= 0) cardStatus.IsTired = true;
            if (cardStatus.Dexterity >= Character.Dexterity) cardStatus.IsTired = false;
        }

        public void RegenerateDexterity()
        {
            if (!CardStatus.IsTired) return;
            AdvanceDexterity(1, this);
        }

        public void AdvanceHealth(int value, CardSpriteBehaviour spellSource = null)
        {
            if (spellSource != null && resistChar.Contains(spellSource.Character)) return;
            cardStatus.Health += value;
            StartCoroutine(AdvanceHealthCoroutine(value));
        }

        private IEnumerator AdvanceHealthCoroutine(int value)
        {
            yield return StartCoroutine(UpdateBar(3, true, true));
            NewHealthAdjustment(value);
            yield return null;
        }

        public void NewHealthAdjustment(int valueChange)
        {
            if (CanUseSkill()) Character.SkillAdjustHealthChange(valueChange, this);
            if (IsDead()) KillCard();
        }

        public bool IsDead()
        {
            return cardStatus.Health <= 0;
        }

        private void KillCard()
        {
            if (!gameObject.activeSelf) return;
            Character.SkillOnDeath(this);
            foreach (FieldBehaviour field in Grid.Fields)
                if (field.IsOccupied() && field.OccupantCard.CanUseSkill())
                    field.OccupantCard.Character.SkillOnOtherCardDeath(field.OccupantCard, this);
            DeactivateCard();
            cardManager.KillCard(imageReference);
        }
        #endregion

        #region BarsDisplay
        public void UpdateBars(bool animate)
        {
            for (int i = 0; i < cardBar.Length; i++) StartCoroutine(UpdateBar(i, animate, true));
        }

        private IEnumerator UpdateBar(int index, bool animate, bool waitUntilFinished = false)
        {
            if (!gameObject.activeSelf) animate = false;
            if (!animate) StartCoroutine(cardBar[index].UpdateBar(null));
            else if (!waitUntilFinished) StartCoroutine(cardBar[index].UpdateBar(animating));
            else yield return StartCoroutine(cardBar[index].UpdateBar(animating));
            yield return null;
        }

        public void HideBars()
        {
            foreach (CardBar bar in cardBar) bar.HideBar();
        }

        public void ShowBars()
        {
            foreach (CardBar bar in cardBar) bar.ShowBar();
        }
        #endregion

        #region ButtonDisplay
        public void EnableButtons()
        {
            if (!IsAnimating() && this == Turn.GetFocusedCard()) state.EnableButtons();
        }

        public void DisableButtons()
        {
            if (!gameObject.activeSelf || this == Turn.GetFocusedCard()) foreach (CardButton button in cardButton) button.DisableButton();
        }

        public void EnableCancelNeutralButton(int index)
        {
            DisableButtons();
            cardButton[index].ChangeButtonToNeutral();
            cardButton[index].EnableButton();
        }

        public void PrepareNeutralRotationButtons()
        {
            cardButton[2].ChangeButtonToNeutral();
            cardButton[3].ChangeButtonToNeutral();
        }

        public void PrepareDexterityButtons()
        {
            DisableButtons();
            for (int i = 2; i <= 7; i++) cardButton[i].ChangeButtonToDexterity();
        }

        public void ShowButtons(bool canMove, bool canRotate, bool canCancel = false)
        {
            DisableButtons();
            if (canCancel) cardButton[1].EnableButton();
            if (canRotate)
            {
                cardButton[2].EnableButton();
                cardButton[3].EnableButton();
            }
            if (canMove) UpdateMoveButtons();
        }

        public void UpdateMoveButtons()
        {
            for (int i = 4; i <= 7; i++)
            {
                FieldBehaviour targetField = GetAdjacentField(i * 90);
                if (targetField == null || targetField.IsOccupied()) cardButton[i].DisableButton();
                else cardButton[i].EnableButton();
            }
        }
        #endregion

        #region Navigation
        public void MoveCard(int angle)
        {
            int returnButtonIndex = (angle / 90 + 2) % 4 + 4;
            FieldBehaviour toField = GetAdjacentField(angle);
            state = state.AdjustTransformChange(returnButtonIndex);
            Grid.SwapCards(occupiedField, toField);
            //UpdateRelativeCoordinates();
        }

        public void ConfirmMove()
        {
            //Debug.Log("Confirming move for " + Character.Name);
            if (CanUseSkill()) Character.SkillOnMove(this);
            TakeNeighborsEffect();
        }

        public void RotateCard(int angle, bool skipAnimation = false)
        {
            //Debug.Log("Rotating card for " + name);
            int returnButtonIndex = (450 - angle) / 180;
            if (!gameObject.activeSelf || animating == null) skipAnimation = true;
            if (skipAnimation)
            {
                transform.Rotate(0, 0, -angle);
                AfterRotationAdjustment(returnButtonIndex, false);
            }
            else
            {
                StartCoroutine(RotateCardCoroutine(angle, returnButtonIndex));
            }
        }

        private IEnumerator RotateCardCoroutine(int angle, int returnButtonIndex)
        {
            DisableButtons();
            HideBars();
            Turn.DisableInteractions(false);
            yield return StartCoroutine(animating.RotateObject(-angle, 1f));
            AfterRotationAdjustment(returnButtonIndex, true);
            yield return null;
        }

        public void AfterRotationAdjustment(int returnButtonIndex, bool wasAnimated)
        {
            if (wasAnimated)
            {
                EnableButtons();
                ShowBars();
                if (!IsAnimating()) Turn.EnableInteractions();
            }
            UpdateRelativeCoordinates();
            if (gameObject.activeSelf) state = state.AdjustTransformChange(returnButtonIndex);
        }

        public void SwapWith(FieldBehaviour targetField)
        {
            FieldBehaviour sourceField = null;
            if (targetField.IsOccupied())
            {
                RotateCard(180);
                targetField.OccupantCard.RotateCard(180);
                sourceField = occupiedField;
            }
            Grid.SwapCards(occupiedField, targetField);
            ConfirmMove(); // Note: experimental!
            if (sourceField != null) sourceField.OccupantCard.ConfirmMove();
        }
        #endregion

        #region CharacterSkillBased
        public void UpdateCard(CardImage image)
        {
            imageReference = image;
            Character = image.Character;
            UpdateBars(false);
            ConfirmNewCard(); // experimental - maybe tested?
        }

        public void ReturnCharacter()
        {
            cardManager.ReturnCharacter(imageReference);
        }

        private void TakeNeighborsEffect()
        {
            for (int i = 0; i < 4; i++)
            {
                FieldBehaviour adjacentField = GetAdjacentField(i * 90);
                if (adjacentField == null || !adjacentField.IsOccupied()) continue;
                if (adjacentField.OccupantCard.CanUseSkill())
                    adjacentField.OccupantCard.Character.SkillOnNeighbor(adjacentField.OccupantCard, this);
            }
        }

        private bool VenturaCheck()
        {
            foreach (CardSpriteBehaviour card in GetAdjacentCards())
            {
                if (card.Character.GetType() != typeof(BertVentura) || IsAllied(card.OccupiedField)) continue;
                foreach (int[] range in Character.AttackRange)
                {
                    FieldBehaviour targetField = GetTargetField(range);
                    if (targetField == null || !targetField.IsOccupied()) continue;
                    if (targetField.OccupantCard.Character.GetType() == typeof(BertVentura)) return false;
                }
                return true;
            }
            return false;
        }

        public bool CanBeTelecinetic()
        {
            if (resistChar.OfType<RycerzBerti>().Any()) return false;
            return Grid.IsTelekineticMovement();
        }

        public void SetTelecinetic()
        {
            state = state.SetTelecinetic();
        }

        public void SetTargetable()
        {
            state = state.SetTargetable();
        }
        #endregion

        #region PositionRead
        public float GetRelativeAngle()
        {
            return Grid.GetDefaultAngle() - transform.localRotation.eulerAngles.z;
        }

        private FieldBehaviour GetRelativeField(int targetX, int targetY)
        {
            return Grid.GetRelativeField(targetX, targetY, GetRelativeAngle());
        }

        public FieldBehaviour GetTargetField(int[] distance)
        {
            if (distance.Length > 2) throw new IndexOutOfRangeException("Wrong distance argument.");
            return GetRelativeField(distance[0] + relCoord[0], distance[1] + relCoord[1]);
        }

        public FieldBehaviour GetAdjacentField(float angle)
        {
            int targetX = (int)Math.Round(relCoord[0] + Math.Sin(angle * Math.PI / 180));
            int targetY = (int)Math.Round(relCoord[1] + Math.Cos(angle * Math.PI / 180));
            int[] target = { targetX, targetY };
            return GetRelativeField(target[0], target[1]);
        }

        public List<CardSpriteBehaviour> GetAdjacentCards()
        {
            List<CardSpriteBehaviour> adjacentCards = new List<CardSpriteBehaviour>();
            for (int i = 0; i < 4; i++)
            {
                FieldBehaviour adj = GetAdjacentField(i * 90);
                if (adj != null && adj.IsOccupied()) adjacentCards.Add(adj.OccupantCard);
            }
            return adjacentCards;
        }

        public int[] GetFieldDistance(FieldBehaviour targetField)
        {
            int[] fieldRel = targetField.GetRelativeCoordinates(GetRelativeAngle());
            int[] fieldDistance = { fieldRel[0] - relCoord[0], fieldRel[1] - relCoord[1] };
            return fieldDistance;
        }
        #endregion

        #region StatusRead
        public bool IsLocked()
        {
            return Turn.InteractableDisabled;
        }

        public bool IsAnimating()
        {
            if (animating == null) return false;
            return animating.CoroutineCount > 0;
        }

        public Role GetRole()
        {
            if (Grid.CurrentStatus.IsJudgement) return Role.Special;
            return Character.Role;
        }

        public int GetStrength()
        {
            return cardStatus.Strength;
        }

        public bool IsAllied(FieldBehaviour targetField)
        {
            return targetField.IsAligned(occupiedField.Align);
        }

        public bool CanUseSkill()
        {
            if (OccupiedField.IsOpposed(Grid.CurrentStatus.Revolution) && GetRole() == Role.Special) return false;
            return true;
        }
        #endregion

        #region StatusUpdate
        public void ProgressTemporaryStats()
        {
            if (cardStatus.CurrentTempStatBonus.All(x => x == 0)) return; ;
            cardStatus.CurrentTempStatBonus = (int[])cardStatus.NextTempStatBonus.Clone();
            Array.Clear(cardStatus.NextTempStatBonus, 0, 4);
            UpdateBars(true);
        }

        public void SetField(FieldBehaviour field, bool wasAnimated)
        {
            occupiedField = field;
            transform.SetParent(field.transform, wasAnimated);
            UpdateRelativeCoordinates();
        }

        public void ApplyField(FieldBehaviour field)
        {
            occupiedField = field;
        }

        public void SetActive()
        {
            //Debug.Log($"Set active for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
            if (!cardStatus.IsTired) state = state.SetActive();
            else state = state.SetIdle;
        }

        public void SetIdle()
        {
            //Debug.Log($"Set idle for card on field: {occupiedField.GetX()}, {occupiedField.GetY()}");
            state = state.SetIdle;
        }
        public void AddResistance(Character character)
        {
            //Debug.Log($"Adding resistance of {character} to card: {name}");
            if (character == null) throw new Exception("Trying to resist null.");
            if (character == Character) throw new Exception("Trying to resist self.");
            if (resistChar.Contains(character)) return;
            resistChar.Add(character);
        }


        public void UpdateRelativeCoordinates() // NOTE: Moving to adjacent field displayed it 9 times - not anymore for unknown reason.
        {
            //Debug.Log("Updating relative coords for: " + name);
            //if (IsAnimating()) Debug.LogWarning("Updating animating object!");
            relCoord = occupiedField.GetRelativeCoordinates(GetRelativeAngle());
            //Turn.RefreshCardSpriteFocus();
        }
        #endregion

        #region Debug
        public void DebugForceDeactivateCard()
        {
            if (!Debug.isDebugBuild) return;
            Debug.Log($"Force deactivating card: {name}");
            occupiedField.AdjustCardRemoval();
            state = new InactiveState(this);
        }

        public CardImage DebugGetReference()
        {
            if (!Debug.isDebugBuild) return null;
            return imageReference;
        }

        public void DebugForceActivateCard(CardImage image, int angle)
        {
            gameObject.SetActive(true);
            imageReference = image;
            Character = imageReference.Character;
            UpdateBars(false);
            transform.Rotate(0, 0, -angle);
            UpdateRelativeCoordinates();
            //SynchronizeRotation();
            ClearCardResistance();
            ApplyPhysics();
            ConfirmNewCard();
            if (occupiedField.IsAligned(Turn.CurrentAlignment)) state = new ActiveState(this);
            else if (occupiedField.IsOpposed(Turn.CurrentAlignment)) state = new IdleState(this);
            else Debug.LogError("Wrongly activated card!");
        }
        #endregion
    }
}