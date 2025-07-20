using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Field.Entities
{
    public class BoardField
    {
        private BoardCard _occupantCard;
        public BoardCard OccupantCard
        {
            get => _occupantCard;
            private set
            {
                _occupantCard = value;
                if (_occupantCard != null) return;
                Align = AlignmentEnum.None;
                if (BackupCard != null) throw new Exception($"BackupCard shouldn't exist without OccupantCard in {GetName()}!");
            }
        }
        public BoardCard BackupCard { get; private set; }
        public AlignmentEnum Align { get; private set; }
        public Vector2Int Coordinates { get; }

        public BoardField(int x, int y)
        {
            Coordinates = new Vector2Int(x, y);
            Align = AlignmentEnum.None;
        }

        public BoardCard AddCard(CharacterConfig characterConfig, AlignmentEnum newAlign)
        {
            if (OccupantCard == null)
            {
                OccupantCard = new BoardCard(characterConfig, this);
                Align = newAlign;
            }
            else
            {
                if (BackupCard != null) throw new InvalidOperationException($"Field named {GetName()} is full!");
                BackupCard = OccupantCard;
                OccupantCard = new BoardCard(characterConfig, this);
            }
            return OccupantCard;
        }

        public void PlaceCard(BoardCard card, AlignmentEnum newAlign)
        {
            OccupantCard = card;
            card.SetField(this);
            Align = newAlign;
        }

        public void RemoveCard()
        {
            OccupantCard = BackupCard;
            BackupCard = null;
            if (OccupantCard == null) Align = AlignmentEnum.None;
        }

        public void RemoveAllCards()
        {
            BackupCard = null;
            OccupantCard = null;
            Align = AlignmentEnum.None;
        }

        public void PlaceBackupCard(BoardCard card)
        {
            BackupCard = OccupantCard;
            OccupantCard = card;
            card.PlaceCard(this, BackupCard.Direction);
        }

        public bool AreThereTwoCards() => BackupCard != null;

        public void SwitchSides()
        {
            if (Align == AlignmentEnum.Player) Align = AlignmentEnum.Opponent;
            else if (Align == AlignmentEnum.Opponent) Align = AlignmentEnum.Player;
            else throw new Exception($"Can't switch sides for field {GetName()}");
        }

        public bool IsAligned(AlignmentEnum alignment)
        {
            return Align == alignment;
        }

        public bool IsOpposed(AlignmentEnum alignment)
        {
            if (alignment == AlignmentEnum.None) return false;
            if (Align == AlignmentEnum.None) return false;
            return Align != alignment;
        }

        public bool IsOccupied()
        {
            if (Align != AlignmentEnum.None) return true;
            return false;
        }

        private string GetName()
        {
            return $"Card {Coordinates[0]}, {Coordinates[1]}";
        }
    }
}
