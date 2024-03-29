﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GlobalStatus
{
    private FieldGrid grid;
    //private int takeDouble = 0;
    private List<Alignment> TakeNextTurn;
    private bool judgementState;
    private Alignment judgementAwaiting;
    private Alignment judgementRevenge;
    private Alignment revolution;
    private Alignment telekinesis;
    //private Alignment resurrection;
    private int telekinesisDex;

    public bool IsJudgement => judgementState;
    public Alignment JudgementRevenge => judgementRevenge;
    public Alignment Revolution => revolution;
    public Alignment Telekinesis => telekinesis;
    //public Alignment Resurrection => resurrection;
    public int TelekinesisDex => telekinesisDex;
    public GlobalStatus(FieldGrid newGrid)
    {
        grid = newGrid;
        TakeNextTurn = new List<Alignment>();
        judgementState = false;
        judgementAwaiting = Alignment.None;
        judgementRevenge = Alignment.None;
        revolution = Alignment.None;
        telekinesis = Alignment.None;
        telekinesisDex = 0;
        //resurrection = Alignment.None;
    }

    public void AdjustNewTurn(Alignment currentAlign)
    {
        TakeQueuedCards();
        ProgressJudgementRevenge(currentAlign);
        //Resurrect(currentAlign);
    }

    private void TakeQueuedCards()
    {
        Alignment currentTurn = grid.Turn.CurrentAlignment;
        for (int i = TakeNextTurn.Where(x => x == currentTurn).Count(); i > 0; i--)
        {
            grid.Turn.CM.PullCard(currentTurn);
            TakeNextTurn.Remove(currentTurn);
        }
    }

    internal void RequestCard(Alignment align)
    {
        TakeNextTurn.Add(align);
    }

    internal void SetJudgement()
    {
        judgementState = true;
    }

    internal void RemoveJudgement(Alignment align)
    {
        judgementState = false;
        judgementAwaiting = align;
        //judgementRevenge = align;
        //grid.RefreshBars();
    }

    internal void CalmJudgement()
    {
        judgementState = false;
    }

    internal void SetRevolution(Alignment align)
    {
        revolution = align;
    }

    internal void RemoveRevolution()
    {
        revolution = Alignment.None;
    }

    internal void SetTelekinesis(Alignment align)
    {
        telekinesis = align;
    }

    internal void SetTelekinesisDexterity(int value)
    {
        telekinesisDex = value;
    }

    internal void RemoveTelekinesis()
    {
        telekinesis = Alignment.None;
        telekinesisDex = 0;
    }

    //internal void InitiateResurrection(Alignment align)
    //{
    //    if (align == Alignment.None) throw new Exception("Initiating no resurrection!");
    //    resurrection = align;
    //}

    private void ProgressJudgementRevenge(Alignment currentAlign)
    {
        if (judgementAwaiting == currentAlign)
        {
            judgementRevenge = judgementAwaiting;
            judgementAwaiting = Alignment.None;
        }
        else if (judgementRevenge == Alignment.None) return;
        if (judgementRevenge == currentAlign) grid.ShowJudgement(currentAlign);
        else judgementRevenge = Alignment.None;
        //grid.RefreshBars();
    }

    //private void Resurrect(Alignment currentAlign)
    //{
    //    if (resurrection != currentAlign) return;
    //    grid.Turn.ExecuteResurrection(currentAlign);
    //    resurrection = Alignment.None;
    //}

    //private void CarryOnRevolution()
    //{
    //    if (revolution != Alignment.None) grid.CarryOnRevolution(revolution);
    //}
}