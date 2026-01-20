using Berty.BoardCards.ConfigData;
using Berty.UI.Card.Entities;
using Berty.Grid.Entities;
using Berty.Enums;
using Berty.Gameplay.ConfigData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Berty.BoardCards.Entities;

namespace Berty.Gameplay.Entities
{
    public class Status
    {
        public StatusEnum Name { get; }
        public BoardCard Provider { get; }
        private AlignmentEnum TargetAlign { get; }
        public int Charges { get; private set; }

        public Status(StatusEnum name, BoardCard provider, int charges = 0)
        {
            Name = name;
            Provider = provider;
            TargetAlign = AlignmentEnum.None;
            Charges = charges;
        }

        public Status(StatusEnum name, BoardCard provider, AlignmentEnum targetAlign)
        {
            Name = name;
            Provider = provider;
            TargetAlign = targetAlign;
        }

        public Status(StatusEnum name, AlignmentEnum align, int charges = 0)
        {
            Name = name;
            TargetAlign = align;
            Charges = charges;
        }

        public Status(StatusSaveData data, BoardGrid grid)
        {
            Name = data.Name;
            Provider = grid.FindCardByNameOrThrow(data.ProviderName);
            TargetAlign = data.TargetAlign;
            Charges = data.Charges;
        }

        public StatusSaveData SaveEntity()
        {
            return new()
            {
                Name = Name,
                ProviderName = Provider.CharacterConfig.Name,
                TargetAlign = TargetAlign,
                Charges = Charges
            };
        }

        public AlignmentEnum GetAlign()
        {
            if (Provider != null) return Provider.Align;
            return TargetAlign;
        }

        public AlignmentEnum GetTargetAlign()
        {
            return TargetAlign;
        }

        public void SetCharges(int charges)
        {
            Charges = charges;
        }

        public void IncrementCharges(int delta)
        {
            Charges += delta;
        }
    }

    [Serializable]
    public struct StatusSaveData
    {
        public StatusEnum Name;
        public string ProviderName;
        public AlignmentEnum TargetAlign;
        public int Charges;
    }
}