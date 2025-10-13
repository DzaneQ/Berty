using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class GotkaBerta : CharacterConfig
    {
        public GotkaBerta()
        {
            AddName("gotka berta");
            AddSkill(SkillEnum.GotkaBerta);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(1, 4, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("467777__sgak__thunder");
        }
    }
}