using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Entities
{
    public class GlobalEffects
    {
        private List<AlignmentEnum> takeNextTurn;
        private AlignmentEnum judgementState;
        private AlignmentEnum judgementAwaiting;
        private AlignmentEnum judgementRevenge;
        private AlignmentEnum revolution;
        private AlignmentEnum telekinesis;
        private int telekinesisDex;

        public GlobalEffects()
        {
            takeNextTurn = new List<AlignmentEnum>();
            judgementState = AlignmentEnum.None;
            judgementAwaiting = AlignmentEnum.None;
            judgementRevenge = AlignmentEnum.None;
            revolution = AlignmentEnum.None;
            telekinesis = AlignmentEnum.None;
        }
    }
}