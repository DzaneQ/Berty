using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class KuglarzBert : Character
    {
        public KuglarzBert()
        {
            AddName("kuglarz bert");
            AddProperties(Gender.Male, Role.Agile);
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