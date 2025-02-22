﻿public class BertWho : Character
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

    //public override void SkillOnNewTurn(CardSprite card)
    //{
    //    if (turnCounter <= 0) return;
    //    turnCounter--;
    //    card.CardManager.PullCard(card.Grid.Turn.CurrentAlignment);
    //    card.CardManager.PullCard(card.Grid.Turn.CurrentAlignment);
    //}

    public override void SkillOnNewCard(CardSprite card)
    {
        card.Grid.AddCardIntoQueue(Alignment.Player);
        card.Grid.AddCardIntoQueue(Alignment.Player);
        card.Grid.AddCardIntoQueue(Alignment.Opponent);
        card.Grid.AddCardIntoQueue(Alignment.Opponent);
        //card.Grid.AcceptCharacterGlobalState(this);
        //card.Grid.MoreCardsNextTurns(2);
        //turnCounter = 2;
        foreach (CardSprite adjCard in card.GetAdjacentCards()) SkillOnNeighbor(card, adjCard);
    }

    public override void SkillOnNeighbor(CardSprite card, CardSprite target)
    {
        target.AdvancePower(-1, card);
        target.AddResistance(this);
    }

    public override void SkillOnMove(CardSprite card) => SkillOnNewCard(card);

    public override void SkillOnDeath(CardSprite card)
    {
        card.ReturnCharacter();
    }
}
