using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class PrymusBert : CharacterConfig
    {
        public PrymusBert()
        {
            AddName("prymus bert");
            AddProperties(Gender.Kid, Role.Support);
            AddStats(1, 3, 5, 3);
            AddRange(0, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("370814__ruviyamin__finger_snap");
        }

        public override int SkillDefenceModifier(int damage, CardSpriteBehaviour attacker)
        {
            if (damage > 0) return damage - 1;
            return damage;
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            target.AdvancePower(3, card);
            target.AddResistance(this);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
    }
}