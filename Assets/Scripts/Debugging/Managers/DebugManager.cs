using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Utility;

namespace Berty.Debugging
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

        public void TakeCardIfInPile(AlignmentEnum align)
        {
            if (align == AlignmentEnum.Player) game.CardPile.PullCardIfInPile(SkillEnum.BertaSJW, align);
            if (align == AlignmentEnum.Player) game.CardPile.PullCardIfInPile(SkillEnum.ShaolinBert, align);
            if (align == AlignmentEnum.Opponent) game.CardPile.PullCardIfInPile(SkillEnum.CheBert, align);
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
