using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Managers;
using Berty.UI.Managers;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class StatusManager : ManagerSingleton<StatusManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void AddUniqueStatusWithProvider(StatusEnum name, BoardCard card)
        {
            if (game.HasStatusByName(name)) return;
            game.AddStatusWithNameAndProvider(name, card);
            // Event on status added
        }

        public void AddStatusWithAlignment(StatusEnum name, AlignmentEnum align)
        {
            // TODO: Add status by name and alignment
            // Event on status added
        }

        public void IncrementChargedStatusWithAlignment(StatusEnum name, AlignmentEnum align, int charges)
        {
            game.IncrementChargedStatusWithNameAndAlignment(name, align, charges);
            // Event on status updated
        }

        public void RemoveStatusFromProvider(BoardCard card)
        {
            Status status = game.GetStatusFromProviderOrNull(card);
            if (status == null) return;
            game.RemoveStatusByName(status.Name);
            // Event on status removal
        }

        public void RemoveStatus(Status status)
        {
            game.RemoveStatus(status);
            // Event on status removal
        }
    }
}
