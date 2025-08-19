using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class SamurajBert : CharacterConfig
    {
        private bool activeSpell = true;
        public SamurajBert()
        {
            AddName("samuraj bert");
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(2, 3, 3, 4);
            AddRange(0, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("456658__ethanchase7744__samurai-sword");
        }

        //public override void SkillOnNewCard(CardSpriteBehaviour card)
        //{
        //    activeSpell = true;
        //    SkillOnNeighbor(card, card);
        //}

        //public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        //{
        //    if (!activeSpell || card.GetAdjacentCards().Count < 3) return;
        //    card.AdvanceDexterity(1, card);
        //    card.AdvancePower(1, card);
        //    activeSpell = false;
        //}

        //public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNeighbor(card, card);
    }
}