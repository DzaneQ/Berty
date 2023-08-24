using System;
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

    public bool IsJudgement => judgementState;
    public Alignment JudgementRevenge => judgementRevenge;
    public GlobalStatus(FieldGrid newGrid)
    {
        grid = newGrid;
        TakeNextTurn = new List<Alignment>();
        judgementState = false;
        judgementAwaiting = Alignment.None;
        judgementRevenge = Alignment.None;
    }

    public void AdjustNewTurn(Alignment currentAlign)
    {
        TakeQueuedCards();
        ProgressJudgementRevenge(currentAlign);
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
}