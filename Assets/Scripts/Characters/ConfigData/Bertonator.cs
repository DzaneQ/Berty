using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class Bertonator : CharacterConfig
    {
        public Bertonator()
        {
            AddName("bertonator");
            AddSkill(SkillEnum.Bertonator);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(0, 3, 4, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("233053__lukeupf__footballtable");
        }
    }
}