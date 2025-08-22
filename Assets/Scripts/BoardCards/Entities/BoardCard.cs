using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Grid.Field.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.Entities
{
    public class BoardCard
    {
        private Vector2Int? _cached_relativeCoordinates;
        private BoardField _occupiedField;
        private DirectionEnum _direction;
        private readonly List<CharacterConfig> resistance;
        public CharacterConfig CharacterConfig { get; }
        public BoardField OccupiedField {
            get => _occupiedField;
            private set
            {  
                _occupiedField = value;
                _cached_relativeCoordinates = null;
            }
        }
        public DirectionEnum Direction {
            get => _direction;
            private set
            {
                _direction = value;
                _cached_relativeCoordinates = null;
            }
        }
        public CardStats Stats { get; }
        public bool HasAttacked { get; private set; }
        public bool IsTired { get; private set; }
        public AlignmentEnum Align { get => OccupiedField.Align;  }
        public Vector2Int RelativeCoordinates { 
            get
            {
                if (!_cached_relativeCoordinates.HasValue) _cached_relativeCoordinates = CalculateCoordinates();
                return _cached_relativeCoordinates.Value;
            }
        }

        public BoardCard(CharacterConfig character, BoardField field)
        {
            CharacterConfig = character;
            OccupiedField = field;
            Stats = new CardStats(character);
            HasAttacked = false;
            IsTired = false;
            resistance = new List<CharacterConfig>();
        }

        public void DeactivateCard()
        {
            OccupiedField.RemoveCard();
            OccupiedField = null;
        }

        public void PlaceCard(BoardField field, DirectionEnum direction)
        {
            SetField(field);
            SetDirection(direction);
        }

        public void MarkAsHasAttacked()
        {
            HasAttacked = true;
        }

        public void MarkAsHasNotAttacked()
        {
            HasAttacked = false;
        }

        public int GetAngle()
        {
            return (int)Direction;
        }

        public void SetField(BoardField field)
        {
            OccupiedField = field;
        }

        private void SetDirection(DirectionEnum direction)
        {
            Direction = direction;
        }

        public void AdvanceAngleBy(int angle)
        {
            Direction = (DirectionEnum)(((int)Direction + angle) % 360);
        }

        public void AdvanceStrength(int value)
        {
            Stats.Strength += value;
        }

        public void AdvanceTempStrength(int value)
        {
            Stats.TempStrength += value;
        }

        public void AdvancePower(int value)
        {
            Stats.Power += value;
        }

        public void SetPower(int value)
        {
            Stats.Power = value - Stats.TempPower;
        }

        public void AdvanceTempPower(int value)
        {
            Stats.TempPower += value;
        }

        public void AdvanceDexterity(int value)
        {
            Stats.Dexterity += value;
        }

        public void AdvanceHealth(int value)
        {
            Stats.Health += value;
        }

        public bool CanAttack()
        {
            return !HasAttacked && Stats.Strength > 0;
        }

        public void MarkAsTired()
        {
            IsTired = true;
        }

        public void MarkAsRested()
        {
            IsTired = false;
        }

        public bool IsResistantTo(BoardCard card)
        {
            return resistance.Find(x => x == card.CharacterConfig) != null;
        }

        public void AddResistanceToCharacter(CharacterConfig character)
        {
            resistance.Add(character);
        }

        public Vector2Int GetDistanceTo(BoardCard target)
        {
            Vector2Int targetRelCoords = target.GetCoordinatesByDirection(Direction);
            return targetRelCoords - RelativeCoordinates;
        }

        public Vector2Int GetCoordinatesByDirection(DirectionEnum direction)
        {
            int x = OccupiedField.Coordinates.x;
            int y = OccupiedField.Coordinates.y;
            float angle = (float)direction;
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            return new Vector2Int(cosinus * x - sinus * y, cosinus * y + sinus * x);
        }

        private Vector2Int CalculateCoordinates()
        {
            return GetCoordinatesByDirection(Direction);
        }
    }
}
