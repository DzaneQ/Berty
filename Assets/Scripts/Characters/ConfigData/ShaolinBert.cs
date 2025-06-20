using Berty.BoardCards;
using Berty.Enums;
using Berty.Grid.Field;

namespace Berty.BoardCards.ConfigData
{
    public class ShaolinBert : CharacterConfig
    {
        public ShaolinBert()
        {
            AddName("shaolin bert");
            AddProperties(Gender.Male, Role.Support);
            AddStats(1, 3, 5, 4);
            AddRange(1, 1, attackRange);
            AddRange(-1, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("518292__logicogonist__gong-2");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            foreach (OutdatedFieldBehaviour field in card.Grid.Fields)
                if (field.IsOccupied() && !card.IsAllied(field))
                    field.OccupantCard.AdvanceStrength(-field.OccupantCard.CardStatus.Power / 3, card);
        }
    }
}