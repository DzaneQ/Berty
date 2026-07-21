using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.UI.Card.Entities;
using Berty.Utility;
using UnityEngine;

namespace Berty.Debugging.Managers
{
#if DEBUG
    public class DebugManager : ManagerSingleton<DebugManager>
    {
        public void TakeCardIfInPile(AlignmentEnum align, CardPile pile)
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
