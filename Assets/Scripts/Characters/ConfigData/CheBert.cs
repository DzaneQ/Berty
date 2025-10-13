using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class CheBert : CharacterConfig
    {
        public CheBert()
        {
            AddName("che bert");
            AddSkill(SkillEnum.CheBert);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 3, 5, 3);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("251243__endlessenigma__piledriver");
        }
    }
}