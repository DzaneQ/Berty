﻿using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class CheBert : CharacterConfig
    {
        public CheBert()
        {
            AddName("che bert");
            AddProperties(Gender.Male, Role.Special);
            AddStats(1, 3, 5, 3);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("251243__endlessenigma__piledriver");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            card.Grid.SetRevolution(card.OccupiedField.Align);
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
            {
                if (!field.IsAligned(card.OccupiedField.Align)) continue;
                if (field.OccupantCard.GetRole() == Role.Special) field.OccupantCard.AdvanceStrength(1);
            }
        }

        public override void SkillAdjustPowerChange(int value, CardSpriteBehaviour card, CardSpriteBehaviour spellSource)
        {
            if (card.CardStatus.Power > 0) return; // Not tested.
            Alignment newAlign = Alignment.Player;
            if (card.OccupiedField.IsAligned(Alignment.Player)) newAlign = Alignment.Opponent;
            card.Grid.SetRevolution(newAlign);
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
            {
                if (!field.IsOccupied()) continue;
                if (field.OccupantCard.GetRole() != Role.Special) continue;
                if (field.OccupantCard == card) continue;
                if (field.IsAligned(newAlign)) field.OccupantCard.AdvanceStrength(1);
                else field.OccupantCard.AdvanceStrength(-1);
                if (field.IsAligned(Alignment.None)) throw new System.Exception("No alignment for non-occupied field.");
            }
        }

        public override void SkillOnDeath(CardSpriteBehaviour card)
        {
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
            {
                if (!field.IsAligned(card.Grid.CurrentStatus.Revolution)) continue;
                if (field.OccupantCard == card) continue;
                if (field.OccupantCard.GetRole() == Role.Special) field.OccupantCard.AdvanceStrength(-1);
            }
            card.Grid.RemoveRevolution();
        }
    }
}