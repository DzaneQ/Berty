using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class EventManager : ManagerSingleton<EventManager>
    {
        public event Action OnNewTurn;

        public void RaiseOnNewTurn()
        {
            OnNewTurn?.Invoke();
        }
    }
}
