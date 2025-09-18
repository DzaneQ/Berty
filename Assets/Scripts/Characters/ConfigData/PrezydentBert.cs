using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class PrezydentBert : CharacterConfig
    {
        public PrezydentBert()
        {
            AddName("prezydent bert");
            AddSkill(SkillEnum.PrezydentBert);
            AddProperties(GenderEnum.Male, RoleEnum.Support);
            AddStats(1, 3, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, -1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("557114__firediesproductions__fdp-coin-flip-3");
        }

        //public override void SkillOnAttack(CardSpriteBehaviour card)
        //{
        //    foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) if (card.IsAllied(adjCard.OccupiedField)) adjCard.AdvanceStrength(1, card);
        //    card.AdvancePower(-1, card);
        //}

        //public override void SkillOnMove(CardSpriteBehaviour card)
        //{
        //    SkillOnAttack(card);
        //}
    }
}