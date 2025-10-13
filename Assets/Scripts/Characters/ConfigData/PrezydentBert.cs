using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class PrezydentBert : CharacterConfig
    {
        public PrezydentBert()
        {
            AddName("prezydent bert");
            AddSkill(SkillEnum.PrezydentBert);
            AddProperties(GenderEnum.Male, RoleEnum.Support);
            AddStats(1, 3, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, -1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("557114__firediesproductions__fdp-coin-flip-3");
        }
    }
}