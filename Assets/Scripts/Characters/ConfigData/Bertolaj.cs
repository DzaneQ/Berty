using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class Bertolaj : CharacterConfig
    {
        public Bertolaj()
        {
            AddName("bertolaj");
            AddSkill(SkillEnum.Bertolaj);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 4, 4, 3);
            AddRange(0, 1, attackRange);
            AddRange(1, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(1, -1, attackRange);
            AddRange(0, -1, attackRange);
            AddRange(-1, -1, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("441649__crownieyt__open-package-box-parcel");
        }
    }
}