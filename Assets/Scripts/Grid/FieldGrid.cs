using Berty.BoardCards;
using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay;
using Berty.Grid.Field;
using Berty.Grid.Init;
using Berty.Structs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid
{
    public class FieldGrid : MonoBehaviour
    {
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material player;
        [SerializeField] private Material opponent;

        [SerializeField] private Material defaultAttacked;
        //[SerializeField] private Material playerAttacked;
        //[SerializeField] private Material opponentAttacked;

        private Turn turn;
        private OutdatedFieldBehaviour[] fields;
        private DefaultTransform cardOnBoard;
        private Color defaultColor;
        private GlobalStatus temporaryStatuses;
        private CardSpriteBehaviour backupCard;

        public Turn Turn => turn;
        public OutdatedFieldBehaviour[] Fields => fields;
        public GlobalStatus CurrentStatus => temporaryStatuses;

        private void Awake()
        {
            turn = FindObjectOfType<Turn>();
        }

        private void Start()
        {
            GridInitialization init = GetComponent<GridInitialization>();
            temporaryStatuses = new GlobalStatus(this);
            init.InitializeFields(out fields);
            init.InitializeDefaultCardTransform(out cardOnBoard, out defaultColor);
            backupCard = init.InitializeBackupCard();
            Destroy(init);
        }

        #region SingleFieldRead
        public Material GetMaterial(AlignmentEnum alignment, bool underAttack)
        {
            switch (alignment)
            {
                case AlignmentEnum.Player: return player;
                case AlignmentEnum.Opponent: return opponent;
                default: return underAttack ? defaultAttacked : defaultMaterial;
            }
        }

        public float GetDefaultAngle()
        {
            return cardOnBoard.defaultRotation.eulerAngles.z;
        }
        #endregion

        #region SingleFieldAction
        public void CancelCard()
        {
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsOccupied()) continue;
                if (field.OccupantCard.CancelDecision()) return;
            }
        }

        public void ResetCardTransform(Transform cardSprite)
        {
            cardSprite.localPosition = cardOnBoard.defaultPosition;
            cardSprite.localRotation = cardOnBoard.defaultRotation;
        }

        public void UnhighlightCard(SpriteRenderer cardRenderer)
        {
            cardRenderer.color = defaultColor;
        }

        public void AttackNewStand(OutdatedFieldBehaviour targetField)
        {
            int targetPower = targetField.OccupantCard.CardStatus.Power;
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (field.IsAligned(AlignmentEnum.None)) continue;
                if (field.IsAligned(turn.CurrentAlignment)) continue;
                if (field.OccupantCard.CardStatus.Power > targetPower) field.OccupantCard.TryToAttackTarget(targetField);
            }
        }
        #endregion

        public void AdjustNewTurn() // Can be unstable due to unclear priorities.
        {
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (field.IsAligned(AlignmentEnum.None)) continue;
                CardSpriteBehaviour card = field.OccupantCard;
                if (field.IsAligned(turn.CurrentAlignment))
                {
                    card.ResetAttack();
                    card.RegenerateDexterity();
                }
                card.ProgressTemporaryStats();
                if (!card.CanUseSkill()) continue;
                card.Character.SkillOnNewTurn(field.OccupantCard);
            }
            temporaryStatuses.AdjustNewTurn(turn.CurrentAlignment);
            UpdateCardStates();
        }

        #region StateControl
        public void UpdateCardStates()
        {
            if (!Turn.IsItMoveTime()) return;
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (field.IsOpposed(turn.CurrentAlignment) && field.OccupantCard.CanBeTelecinetic()) field.OccupantCard.SetTelecinetic();
                if (!field.IsAligned(turn.CurrentAlignment)) continue;
                field.OccupantCard.SetActive();
            }
        }

        public void MakeAllStatesIdle(OutdatedFieldBehaviour exceptionField = null)
        {
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsOccupied()) continue;
                if (field == exceptionField) continue;
                //if (field.IsAligned(turn.CurrentAlignment))
                field.OccupantCard.SetIdle();
            }
        }
        #endregion

        #region FieldRead
        public OutdatedFieldBehaviour GetField(int x, int y)
        {
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (field.GetX() != x) continue;
                if (field.GetY() != y) continue;
                //Debug.Log("Got x=" + x + "; y=" + y);
                return field;
            }
            //Debug.Log("Got x=" + x + "; y=" + y + "as null!");
            return null;
        }

        public int[] GetRelativeCoordinates(int x, int y, float angle = 0)
        {
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            int[] relCoord = new int[2];
            relCoord[0] = cosinus * x + sinus * y;
            relCoord[1] = cosinus * y - sinus * x;
            return relCoord;
        }

        public OutdatedFieldBehaviour GetRelativeField(int x, int y, float angle = 0)
        {
            int[] relCoord = GetRelativeCoordinates(x, y, angle);
            return GetField(relCoord[0], relCoord[1]);
        }

        public List<OutdatedFieldBehaviour> AlignedFields(AlignmentEnum alignment, bool countBackup = false)
        {
            List<OutdatedFieldBehaviour> alignedFields = new List<OutdatedFieldBehaviour>();
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsAligned(alignment)) continue;
                alignedFields.Add(field);
                if (countBackup && field.AreThereTwoCards()) alignedFields.Add(field);
            }
            return alignedFields;
        }
        #endregion

        #region CardMove
        public void SwapCards(OutdatedFieldBehaviour first, OutdatedFieldBehaviour second)
        {
            CardSpriteBehaviour tempCard = first.OccupantCard;
            AlignmentEnum tempAlign = first.Align;
            SwapBackupCards(first, second);
            first.PlaceCard(second.OccupantCard, second.Align);
            //first.ConvertField(second.Align);
            second.PlaceCard(tempCard, tempAlign);
            //second.ConvertField(tempAlign);
        }

        private void SwapBackupCards(OutdatedFieldBehaviour first, OutdatedFieldBehaviour second)
        {
            if (first.AreThereTwoCards()) first.TransferBackupCard(second);
            else if (second.AreThereTwoCards()) second.TransferBackupCard(first);
        }
        #endregion

        #region CharacterSkillBased
        public void AddCardIntoQueue(AlignmentEnum align)
        {
            temporaryStatuses.RequestCard(align);
        }

        public void SetJudgement(AlignmentEnum align)
        {
            temporaryStatuses.SetJudgement(align);
            if (temporaryStatuses.Revolution == AlignmentEnum.None) return;
            foreach (OutdatedFieldBehaviour field in fields) // not tested
            {
                if (!field.IsOccupied()) continue;
                if (!field.IsAligned(temporaryStatuses.Revolution)) continue;
                if (field.OccupantCard.GetRole() != field.OccupantCard.Character.Role) field.OccupantCard.AdvanceStrength(1);
            }
        }

        public void RemoveJudgement()
        {
            if (temporaryStatuses.Revolution != AlignmentEnum.None)
                foreach (OutdatedFieldBehaviour field in fields) // not tested
                {
                    if (!field.IsOccupied()) continue;
                    if (!field.IsAligned(temporaryStatuses.Revolution)) continue;
                    if (field.OccupantCard.GetRole() != field.OccupantCard.Character.Role) field.OccupantCard.AdvanceStrength(-1);
                }
            temporaryStatuses.RemoveJudgement();
        }

        public void SetRevolution(AlignmentEnum align)
        {
            temporaryStatuses.SetRevolution(align);
            //CalmJudgement();
        }

        /*private void CalmJudgement()
        {
            if (temporaryStatuses.Revolution == Alignment.None) throw new Exception("There's no revolution to adjust judgement!");
            if (!temporaryStatuses.IsJudgement) return;
            if (IsJudgementWithRevolution()) return;
            temporaryStatuses.CalmJudgement();
        }*/

        private bool IsJudgementWithRevolution()
        {
            foreach (OutdatedFieldBehaviour field in fields) // not tested
            {
                if (!field.IsOccupied()) continue;
                if (field.OccupantCard.Character.Name != "sedzia bertt") continue;
                if (field.IsAligned(temporaryStatuses.Revolution)) return true;
                if (field.IsOpposed(temporaryStatuses.Revolution)) return false;
            }
            throw new Exception("Card sedzia bertt not found!");
        }

        public void RemoveRevolution()
        {
            temporaryStatuses.RemoveRevolution();
        }

        public void SetTelekinesis(AlignmentEnum align, int dexterity)
        {
            temporaryStatuses.SetTelekinesis(align);
            temporaryStatuses.SetTelekinesisDexterity(dexterity);
        }

        public void RemoveTelekinesis()
        {
            temporaryStatuses.RemoveTelekinesis();
        }

        public bool IsTelekineticMovement()
        {
            return turn.CurrentAlignment == temporaryStatuses.Telekinesis;
        }

        public void InitiateResurrection()
        {
            turn.ExecuteResurrection();
        }

        public void SetTargetableCards(AlignmentEnum align)
        {
            foreach (OutdatedFieldBehaviour field in AlignedFields(align)) field.OccupantCard.SetTargetable();
        }

        public void ApplyPrincessBuff(CardSpriteBehaviour card)
        {
            card.AdvanceStrength(2);
            card.AdvanceHealth(1);
        }

        public void ShowJudgement(AlignmentEnum align)
        {
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsAligned(align)) continue;
                field.OccupantCard.AdvanceTempStrength(1);
            }
        }

        public void SetBackupCard(OutdatedFieldBehaviour field)
        {
            //backupCard.transform.SetParent(field.transform, false);
            field.PlaceBackupCard(backupCard);
        }

        public List<CharacterConfig> AllInsideCharacters()
        {
            List<CharacterConfig> list = new List<CharacterConfig>();
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsOccupied()) continue;
                list.Add(field.OccupantCard.Character);
            }
            //Debug.Log("AllInsideCharacters count: " + list.Count);
            return list;
        }
        #endregion

        #region WinConditionCheck
        private int HighestAmountOfType(AlignmentEnum alignment)
        {
            int result = AmountOfType(alignment, RoleEnum.Offensive);
            if (result < AmountOfType(alignment, RoleEnum.Support)) result = AmountOfType(alignment, RoleEnum.Support);
            if (result < AmountOfType(alignment, RoleEnum.Agile)) result = AmountOfType(alignment, RoleEnum.Agile);
            if (result < AmountOfType(alignment, RoleEnum.Special)) result = AmountOfType(alignment, RoleEnum.Special);
            return result;
        }

        public AlignmentEnum HigherByAmountOfType()
        {
            if (HighestAmountOfType(AlignmentEnum.Player) > HighestAmountOfType(AlignmentEnum.Opponent)) return AlignmentEnum.Player;
            if (HighestAmountOfType(AlignmentEnum.Player) < HighestAmountOfType(AlignmentEnum.Opponent)) return AlignmentEnum.Opponent;
            return AlignmentEnum.None;
        }

        private int AmountOfType(AlignmentEnum alignment, RoleEnum role)
        {
            int result = 0;
            foreach (OutdatedFieldBehaviour field in AlignedFields(alignment))
            {
                if (field.OccupantCard.Character.Role == role) result++;
                if (role == RoleEnum.Offensive && field.AreThereTwoCards()) result++;
            }
            return result;
        }
        #endregion

        #region AutomaticOpponentHelper
        public int HeatLevel(OutdatedFieldBehaviour field, AlignmentEnum enemy)
        {
            int heat = 0;
            foreach (OutdatedFieldBehaviour enemyField in AlignedFields(enemy))
            {
                CardSpriteBehaviour enemyCard = enemyField.OccupantCard;
                if (enemyCard.CanAttackField(field)) heat += enemyCard.GetStrength();
            }
            return heat;
        }
        #endregion

        #region Debug
        public void DebugForceRemoveCardFromField(CardSpriteBehaviour card)
        {
            if (!Debug.isDebugBuild) return;
            if (card == null) return;
            foreach (OutdatedFieldBehaviour field in fields)
            {
                if (!field.IsOccupied()) continue;
                if (field.OccupantCard == card)
                {
                    field.OccupantCard.DebugForceDeactivateCard();
                    return;
                }
            }
        }
        #endregion
    }
}