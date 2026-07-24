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
            if (align == AlignmentEnum.Player) pile.PullCardIfInPile(CharacterEnum.KsiezniczkaBerta, align);
            if (align == AlignmentEnum.Opponent) pile.PullCardIfInPile(CharacterEnum.BertaSJW, align);
            if (align == AlignmentEnum.Opponent) pile.PullCardIfInPile(CharacterEnum.RycerzBerti, align);
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
