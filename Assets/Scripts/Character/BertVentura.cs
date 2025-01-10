using System.Collections.Generic;

public class BertVentura : Character
{
    int opponentNeighborCount;

    public BertVentura()
    {
        AddName("bert ventura");
        AddProperties(Gender.Male, Role.Offensive);
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

    public override void SkillOnNewCard(CardSprite card)
    {
        opponentNeighborCount = 0;
        AdjustToNeighbors(card);
        //SkillOnNeighbor(card, card);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        AdjustToNeighbors(card);
        //if (card.GetAdjacentCards().Count < 2) return;
        //card.AdvanceStrength(1, card);
    }

    public override void SkillOnMove(CardSprite card) => AdjustToNeighbors(card);

    private void AdjustToNeighbors(CardSprite card)
    {
        int count = CountOpponentNeighbors(card);
        UpdateStrength(card, count);
    }

    private int CountOpponentNeighbors(CardSprite card)
    {
        int count = 0;
        List<CardSprite> neighbors = card.GetAdjacentCards();
        foreach (CardSprite neighbor in neighbors) if (!neighbor.IsAllied(card.OccupiedField)) count++;
        return count;
    }

    private void UpdateStrength(CardSprite card, int newCount)
    {
        int oldBonus = opponentNeighborCount / 2;
        int newBonus = newCount / 2;
        card.AdvanceStrength(newBonus - oldBonus);
        opponentNeighborCount = newCount;
    }
}
