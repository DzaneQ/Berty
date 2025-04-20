using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class Tankbert : CharacterConfig
    {
        public Tankbert()
        {
            AddName("tankbert");
            AddProperties(Gender.Male, Role.Offensive);
            AddStats(5, 5, 2, 5);
            AddRange(0, 1, attackRange);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("182429__qubodup__explosion");
        }

        public override void SkillAdjustHealthChange(int value, CardSpriteBehaviour card)
        {
            if (0 <= value) return;
            card.AdvanceStrength(value, card);
            card.AdvanceDexterity(-value, card);
        }
    }
}