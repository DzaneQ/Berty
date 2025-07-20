using Berty.BoardCards.Entities;
using Berty.Enums;
using Berty.Grid.Entities;
using Berty.Grid.Field;
using Berty.Grid.Field.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Grid.Entities
{
    public class BoardGrid
    {
        public BoardField[] Fields { get; private set; }

        public GlobalEffects GlobalEffects { get; private set; }

        public BoardGrid()
        {
            Fields = new BoardField[9];
            int index = 0;
            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                {
                    Fields[index] = new BoardField(x, y);
                    index++;
                }
        }

        public BoardField GetFieldFromCoordsOrThrow(int x, int y)
        {
            foreach (BoardField field in Fields)
            {
                if (field.Coordinates.x != x) continue;
                if (field.Coordinates.y != y) continue;
                //Debug.Log("Got x=" + x + "; y=" + y);
                return field;
            }
            //Debug.Log("Got x=" + x + "; y=" + y + "as null!");
            throw new ArgumentException($"Invalid coordination field: ({x}, {y})");
        }

        public Vector2Int GetToRelativeCoordinates(int x, int y, float angle = 0)
        {
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            return new Vector2Int(cosinus * x - sinus * y, cosinus * y + sinus * x);
        }

        public Vector2Int GetFromRelativeCoordinates(int x, int y, float angle = 0)
        {
            return GetToRelativeCoordinates(x, y, -angle);
        }

        public BoardField GetRelativeFieldOrThrow(int x, int y, float angle = 0)
        {
            Vector2Int relCoord = GetToRelativeCoordinates(x, y, angle);
            return GetFieldFromCoordsOrThrow(relCoord.x, relCoord.y);
        }

        public BoardField GetFieldFromRelativeCoordinatesOrNull(int x, int y, float angle = 0)
        {
            if (Math.Abs(x) > 1 || Math.Abs(y) > 1) return null;
            Vector2Int coord = GetFromRelativeCoordinates(x, y, angle);
            return GetFieldFromCoordsOrThrow(coord.x, coord.y);
        }

        public BoardField GetFieldDistancedFromCardOrNull(int x, int y, BoardCard card)
        {
            Vector2Int relCoord = card.RelativeCoordinates;
            return GetFieldFromRelativeCoordinatesOrNull(relCoord.x + x, relCoord.y + y, card.GetAngle());
        }

        public BoardField GetFieldDistancedFromCardOrThrow(int x, int y, BoardCard card)
        {
            return GetFieldDistancedFromCardOrNull(x, y, card) ?? throw new Exception($"There is not field at distance ({x},{y}) away from {card.CharacterConfig.Name}");
        }

        public List<BoardField> AlignedFields(AlignmentEnum alignment, bool countBackup = false)
        {
            List<BoardField> alignedFields = new List<BoardField>();
            foreach (BoardField field in Fields)
            {
                if (!field.IsAligned(alignment)) continue;
                alignedFields.Add(field);
                if (countBackup && field.AreThereTwoCards()) alignedFields.Add(field);
            }
            return alignedFields;
        }

        private int AlignedCardCount(AlignmentEnum alignment)
        {
            return AlignedFields(alignment, true).Count;
        }

        public AlignmentEnum WinningSide()
        {
            if (AlignedCardCount(AlignmentEnum.Player) > AlignedCardCount(AlignmentEnum.Opponent)) return AlignmentEnum.Player;
            if (AlignedCardCount(AlignmentEnum.Player) < AlignedCardCount(AlignmentEnum.Opponent)) return AlignmentEnum.Opponent;
            return HigherByAmountOfType();
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
}
