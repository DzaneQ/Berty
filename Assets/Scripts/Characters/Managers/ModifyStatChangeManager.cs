using Berty.BoardCards;
using Berty.BoardCards.Animation;
using Berty.BoardCards.Behaviours;
using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Managers;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Gameplay.Managers;
using Berty.Grid.Entities;
using Berty.Grid.Field.Entities;
using Berty.Structs;
using Berty.UI.Card.Collection;
using Berty.Utility;
using System;
using System.Linq;
using UnityEngine;

namespace Berty.Characters.Managers
{
    public class ModifyStatChangeManager : ManagerSingleton<ModifyStatChangeManager>
    {
 
        // output: If true, prevent stat change

        public bool BeforeHealthChange(BoardCardCore target, ref int value, BoardCardCore source)
        {
            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.PrymusBert:
                    if (value < 0) value++;
                    return value == 0;
                default:
                    return false;
            }
        }

        // NOTE: After<stat>Change is executed during stat change animation

        public void AfterHealthChange(BoardCardCore target, int value, BoardCardCore source)
        {
            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.KrzyzowiecBert:
                    if (value < 0) target.StatChange.AdvanceStrength(-value, null);
                    break;
            }
        }
    }
}