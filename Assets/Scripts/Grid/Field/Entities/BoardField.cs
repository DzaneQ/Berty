using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Grid.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Field.Entities
{
    [Serializable]
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
        public BoardGrid Grid { get; }

        public BoardField(int x, int y, BoardGrid grid)
        {
            Coordinates = new Vector2Int(x, y);
            Align = AlignmentEnum.None;
            Grid = grid;
        }

        public BoardField(BoardFieldSaveData data, List<CharacterConfig> allCharacters, BoardGrid grid)
        {
            Coordinates = data.Coordinates;
            Align = data.Align;
            if (data.OccupantCard.CharacterName != "") OccupantCard = new BoardCard(data.OccupantCard, allCharacters, this);
            if (data.BackupCard.CharacterName != "") BackupCard = new BoardCard(data.BackupCard, allCharacters, this);
            Grid = grid;
        }

        public BoardFieldSaveData SaveEntity()
        {
            return new()
            {
                OccupantCard = this.OccupantCard != null ? this.OccupantCard.SaveEntity() : new(),
                BackupCard = this.BackupCard != null ? this.BackupCard.SaveEntity() : new(),
                Align = this.Align,
                Coordinates = this.Coordinates
            };
        }

        public BoardCard AddNewCard(CharacterConfig characterConfig, AlignmentEnum newAlign)
        {
            if (OccupantCard == null)
            {
                OccupantCard = new BoardCard(characterConfig, this);
                Align = newAlign;
            }
            else
            {
                if (Align != newAlign) throw new InvalidOperationException($"Putting extra card on field named {GetName()} should not change align!");
                if (BackupCard != null) throw new InvalidOperationException($"Field named {GetName()} is full!");
                BackupCard = OccupantCard;
                OccupantCard = new BoardCard(characterConfig, this);
                OccupantCard.SetDirection(BackupCard.Direction);
            }
            return OccupantCard;
        }

        public void PlaceExistingCard(BoardCard card, AlignmentEnum newAlign)
        {
            if (card == null) throw new InvalidOperationException($"Should not place null card for {GetName()}.");
            OccupantCard = card;
            card.SetField(this);
            Align = newAlign;
        }

        public void SetBackupCard(BoardCard card)
        {
            if (OccupantCard == null) throw new InvalidOperationException($"Field {GetName()} should be occupied before setting a backup card.");
            BackupCard = card;
            if (BackupCard != null) card.SetField(this);
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

        public bool AreThereTwoCards() => BackupCard != null;

        public void SwitchSides()
        {
            if (Align == AlignmentEnum.Player) Align = AlignmentEnum.Opponent;
            else if (Align == AlignmentEnum.Opponent) Align = AlignmentEnum.Player;
            else throw new Exception($"Can't switch sides for field {GetName()}");
        }

        public bool IsOccupied()
        {
            return OccupantCard != null;
        }

        public void SynchronizeBackupCardRotation()
        {
            if (BackupCard == null) return;
            BackupCard.SetDirection(OccupantCard.Direction);
        }

        private string GetName()
        {
            return $"Card {Coordinates[0]}, {Coordinates[1]}";
        }
    }

    [Serializable]
    public struct BoardFieldSaveData
    {
        public BoardCardSaveData OccupantCard;
        public BoardCardSaveData BackupCard;
        public AlignmentEnum Align;
        public Vector2Int Coordinates;
    }
}
