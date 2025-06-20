using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Entities
{
    public class GlobalEffects
    {
        private List<Alignment> takeNextTurn;
        private Alignment judgementState;
        private Alignment judgementAwaiting;
        private Alignment judgementRevenge;
        private Alignment revolution;
        private Alignment telekinesis;
        private int telekinesisDex;

        public GlobalEffects()
        {
            takeNextTurn = new List<Alignment>();
            judgementState = Alignment.None;
            judgementAwaiting = Alignment.None;
            judgementRevenge = Alignment.None;
            revolution = Alignment.None;
            telekinesis = Alignment.None;
        }
    }
}