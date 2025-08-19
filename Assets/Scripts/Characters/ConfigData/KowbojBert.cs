using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KowbojBert : CharacterConfig
    {
        public KowbojBert()
        {
            AddName("kowboj bert");
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(2, 3, 4, 3);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("182577__qubodup__whip-2.8-3.7");
        }

        //public override void SkillOnSuccessfulAttack(CardSpriteBehaviour card)
        //{
        //    card.AdvanceDexterity(1, card);
        //    foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards())
        //        if (card.IsAllied(adjCard.OccupiedField))
        //            adjCard.AdvanceDexterity(1, card);
        //}
    }
}