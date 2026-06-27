using Berty.Enums;
using Berty.Network.Managers;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Berty.Grid.Managers
{
    public class ClientColorizeObjectManager : ColorizeObjectManager
    {

        public void AdjustColorsToAlignment(AlignmentEnum align)
        {
            switch (align)
            {
                case AlignmentEnum.Player:
                    return;
                case AlignmentEnum.Opponent:
                    SwitchColors();
                    return;
                default:
                    throw new Exception("Unknown alignment to colorize object: " + align);
            }

        }

        private void SwitchColors()
        {
            (opponent, player) = (player, opponent);
        }
    }
}