using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid;
using Berty.Grid.Field;
using Berty.UI.Card;

namespace Berty.BoardCards.ConfigData
{
    public class KrolPopuBert : CharacterConfig
    {
        public KrolPopuBert()
        {
            AddName("krol popu bert");
            AddSkill(SkillEnum.KrolPopuBert);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 3, 5, 2);
            AddRange(0, 1, attackRange);
            AddRange(1, 2, attackRange);
            AddRange(-1, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("570804__soundslikewillem__orchestra-hit-2");
        }
    }
}