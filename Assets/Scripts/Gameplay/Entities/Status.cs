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
        private AlignmentEnum Align { get; }

        public Status(StatusEnum name, BoardCard provider)
        {
            Name = name;
            Provider = provider;
            Align = AlignmentEnum.None;
        }

        public Status(StatusEnum name, AlignmentEnum align)
        {
            Name = name;
            Align = align;
        }

        public AlignmentEnum GetAlign()
        {
            if (Provider != null) return Provider.Align;
            return Align;
        }
    }
}