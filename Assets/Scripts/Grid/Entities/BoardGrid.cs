using Berty.BoardCards.ConfigData;
using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Gameplay.Entities;
using Berty.Grid.Entities;
using Berty.Grid.Field;
using Berty.Grid.Field.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Berty.Grid.Entities
{
    public class BoardGrid
    {
        public BoardField[] Fields { get; }

        public Game Game { get; }

        public BoardGrid(Game game)
        {
            Game = game;
            Fields = new BoardField[9];
            int index = 0;
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    Fields[index] = new BoardField(x, y, this);
                    index++;
                }
        }

        public BoardGrid(BoardGridSaveData data, List<CharacterConfig> allCharacters, Game game)
        {
            Game = game;
            Fields = new BoardField[9];
            for (int index = 0; index < 9; index++)
            {
                Fields[index] = new BoardField(data.Fields[index], allCharacters, this);
            }
        }

        public void OverwriteEntity(BoardGridSaveData data, List<CharacterConfig> allCharacters)
        {
            for (int index = 0; index < 9; index++)
            {
                Fields[index].OverwriteEntity(data.Fields[index], allCharacters);
            }
        }

        public BoardGridSaveData SaveEntity()
        {
            return new()
            {
                Fields = this.Fields.Select(field => field.SaveEntity()).ToArray()
            };
        }

        public BoardField GetFieldFromCoordsOrThrow(Vector2Int coords)
        {
            return Fields.FirstOrDefault(field => field.Coordinates == coords) ?? throw new ArgumentException($"Invalid coordination field: ({coords.x}, {coords.y})");
        }

        public Vector2Int GetToRelativeCoordinates(Vector2Int coords, float angle = 0)
        {
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            return new Vector2Int(cosinus * coords.x - sinus * coords.y, cosinus * coords.y + sinus * coords.x);
        }

        public Vector2Int GetFromRelativeCoordinates(Vector2Int coords, float angle = 0)
        {
            return GetToRelativeCoordinates(coords, -angle);
        }

        public BoardField GetRelativeFieldOrThrow(Vector2Int fixedCoords, float angle = 0)
        {
            Vector2Int relCoords = GetToRelativeCoordinates(fixedCoords, angle);
            return GetFieldFromCoordsOrThrow(relCoords);
        }

        public BoardField GetFieldFromRelativeCoordinatesOrNull(Vector2Int relCoords, float angle = 0)
        {
            if (Math.Abs(relCoords.x) > 1 || Math.Abs(relCoords.y) > 1) return null;
            Vector2Int fixedCoords = GetFromRelativeCoordinates(relCoords, angle);
            return GetFieldFromCoordsOrThrow(fixedCoords);
        }

        public BoardField GetFieldDistancedFromCardOrNull(Vector2Int distance, BoardCard card)
        {
            Vector2Int relCoords = card.RelativeCoordinates;
            return GetFieldFromRelativeCoordinatesOrNull(relCoords + distance, card.GetAngle());
        }

        public BoardField GetFieldDistancedFromCardOrThrow(Vector2Int distance, BoardCard card)
        {
            return GetFieldDistancedFromCardOrNull(distance, card) ?? throw new Exception($"There is not field at distance ({distance.x},{distance.y}) away from {card.CharacterConfig.Name}");
        }

        public List<BoardCard> GetAllNeighbors(BoardCard card)
        {
            List<BoardCard> neighbors = new();
            for (int i = 0; i < 4; i++)
            {
                BoardField neighboringField = GetFieldDistancedFromCardOrNull(new Vector2Int(Mathf.RoundToInt(Mathf.Sin(i / 2f * Mathf.PI)), Mathf.RoundToInt(Mathf.Cos(i / 2f * Mathf.PI))), card);
                if (neighboringField == null || !neighboringField.IsOccupied()) continue;
                neighbors.Add(neighboringField.OccupantCard);
                if (neighboringField.BackupCard != null) neighbors.Add(neighboringField.BackupCard);
            }
            return neighbors;
        }

        public List<BoardCard> GetOccupantNeighbors(BoardCard card)
        {
            List<BoardCard> neighbors = new();
            for (int i = 0; i < 4; i++)
            {
                BoardField neighboringField = GetFieldDistancedFromCardOrNull(new Vector2Int(Mathf.RoundToInt(Mathf.Sin(i / 2f * Mathf.PI)), Mathf.RoundToInt(Mathf.Cos(i / 2f * Mathf.PI))), card);
                if (neighboringField == null || !neighboringField.IsOccupied()) continue;
                neighbors.Add(neighboringField.OccupantCard);
            }
            return neighbors;
        }

        public int GetEnemyNeighborCount(BoardCard card)
        {
            return GetAllNeighbors(card).Count(neighbor => neighbor.Align != card.Align);
        }

        public List<BoardField> GetFieldsInRange(BoardCard card, List<Vector2Int> range)
        {
            List<BoardField> fields = new List<BoardField>();
            foreach (Vector2Int distance in range)
            {
                BoardField target = GetFieldDistancedFromCardOrNull(distance, card);
                if (target != null) fields.Add(target);
            }
            return fields;
        }

        public List<BoardField> AlignedFields(AlignmentEnum alignment, bool countBackup = false)
        {
            List<BoardField> alignedFields = new List<BoardField>();
            foreach (BoardField field in Fields)
            {
                if (field.Align != alignment) continue;
                alignedFields.Add(field);
                if (countBackup && field.AreThereTwoCards()) alignedFields.Add(field);
            }
            return alignedFields;
        }

        public AlignmentEnum WinningSide()
        {
            if (AlignedCardCount(AlignmentEnum.Player) > AlignedCardCount(AlignmentEnum.Opponent)) return AlignmentEnum.Player;
            if (AlignedCardCount(AlignmentEnum.Player) < AlignedCardCount(AlignmentEnum.Opponent)) return AlignmentEnum.Opponent;
            return HigherByAmountOfType();
        }

        public bool AreNeighboring(BoardField firstField, BoardField secondField)
        {
            int xDiff = Mathf.Abs(firstField.Coordinates.x - secondField.Coordinates.x);
            int yDiff = Mathf.Abs(firstField.Coordinates.y - secondField.Coordinates.y);
            return xDiff + yDiff == 1;
        }

        public bool AreAligned(BoardField firstField, BoardField secondField)
        {
            return firstField.Align == secondField.Align;
        }

        public List<CharacterConfig> GetAllCharactersOnFields()
        {
            return Fields.SelectMany(field => new CharacterConfig[]{ field.OccupantCard?.CharacterConfig, field.BackupCard?.CharacterConfig }).OfType<CharacterConfig>().ToList();
        }

        public BoardCard FindCardByNameOrThrow(string characterName)
        {
            BoardField field = Fields.First(field => field.OccupantCard?.CharacterConfig.Name == characterName || field.BackupCard?.CharacterConfig.Name == characterName);
            if (field.OccupantCard.CharacterConfig.Name == characterName) return field.OccupantCard;
            if (field.BackupCard.CharacterConfig.Name == characterName) return field.BackupCard;
            throw new Exception("The code to find card by name shouldn't reach here.");
        }

        public BoardCard FindCardByCharacterNameOrNull(CharacterEnum characterName)
        {
            BoardField field = Fields.FirstOrDefault(field => field.OccupantCard?.CharacterConfig.CharacterName == characterName || field.BackupCard?.CharacterConfig.CharacterName == characterName);
            if (field == null) return null;
            if (field.OccupantCard.CharacterConfig.CharacterName == characterName) return field.OccupantCard;
            if (field.BackupCard.CharacterConfig.CharacterName == characterName) return field.BackupCard;
            throw new Exception("The code to find card by character name shouldn't reach here.");
        }

        public BoardCard FindCardByCharacterNameOrThrow(CharacterEnum characterName)
        {
            return FindCardByCharacterNameOrNull(characterName) ?? throw new Exception($"Card {characterName} has not been found.");
        }

        private int AlignedCardCount(AlignmentEnum alignment)
        {
            return AlignedFields(alignment, true).Count;
        }

        private AlignmentEnum HigherByAmountOfType()
        {
            if (HighestAmountOfType(AlignmentEnum.Player) > HighestAmountOfType(AlignmentEnum.Opponent)) return AlignmentEnum.Player;
            if (HighestAmountOfType(AlignmentEnum.Player) < HighestAmountOfType(AlignmentEnum.Opponent)) return AlignmentEnum.Opponent;
            return AlignmentEnum.None;
        }

        private int HighestAmountOfType(AlignmentEnum alignment)
        {
            return Mathf.Max(AmountOfType(alignment, RoleEnum.Offensive),
                AmountOfType(alignment, RoleEnum.Support),
                AmountOfType(alignment, RoleEnum.Agile),
                AmountOfType(alignment, RoleEnum.Special));
        }

        private int AmountOfType(AlignmentEnum alignment, RoleEnum role)
        {
            int result = 0;
            foreach (BoardField field in AlignedFields(alignment))
            {
                if (field.OccupantCard.CharacterConfig.Role == role) result++;
                if (role == RoleEnum.Offensive && field.AreThereTwoCards()) result++;
            }
            return result;
        }
    }

    [Serializable]
    public struct BoardGridSaveData
    {
        public BoardFieldSaveData[] Fields;
    }
}
