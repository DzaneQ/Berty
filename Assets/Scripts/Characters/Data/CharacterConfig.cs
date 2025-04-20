using Berty.CardSprite;
using Berty.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Berty.Characters.Data
{
    public abstract class CharacterConfig
    {
        protected string name;
        protected Gender gender;
        protected Role role;
        protected int strength;
        protected int power;
        protected int dexterity;
        protected int health;
        protected List<int[]> blockRange = new List<int[]>();
        protected List<int[]> riposteRange = new List<int[]>();
        protected List<int[]> attackRange = new List<int[]>();
        protected AudioClip attackSound;
        public string Name { get => name; }
        public Gender Gender { get => gender; }
        public Role Role { get => role; }
        public int Strength { get => strength; }
        public int Power { get => power; }
        public int Dexterity { get => dexterity; }
        public int Health { get => health; }
        public List<int[]> AttackRange { get => attackRange; }
        public AudioClip AttackSound { get => attackSound; }


        public bool CanBlock(int[] source)
        {
            foreach (int[] block in blockRange)
            {
                if (AreCoordinatesEqual(source, block)) return true;
            }
            return false;
        }

        public bool CanRiposte(int[] source)
        {
            foreach (int[] riposte in riposteRange)
                if (AreCoordinatesEqual(source, riposte)) return true;
            return false;
        }

        protected void AddName(string characterName)
        {
            name = characterName;
        }

        protected void AddStats(int str, int pwr, int dex, int hp)
        {
            strength = str;
            power = pwr;
            dexterity = dex;
            health = hp;
        }
        protected void AddProperties(Gender gndr, Role rl)
        {
            gender = gndr;
            role = rl;
        }
        protected void AddRange(int relativeX, int relativeY, List<int[]> range)
        {
            int[] coordinate = { relativeX, relativeY };
            if (relativeX == 0 && relativeY == 0) throw new Exception("Attempt to target self as a range.");
            if (range.Contains(coordinate)) throw new Exception("Duplicate coordinates.");
            range.Add(coordinate);
        }
        protected void AddSoundEffect(string fileName)
        {
            attackSound = Resources.Load<AudioClip>("CharacterAttackSfx/" + fileName);
        }

        private bool AreCoordinatesEqual(int[] first, int[] second)
        {
            if (first.Length != second.Length || first.Length != 2) return false;
            for (int i = 0; i < first.Length; i++)
            {
                if (first[i] != second[i]) return false;
            }
            return true;
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