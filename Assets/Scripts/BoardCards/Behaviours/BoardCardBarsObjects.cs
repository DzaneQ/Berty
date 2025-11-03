using Berty.BoardCards.Bar;
using Berty.Enums;
using UnityEngine;

namespace Berty.BoardCards.Behaviours
{
    public class BoardCardBarsObjects : BoardCardBehaviour
    {
        /* CardBars index:
            0 - Strength
            1 - Power
            2 - Dexterity
            3 - Health
         */
        private CardBar[] CardBars { get; set; }


        protected override void Awake()
        {
            base.Awake();
            CardBars = transform.GetChild(1).GetComponentsInChildren<CardBar>();
        }

        public void UpdateBars()
        {
            UpdateBar(StatEnum.Strength);
            UpdateBar(StatEnum.Power);
            UpdateBar(StatEnum.Dexterity);
            UpdateBar(StatEnum.Health);
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
            Core.StatChange.HandleAfterAnimationStatChange();
        }
    }
}