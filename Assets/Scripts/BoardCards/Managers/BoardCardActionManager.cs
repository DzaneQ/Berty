using Berty.BoardCards.Behaviours;
using Berty.Enums;
using Berty.Utility;
using System;
using UnityEngine;

namespace Berty.BoardCards.Managers
{
    public class BoardCardActionManager : ManagerSingleton<BoardCardActionManager>
    {
        public void RotateCard(BoardCardMovableObject card, NavigationEnum navigation)
        {
            int angle = navigation switch
            {
                NavigationEnum.RotateLeft => -90,
                NavigationEnum.RotateRight => 90,
                _ => throw new ArgumentException("Invalid NavigationEnum for RotateCard")
            };
            card.RotateCardObject(angle);
            
        }
    }
}