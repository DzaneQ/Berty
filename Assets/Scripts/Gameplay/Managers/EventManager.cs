using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using Berty.BoardCards.Behaviours;

namespace Berty.Gameplay.Managers
{
    public class EventManager : ManagerSingleton<EventManager>
    {
        public event Action OnNewTurn;
        public event EventHandler OnPaymentStart;
        public event Action OnPaymentConfirm;
        public event Action OnPaymentCancel;

        public void RaiseOnNewTurn()
        {
            OnNewTurn?.Invoke();
        }

        public void RaiseOnPaymentStart(BoardCardCore card)
        {
            OnPaymentStart?.Invoke(card, EventArgs.Empty);
        }

        public void RaiseOnPaymentConfirm()
        {
            OnPaymentConfirm?.Invoke();
        }

        public void RaiseOnPaymentCancel()
        {
            OnPaymentCancel?.Invoke();
        }
    }
}
