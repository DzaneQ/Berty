using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertWho : CharacterConfig
    {
        //int turnCounter = 0;

        public BertWho()
        {
            AddName("bert who");
            AddSkill(SkillEnum.BertWho);
            AddProperties(GenderEnum.Male, RoleEnum.Support);
            AddStats(1, 3, 5, 4);
            AddRange(0, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("189575__unopiate__breaking-glass");
        }
    }
}