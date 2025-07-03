using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
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
                if (_game == null) _game = new Game(Alignment.Player);
                return _game;
            }
        }
    }
}
