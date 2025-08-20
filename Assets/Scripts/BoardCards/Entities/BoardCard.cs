using Berty.BoardCards.ConfigData;
using Berty.Enums;
using Berty.Grid.Field.Entities;
using System;
using System.Collections;
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

        // TODO: Change argument types of external methods from CardSpriteBehaviour to BoardCard
        public void AdvanceStrength(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            //if (!CharacterConfig.CanAffectStrength(this, skillSource)) return;
            if (CharacterConfig.GlobalSkillResistance() && skillSource == null) return;
            Stats.Strength += value;
        }

        public void AdvanceTempStrength(int value, BoardCard skillSource = null)
        {
            //if (!CharacterConfig.CanAffectStrength(this, skillSource)) return;
            if (CharacterConfig.GlobalSkillResistance() && skillSource == null) return;
            Stats.TempStrength += value;
        }

        public void AdvancePower(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            //if (!CharacterConfig.CanAffectPower(this, skillSource)) return;
            if (CharacterConfig.GlobalSkillResistance() && skillSource == null) return;
            Stats.Power += value;
        }

        public void AdvanceTempPower(int value, BoardCard skillSource = null)
        {
            //if (!CharacterConfig.CanAffectPower(this, skillSource)) return;
            if (CharacterConfig.GlobalSkillResistance() && skillSource == null) return;
            Stats.TempPower += value;
        }

        public void AdvanceDexterity(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            if (skillSource == null) Debug.LogWarning($"No source affecting {CharacterConfig.Name}");
            Stats.Dexterity += value;
            if (Stats.Dexterity <= 0) MarkAsTired();
        }

        public void AdvanceHealth(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            Stats.Health += value;
            //if (CanUseSkill()) CharacterConfig.SkillAdjustHealthChange(value, this);
        }

        public bool CanAttack()
        {
            return !HasAttacked && Stats.Strength > 0;
        }

        private void SwitchSides()
        {
            OccupiedField.SwitchSides();
            Stats.Power = CharacterConfig.Power;
        }

        private void MarkAsTired()
        {
            IsTired = true;
        }

        public void MarkAsRested()
        {
            IsTired = false;
        }

        private void RegenerateDexterity()
        {
            if (Stats.Dexterity < CharacterConfig.Dexterity) AdvanceDexterity(1);
            if (Stats.Dexterity >= CharacterConfig.Dexterity) IsTired = false;
        }

        private void KillCard()
        {
            //CharacterConfig.SkillOnDeath(this);
            //foreach (BoardField field in Grid.Fields)
            //    if (field.IsOccupied() && field.OccupantCard.CanUseSkill())
            //        field.OccupantCard.CharacterConfig.SkillOnOtherCardDeath(field.OccupantCard, this);
            DeactivateCard();
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
