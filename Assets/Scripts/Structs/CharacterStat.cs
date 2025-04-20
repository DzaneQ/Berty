using Berty.Characters.Data;

namespace Berty.Structs
{
    public struct CharacterStat
    {
        //private int strength;
        //private int power;
        //private int dexterity;
        //private int health;
        private int[] currentStat;
        private int[] currentTempStatBonus;
        private int[] nextTempStatBonus;
        private bool hasAttacked;
        private bool isTired;
        public int Strength { get => Stat(currentStat[0] + currentTempStatBonus[0]); set => currentStat[0] = Stat(value); }
        public int Power { get => Stat(currentStat[1] + currentTempStatBonus[1]); set => currentStat[1] = Stat(value); }
        public int Dexterity { get => Stat(currentStat[2] + currentTempStatBonus[2]); set => currentStat[2] = Stat(value); }
        public int Health { get => Stat(currentStat[3] + currentTempStatBonus[3]); set => currentStat[3] = Stat(value); }
        public int TempStrength { get => currentTempStatBonus[0]; set { currentTempStatBonus[0] = value; nextTempStatBonus[0] = value; } }
        public int TempPower { get => currentTempStatBonus[1]; set { currentTempStatBonus[1] = value; nextTempStatBonus[1] = value; } }
        public int TempDexterity { get => currentTempStatBonus[2]; set { currentTempStatBonus[2] = value; nextTempStatBonus[2] = value; } }
        public int TempHealth { get => currentTempStatBonus[3]; set { currentTempStatBonus[3] = value; nextTempStatBonus[3] = value; } }
        public bool HasAttacked { get => hasAttacked; set => hasAttacked = value; }
        public bool IsTired { get => isTired; set => isTired = value; }
        public int[] CurrentTempStatBonus { get => currentTempStatBonus; set => currentTempStatBonus = value; }
        public int[] NextTempStatBonus { get => nextTempStatBonus; set => nextTempStatBonus = value; }

        public CharacterStat(CharacterConfig character)
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
            nextTempStatBonus = new int[4];
        }

        private int Stat(int value)
        {
            if (value <= 0) return 0;
            if (value >= 6) return 6;
            return value;
        }
    }
}