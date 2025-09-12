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
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

namespace Berty.Characters.Managers
{
    public class ModifyStatChangeManager : ManagerSingleton<ModifyStatChangeManager>
    {
        // NOTE: Ensure that BigMadB, PogromcaBert (and other prevention skill cards) have logic applied when new stat modifier is made
 
        // output: If true, prevent stat change
        public bool BeforeHealthChange(BoardCardCore target, ref int value, BoardCardCore source, bool isBasicAttack = false)
        {
            bool shouldPreventStatChange = false;

            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special && !isBasicAttack) return true;
                    break;
                case CharacterEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support && !isBasicAttack) return true;
                    break;
                case CharacterEnum.PrymusBert:
                    if (value < 0) value++;
                    break;
                case CharacterEnum.ZalobnyBert:
                    if (value < 0 && source.BoardCard.GetRole() == RoleEnum.Offensive) shouldPreventStatChange = true;
                    break;
            }

            return shouldPreventStatChange;
        }

        // NOTE: After<stat>Change is executed during stat change animation

        public void AfterHealthChange(BoardCardCore target, int value, BoardCardCore source)
        {
            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special) return;
                    break;
                case CharacterEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support) return;
                    break;
                case CharacterEnum.KrzyzowiecBert:
                    if (value < 0) target.StatChange.AdvanceStrength(-value, null);
                    break;
                case CharacterEnum.Tankbert:
                    if (value < 0)
                    {
                        target.StatChange.AdvanceStrength(value, null);
                        target.StatChange.AdvanceDexterity(-value, null);
                    }
                    break;
                case CharacterEnum.ZalobnyBert:
                    if (value < 0) EventManager.Instance.RaiseOnValueChange(target, value);
                    break;
            }

            if (source == null) return;

            switch (source.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.KuglarzBert:
                    if (value < 0)
                    {
                        source.StatChange.AdvanceHealth(1, null);
                        source.StatChange.AdvanceDexterity(-1, null);
                    }
                    break;
                case CharacterEnum.Zombert:
                    if (target.BoardCard.Align == source.BoardCard.Align) break;
                    if (value < 0) target.StatChange.AdvancePower(-1, source);
                    break;
            }
        }

        public int GetModifiedStrengthForAttack(BoardCardCore target, BoardCardCore source)
        {
            int strength = source.BoardCard.Stats.Strength;

            switch (target.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertPogromca:
                    if (source.BoardCard.GetRole() == RoleEnum.Special) return strength;
                    break;
                case CharacterEnum.BigMadB:
                    if (source.BoardCard.GetRole() == RoleEnum.Support) return strength;
                    break;
            }    

            switch (source.BoardCard.CharacterConfig.Character)
            {
                case CharacterEnum.BertPogromca:
                    if (target.BoardCard.GetRole() == RoleEnum.Special) strength = strength + 2;
                    break;
                case CharacterEnum.KonstablBert:
                    RoleEnum targetRole = target.BoardCard.GetRole();
                    RoleEnum[] vulnerableRoles = { RoleEnum.Special, RoleEnum.Support };
                    if (vulnerableRoles.Contains(targetRole))
                        strength++;
                    break;
                case CharacterEnum.StaryBert:
                    if (target.BoardCard.Stats.Health < target.BoardCard.CharacterConfig.Health)
                        strength = strength + 2;
                    break;
            }

            return strength;
        }
    }
}