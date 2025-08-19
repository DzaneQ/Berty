using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Berty.Grid.Managers
{
    public class ColorizeObjectManager : ManagerSingleton<ColorizeObjectManager>
    {

        [SerializeField] private Material neutral;
        [SerializeField] private Material player;
        [SerializeField] private Material opponent;
        [SerializeField] private Material attacked;

        private Color attackColor = new Color(1f, 0.55f, 0f, 1f);
        private Color blockColor = new Color(0f, 0.35f, 0.8f, 1f);

        public Material GetMaterialFromAlignment(AlignmentEnum align, HighlightEnum highlight)
        {
            return align switch
            {
                AlignmentEnum.None => highlight == HighlightEnum.None ? neutral : attacked,
                AlignmentEnum.Player => player,
                AlignmentEnum.Opponent => opponent,
                _ => throw new Exception("Unknown alignment to get material from."),
            };
        }

        public Color GetColorForCard(HighlightEnum highlight)
        {
            return highlight switch
            {
                HighlightEnum.UnderAttack => attackColor,
                HighlightEnum.UnderBlock => blockColor,
                _ => throw new Exception("Unknown highlight to get color from."),
            };
        }
    }
}