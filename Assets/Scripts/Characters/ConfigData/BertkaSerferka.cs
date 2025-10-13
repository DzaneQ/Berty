using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class BertkaSerferka : CharacterConfig
    {
        public BertkaSerferka()
        {
            AddName("bertka serferka");
            AddSkill(SkillEnum.BertkaSerferka);
            AddProperties(GenderEnum.Female, RoleEnum.Agile);
            AddStats(2, 3, 5, 4);
            AddRange(0, 2, attackRange);
            AddRange(1, 2, attackRange);
            AddRange(-1, 2, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("756350__indythecringemaster__retro-ocean-noise");
        }
    }
}