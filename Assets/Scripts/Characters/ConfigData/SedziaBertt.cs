using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class SedziaBertt : CharacterConfig
    {
        public SedziaBertt()
        {
            AddName("sedzia bertt");
            AddSkill(SkillEnum.SedziaBertt);
            AddProperties(GenderEnum.Male, RoleEnum.Special);
            AddStats(1, 4, 4, 4);
            AddRange(1, 2, attackRange);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(-1, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("351429__kinoton__gun-laser-single-shot-sci-fi");
        }
    }
}