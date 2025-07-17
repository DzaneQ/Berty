using Berty.BoardCards;
using Berty.Enums;
using System.Collections.Generic;

namespace Berty.BoardCards.ConfigData
{
    public class BertVentura : CharacterConfig
    {
        int opponentNeighborCount;

        public BertVentura()
        {
            AddName("bert ventura");
            AddProperties(GenderEnum.Male, RoleEnum.Offensive);
            AddStats(0, 2, 4, 5);
            AddRange(0, 1, attackRange);
            AddRange(1, 0, attackRange);
            AddRange(-1, 0, attackRange);
            AddRange(0, 1, riposteRange);
            //AddRange(1, 1, riposteRange);
            AddRange(1, 0, riposteRange);
            //AddRange(1, -1, riposteRange);
            AddRange(0, -1, riposteRange);
            //AddRange(-1, -1, riposteRange);
            AddRange(-1, 0, riposteRange);
            //AddRange(-1, 1, riposteRange);
            AddSoundEffect("759965__thekingofgeeks360__parrots-goffins-cockatoo-squawk");
        }

        public override void SkillOnNewCard(CardSpriteBehaviour card)
        {
            opponentNeighborCount = 0;
            AdjustToNeighbors(card);
            //SkillOnNeighbor(card, card);
        }

        public override void SkillOnNeighbor(CardSpriteBehaviour card, CardSpriteBehaviour target)
        {
            AdjustToNeighbors(card);
            //if (card.GetAdjacentCards().Count < 2) return;
            //card.AdvanceStrength(1, card);
        }

        public override void SkillOnMove(CardSpriteBehaviour card) => AdjustToNeighbors(card);

        private void AdjustToNeighbors(CardSpriteBehaviour card)
        {
            int count = CountOpponentNeighbors(card);
            UpdateStrength(card, count);
        }

        private int CountOpponentNeighbors(CardSpriteBehaviour card)
        {
            int count = 0;
            List<CardSpriteBehaviour> neighbors = card.GetAdjacentCards();
            foreach (CardSpriteBehaviour neighbor in neighbors) if (!neighbor.IsAllied(card.OccupiedField)) count++;
            return count;
        }

        private void UpdateStrength(CardSpriteBehaviour card, int newCount)
        {
            int oldBonus = opponentNeighborCount / 2;
            int newBonus = newCount / 2;
            card.AdvanceStrength(newBonus - oldBonus);
            opponentNeighborCount = newCount;
        }
    }
}