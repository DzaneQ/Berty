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
        protected SkillEnum skill;
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
        public SkillEnum Skill { get => skill; }
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

        protected void AddSkill(SkillEnum character)
        {
            this.skill = character;
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
    }
}