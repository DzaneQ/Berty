public struct CharacterStat
{
    private int strength;
    private int power;
    private int dexterity;
    private int health;
    public int Strength { get => strength; set => strength = SetStat(value); }
    public int Power { get => power; set => power = SetStat(value); }
    public int Dexterity { get => dexterity; set => dexterity = SetStat(value); }
    public int Health { get => health; set => health = SetStat(value); }
    public bool hasAttacked;
    public bool isTired;

    public CharacterStat(Character character)
    {
        strength = character.Strength;
        power = character.Power;
        dexterity = character.Dexterity;
        health = character.Health;
        hasAttacked = false;
        isTired = false;
    }

    private int SetStat(int value)
    {
        if (value <= 0) return 0;
        if (value >= 6) return 6;
        return value;
    }

    //public bool CanAttack()
    //{
    //    if (Strength == 0 || !hasAttacked) return false;
    //    return true;
    //}

    //public void ResetAttack()
    //{
    //    hasAttacked = true;
    //}

    //public void DisableAttack()
    //{
    //    hasAttacked = false;
    //}

    //public void AffectWill(int value)
    //{
    //    Power += value;
    //}

    //public bool IsMindless()
    //{
    //    if (Power <= 0) return true;
    //    return false;
    //}

    //public void RestorePower()
    //{
    //    Power = initPower;
    //}

    //public void BoostDexterity(int value)
    //{
    //    Dexterity += value;
    //}

    //public bool isTired()
    //{
    //    if (Dexterity == 0) isTired = true;
    //    if (Dexterity >= initDexterity) isTired = false;
    //    return isTired;
    //}

    //public void TakeDamage(int damage)
    //{
    //    Health -= damage;
    //}

    //public bool IsDead()
    //{
    //    if (Health <= 0) return true;
    //    return false;
    //}
}