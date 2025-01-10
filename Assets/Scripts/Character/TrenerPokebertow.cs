public class TrenerPokebertow : Character
{
    public TrenerPokebertow()
    {
        AddName("trener pokebertow");
        AddProperties(Gender.Kid, Role.Offensive);
        AddStats(1, 2, 5, 2);
        AddRange(0, 1, attackRange);
        //AddRange(1, 1, riposteRange);
        //AddRange(1, -1, riposteRange);
        //AddRange(-1, -1, riposteRange);
        //AddRange(-1, 1, riposteRange);
        AddSoundEffect("453001__breviceps__pokemon-cry-parody");
    }

    public override void SkillCardClick(CardSprite card)
    {
        card.Grid.SetBackupCard(card.OccupiedField);
        card.CardManager.DeselectCards();
    }
}
