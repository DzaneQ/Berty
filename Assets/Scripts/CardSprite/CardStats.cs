using Berty.Characters.Data;
using Berty.Entities;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.CardSprite
{
    public class CardStats
    {
        private Dictionary<Stat, int> baseStat;
        private Dictionary<Stat, int> currentTempStat;
        private Dictionary<Stat, int> nextTempStat;

        public int Strength 
        { 
            get => GetStat(baseStat[Stat.Strength] + TempStrength); 
            set { baseStat[Stat.Strength] = GetStat(value); } 
        }
        public int Power
        {
            get => GetStat(baseStat[Stat.Power] + TempPower);
            set { baseStat[Stat.Power] = GetStat(value); }
        }
        public int Dexterity
        {
            get => GetStat(baseStat[Stat.Dexterity] + TempDexterity);
            set { baseStat[Stat.Dexterity] = GetStat(value); }
        }
        public int Health
        {
            get => GetStat(baseStat[Stat.Health] + TempHealth);
            set { baseStat[Stat.Health] = GetStat(value); }
        }

        public int TempStrength
        {
            get => currentTempStat[Stat.Strength];
            set { currentTempStat[Stat.Strength] = value; nextTempStat[Stat.Strength] = value; }
        }
        public int TempPower
        {
            get => currentTempStat[Stat.Power];
            set { currentTempStat[Stat.Power] = value; nextTempStat[Stat.Power] = value; }
        }
        public int TempDexterity
        {
            get => currentTempStat[Stat.Dexterity];
            set { currentTempStat[Stat.Dexterity] = value; nextTempStat[Stat.Dexterity] = value; }
        }
        public int TempHealth
        {
            get => currentTempStat[Stat.Health];
            set { currentTempStat[Stat.Health] = value; nextTempStat[Stat.Health] = value; }
        }

        public CardStats(CharacterConfig character)
        {
            baseStat = new Dictionary<Stat, int>
            {
                { Stat.Strength, character.Strength },
                { Stat.Power, character.Power },
                { Stat.Dexterity, character.Dexterity },
                { Stat.Health, character.Health }
            };

            currentTempStat = InitZeroStat();
            nextTempStat = InitZeroStat();
        }

        public void HandleNewTurn()
        {
            ProgressTempStats();
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

        private void ProgressTempStats()
        {
            currentTempStat = new Dictionary<Stat, int>(nextTempStat);
            nextTempStat = nextTempStat.ToDictionary(keyValue => keyValue.Key, keyValue => 0);
        }

        private int GetStat(int value)
        {
            return Math.Clamp(value, 0, 6);
        }

        private Dictionary<Stat, int> InitZeroStat()
        {
            return new Dictionary<Stat, int>
            {
                { Stat.Strength, 0 },
                { Stat.Power, 0 },
                { Stat.Dexterity, 0 },
                { Stat.Health, 0 }
            };
        }
    }
}
