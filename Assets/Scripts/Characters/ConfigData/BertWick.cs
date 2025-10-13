using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertWick : CharacterConfig
    {
        public BertWick()
        {
            AddName("bert wick");
            AddSkill(SkillEnum.BertWick);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(1, 3, 5, 2);
            AddRange(-1, 0, attackRange);
            AddRange(1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("587172__derplayer__pistol-fire-until-empty");
        }
    }
}