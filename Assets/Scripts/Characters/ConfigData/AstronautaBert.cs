using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class AstronautaBert : CharacterConfig
    {
        public AstronautaBert()
        {
            AddName("astronauta bert");
            AddSkill(SkillEnum.AstronautaBert);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(1, 2, 5, 2);
            AddRange(1, 1, attackRange);
            AddRange(1, -1, attackRange);
            AddRange(-1, -1, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("657936__matrixxx__satellite-signal-02");
        }
    }
}