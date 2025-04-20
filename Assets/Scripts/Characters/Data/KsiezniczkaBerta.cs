using Berty.CardSprite;
using Berty.Enums;

namespace Berty.Characters.Data
{
    public class KsiezniczkaBerta : CharacterConfig
    {
        public KsiezniczkaBerta()
        {
            AddName("ksiezniczka berta");
            AddProperties(Gender.Female, Role.Support);
            AddStats(1, 4, 4, 6);
            AddRange(1, 1, attackRange);
            AddRange(0, 1, blockRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("335354__littlerainyseasons__magic");
        }

        public override void SkillAdjustPowerChange(int value, CardSpriteBehaviour card, CardSpriteBehaviour spellSource)
        {
            if (card.CardStatus.Power > 0) return;
            card.ResetPower();
            Alignment buffTurn = card.Grid.Turn.CurrentAlignment;
            if (spellSource != null) buffTurn = spellSource.OccupiedField.Align;
            card.Grid.Turn.ExecutePrincessTurn(buffTurn);
        }
    }
}