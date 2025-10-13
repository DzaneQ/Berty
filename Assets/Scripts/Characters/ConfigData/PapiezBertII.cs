using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class PapiezBertII : CharacterConfig
    {
        public PapiezBertII()
        {
            AddName("papiez bert II");
            AddSkill(SkillEnum.PapiezBertII);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(2, 3, 5, 3);
            AddRange(0, 2, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("676109__vcs2683__church-claps-2.1-4.4");
        }
    }
}