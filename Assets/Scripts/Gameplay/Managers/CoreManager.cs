using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Utility;

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
    }
}
