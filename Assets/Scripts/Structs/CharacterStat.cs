public struct CharacterStat
{
    //private int strength;
    //private int power;
    //private int dexterity;
    //private int health;
    private int[] currentStat;
    private int[] currentTempStatBonus;
    //private int[] futureTempStatBonus;
    public int Strength { get => Stat(currentStat[0] + currentTempStatBonus[0]); set => currentStat[0] = Stat(value); }
    public int Power { get => Stat(currentStat[1] + currentTempStatBonus[1]); set => currentStat[1] = Stat(value); }
    public int Dexterity { get => Stat(currentStat[2] + currentTempStatBonus[2]); set => currentStat[2] = Stat(value); }
    public int Health { get => Stat(currentStat[3] + currentTempStatBonus[3]); set => currentStat[3] = Stat(value); }
    public int TempStrength { set => currentTempStatBonus[0] = value; }
    public int TempPower { set => currentTempStatBonus[1] = value; }
    public int TempDexterity { set => currentTempStatBonus[2] = value; }
    public int TempHealth { set => currentTempStatBonus[3] = value; }
    public bool hasAttacked;
    public bool isTired;
    public int[] CurrentTempStatBonus { get => currentTempStatBonus; set => currentTempStatBonus = value; }
    //public int[] FutureTempStatBonus { get => futureTempStatBonus; set => futureTempStatBonus = value; }

    public CharacterStat(Character character)
    {
        //strength = character.Strength;
        //power = character.Power;
        //dexterity = character.Dexterity;
        //health = character.Health;
        currentStat = new int[4];
        currentStat[0] = character.Strength;
        currentStat[1] = character.Power;
        currentStat[2] = character.Dexterity;
        currentStat[3] = character.Health;
        hasAttacked = false;
        isTired = false;
        currentTempStatBonus = new int[4];
        //futureTempStatBonus = new int[4];
    }

    private int Stat(int value)
    {
        if (value <= 0) return 0;
        if (value >= 6) return 6;
        return value;
    }
}