using Berty.BoardCards.ConfigData;
using Berty.Grid.Entities;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Berty.Gameplay.Entities;

namespace Berty.BoardCards.Entities
{
    public class CardStats
    {
        private BoardCard BoardCard { get; }
        private Dictionary<StatEnum, int> baseStat;
        private Dictionary<StatEnum, int> currentTempStat;
        private Dictionary<StatEnum, int> nextTempStat;

        public int Strength 
        { 
            get => GetStat(baseStat[StatEnum.Strength] + TempStrength + StrengthBonus()); 
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

        public CardStats(BoardCard card)
        {
            BoardCard = card;

            baseStat = new Dictionary<StatEnum, int>
            {
                { StatEnum.Strength, card.CharacterConfig.Strength },
                { StatEnum.Power, card.CharacterConfig.Power },
                { StatEnum.Dexterity, card.CharacterConfig.Dexterity },
                { StatEnum.Health, card.CharacterConfig.Health }
            };

            currentTempStat = InitZeroStat();
            nextTempStat = InitZeroStat();
        }

        public CardStats(CardStatsSaveData data, BoardCard card)
        {
            BoardCard = card;

            baseStat = GetStatDictionary(data.BaseStat);
            currentTempStat = GetStatDictionary(data.CurrentTempStat);
            nextTempStat = GetStatDictionary(data.NextTempStat);
        }

        public CardStatsSaveData SaveEntity()
        {
            return new()
            {
                BaseStat = GetStatArray(baseStat),
                CurrentTempStat = GetStatArray(currentTempStat),
                NextTempStat = GetStatArray(nextTempStat)
            };
        }

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

        private int StrengthBonus()
        {
            Status ownedStatus = BoardCard.OccupiedField.Grid.Game.GetStatusFromProviderOrNull(BoardCard);
            if (ownedStatus?.Name == StatusEnum.Ventura) return ownedStatus.Charges / 2;
            return 0;
        }

        private int[] GetStatArray(Dictionary<StatEnum, int> stat)
        {
            return new int[4] {
                stat[StatEnum.Strength],
                stat[StatEnum.Power],
                stat[StatEnum.Dexterity],
                stat[StatEnum.Health],
            };
        }

        private Dictionary<StatEnum, int> GetStatDictionary(int[] statData)
        {
            return new Dictionary<StatEnum, int>
            {
                { StatEnum.Strength, statData[0] },
                { StatEnum.Power, statData[1] },
                { StatEnum.Dexterity, statData[2] },
                { StatEnum.Health, statData[3] }
            };
        }
    }

    [Serializable]
    public struct CardStatsSaveData
    {
        public int[] BaseStat;
        public int[] CurrentTempStat;
        public int[] NextTempStat;
    }
}
