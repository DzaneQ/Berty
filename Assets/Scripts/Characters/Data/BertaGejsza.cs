﻿using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class BertaGejsza : Character
    {
        public BertaGejsza()
        {
            AddName("berta gejsza");
            AddProperties(Gender.Female, Role.Support);
            AddStats(1, 2, 5, 3);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("217656__reitanna__knuckles-cracking");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            if (card.IsAllied(target.OccupiedField)) target.AdvanceDexterity(-1, card);
            else target.AdvanceDexterity(-3, card);
            target.AddResistance(this);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
    }
}