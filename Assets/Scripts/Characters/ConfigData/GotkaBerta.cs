using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class GotkaBerta : CharacterConfig
    {
        public GotkaBerta()
        {
            AddName("gotka berta");
            AddProperties(Gender.Female, Role.Support);
            AddStats(1, 4, 5, 4);
            AddRange(0, 1, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("467777__sgak__thunder");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            card.Grid.InitiateResurrection();
            card.Grid.MakeAllStatesIdle(card.OccupiedField);
        }

        public override void SkillSideClick(CardSpriteBehaviour card)
        {
            //if (card.Grid.CurrentStatus.Resurrection != Alignment.None) return;
            SkillOnNewCard(card);
            card.SetIdle();
            card.DeactivateCard();
        }
    }
}