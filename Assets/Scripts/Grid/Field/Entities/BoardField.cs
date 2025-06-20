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
                Align = Alignment.None;
                if (BackupCard != null) throw new Exception($"BackupCard shouldn't exist without OccupantCard in {GetName()}!");
            }
        }
        public BoardCard BackupCard { get; private set; }
        public Alignment Align { get; private set; }
        public Vector2Int Coordinates { get; }

        public BoardField(int x, int y)
        {
            Coordinates = new Vector2Int(x, y);
            Align = Alignment.None;
        }

        public void AddCard(CharacterConfig characterConfig)
        {
            if (OccupantCard == null)
            {
                OccupantCard = new BoardCard(characterConfig);
            }
            else
            {
                if (BackupCard != null) throw new InvalidOperationException($"Field named {GetName()} is full!");
                BackupCard = OccupantCard;
                OccupantCard = new BoardCard(characterConfig);
            }
        }

        public void PlaceCard(BoardCard card, Alignment newAlign)
        {
            OccupantCard = card;
            Align = newAlign;
        }

        public void RemoveCard()
        {
            OccupantCard = BackupCard;
            BackupCard = null;
        }

        public void RemoveAllCards()
        {
            BackupCard = null;
            OccupantCard = null;      
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
            if (Align == Alignment.Player) Align = Alignment.Opponent;
            else if (Align == Alignment.Opponent) Align = Alignment.Player;
            else throw new Exception($"Can't switch sides for field {GetName()}");
        }

        public bool IsAligned(Alignment alignment)
        {
            return Align == alignment;
        }

        public bool IsOpposed(Alignment alignment)
        {
            if (alignment == Alignment.None) return false;
            if (Align == Alignment.None) return false;
            return Align != alignment;
        }

        public bool IsOccupied()
        {
            if (Align != Alignment.None) return true;
            return false;
        }

        private string GetName()
        {
            return $"Card {Coordinates[0]}, {Coordinates[1]}";
        }
    }
}
