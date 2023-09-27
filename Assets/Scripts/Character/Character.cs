using System;
using System.Collections;
using System.Collections.Generic;

/*
TODO:
BertPogromca(); - Special GLOBAL cards resistance and during judgements.
GotkaBerta();
KsiezniczkaBerta(); - Targetable newly converted cards.
*/

public abstract class Character
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
    public string Name { get => name; }
    public Gender Gender { get => gender; }
    public Role Role { get => role; }
    public int Strength { get => strength; }
    public int Power { get => power; }
    public int Dexterity { get => dexterity; }
    public int Health { get => health; }
    public List<int[]> AttackRange { get => attackRange; }


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
        if (relativeX == 0 && relativeY == 0) throw new System.Exception("Attempt to target self as a range.");
        if (range.Contains(coordinate)) throw new System.Exception("Duplicate coordinates.");
        range.Add(coordinate);
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

    public virtual void SkillOnSuccessfulAttack(CardSprite cardSprite) { }

    public virtual void SkillOnNewCard(CardSprite cardSprite) { }

    public virtual bool SkillSpecialAttack(CardSprite cardSprite) => false;

    public virtual void SkillOnNewTurn(CardSprite cardSprite) { }

    public virtual bool CanAffectStrength(CardSprite cardSprite, CardSprite spellSource) => true;

    public virtual void SkillAdjustStrengthChange(int value, CardSprite cardSprite) { }

    public virtual bool CanAffectPower(CardSprite cardSprite, CardSprite spellSource) => true;

    public virtual void SkillAdjustPowerChange(int value, CardSprite cardSprite, CardSprite spellSource) { }

    public virtual void SkillAdjustDexterityChange(int value, CardSprite cardSprite) { }

    public virtual void SkillAdjustHealthChange(int value, CardSprite cardSprite) { }

    public virtual void SkillOnAttack(CardSprite cardSprite) { }

    public virtual void SkillOnMove(CardSprite cardSprite) { }

    public virtual void SkillOnOtherCardDeath(CardSprite cardSprite, CardSprite source) { }

    public virtual int SkillDefenceModifier(int damage, CardSprite attacker) => damage;

    public virtual void SkillOnNeighbor(CardSprite cardSprite, CardSprite target) { }

    public virtual int SkillAttackModifier(int damage, CardSprite target) => damage;

    public virtual void SkillOnDeath(CardSprite cardSprite) { }

    public virtual bool SkillCardClick(CardSprite cardSprite) => false;
}
