using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Settings;
using Berty.Utility;

namespace Berty.Gameplay.Managers
{
    public class EntityLoadManager : ManagerSingleton<EntityLoadManager>
    {
        private Game _game;
        public Game Game
        {
            get
            {
                if (_game == null) _game = LoadGame();
                return _game;
            }
            private set
            {
                _game = value;
            }
        }

        private Game LoadGame()
        {
            GameSaveData? data = StartGameBufferManager.Instance.Data;
            if (data == null) return new Game(AlignmentEnum.Player);
            return new Game((GameSaveData)data);
        }
    }
}
