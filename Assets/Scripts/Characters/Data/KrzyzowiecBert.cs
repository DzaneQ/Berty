using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class KrzyzowiecBert : CharacterConfig
    {
        public KrzyzowiecBert()
        {
            AddName("krzyzowiec bert");
            AddProperties(Gender.Male, Role.Offensive);
            AddStats(2, 3, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("733887__velcronator__sword-impact");
        }

        public override void SkillAdjustHealthChange(int value, CardSpriteBehaviour card)
        {
            if (value >= 0) return;
            card.AdvanceStrength(-value, card);
        }
    }
}