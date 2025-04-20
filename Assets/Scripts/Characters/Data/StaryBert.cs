using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class StaryBert : CharacterConfig
    {
        public StaryBert()
        {
            AddName("stary bert i moze");
            AddProperties(Gender.Male, Role.Offensive);
            AddStats(1, 2, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("212143__qubodup__splash-by-blaukreuz");
        }

        public override int SkillAttackModifier(int damage, CardSpriteBehaviour target)
        {
            if (IsTheCardDamaged(target)) return damage + 2;
            return damage;
        }

        private bool IsTheCardDamaged(CardSpriteBehaviour targetCard)
        {
            return targetCard.CardStatus.Health < targetCard.Character.Health;
        }
    }
}