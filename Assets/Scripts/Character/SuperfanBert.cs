using System;
public class SuperfanBert : Character
{
    public SuperfanBert()
    {
        AddName("superfan bert");
        AddProperties(Gender.Kid, Role.Agile);
        AddStats(1, 2, 5, 2);
        AddRange(1, 2, attackRange);
        AddRange(-1, 2, attackRange);
        //AddRange(1, 1, riposteRange);
        AddRange(1, 0, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        AddRange(-1, 0, riposteRange);
        //AddRange(-1, 1, riposteRange);
    }

    public override void SkillOnNewCard(CardSprite card)
    {
        int hour = DateTime.Now.Hour;
        UnityEngine.Debug.Log($"Current hour is: {hour}");
        if (hour < 5 || 18 <= hour)
        {
            card.AdvanceStrength(1, card);
            card.AdvancePower(2, card);
        }
        SkillOnMove(card);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        if (!card.IsAllied(target.OccupiedField)) return;
        target.AdvancePower(1, card);
        target.AddResistance(this);
    }

    public override void SkillOnMove(CardSprite card)
    {
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }
}
