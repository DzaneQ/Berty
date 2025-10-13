using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class KonstablBert : CharacterConfig
    {
        public KonstablBert()
        {
            AddName("konstabl bert");
            AddSkill(SkillEnum.KonstablBert);
            AddProperties(GenderEnum.Male, RoleEnum.Agile);
            AddStats(2, 3, 5, 3);
            AddRange(0, 2, attackRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("577354__thecrow_br__club");
        }
    }
}