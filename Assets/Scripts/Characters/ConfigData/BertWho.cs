using Berty.BoardCards;
using Berty.Enums;

namespace Berty.BoardCards.ConfigData
{
    public class BertWho : CharacterConfig
    {
        //int turnCounter = 0;

        public BertWho()
        {
            AddName("bert who");
            AddProperties(Gender.Male, Role.Support);
            AddStats(1, 3, 5, 4);
            AddRange(0, 1, attackRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, blockRange);
            //AddRange(1, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, blockRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("189575__unopiate__breaking-glass");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            card.Grid.AddCardIntoQueue(Alignment.Player);
            card.Grid.AddCardIntoQueue(Alignment.Player);
            card.Grid.AddCardIntoQueue(Alignment.Opponent);
            card.Grid.AddCardIntoQueue(Alignment.Opponent);
            foreach (CardSpriteBehaviour adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            target.AdvancePower(-1, card);
            target.AddResistance(this);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => SkillOnNewCard(card);

        public override void SkillOnDeath(CardSpriteBehaviour card)
        {
            card.ReturnCharacter();
        }
    }
}