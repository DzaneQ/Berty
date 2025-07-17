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
            cardNavigation.RotateCardObject(90);
        }

        protected override bool CanNavigate() => true;

        public override NavigationEnum GetName() => NavigationEnum.RotateRight;
    }
}