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
        private BoardCardCore core;

        /* CardBars index:
            0 - Strength
            1 - Power
            2 - Dexterity
            3 - Health
         */
        private CardBar[] CardBars { get; set; }


        private void Awake()
        {
            core = GetComponent<BoardCardCore>();
            CardBars = transform.GetChild(1).GetComponentsInChildren<CardBar>();
        }

        public void UpdateBar(StatEnum stat)
        {
            switch (stat)
            {
                case StatEnum.Strength:
                    CardBars[0].UpdateBar();
                    break;
                case StatEnum.Power:
                    CardBars[1].UpdateBar();
                    break;
                case StatEnum.Dexterity:
                    CardBars[2].UpdateBar();
                    break;
                case StatEnum.Health:
                    CardBars[3].UpdateBar();
                    break;
            }
        }

        public void HideBars()
        {
            foreach (CardBar bar in CardBars) bar.HideBar();
        }

        public void ShowBars()
        {
            foreach (CardBar bar in CardBars) bar.ShowBar();
        }

        public bool AreBarsAnimating()
        {
            foreach (CardBar bar in CardBars) if (bar.IsAnimating()) return true;
            return false;
        }

        public void HandleAfterBarChange()
        {
            if (AreBarsAnimating()) return;
            if (core.BoardCard.Stats.Health <= 0) core.KillCard();
        }
    }
}