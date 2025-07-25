﻿using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KuglarzBert : CharacterConfig
    {
        public KuglarzBert()
        {
            AddName("kuglarz bert");
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(1, 2, 6, 2);
            AddRange(1, 1, attackRange);
            AddRange(-1, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("181679__gingie__knife-throw");
        }

        public override void SkillOnSuccessfulAttack(CardSpriteBehaviour card)
        {
            card.AdvanceDexterity(-1, card);
            card.AdvanceHealth(1);
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            if (!card.IsAllied(target.OccupiedField)) return;
            target.AdvancePower(1, card);
            target.AddResistance(this);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
    }
}