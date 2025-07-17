using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.UI.Card.Systems;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.Managers
{
    public class CoreManager : ManagerSingleton<CoreManager>
    {
        private Game _game;
        public Game Game
        {
            get
            {
                if (_game == null) _game = new Game(AlignmentEnum.Player);
                return _game;
            }
        }

        private SelectionAndPaymentSystem _selectionAndPaymentSystem;
        public SelectionAndPaymentSystem SelectionAndPaymentSystem
        {
            get
            {
                if (_selectionAndPaymentSystem == null) _selectionAndPaymentSystem = new SelectionAndPaymentSystem();
                return _selectionAndPaymentSystem;
            }
        }
    }
}
