using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class KoszmarZBertwood : CharacterConfig
    {
        private bool attackPause;

        public KoszmarZBertwood()
        {
            AddName("koszmar z bertwood");
            AddSkill(SkillEnum.KoszmarZBertwood);
            AddProperties(GenderEnum.Kid, RoleEnum.Special);
            AddStats(3, 5, 3, 5);
            AddRange(0, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(1, -1, attackRange);
            AddRange(0, -1, attackRange);
            AddRange(-1, -1, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(-1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, blockRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("564485__rizzard__monster-growl");
        }
    }
}