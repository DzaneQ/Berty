using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.Grid
{
    public class GlobalStatus // TODO: Adjust switching sides. Maybe put characters here.
    {
        private FieldGrid grid;
        private List<AlignmentEnum> TakeNextTurn;
        private AlignmentEnum judgementState;
        private AlignmentEnum judgementAwaiting;
        private AlignmentEnum judgementRevenge;
        private AlignmentEnum revolution;
        private AlignmentEnum telekinesis;
        private int telekinesisDex;

        public bool IsJudgement => judgementState != AlignmentEnum.None;
        public AlignmentEnum JudgementRevenge => judgementRevenge;
        public AlignmentEnum Revolution => revolution;
        public AlignmentEnum Telekinesis => telekinesis;
        public int TelekinesisDex => telekinesisDex;
        public GlobalStatus(FieldGrid newGrid)
        {
            grid = newGrid;
            TakeNextTurn = new List<AlignmentEnum>();
            judgementState = AlignmentEnum.None;
            judgementAwaiting = AlignmentEnum.None;
            judgementRevenge = AlignmentEnum.None;
            revolution = AlignmentEnum.None;
            telekinesis = AlignmentEnum.None;
            telekinesisDex = 0;
        }

        public void AdjustNewTurn(AlignmentEnum currentAlign)
        {
            TakeQueuedCards();
            ProgressJudgementRevenge(currentAlign);
        }

        private void TakeQueuedCards()
        {
            AlignmentEnum currentTurn = grid.Turn.CurrentAlignment;
            for (int i = TakeNextTurn.Where(x => x == currentTurn).Count(); i > 0; i--)
            {
                grid.Turn.CM.PullCard(currentTurn);
                TakeNextTurn.Remove(currentTurn);
            }
        }

        internal void RequestCard(AlignmentEnum align)
        {
            TakeNextTurn.Add(align);
        }

        internal void SetJudgement(AlignmentEnum align)
        {
            if (Revolution != AlignmentEnum.None && Revolution != align) return;
            judgementState = align;
        }

        internal void RemoveJudgement()
        {
            judgementAwaiting = judgementState;
            judgementState = AlignmentEnum.None;
        }

        private void CalmJudgement()
        {
            if (Revolution == AlignmentEnum.None) throw new Exception("There's no revolution to adjust judgement!");
            if (!IsJudgement) return;
            if (judgementState == Revolution) return;
            judgementState = AlignmentEnum.None;
        }

        internal void SetRevolution(AlignmentEnum align)
        {
            revolution = align;
            CalmJudgement();
        }

        internal void RemoveRevolution()
        {
            revolution = AlignmentEnum.None;
        }

        internal void SetTelekinesis(AlignmentEnum align)
        {
            telekinesis = align;
        }

        internal void SetTelekinesisDexterity(int value)
        {
            telekinesisDex = value;
        }

        internal void RemoveTelekinesis()
        {
            telekinesis = AlignmentEnum.None;
            telekinesisDex = 0;
        }

        private void ProgressJudgementRevenge(AlignmentEnum currentAlign)
        {
            if (judgementAwaiting == currentAlign)
            {
                judgementRevenge = judgementAwaiting;
                judgementAwaiting = AlignmentEnum.None;
            }
            else if (judgementRevenge == AlignmentEnum.None) return;
            if (judgementRevenge == currentAlign) grid.ShowJudgement(currentAlign);
            else judgementRevenge = AlignmentEnum.None;
        }
    }
}