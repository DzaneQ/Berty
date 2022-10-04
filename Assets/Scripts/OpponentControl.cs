using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpponentControl : MonoBehaviour
{
    const int timeDelay = 2;

    private FieldGrid fg;
    private CardManager cm;
    private Turn turn;
    private System.Random rng = new System.Random();

    [SerializeField] private GameObject opponentTable;

    private void Awake()
    {
        turn = GetComponent<Turn>();
        cm = GetComponent<CardManager>();
    }

    private void Start()
    {
        fg = GameObject.Find("FieldBoard").GetComponent<FieldGrid>();
    }

    public void PlayTurn()
    {
        StartCoroutine(MakeMove(timeDelay));
    }

    private IEnumerator MakeMove(int time)
    {
        bool nextMove = true;
        while (nextMove)
        {
            yield return new WaitForSeconds(time);
            if (cm.EnabledCards.Count == 0) break;
            TryToPlayCard(out nextMove);
            if (!nextMove) TryToAttack(out nextMove);
        }
        turn.EndTurn();
    }

    private void TryToAttack(out bool isSuccessful)
    {
        Debug.Log("Attack attempt!");
        CardSprite attackingCard = BestAttackingCard();
        if (attackingCard != null)
        {
            Debug.Log("Attack!");
            attackingCard.PrepareToAttack();
            Pay(attackingCard, 6 - attackingCard.CardStatus.Dexterity);
            isSuccessful = true;
        }
        else isSuccessful = false;
    }

    private CardSprite BestAttackingCard()
    {
        CardSprite bestCard = null;
        int highestEfficiency = 0;
        foreach (Field field in fg.AlignedFields(Alignment.Opponent))
        {
            CardSprite card = field.OccupantCard;
            if (!card.CanCharacterAttack()) continue;
            if (cm.EnabledCards.Count < 6 - card.CardStatus.Dexterity) continue;
            int efficiency = Efficiency(card, card.CardStatus.Strength);
            if (efficiency <= highestEfficiency) continue;
            highestEfficiency = efficiency;
            bestCard = card;
        }
        return bestCard;
    }

    private void TryToPlayCard(out bool isSuccessful)
    {
        CardImage selectedCard = cm.EnabledCards[0];
        int price = selectedCard.Character.Power;
        if (price < cm.EnabledCards.Count && GetSafestFields().Count > 0)
        {
            PlayCard(selectedCard, price);
            isSuccessful = true;
        }
        else isSuccessful = false;
    }

    private void PlayCard(CardImage card, int price)
    {       
        CardSprite cardSprite = PlaceCard(card);
        //CardSprite cardSprite = safeFields[index].OccupantCard;
        RotateCard(cardSprite);
        Pay(cardSprite, price);
    }

    private CardSprite PlaceCard(CardImage card)
    {
        List<Field> safeFields = GetSafestFields();
        card.ChangePosition();
        int index = rng.Next(safeFields.Count);
        //Debug.Log("Rolled field index: " + index);
        safeFields[index].PlayCard();
        return safeFields[index].OccupantCard;
    }

    private void RotateCard(CardSprite card)
    {
        List<int> bestRotation = GetBestRotation(card);
        int index = rng.Next(bestRotation.Count);
        card.RotateCard(bestRotation[index]);
    }

    private void Pay(CardSprite card, int price)
    {
        for (int i = 0; i < price; i++)
        {
            //Debug.Log("Paying card no. " + i);
            cm.EnabledCards[i].ChangePosition();
        }
        card.ConfirmPayment();
    }

    private List<Field> GetSafestFields()
    {
        List<Field> freeFields = fg.AlignedFields(Alignment.None);
        List<Field> safeFields = new List<Field>();
        for (int i = 0; i < 6; i++)
        {
            foreach (Field field in freeFields) if (fg.HeatLevel(field, Alignment.Player) <= i) safeFields.Add(field);
            if (safeFields.Count > 0) break;
        }
        return safeFields;
    }

    private List<int> GetBestRotation(CardSprite card)
    {
        int[] efficiency = new int[4];
        int maxEfficiency = -16;
        List<int> bestRotation = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("Checking rotation: " + (i * 90));
            efficiency[i] = Efficiency(card, 2, 1);
            if (maxEfficiency < efficiency[i]) maxEfficiency = efficiency[i];
            card.RotateCard(90);
        }
        //Debug.Log("Max efficiency: " + maxEfficiency);
        for (int i = 0; i < 4; i++)
        {
            //Debug.Log("Rotation: " + (i * 90) + " has efficiency: " + efficiency[i]);
            if (efficiency[i] == maxEfficiency) bestRotation.Add(i * 90);
        }
        return bestRotation;
    }

    private int Efficiency(CardSprite card, int alignedWeight = 1, int neutralWeight = 0)
    {
        int efficiency = 0;
        foreach (Field field in fg.AlignedFields(Alignment.None))
        {
            if (card.CanAttackField(field)) efficiency += neutralWeight;
            //if (card.CanAttack(field)) Debug.Log("Adding " + neutralWeight + " cause neutral: " + field.GetX() + "," + field.GetY());
        }
        foreach (Field field in fg.AlignedFields(Alignment.Player))
        {
            if (card.CanAttackField(field)) efficiency += alignedWeight;
            //if (card.CanAttack(field)) Debug.Log("Adding " + alignedWeight + " cause not ally: " + field.GetX() + "," + field.GetY());
        }
        foreach (Field field in fg.AlignedFields(Alignment.Opponent))
        {
            if (card.CanAttackField(field)) efficiency -= alignedWeight;
            //if (card.CanAttack(field)) Debug.Log("Substracting " + alignedWeight + " cause ally: " + field.GetX() + "," + field.GetY());
        }
        return efficiency;
    }
}
