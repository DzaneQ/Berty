using Berty.CardSprite;
using Berty.Enums;
using Berty.Field;

namespace Berty.Characters.Data
{
    public class ShaolinBert : Character
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
            foreach (FieldBehaviour field in card.Grid.Fields)
                if (field.IsOccupied() && !card.IsAllied(field))
                    field.OccupantCard.AdvanceStrength(-field.OccupantCard.CardStatus.Power / 3, card);
        }
    }
}