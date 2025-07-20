using Berty.Enums;
using Berty.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Managers
{
    public class ColorizeFieldManager : ManagerSingleton<ColorizeFieldManager>
    {

        [SerializeField] private Material neutral;
        [SerializeField] private Material player;
        [SerializeField] private Material opponent;

        public Material GetMaterialFromAlignment(AlignmentEnum align)
        {
            return align switch
            {
                AlignmentEnum.None => neutral,
                AlignmentEnum.Player => player,
                AlignmentEnum.Opponent => opponent,
                _ => throw new Exception("Unknown alignment to get material from."),
            };
        }

        public Material GetNeutralMaterial()
        {
            return neutral;
        }
    }
}