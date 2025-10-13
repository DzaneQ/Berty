using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertkaIdolka : CharacterConfig
    {
        public BertkaIdolka()
        {
            AddName("bertka idolka");
            AddSkill(SkillEnum.BertkaIdolka);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(2, 2, 4, 4);
            AddRange(0, 1, attackRange);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("392483__gpag1__footsteps-boots-1.5");
        }
    }
}