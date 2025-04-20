using Berty.CardSprite;
using Berty.Characters.Data;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Entities
{
    public class BoardCard
    {
        private Vector2Int? _cached_relativeCoordinates;
        private BoardField _occupiedField;
        private Direction _direction;
        private List<CharacterConfig> resistance;
        public CharacterConfig CharacterConfig { get; private set; }
        public BoardField OccupiedField {
            get => _occupiedField;
            private set
            {
                _cached_relativeCoordinates = null;
                _occupiedField = value;
            }
        }
        public Direction Direction { 
            get => _direction; 
            private set
            {
                _cached_relativeCoordinates = null;
                _direction = value;
            } 
        }
        public CardStats Stats { get; private set; }
        public bool HasAttacked { get; private set; }
        public bool IsTired { get; private set; }
        public Alignment Align { get => OccupiedField.Align;  }
        public Vector2Int RelativeCoordinates { 
            get
            {
                if (!_cached_relativeCoordinates.HasValue) _cached_relativeCoordinates = CalculateCoordinates();
                return _cached_relativeCoordinates.Value;
            }
        }


        public void ActivateCard(CharacterConfig character, BoardField field, Direction direction)
        {
            LoadCharacterFromConfig(character);
            PlaceCard(field, direction);
        }

        public void DeactivateCard()
        {
            OccupiedField.RemoveCard();
            OccupiedField = null;
        }

        private void LoadCharacterFromConfig(CharacterConfig character)
        {
            CharacterConfig = character;
            Stats = new CardStats(character);
            HasAttacked = false;
            IsTired = false;
            resistance = new List<CharacterConfig>();
        }

        public void PlaceCard(BoardField field, Direction direction)
        {
            SetField(field);
            SetDirection(direction);
        }

        public void HandleNewTurn()
        {
            Stats.HandleNewTurn();
        }

        public void HandleOwnNewTurn()
        {
            ResetAttack();
            if (IsTired) RegenerateDexterity();
        }

        public void HandleAttack()
        {
            HasAttacked = true;
        }

        private void SetField(BoardField field)
        {
            OccupiedField = field;
        }

        private void SetDirection(Direction direction)
        {
            Direction = direction;
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
            if (Stats.Power <= 0) SwitchSides();
        }

        public void AdvanceTempPower(int value, BoardCard skillSource = null)
        {
            //if (!CharacterConfig.CanAffectPower(this, skillSource)) return;
            if (CharacterConfig.GlobalSkillResistance() && skillSource == null) return;
            Stats.TempPower += value;
            if (Stats.Power <= 0) SwitchSides();
        }

        public void AdvanceDexterity(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            if (skillSource == null) Debug.LogWarning($"No source affecting {CharacterConfig.Name}");
            Stats.Dexterity += value;
            if (Stats.Dexterity <= 0) ExhaustCard();
        }

        public void AdvanceHealth(int value, BoardCard skillSource = null)
        {
            if (skillSource != null && resistance.Contains(skillSource.CharacterConfig)) return;
            Stats.Health += value;
            //if (CanUseSkill()) CharacterConfig.SkillAdjustHealthChange(value, this);
            if (Stats.Health <= 0) KillCard();
        }

        public bool CanAttack()
        {
            return !HasAttacked && Stats.Strength > 0;
        }

        private void ResetAttack()
        {
            HasAttacked = false;
        }

        private void SwitchSides()
        {
            OccupiedField.SwitchSides();
            Stats.Power = CharacterConfig.Power;
        }

        private void ExhaustCard()
        {
            IsTired = true;
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

        private Vector2Int CalculateCoordinates()
        {
            int x = OccupiedField.Coordinates.x;
            int y = OccupiedField.Coordinates.y;
            int angle = (int)Direction;
            int sinus = (int)Math.Round(Math.Sin(angle / 180 * Math.PI));
            int cosinus = (int)Math.Round(Math.Cos(angle / 180 * Math.PI));
            return new Vector2Int(cosinus * x + sinus * y, cosinus * y - sinus * x);
        }
    }
}
