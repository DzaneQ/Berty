using Berty.BoardCards;
using Berty.Enums;
using System.Linq;

namespace Berty.BoardCards.ConfigData
{
    public class EBerta : CharacterConfig
    {
        public EBerta()
        {
            AddName("eberta");
            AddProperties(GenderEnum.Female, RoleEnum.Offensive);
            AddStats(1, 2, 5, 3);
            AddRange(0, -1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("487832__bendrain__cardboard_impact_02");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        //}

        //public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        //{
        //    if (!card.IsAllied(target.OccupiedField)) return;
        //    int[] stats = { target.GetStrength(), target.CardStatus.Power, target.CardStatus.Dexterity, target.CardStatus.Health };
        //    int minStat = stats.Min();
        //    if (minStat == stats[0]) target.AdvanceStrength(1, card);
        //    if (minStat == stats[1]) target.AdvancePower(1, card);
        //    if (minStat == stats[2]) target.AdvanceDexterity(1, card);
        //    if (minStat == stats[3]) target.AdvanceHealth(1, card);
        //    target.AddResistance(this);
        //}

        //public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);
    }
}