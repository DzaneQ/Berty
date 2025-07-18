using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Managers;
using Berty.Grid.Field.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Button
{
    public class MoveDown : CardButton
    {
        private void OnMouseDown()
        {
            BoardCardActionManager.Instance.MoveCard(card, GetName());
        }

        protected override bool CanNavigate()
        {
            BoardField targetField = CoreManager.Instance.Game.Grid.GetFieldDistancedFromCardOrNull(0, -1, card.BoardCard);
            if (targetField == null) return false;
            if (targetField.IsOccupied()) return false;
            return true;
        }

        public override bool IsMoveButton() => true;

        public override NavigationEnum GetName() => NavigationEnum.MoveDown;
    }
}