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

        public Vector2Int GetRelativeCoordinates(int x, int y, float angle = 0)
        {
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            return new Vector2Int(cosinus * x + sinus * y, cosinus * y - sinus * x);
        }

        public BoardField GetRelativeFieldOrThrow(int x, int y, float angle = 0)
        {
            Vector2Int relCoord = GetRelativeCoordinates(x, y, angle);
            return GetFieldFromCoordsOrThrow(relCoord.x, relCoord.y);
        }

        public List<BoardField> AlignedFields(Alignment alignment, bool countBackup = false)
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

        private int AlignedCardCount(Alignment alignment)
        {
            return AlignedFields(alignment, true).Count;
        }

        public Alignment WinningSide()
        {
            if (AlignedCardCount(Alignment.Player) > AlignedCardCount(Alignment.Opponent)) return Alignment.Player;
            if (AlignedCardCount(Alignment.Player) < AlignedCardCount(Alignment.Opponent)) return Alignment.Opponent;
            return HigherByAmountOfType();
        }

        private Alignment HigherByAmountOfType()
        {
            if (HighestAmountOfType(Alignment.Player) > HighestAmountOfType(Alignment.Opponent)) return Alignment.Player;
            if (HighestAmountOfType(Alignment.Player) < HighestAmountOfType(Alignment.Opponent)) return Alignment.Opponent;
            return Alignment.None;
        }

        private int HighestAmountOfType(Alignment alignment)
        {
            return Mathf.Max(AmountOfType(alignment, Role.Offensive),
                AmountOfType(alignment, Role.Support),
                AmountOfType(alignment, Role.Agile),
                AmountOfType(alignment, Role.Special));
        }

        private int AmountOfType(Alignment alignment, Role role)
        {
            int result = 0;
            foreach (BoardField field in AlignedFields(alignment))
            {
                if (field.OccupantCard.CharacterConfig.Role == role) result++;
                if (role == Role.Offensive && field.AreThereTwoCards()) result++;
            }
            return result;
        }
    }
}
