using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class PrymusBert : CharacterConfig
    {
        public PrymusBert()
        {
            AddName("prymus bert");
            AddSkill(SkillEnum.PrymusBert);
            AddProperties(GenderEnum.Kid, RoleEnum.Support);
            AddStats(1, 3, 5, 3);
            AddRange(0, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("370814__ruviyamin__finger_snap");
        }
    }
}