using Berty.BoardCards.Bar;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.BoardCards.State;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.UI.Card;
using Berty.UI.Card.Managers;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardBarsObjects : MonoBehaviour
    {
        /* CardBars index:
            0 - Strength
            1 - Power
            2 - Dexterity
            3 - Health
         */
        private CardBar[] CardBars { get; set; }
        private BoardCardCore core;

        private void Awake()
        {
            CardBars = transform.GetChild(1).GetComponentsInChildren<CardBar>();
            core = GetComponent<BoardCardCore>();
        }

        public void HideBars()
        {
            foreach (CardBar bar in CardBars) bar.HideBar();
        }

        public void ShowBars()
        {
            foreach (CardBar bar in CardBars) bar.ShowBar();
        }
    }
}