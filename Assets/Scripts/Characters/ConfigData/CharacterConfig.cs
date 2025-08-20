using Berty.BoardCards;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.BoardCards.ConfigData
{
    public abstract class CharacterConfig
    {
        protected string name;
        protected CharacterEnum character;
        protected GenderEnum gender;
        protected RoleEnum role;
        protected int strength;
        protected int power;
        protected int dexterity;
        protected int health;
        protected List<Vector2Int> blockRange = new List<Vector2Int>();
        protected List<Vector2Int> riposteRange = new List<Vector2Int>();
        protected List<Vector2Int> attackRange = new List<Vector2Int>();
        protected AudioClip attackSound;
        public string Name { get => name; }
        public CharacterEnum Character { get => character; }
        public GenderEnum Gender { get => gender; }
        public RoleEnum Role { get => role; }
        public int Strength { get => strength; }
        public int Power { get => power; }
        public int Dexterity { get => dexterity; }
        public int Health { get => health; }
        public List<Vector2Int> AttackRange { get => attackRange; }
        public AudioClip AttackSound { get => attackSound; }

        public bool CanAttack(Vector2Int target)
        {
            return attackRange.Contains(target);
        }

        public bool CanBlock(Vector2Int source)
        {
            return blockRange.Contains(source);
        }

        public bool CanRiposte(Vector2Int source)
        {
            return riposteRange.Contains(source);
        }

        protected void AddName(string characterName)
        {
            name = characterName;
        }

        protected void SetCharacter(CharacterEnum character)
        {
            this.character = character;
        }

        protected void AddStats(int str, int pwr, int dex, int hp)
        {
            strength = str;
            power = pwr;
            dexterity = dex;
            health = hp;
        }
        protected void AddProperties(GenderEnum gndr, RoleEnum rl)
        {
            gender = gndr;
            role = rl;
        }
        protected void AddRange(int relativeX, int relativeY, List<Vector2Int> range)
        {
            Vector2Int coordinate = new(relativeX, relativeY);
            if (relativeX == 0 && relativeY == 0) throw new Exception("Attempt to target self as a range.");
            if (range.Contains(coordinate)) throw new Exception("Duplicate coordinates.");
            range.Add(coordinate);
        }
        protected void AddSoundEffect(string fileName)
        {
            attackSound = Resources.Load<AudioClip>("CharacterAttackSfx/" + fileName);
        }

        public virtual void SkillOnSuccessfulAttack(CardSpriteBehaviour cardSprite) { }

        public virtual void SkillOnNewCard(CardSpriteBehaviour cardSprite) { }

        public virtual bool SkillSpecialAttack(CardSpriteBehaviour cardSprite) => false;

        public virtual void SkillOnNewTurn(CardSpriteBehaviour cardSprite) { }

        public virtual bool CanAffectStrength(CardSpriteBehaviour cardSprite, CardSpriteBehaviour spellSource) => true;

        public virtual void SkillAdjustStrengthChange(int value, CardSpriteBehaviour cardSprite) { }

        public virtual bool CanAffectPower(CardSpriteBehaviour cardSprite, CardSpriteBehaviour spellSource) => true;

        public virtual void SkillAdjustPowerChange(int value, CardSpriteBehaviour cardSprite, CardSpriteBehaviour spellSource) { }

        public virtual void SkillAdjustDexterityChange(int value, CardSpriteBehaviour cardSprite) { }

        public virtual void SkillAdjustHealthChange(int value, CardSpriteBehaviour cardSprite) { }

        public virtual void SkillOnAttack(CardSpriteBehaviour cardSprite) { }

        public virtual void SkillOnMove(CardSpriteBehaviour cardSprite) { }

        public virtual void SkillOnOtherCardDeath(CardSpriteBehaviour cardSprite, CardSpriteBehaviour source) { }

        public virtual int SkillDefenceModifier(int damage, CardSpriteBehaviour attacker) => damage;

        public virtual void SkillOnNeighbor(CardSpriteBehaviour cardSprite, CardSpriteBehaviour target) { }

        public virtual int SkillAttackModifier(int damage, CardSpriteBehaviour target) => damage;

        public virtual void SkillOnDeath(CardSpriteBehaviour cardSprite) { }

        public virtual void SkillCardClick(CardSpriteBehaviour cardSprite) { }

        public virtual void SkillSideClick(CardSpriteBehaviour cardSprite) { }

        public virtual bool GlobalSkillResistance() => false;
    }
}