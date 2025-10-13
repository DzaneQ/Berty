using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KsiezniczkaBerta : CharacterConfig
    {
        public KsiezniczkaBerta()
        {
            AddName("ksiezniczka berta");
            AddSkill(SkillEnum.KsiezniczkaBerta);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(1, 4, 4, 6);
            AddRange(1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("335354__littlerainyseasons__magic");
        }
    }
}