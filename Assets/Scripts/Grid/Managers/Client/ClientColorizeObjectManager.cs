using Berty.Enums;
using System;

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