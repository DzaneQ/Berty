using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Managers
{
    public class CoreManager : ManagerSingleton<CoreManager>
    {
        private Game _game;
        public Game Game
        {
            get
            {
                if (_game != null) return new Game(Alignment.Player);
                return _game;
            }
        }

        protected override void Awake()
        {
            InitializeSingleton();
        }
    }
}
