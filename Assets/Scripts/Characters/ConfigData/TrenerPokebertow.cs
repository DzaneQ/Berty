using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class TrenerPokebertow : CharacterConfig
    {
        public TrenerPokebertow()
        {
            AddName("trener pokebertow");
            AddProperties(GenderEnum.Kid, RoleEnum.Offensive);
            AddStats(1, 2, 5, 2);
            AddRange(0, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("453001__breviceps__pokemon-cry-parody");
        }

        //public override void SkillCardClick(CardSpriteBehaviour card)
        //{
        //    card.Grid.SetBackupCard(card.OccupiedField);
        //    card.CardManager.DeselectCards();
        //}
    }
}