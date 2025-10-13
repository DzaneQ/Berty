using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class KuglarzBert : CharacterConfig
    {
        public KuglarzBert()
        {
            AddName("kuglarz bert");
            AddSkill(SkillEnum.KuglarzBert);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(1, 2, 6, 2);
            AddRange(1, 1, attackRange);
            AddRange(-1, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("181679__gingie__knife-throw");
        }
    }
}