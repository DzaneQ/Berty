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
            if (data == null) Game = new Game(AlignmentEnum.Player);
            else Game = new Game((GameSaveData)data);
        }

        public void OverwriteGameFromData(GameSaveData data)
        {
            Game.OverwriteEntity(data);
        }
    }
}
