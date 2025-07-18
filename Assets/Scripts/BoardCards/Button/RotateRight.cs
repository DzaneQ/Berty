using Berty.BoardCards.Managers;
using Berty.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Button
{
    public class RotateRight : CardButton
    {
        private void OnMouseDown()
        {
            BoardCardActionManager.Instance.RotateCard(card, GetName());
        }

        protected override bool CanNavigate() => true;

        public override NavigationEnum GetName() => NavigationEnum.RotateRight;
    }
}