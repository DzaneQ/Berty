using Berty.BoardCards;
using Berty.Debugging;
using Berty.Enums;
using Berty.Grid.Field;
using Berty.Grid;
using Berty.Gameplay;
using Berty.UI.Card;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.AutomaticPlayer
{
    public class OpponentControl : MonoBehaviour
    {
        const int timeDelay = 2;

        private FieldGrid fg;
        private OutdatedCardManager cm;
        private Turn turn;
        private System.Random rng = new System.Random();
        private DevTools dt;

        [SerializeField] private GameObject opponentTable;

        private void Awake()
        {
            turn = GetComponent<Turn>();
            cm = GetComponent<OutdatedCardManager>();
        }

        private void Start()
        {
            fg = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
        }

        public void PlayTurn()
        {
            StartCoroutine(MakeMove(timeDelay));
        }

        public void ExecutePrincessTurn()
        {
            //CardSpriteBehaviour targetCard = BestTargetCard();
            //if (targetCard == null) throw new Exception("No automatic opponent cards to execute special turn.");
            fg.ApplyPrincessBuff(BestTargetCard());
        }

        public void ExecuteResurrection()
        {
            cm.ReviveCard(cm.FirstDeadCardForOpponent());
        }

        private IEnumerator MakeMove(int time)
        {
            bool nextMove = true;
            while (nextMove)
            {
                yield return new WaitForSeconds(time);
                turn.UnselectField();
                if (cm.EnabledCards.Count == 0) break;
                TryToPlayCard(out nextMove);
                if (!nextMove) TryToAttack(out nextMove);
                if (!turn.IsItMoveTime()) break;
            }
            yield return new WaitForSeconds(0.5f);
            turn.EndTurn();
        }

        private void TryToAttack(out bool isSuccessful)
        {
            //Debug.Log("Attack attempt!");
            CardSpriteBehaviour attackingCard = BestAttackingCard();
            if (attackingCard != null)
            {
                Debug.Log("Attack!");
                attackingCard.PrepareToAttack();
                Pay(attackingCard, 6 - attackingCard.CardStatus.Dexterity);
                isSuccessful = true;
                turn.SelectField(attackingCard.OccupiedField);
            }
            else isSuccessful = false;
        }

        private CardSpriteBehaviour BestAttackingCard()
        {
            CardSpriteBehaviour bestCard = null;
            int highestEfficiency = 0;
            foreach (OutdatedFieldBehaviour field in fg.AlignedFields(AlignmentEnum.Opponent))
            {
                CardSpriteBehaviour card = field.OccupantCard;
                if (!card.CanCharacterAttack()) continue;
                if (cm.EnabledCards.Count < 6 - card.CardStatus.Dexterity) continue;
                int efficiency = Efficiency(card, card.GetStrength());
                if (efficiency <= highestEfficiency) continue;
                highestEfficiency = efficiency;
                bestCard = card;
            }
            return bestCard;
        }

        private CardSpriteBehaviour BestTargetCard()
        {
            CardSpriteBehaviour bestCard = null;
            foreach (OutdatedFieldBehaviour field in fg.AlignedFields(AlignmentEnum.Opponent))
            {
                CardSpriteBehaviour card = field.OccupantCard;
                if (bestCard != null)
                {
                    if (card.CardStatus.Strength > 4 && bestCard.CardStatus.Strength < 6) continue;
                    if (card.CardStatus.Strength < bestCard.CardStatus.Strength) continue;
                    if (card.CardStatus.Health == 6 && bestCard.CardStatus.Health < 6) continue;
                }
                bestCard = card;
            }
            if (bestCard == null) throw new Exception("No automatic opponent cards to execute special turn.");
            return bestCard;
        }

        private void TryToPlayCard(out bool isSuccessful)
        {
            HandCardBehaviour selectedCard = cm.EnabledCards[0];
            int price = selectedCard.Character.Power;
            if (price < cm.EnabledCards.Count && GetSafestFields().Count > 0)
            {
                PlayCard(selectedCard, price);
                isSuccessful = true;
            }
            else isSuccessful = false;
        }

        private void PlayCard(HandCardBehaviour card, int price)
        {
            CardSpriteBehaviour cardSprite = PlaceCard(card);
            //CardSpriteBehaviour cardSprite = safeFields[index].OccupantCard;
            RotateCard(cardSprite);
            Pay(cardSprite, price);
            turn.SelectField(cardSprite.OccupiedField);
        }

        private CardSpriteBehaviour PlaceCard(HandCardBehaviour card)
        {
            card.ChangeSelection(true);
            if (dt != null)
            {
                OutdatedFieldBehaviour field = dt.OpponentPriorityField();
                if (field != null)
                {
                    Debug.LogWarning("Executing priority card!");
                    field.PlayCard();
                    return field.OccupantCard;
                }
            }
            List<OutdatedFieldBehaviour> safeFields = GetSafestFields();
            int index = rng.Next(safeFields.Count);
            //Debug.Log("Rolled field index: " + index);
            safeFields[index].PlayCard();
            return safeFields[index].OccupantCard;
        }

        private void RotateCard(CardSpriteBehaviour card)
        {
            List<int> bestRotation = GetBestRotation(card);
            int index = rng.Next(bestRotation.Count);
            card.RotateCard(bestRotation[index], true);
        }

        private void Pay(CardSpriteBehaviour card, int price)
        {
            for (int i = 0; i < price; i++)
            {
                //Debug.Log("Paying card no. " + i);
                cm.EnabledCards[i].ChangeSelection(true);
            }
            card.ConfirmPayment(false);
        }

        private List<OutdatedFieldBehaviour> GetSafestFields()
        {
            List<OutdatedFieldBehaviour> freeFields = fg.AlignedFields(AlignmentEnum.None);
            List<OutdatedFieldBehaviour> safeFields = new List<OutdatedFieldBehaviour>();
            for (int i = 0; i < 6; i++)
            {
                foreach (OutdatedFieldBehaviour field in freeFields) if (fg.HeatLevel(field, AlignmentEnum.Player) <= i) safeFields.Add(field);
                if (safeFields.Count > 0) break;
            }
            return safeFields;
        }

        private List<int> GetBestRotation(CardSpriteBehaviour card)
        {
            int[] efficiency = new int[4];
            int maxEfficiency = -16;
            List<int> bestRotation = new List<int>();
            for (int i = 0; i < 4; i++)
            {
                //Debug.Log("Checking rotation: " + (i * 90));
                efficiency[i] = Efficiency(card, 2, 1);
                if (maxEfficiency < efficiency[i]) maxEfficiency = efficiency[i];
                card.RotateCard(90, true);
            }
            //Debug.Log("Max efficiency: " + maxEfficiency);
            for (int i = 0; i < 4; i++)
            {
                //Debug.Log("Rotation: " + (i * 90) + " has efficiency: " + efficiency[i]);
                if (efficiency[i] == maxEfficiency) bestRotation.Add(i * 90);
            }
            return bestRotation;
        }

        private int Efficiency(CardSpriteBehaviour card, int alignedWeight = 1, int neutralWeight = 0)
        {
            int efficiency = 0;
            foreach (OutdatedFieldBehaviour field in fg.AlignedFields(AlignmentEnum.None))
            {
                if (card.CanAttackField(field)) efficiency += neutralWeight;
                //if (card.CanAttack(field)) Debug.Log("Adding " + neutralWeight + " cause neutral: " + field.GetX() + "," + field.GetY());
            }
            foreach (OutdatedFieldBehaviour field in fg.AlignedFields(AlignmentEnum.Player))
            {
                if (card.CanAttackField(field)) efficiency += alignedWeight;
                //if (card.CanAttack(field)) Debug.Log("Adding " + alignedWeight + " cause not ally: " + field.GetX() + "," + field.GetY());
            }
            foreach (OutdatedFieldBehaviour field in fg.AlignedFields(AlignmentEnum.Opponent))
            {
                if (card.CanAttackField(field)) efficiency -= alignedWeight;
                //if (card.CanAttack(field)) Debug.Log("Substracting " + alignedWeight + " cause ally: " + field.GetX() + "," + field.GetY());
            }
            return efficiency;
        }

        public void DebugInit(DevTools dt)
        {
            if (!Debug.isDebugBuild) return;
            this.dt = dt;
        }
    }
}