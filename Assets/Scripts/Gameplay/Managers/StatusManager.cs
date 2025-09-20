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

        public void AddUniqueStatusWithProvider(StatusEnum name, BoardCard card, AlignmentEnum targetAlign = AlignmentEnum.None)
        {
            if (game.HasStatusByName(name)) return;
            Status status = game.AddStatusWithNameAndProvider(name, card, targetAlign);
            EventManager.Instance.RaiseOnStatusUpdated(status);
        }

        public void SetChargedStatusWithProvider(StatusEnum name, BoardCard card, int charges)
        {
            Status status = game.SetChargedStatusWithNameAndProvider(name, card, charges);
            EventManager.Instance.RaiseOnStatusUpdated(status);
        }

        public void IncrementChargedStatusWithAlignment(StatusEnum name, AlignmentEnum align, int charges)
        {
            Status status = game.IncrementChargedStatusWithNameAndAlignment(name, align, charges);
            EventManager.Instance.RaiseOnStatusUpdated(status);
        }

        public void RemoveStatusFromProvider(BoardCard card)
        {
            Status status = game.GetStatusFromProviderOrNull(card);
            if (status == null) return;
            RemoveStatus(status);
        }

        public void RemoveStatus(Status status)
        {
            StatusEnum deletedStatusName = status.Name;
            AlignmentEnum align = status.GetAlign();
            game.RemoveStatus(status);
            EventManager.Instance.RaiseOnStatusRemoved(deletedStatusName, align);
        }
    }
}
