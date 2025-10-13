using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertaGejsza : CharacterConfig
    {
        public BertaGejsza()
        {
            AddName("berta gejsza");
            AddSkill(SkillEnum.BertaGejsza);
            AddProperties(GenderEnum.Female, RoleEnum.Support);
            AddStats(1, 2, 5, 3);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("217656__reitanna__knuckles-cracking");
        }
    }
}