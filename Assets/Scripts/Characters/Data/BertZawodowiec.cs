using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class BertZawodowiec : CharacterConfig
    {
        public BertZawodowiec()
        {
            AddName("bert zawodowiec");
            AddProperties(Gender.Male, Role.Agile);
            AddStats(2, 4, 3, 4);
            AddRange(1, 1, attackRange);
            AddRange(2, 2, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("632821__cloud-10__gunshot");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards())
            {
                card.AdvanceStrength(1, card);
                if (card.IsAllied(adjCard.OccupiedField)) adjCard.AdvancePower(1, card);
            }
        }
    }
}