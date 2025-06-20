using Berty.Entities;
using Berty.Enums;
using Berty.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Gameplay.ConfigData
{
    public class GameConfig
    {
        public int TableCapacity { get; }
        public Language Language { get; }

        public GameConfig()
        {
            TableCapacity = 6;
            Language = Language.Polish;
        }
    }
}
