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

        public void AddStatusWithProvider(BoardCard card)
        {
            // TODO: Add status by name and provider
            // Event on status added
        }

        public void AddStatusWithAlignment(AlignmentEnum align)
        {
            // TODO: Add status by name and alignment
            // Event on status added
        }

        public void RemoveStatusFromProvider(BoardCard card)
        {
            Status status = game.FindStatusFromProviderOrNull(card);
            if (status == null) return;
            game.RemoveStatusByName(status.Name);
            // Event on status removal
        }
    }
}
