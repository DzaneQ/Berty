using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GlobalStatus // TODO: Adjust switching sides. Maybe put characters here.
{
    private FieldGrid grid;
    private List<Alignment> TakeNextTurn;
    private Alignment judgementState;
    private Alignment judgementAwaiting;
    private Alignment judgementRevenge;
    private Alignment revolution;
    private Alignment telekinesis;
    private int telekinesisDex;

    public bool IsJudgement => judgementState != Alignment.None;
    public Alignment JudgementRevenge => judgementRevenge;
    public Alignment Revolution => revolution;
    public Alignment Telekinesis => telekinesis;
    public int TelekinesisDex => telekinesisDex;
    public GlobalStatus(FieldGrid newGrid)
    {
        grid = newGrid;
        TakeNextTurn = new List<Alignment>();
        judgementState = Alignment.None;
        judgementAwaiting = Alignment.None;
        judgementRevenge = Alignment.None;
        revolution = Alignment.None;
        telekinesis = Alignment.None;
        telekinesisDex = 0;
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

    internal void SetJudgement(Alignment align)
    {
        if (Revolution != Alignment.None && Revolution != align) return;
        judgementState = align;
    }

    internal void RemoveJudgement()
    {
        judgementAwaiting = judgementState;
        judgementState = Alignment.None;
    }

    private void CalmJudgement()
    {
        if (Revolution == Alignment.None) throw new Exception("There's no revolution to adjust judgement!");
        if (!IsJudgement) return;
        if (judgementState == Revolution) return;
        judgementState = Alignment.None;
    }

    internal void SetRevolution(Alignment align)
    {
        revolution = align;
        CalmJudgement();
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
    }
}