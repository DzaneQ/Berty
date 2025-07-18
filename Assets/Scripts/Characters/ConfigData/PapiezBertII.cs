﻿using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class PapiezBertII : CharacterConfig
    {
        public PapiezBertII()
        {
            AddName("papiez bert II");
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(2, 3, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("676109__vcs2683__church-claps-2.1-4.4");
        }

        public override void SkillOnNewTurn(CardSpriteBehaviour card)
        {
            if (!card.OccupiedField.IsAligned(card.Grid.Turn.CurrentAlignment)) return;
            //if (IsBlockedDuringRevolution(card)) return;
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
            {
                if (!field.IsOccupied() || field.IsAligned(card.OccupiedField.Align)) continue;
                field.OccupantCard.AdvancePower(-2, card);
            }
        }

        /*private bool IsBlockedDuringRevolution(CardSprite card)
        {
            return card.OccupiedField.IsOpposed(card.Grid.CurrentStatus.Revolution) && card.GetRole() == Role.Special;
        }*/
    }
}