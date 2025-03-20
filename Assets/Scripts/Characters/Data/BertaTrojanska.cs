﻿using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class BertaTrojanska : Character
    {
        public BertaTrojanska()
        {
            AddName("berta trojanska");
            AddProperties(Gender.Female, Role.Support);
            AddStats(1, 2, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("706382__agglow__slap");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            if (!card.IsAllied(target.OccupiedField)) target.AdvanceStrength(-1, card);
            else target.AdvancePower(1, card);
            target.AddResistance(this);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
    }
}