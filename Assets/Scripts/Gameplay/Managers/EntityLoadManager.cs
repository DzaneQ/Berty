using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Settings;
using Berty.Utility;
using System;

namespace Berty.Gameplay.Managers
{
    public class EntityLoadManager : ManagerSingleton<EntityLoadManager>
    {
        private Game _game;
        public Game Game
        {
            get => _game ?? throw new InvalidOperationException("Attempting to call null game entity.");
            private set
            {
                _game = value;
            }
        }

        public void InitializeGame()
        {
            if (_game != null) throw new InvalidOperationException("Trying to initialize already initialized game entity.");
            GameSaveData? data = StartGameBufferManager.Instance.Data;
            Game = data == null ? new Game() : new Game((GameSaveData)data);
        }

        public void LoadData(GameSaveData data)
        {
            Game = new Game(data);
        }
    }
}
