using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditorInternal;
using UnityEngine;

namespace Berty.Debugging
{
#if DEBUG
    public class DebugManager : ManagerSingleton<DebugManager>
    {
        private Game game;

        protected override void Awake()
        {
            base.Awake();
            game = CoreManager.Instance.Game;
        }

        public void TakeCardIfInPile(AlignmentEnum align)
        {
            if (align == AlignmentEnum.Player) game.CardPile.PullCardIfInPile(CharacterEnum.KrolPopuBert, align);
            else game.CardPile.PullCardIfInPile(CharacterEnum.RoninBert, align);
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
