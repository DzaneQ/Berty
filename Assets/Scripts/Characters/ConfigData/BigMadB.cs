using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BigMadB : CharacterConfig
    {
        public BigMadB()
        {
            AddName("big mad b");
            AddSkill(SkillEnum.BigMadB);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 3, 5, 4);
            AddRange(1, 0, attackRange);
            AddRange(-1, 0, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("539125__badoink__wobblelooper");
        }
    }
}