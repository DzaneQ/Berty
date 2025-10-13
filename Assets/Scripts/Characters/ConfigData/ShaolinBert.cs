using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class ShaolinBert : CharacterConfig
    {
        public ShaolinBert()
        {
            AddName("shaolin bert");
            AddSkill(SkillEnum.ShaolinBert);
            AddProperties(GenderEnum.Male, RoleEnum.Support);
            AddStats(1, 3, 5, 4);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("518292__logicogonist__gong-2");
        }
    }
}