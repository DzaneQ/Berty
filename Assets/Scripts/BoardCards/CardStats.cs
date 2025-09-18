using Berty.BoardCards.ConfigData;
using Berty.Grid.Entities;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.BoardCards
{
    public class CardStats
    {
        private Dictionary<StatEnum, int> baseStat;
        private Dictionary<StatEnum, int> currentTempStat;
        private Dictionary<StatEnum, int> nextTempStat;

        public int Strength 
        { 
            get => GetStat(baseStat[StatEnum.Strength] + TempStrength); 
            set { baseStat[StatEnum.Strength] = GetStat(value); } 
        }
        public int Power
        {
            get => GetStat(baseStat[StatEnum.Power] + TempPower);
            set { baseStat[StatEnum.Power] = GetStat(value); }
        }
        public int Dexterity
        {
            get => GetStat(baseStat[StatEnum.Dexterity] + TempDexterity);
            set { baseStat[StatEnum.Dexterity] = GetStat(value); }
        }
        public int Health
        {
            get => GetStat(baseStat[StatEnum.Health] + TempHealth);
            set { baseStat[StatEnum.Health] = GetStat(value); }
        }

        public int TempStrength
        {
            get => currentTempStat[StatEnum.Strength];
            set { nextTempStat[StatEnum.Strength] = value; }
        }
        public int TempPower
        {
            get => currentTempStat[StatEnum.Power];
            set { nextTempStat[StatEnum.Power] = value; }
        }
        public int TempDexterity
        {
            get => currentTempStat[StatEnum.Dexterity];
            set { nextTempStat[StatEnum.Dexterity] = value; }
        }
        public int TempHealth
        {
            get => currentTempStat[StatEnum.Health];
            set { nextTempStat[StatEnum.Health] = value; }
        }

        public CardStats(CharacterConfig character)
        {
            baseStat = new Dictionary<StatEnum, int>
            {
                { StatEnum.Strength, character.Strength },
                { StatEnum.Power, character.Power },
                { StatEnum.Dexterity, character.Dexterity },
                { StatEnum.Health, character.Health }
            };

            currentTempStat = InitZeroStat();
            nextTempStat = InitZeroStat();
        }

        /*public void AdvanceStrength(int value)
        {
            Strength += value;
        }

        public void AdvanceTempStrength(int value)
        {
            TempStrength += value;
        }

        public void AdvancePower(int value)
        {
            Power += value;
        }

        public void AdvanceTempPower(int value)
        {
            TempPower += value;
        }

        public void AdvanceDexterity(int value)
        {
            Dexterity += value;
        }

        public void AdvanceTempDexterity(int value)
        {
            TempDexterity += value;
        }

        public void AdvanceHealth(int value)
        {
            Health += value;
        }

        public void AdvanceTempHealth(int value)
        {
            TempHealth += value;
        }*/

        public void ProgressTempStats()
        {
            currentTempStat = new Dictionary<StatEnum, int>(nextTempStat);
            nextTempStat = nextTempStat.ToDictionary(keyValue => keyValue.Key, keyValue => 0);
        }

        public bool AreTempStatZeros()
        {
            return currentTempStat.Values.All(x => x == 0) && nextTempStat.Values.All(x => x == 0);
        }

        private int GetStat(int value)
        {
            return Math.Clamp(value, 0, 6);
        }

        private Dictionary<StatEnum, int> InitZeroStat()
        {
            return new Dictionary<StatEnum, int>
            {
                { StatEnum.Strength, 0 },
                { StatEnum.Power, 0 },
                { StatEnum.Dexterity, 0 },
                { StatEnum.Health, 0 }
            };
        }
    }
}
