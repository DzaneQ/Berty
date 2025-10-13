using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertZawodowiec : CharacterConfig
    {
        public BertZawodowiec()
        {
            AddName("bert zawodowiec");
            AddSkill(SkillEnum.BertZawodowiec);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(2, 4, 3, 4);
            AddRange(1, 1, attackRange);
            AddRange(2, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("632821__cloud-10__gunshot");
        }
    }
}