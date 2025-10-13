using Berty.BoardCards;
using Berty.Enums;
using System;

namespace Berty.BoardCards.ConfigData
{
    public class SuperfanBert : CharacterConfig
    {
        public SuperfanBert()
        {
            AddName("superfan bert");
            AddSkill(SkillEnum.SuperfanBert);
            AddProperties(GenderEnum.Kid, RoleEnum.Agile);
            AddStats(1, 2, 5, 2);
            AddRange(1, 2, attackRange);
            AddRange(-1, 2, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("718109__riippumattog__fight-punch-hit");
        }
    }
}