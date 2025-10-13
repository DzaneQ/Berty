using Berty.BoardCards;
using Berty.Enums;
using System.Linq;

namespace Berty.BoardCards.ConfigData
{
    public class EBerta : CharacterConfig
    {
        public EBerta()
        {
            AddName("eberta");
            AddSkill(SkillEnum.EBerta);
            AddProperties(GenderEnum.Female, RoleEnum.Offensive);
            AddStats(1, 2, 5, 3);
            AddRange(0, -1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("487832__bendrain__cardboard_impact_02");
        }
    }
}