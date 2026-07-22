using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;
using UnityEngine;

namespace Berty.Debugging.Managers
{
#if DEBUG
    public class DebugManager : ManagerSingleton<DebugManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = EntityLoadManager.Instance.Game;
        }

        public void TakeCardIfInPile(AlignmentEnum align) // NOTE: When debugging, change so CharacterEnum is returned rather than focusing on singleplayer logic
        {
            //Debug.Log("Taking debug card.");
            //if (align == AlignmentEnum.Player) game.CardPile.PullCardIfInPile(CharacterEnum.GotkaBerta, align);
            //if (align == AlignmentEnum.Opponent) game.CardPile.PullCardIfInPile(CharacterEnum.RoninBert, align);
            //if (align == AlignmentEnum.Opponent) game.CardPile.PullCardIfInPile(CharacterEnum.KrolPopuBert, align);
        }
    }
#else
    public class DebugManager
    {
        public static DebugManager Instance => null;

        public void TakeCardIfInPile(AlignmentEnum align) {}
    }
#endif
}
