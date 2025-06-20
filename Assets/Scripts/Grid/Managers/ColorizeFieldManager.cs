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

        public Material GetMaterialFromAlignment(Alignment align)
        {
            return align switch
            {
                Alignment.None => neutral,
                Alignment.Player => player,
                Alignment.Opponent => opponent,
                _ => throw new Exception("Unknown alignment to get material from."),
            };
        }
    }
}