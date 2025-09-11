using Berty.BoardCards;
using Berty.Enums;
using System;

namespace Berty.BoardCards.ConfigData
{
    public class SuperfanBert : CharacterConfig
    {
        public SuperfanBert()
        {
            AddName("superfan bert");
            SetCharacter(CharacterEnum.SuperfanBert);
            AddProperties(GenderEnum.Kid, RoleEnum.Agile);
            AddStats(1, 2, 5, 2);
            AddRange(1, 2, attackRange);
            AddRange(-1, 2, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("718109__riippumattog__fight-punch-hit");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    int hour = DateTime.Now.Hour;
        //    //UnityEngine.Debug.Log($"Current hour is: {hour}");
        //    if (hour < 5 || 18 <= hour)
        //    {
        //        card.AdvanceStrength(1, card);
        //        card.AdvancePower(2, card);
        //    }
        //    SkillOnMove(card);
        //}

        //public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        //{
        //    if (!card.IsAllied(target.OccupiedField)) return;
        //    target.AdvancePower(1, card);
        //    target.AddResistance(this);
        //}

        //public override void SkillOnMove(CardSpriteBehaviour card)
        //{
        //    foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        //}
    }
}